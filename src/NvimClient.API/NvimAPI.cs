using System;
using System.Collections.Concurrent;
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
    private readonly ConcurrentDictionary<string,
      Func<MessagePackObject, MessagePackObject>> _requestHandlers;
    private readonly ConcurrentDictionary<string, Action<MessagePackObject>>
      _notificationHandlers;
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
      _requestHandlers =
        new ConcurrentDictionary<string,
          Func<MessagePackObject, MessagePackObject>>();
      _notificationHandlers =
        new ConcurrentDictionary<string, Action<MessagePackObject>>();

      StartSendLoop();
      StartReceiveLoop();
    }

    public void AddRequestHandler(string name,
      Func<MessagePackObject, MessagePackObject> handler)
    {
      _requestHandlers[name] = handler;
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
          return (TResult) response.Result.ToObject();
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
                  (MessagePackObject[]) uiEvent.Args.ToObject());
              }
            }

            if (_notificationHandlers.TryGetValue(notification.Method,
              out var handler))
            {
              handler(notification.Arguments);
            }

            break;
          }
          case NvimRequest request:
          {
            if (!_requestHandlers.TryGetValue(request.Method, out var handler))
            {
              throw new Exception(
                $"No request handler for \"{request.Method}\"");
            }

            var response = new NvimResponse
                           {
                             MessageId = request.MessageId,
                           };
            try
            {
              response.Result = handler(request.Arguments);
            }
            catch (Exception exception)
            {
              response.Error = exception.ToString();
            }

            _messageQueue.Add(response);
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

    private static T Cast<T>(MessagePackObject msgPackObject)
    {
      if (typeof(T) == typeof(long))
      {
        return (T) (object) msgPackObject.AsInt64();
      }
      if (typeof(T) == typeof(double))
      {
        return (T) (object) msgPackObject.AsDouble();
      }
      return (T) msgPackObject.ToObject();
    }
  }
}
