using MsgPack.Serialization;

namespace NvimClient.NvimMsgpack.Models
{
  public class NvimParameter
  {
    [MessagePackMember(0)] public string Type { get; set; }
    [MessagePackMember(1)] public string Name { get; set; }
  }
}
