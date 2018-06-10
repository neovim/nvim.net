using System;
using System.Diagnostics;
using System.IO;
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
    private int _messageIdCounter;

    public NvimAPI()
    {
      var process = Process.Start(
        new NvimProcessStartInfo(StartOption.Embed | StartOption.Headless));

      var context = new SerializationContext();
      context.Serializers.Register(new NvimMessageSerializer(context));
      _serializer   = MessagePackSerializer.Get<NvimMessage>(context);
      _inputStream  = process.StandardInput.BaseStream;
      _outputStream = process.StandardOutput.BaseStream;
    }

    private Task<NvimMessage> SendAndReceive(NvimRequest request)
    {
      request.MessageId = _messageIdCounter++;
      _serializer.Pack(_inputStream, request);
      // TODO: Wait for actual response with corresponding MessageId
      return _serializer.UnpackAsync(_outputStream);
    }

    private Task<TResult> SendAndReceive<TResult>(NvimRequest request)
    {
      return SendAndReceive(request)
        .ContinueWith(task =>
        {
          var response = (NvimResponse) task.Result;
          return (TResult) Convert.ChangeType(response.Result, typeof(TResult));
        });
    }
  }
}
