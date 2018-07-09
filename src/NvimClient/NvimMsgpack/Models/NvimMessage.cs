using MsgPack.Serialization;

namespace NvimClient.NvimMsgpack.Models
{
  public abstract class NvimMessage
  {
    [MessagePackMember(0)]
    public byte TypeId { get; set; }
  }
}
