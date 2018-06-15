using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
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
            if (!_notificationHandlers.TryGetValue(notification.Method,
              out var handler))
            {
              throw new Exception(
                $"No notification handler for \"{notification.Method}\"");
            }

            handler(notification.Arguments);
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
              throw new Exception("Received response with unknown message ID");
            }

            pendingRequest.Complete(response);
            break;
          default:
            throw new TypeLoadException("Unknown message type");
        }

        Receive();
      }
    }

    private class PendingRequest
    {
      private readonly ManualResetEvent _receivedResponseEvent;
      private NvimResponse _response;          

      internal PendingRequest() =>
        _receivedResponseEvent = new ManualResetEvent(false);

      internal Task<NvimResponse> GetResponse()
      {
        var taskCompletionSource = new TaskCompletionSource<NvimResponse>();
        ThreadPool.RegisterWaitForSingleObject(_receivedResponseEvent,
          (state, timeout) =>
            taskCompletionSource.SetResult(((PendingRequest) state)._response),
          this, Timeout.Infinite, true);
        return taskCompletionSource.Task;
      }

      internal void Complete(NvimResponse response)
      {
        _response = response;
        _receivedResponseEvent.Set();
      }
    }
  }
}
