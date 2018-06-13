using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly BlockingCollection<NvimRequest> _outstandingRequests;
    private readonly ConcurrentDictionary<long, PendingRequest>
      _outstandingResponses;
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
      _outstandingRequests  = new BlockingCollection<NvimRequest>();
      _outstandingResponses = new ConcurrentDictionary<long, PendingRequest>();

      StartSendLoop();
      StartReceiveLoop();
    }

    private Task<NvimResponse> SendAndReceive(NvimRequest request)
    {
      request.MessageId = _messageIdCounter++;
      var pendingRequest = new PendingRequest();
      _outstandingResponses[request.MessageId] = pendingRequest;
      _outstandingRequests.Add(request);
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
        foreach (var request in _outstandingRequests.GetConsumingEnumerable())
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
            // TODO: Handle notifications
            break;
          case NvimResponse response:
            if (!_outstandingResponses.TryRemove(response.MessageId,
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

      public PendingRequest() =>
        _receivedResponseEvent = new ManualResetEvent(false);

      public Task<NvimResponse> GetResponse()
      {
        var taskCompletionSource = new TaskCompletionSource<NvimResponse>();
        ThreadPool.RegisterWaitForSingleObject(_receivedResponseEvent,
          (state, timeout) =>
            taskCompletionSource.SetResult(((PendingRequest) state)._response),
          this, Timeout.Infinite, true);
        return taskCompletionSource.Task;
      }

      public void Complete(NvimResponse response)
      {
        _response = response;
        _receivedResponseEvent.Set();
      }
    }
  }
}
