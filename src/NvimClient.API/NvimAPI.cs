using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MsgPack;
using MsgPack.Serialization;
using NvimClient.NvimMsgpack;
using NvimClient.NvimMsgpack.Models;
using NvimClient.NvimProcess;

namespace NvimClient.API
{
  public partial class NvimAPI
  {
    private readonly Stream _inputStream;
    private readonly Stream _outputStream;
    private readonly MessagePackSerializer<NvimMessage> _serializer;
    private readonly BlockingCollection<NvimMessage> _messageQueue;
    private readonly ConcurrentDictionary<long, PendingRequest>
      _pendingRequests;
    private delegate void NvimHandler(long? requestId, object[] arguments);
    private readonly ConcurrentDictionary<string, NvimHandler> _handlers;
    private long _messageIdCounter;

    public NvimAPI()
    {
      var process = Process.Start(
        new NvimProcessStartInfo(StartOption.Embed | StartOption.Headless));

      var context = new SerializationContext();
      context.Serializers.Register(new NvimMessageSerializer(context));
      _serializer   = MessagePackSerializer.Get<NvimMessage>(context);
      _inputStream  = process.StandardInput.BaseStream;
      _outputStream = process.StandardOutput.BaseStream;
      _messageQueue  = new BlockingCollection<NvimMessage>();
      _pendingRequests = new ConcurrentDictionary<long, PendingRequest>();
      _handlers = new ConcurrentDictionary<string, NvimHandler>();

      StartSendLoop();
      StartReceiveLoop();
    }

    public void RegisterHandler(string name, Func<object[], object> handler) =>
      RegisterHandler(name, (Delegate) handler);

    public void RegisterHandler(string name, Action<object[]> handler) =>
      RegisterHandler(name, (Delegate) handler);

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

    private void CallHandlerAndSendResponse(long requestId, Delegate handler,
      object[] args)
    {
      var response = new NvimResponse {MessageId = requestId};
      try
      {
        var result = handler.DynamicInvoke(new object[] {args});
        response.Result = ConvertToMessagePackObject(result);
      }
      catch (Exception exception)
      {
        response.Error = exception.ToString();
      }

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
          return (TResult) ConvertFromMessagePackObject(response.Result);
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
      });
    }

    private void StartReceiveLoop()
    {
      Receive();

      async void Receive()
      {
        var message = await _serializer.UnpackAsync(_outputStream);
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
                  return data.Select(args => new {Name = name, Args = args})
                             .Skip(1);
                });
              foreach (var uiEvent in uiEvents)
              {
                CallUIEventHandler(uiEvent.Name,
                  (object[]) ConvertFromMessagePackObject(uiEvent.Args));
              }
            }

            if (_handlers.TryGetValue(notification.Method, out var handler))
            {
              var args =
                (object[]) ConvertFromMessagePackObject(notification.Arguments);
              handler(null, args);
            }

            break;
          }
          case NvimRequest request:
          {
            if (!_handlers.TryGetValue(request.Method, out var handler))
            {
              throw new Exception(
                $"No request handler for \"{request.Method}\"");
            }

            var args =
              (object[]) ConvertFromMessagePackObject(request.Arguments);
            handler(request.MessageId, args);

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
                  ((PendingRequest) state)._response);
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

    private static object
      ConvertFromMessagePackObject(MessagePackObject msgPackObject)
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

      return msgPackObject.ToObject();
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
        var keyValuePairs = ConvertEnumerable(dictionary.Keys).Zip(
          ConvertEnumerable(dictionary.Values),
          KeyValuePair.Create);
        foreach (var keyValuePair in keyValuePairs)
        {
          msgPackDictionary.Add(keyValuePair.Key, keyValuePair.Value);
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
