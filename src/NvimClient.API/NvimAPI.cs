using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MsgPack;
using MsgPack.Serialization;
using NvimClient.NvimMsgpack;
using NvimClient.NvimMsgpack.Models;
using NvimClient.NvimProcess;

namespace NvimClient.API
{
  /// <summary>
  /// Interface to the Neovim msgpack-rpc API.
  /// </summary>
  public partial class NvimAPI
  {
    /// <summary>
    /// Handler for requests from Neovim without a handler registered via <see cref="RegisterHandler"/>
    /// </summary>
    public event EventHandler<NvimUnhandledRequestEventArgs> OnUnhandledRequest;

    /// <summary>
    /// Handler for notifications from Neovim without a handler registered via <see cref="RegisterHandler"/>
    /// </summary>
    public event EventHandler<NvimUnhandledNotificationEventArgs>
      OnUnhandledNotification;

    private readonly Stream _inputStream;
    private readonly Stream _outputStream;
    private readonly MessagePackSerializer<NvimMessage> _serializer;
    private readonly BlockingCollection<NvimMessage> _messageQueue;
    private readonly ConcurrentDictionary<long, PendingRequest>
      _pendingRequests;
    private delegate void NvimHandler(uint? requestId, object[] arguments);
    private readonly ConcurrentDictionary<string, NvimHandler> _handlers;
    private uint _messageIdCounter;
    private readonly ManualResetEvent _waitEvent = new ManualResetEvent(false);

    /// <summary>
    /// Starts a new Nvim process and communicates
    /// with it through stdin and stdout streams.
    /// </summary>
    public NvimAPI() : this(Process.Start(
        new NvimProcessStartInfo(StartOption.Embed | StartOption.Headless)))
    {
    }

    /// <summary>
    /// Communicates with an already-running Nvim
    /// process through its stdin and stdout streams.
    /// </summary>
    /// <param name="process"></param>
    public NvimAPI(Process process) : this(process.StandardInput.BaseStream,
      process.StandardOutput.BaseStream)
    {
    }

    /// <summary>
    /// Communicates with Nvim through the specified server address.
    /// </summary>
    /// <param name="serverAddress"></param>
    public NvimAPI(string serverAddress) : this(
      GetStreamFromServerAddress(serverAddress))
    {
    }

    private static Stream GetStreamFromServerAddress(string serverAddress)
    {
      var lastColonIndex = serverAddress.LastIndexOf(':');
      if (lastColonIndex != -1 && lastColonIndex != 0
                               && int.TryParse(
                                    serverAddress.Substring(lastColonIndex + 1),
                                    out var port))
      {
        // TCP socket
        var tcpClient = new TcpClient();
        var hostname = serverAddress.Substring(0, lastColonIndex);
        tcpClient.Connect(hostname, port);
        return tcpClient.GetStream();
      }

      // Interprocess communication socket
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        // Named Pipe on Windows
        var match = Regex.Match(serverAddress,
          @"\\\\(?'serverName'[^\\]+)\\pipe\\(?'pipeName'[^\\]+)");
        var serverName = match.Groups["serverName"].Value;
        var pipeName = match.Groups["pipeName"].Value;
        var pipeStream = new NamedPipeClientStream(serverName, pipeName,
          PipeDirection.InOut, PipeOptions.Asynchronous);
        pipeStream.Connect();
        return pipeStream;
      }

      // Unix Domain Socket on other OSes
      var unixDomainSocket = new Socket(AddressFamily.Unix,
        SocketType.Stream, ProtocolType.Unspecified);
      unixDomainSocket.Connect(new UnixDomainSocketEndPoint(serverAddress));
      return new NetworkStream(unixDomainSocket, true);
    }

    /// <summary>
    /// Communicates with Nvim through the provided stream.
    /// </summary>
    /// <param name="inputOutputStream">The stream to use.</param>
    public NvimAPI(Stream inputOutputStream) : this(inputOutputStream,
      inputOutputStream)
    {
    }

    /// <summary>
    /// Communicates with Nvim through the
    /// provided input and output streams.
    /// </summary>
    /// <param name="inputStream">The input stream to use.</param>
    /// <param name="outputStream">The output stream to use.</param>
    public NvimAPI(Stream inputStream, Stream outputStream)
    {
      var context = new SerializationContext();
      context.Serializers.Register(new NvimMessageSerializer(context));
      _serializer = MessagePackSerializer.Get<NvimMessage>(context);
      _inputStream = inputStream;
      _outputStream = outputStream;
      _messageQueue = new BlockingCollection<NvimMessage>();
      _pendingRequests = new ConcurrentDictionary<long, PendingRequest>();
      _handlers = new ConcurrentDictionary<string, NvimHandler>();

      StartSendLoop();
      StartReceiveLoop();
    }

