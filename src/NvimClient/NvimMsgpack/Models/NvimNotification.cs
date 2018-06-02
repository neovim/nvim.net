using MsgPack;
using MsgPack.Serialization;

namespace NvimClient.NvimMsgpack.Models
{
  [NvimMessageType(2)]
  public class NvimNotification : NvimMessage
  {
    [MessagePackMember(1)] public string Method { get; set; }
    [MessagePackMember(2)] public MessagePackObject Arguments { get; set; }
  }
}
