using MsgPack;
using MsgPack.Serialization;

namespace NvimClient.NvimMsgpack.Models
{
  [NvimMessageType(1)]
  public class NvimResponse : NvimMessage
  {
    [MessagePackMember(1)] public long MessageId { get; set; }
    [MessagePackMember(2)] public MessagePackObject Error { get; set; }
    [MessagePackMember(3)] public MessagePackObject Result { get; set; }
  }
}
