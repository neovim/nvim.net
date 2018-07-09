using MsgPack;
using MsgPack.Serialization;

namespace NvimClient.NvimMsgpack.Models
{
  [NvimMessageType(0)]
  public class NvimRequest : NvimMessage
  {
    [MessagePackMember(1)] public uint MessageId { get; set; }
    [MessagePackMember(2)] public string Method { get; set; }
    [MessagePackMember(3)] public MessagePackObject Arguments { get; set; }
  }
}
