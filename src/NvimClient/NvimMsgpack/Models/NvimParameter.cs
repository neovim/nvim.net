using MsgPack.Serialization;

namespace NvimClient.NvimMsgpack.Models;

public class NvimParameter {
    [MessagePackMember(0)]
    public required string Type { get; set; }

    [MessagePackMember(1)]
    public required string Name { get; set; }
}