    /// <summary>
    /// Register a handler for the <c>name</c> notification or request.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="handler"></param>
    public void RegisterHandler(string name, Func<object[], object> handler) =>
      RegisterHandler(name, (Delegate)handler);

    /// <summary>
    /// Register a handler for the <c>name</c> notification or request.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="handler"></param>
    public void RegisterHandler(string name, Action<object[]> handler) =>
      RegisterHandler(name, (Delegate)handler);

    /// <summary>
    /// Register a handler for the <c>name</c> notification or request.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="handler"></param>
    public void RegisterHandler(string name,
      Func<object[], Task<object>> handler) => RegisterHandler(name,
      (requestId, args) =>
      {
        Task.Run(() =>
        {
          if (requestId.HasValue)
          {
            CallHandlerAndSendResponse(requestId.Value, handler, args);
          }
          else
          {
            handler(args);
          }
        });
      });

    private void RegisterHandler(string name, Delegate handler) =>
      RegisterHandler(name, (requestId, args) =>
      {
        if (requestId.HasValue)
        {
          CallHandlerAndSendResponse(requestId.Value, handler, args);
        }
        else
        {
          handler.DynamicInvoke(args);
        }
      });

    private void RegisterHandler(string name, NvimHandler handler)
    {
      if (!_handlers.TryAdd(name, handler))
      {
        throw new Exception(
          $"Handler for \"{name}\" is already registered");
      }
    }

    private void CallHandlerAndSendResponse(uint requestId, Delegate handler,
      object[] args)
    {
      var response = new NvimResponse { MessageId = requestId };
      try
      {
        var result = handler.DynamicInvoke(new object[] { args });
        response.Result = ConvertToMessagePackObject(result);
      }
      catch (Exception exception)
      {
        response.Error = exception.ToString();
      }

      _messageQueue.Add(response);
    }


    internal void SendResponse(NvimUnhandledRequestEventArgs args, object result,
      object error)
    {
      var response = new NvimResponse
      {
        MessageId = args.RequestId,
        Result = ConvertToMessagePackObject(result),
        Error = ConvertToMessagePackObject(error)
      };
      _messageQueue.Add(response);
    }

    private Task<NvimResponse> SendAndReceive(NvimRequest request)
    {
      request.MessageId = _messageIdCounter++;
      var pendingRequest = new PendingRequest();
      _pendingRequests[request.MessageId] = pendingRequest;
      _messageQueue.Add(request);
      return pendingRequest.GetResponse();
    }

    private Task<TResult> SendAndReceive<TResult>(NvimRequest request)
    {
      return SendAndReceive(request)
        .ContinueWith(task =>
        {
          var response = task.Result;
          var result = ConvertFromMessagePackObject(response.Result);
          if (typeof(TResult).IsArray)
          {
            var objectArray = (object[])result;
            var resultArray = Array.CreateInstance(
              typeof(TResult).GetElementType(),
              objectArray.Length);
            Array.Copy(objectArray, resultArray, resultArray.Length);
            return (TResult)(object)resultArray;
          }

          return (TResult)result;
        });
    }

    private void StartSendLoop()
    {
      Task.Run(async () =>
      {
        foreach (var request in _messageQueue.GetConsumingEnumerable())
        {
          await _serializer.PackAsync(_inputStream, request);
        }
      }).ContinueWith(t => _waitEvent.Set());
    }

