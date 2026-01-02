using MsgPack;
using MsgPack.Serialization;

namespace NvimClient.NvimMsgpack.Models;

/// <summary>
///     Represents an RPC message towards nvim. This can either be a request or a
///     response.
/// </summary>
///
/// <remarks>
///     This object should no be serialized as map. But as a single object for
///     array.
/// </remarks>
public record NvimRequest {
    /// <summary>
    /// The message type
    /// </summary>
    [MessagePackMember(0)]
    public byte Type { get; set; } = MsgPackDefinitions.RequestTypeId;

    /// <summary>
    /// A 32-bit unsigned integer number. This number is used as a sequence number.
    /// The server's response to the "Request" will have the same msgid.
    /// </summary>
    [MessagePackMember(1)]
    public uint MsgId { get; set; }

    /// <summary>
    /// The method name that will be remotely called
    /// </summary>
    [MessagePackMember(2)]
    public required string Method { get; set; }

    /// <summary>
    /// An array of the function arguments. The elements of this array are arbitrary
    /// objects.
    /// </summary>
    [MessagePackMember(3)]
    public MessagePackObject[] Params { get; set; } = [];
}