using MsgPack;
using MsgPack.Serialization;

namespace NvimClient.NvimMsgpack.Models;

/// <summary>
///     Represents an RPC response from nvim. This can either be a request or a
///     response.
/// </summary>
///
/// <remarks>
///     This object should no be serialized as map. But as a single object for
///     array.
/// </remarks>
public class NvimResponse {
    [MessagePackMember(0)]
    public byte Type { get; set; }

    [MessagePackMember(1)]
    public uint MsgId { get; set; }

    [MessagePackMember(2)]
    public MessagePackObject Error { get; set; }

    [MessagePackMember(3)]
    public MessagePackObject Result { get; set; }
}