    private void StartReceiveLoop()
    {
      Receive();

      async void Receive()
      {
        NvimMessage message;
        try
        {
          message = await _serializer.UnpackAsync(_outputStream);

        }
        catch
        {
          _waitEvent.Set();
          throw;
        }

        switch (message)
        {
          case NvimNotification notification:
            {
              if (notification.Method == "redraw")
              {
                var uiEvents = notification.Arguments.AsEnumerable().SelectMany(
                  uiEvent =>
                  {
                    var data = uiEvent.AsList();
                    var name = data.First().AsString();
                    return data.Select(args => new { Name = name, Args = args })
                               .Skip(1);
                  });
                foreach (var uiEvent in uiEvents)
                {
                  CallUIEventHandler(uiEvent.Name,
                    (object[])ConvertFromMessagePackObject(uiEvent.Args));
                }
              }

              var arguments =
                (object[])ConvertFromMessagePackObject(notification.Arguments);
              if (_handlers.TryGetValue(notification.Method, out var handler))
              {
                handler(null, arguments);
              }
              else
              {
                OnUnhandledNotification?.Invoke(this,
                  new NvimUnhandledNotificationEventArgs(notification.Method,
                    arguments));
              }

              break;
            }
          case NvimRequest request:
            {
              var arguments =
                (object[])ConvertFromMessagePackObject(request.Arguments);
              if (_handlers.TryGetValue(request.Method, out var handler))
              {
                handler(request.MessageId, arguments);
              }
              else
              {
                OnUnhandledRequest?.Invoke(this,
                  new NvimUnhandledRequestEventArgs(this, request.MessageId,
                    request.Method, arguments));
              }

              break;
            }
          case NvimResponse response:
            if (!_pendingRequests.TryRemove(response.MessageId,
              out var pendingRequest))
            {
              throw new Exception(
                "Received response with "
                + $"unknown message ID \"{response.MessageId}\"");
            }

            pendingRequest.Complete(response);
            break;
          default:
            throw new TypeLoadException(
              $"Unknown message type \"{message.GetType()}\"");
        }

        Receive();
      }
    }

    /// <summary>
    /// Block the current thread while waiting for Neovim to quit.
    /// </summary>
    public void WaitForDisconnect() => _waitEvent.WaitOne();

    private class PendingRequest
    {
      private readonly TimeSpan _responseTimeout = TimeSpan.FromSeconds(10);
      private readonly ManualResetEvent _receivedResponseEvent;
      private NvimResponse _response;

      internal PendingRequest() =>
        _receivedResponseEvent = new ManualResetEvent(false);

      internal Task<NvimResponse> GetResponse()
      {
        var taskCompletionSource = new TaskCompletionSource<NvimResponse>();

        void RegisterResponseEvent(TimeSpan timeout) =>
          ThreadPool.RegisterWaitForSingleObject(_receivedResponseEvent,
            (state, timedOut) =>
            {
              if (timedOut)
              {
                Debug.WriteLine("Warning: response was not received "
                                + $"within {timeout.TotalSeconds} seconds");
                // Continue waiting without a timeout
                RegisterResponseEvent(Timeout.InfiniteTimeSpan);
              }
              else
              {
                taskCompletionSource.SetResult(
                  ((PendingRequest)state)._response);
              }
            },
            this, timeout, true);

        RegisterResponseEvent(_responseTimeout);

        return taskCompletionSource.Task;
      }

      internal void Complete(NvimResponse response)
      {
        _response = response;
        _receivedResponseEvent.Set();
      }
    }

    private object ConvertFromMessagePackObject(MessagePackObject msgPackObject)
    {
      if (msgPackObject.IsTypeOf(typeof(long)) ?? false)
      {
        return msgPackObject.AsInt64();
      }

      if (msgPackObject.IsTypeOf(typeof(double)) ?? false)
      {
        return msgPackObject.AsDouble();
      }

      if (msgPackObject.IsArray)
      {
        return msgPackObject.AsEnumerable().Select(ConvertFromMessagePackObject)
          .ToArray();
      }

      if (msgPackObject.IsDictionary)
      {
        var msgPackDictionary = msgPackObject.AsDictionary();
        return msgPackDictionary.ToDictionary(
          keyValuePair => ConvertFromMessagePackObject(keyValuePair.Key),
          keyValuePair => ConvertFromMessagePackObject(keyValuePair.Value));
      }

      var obj = msgPackObject.ToObject();
      if (obj is MessagePackExtendedTypeObject msgpackExtObj)
      {
        return GetExtensionType(msgpackExtObj);
      }

      return obj;
    }

    private static MessagePackObject ConvertToMessagePackObject(object obj)
    {
      IEnumerable<MessagePackObject>
      ConvertEnumerable(IEnumerable enumerable) =>
        enumerable.Cast<object>().Select(ConvertToMessagePackObject);

      if (obj is Array array)
      {
        return MessagePackObject.FromObject(ConvertEnumerable(array));
      }

      if (obj is IDictionary dictionary)
      {
        var msgPackDictionary = new MessagePackObjectDictionary();
        var keysAndValues = ConvertEnumerable(dictionary.Keys).Zip(
          ConvertEnumerable(dictionary.Values), (key, value) => (key, value));
        foreach (var (key, value) in keysAndValues)
        {
          msgPackDictionary.Add(key, value);
        }

        return MessagePackObject.FromObject(msgPackDictionary);
      }

      return MessagePackObject.FromObject(obj);
    }

    private static MessagePackObject GetRequestArguments(
      params object[] parameters) =>
      ConvertToMessagePackObject(parameters);
  }
}
