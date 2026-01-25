using System.Collections.Generic;
using System.Linq;
using MsgPack;
using MsgPack.Serialization;
using NvimClient.Models.MsgPack;

namespace NvimClient.Models.Nvim;

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


    /// <summary>
    /// Just a property that makes <see cref="Params"/> displayable as a
    /// human readble string
    /// </summary>
    public string ParamsString() {
        if (Params is null) {
            return "null";
        } else {
            return "[" + string.Join(", ", Params.Select(static p => p.ToString())) + "]";
        }
    }

    public static NvimRequest? FromMessagePackObject(MessagePackObject obj) {
        if (!obj.IsArray) {
            return null;
        }

        IList<MessagePackObject> listItems = obj.AsList();
        if (listItems.Count is not 4) {
            return null;
        }

        byte type = listItems[0].AsByte();
        if (type is not MsgPackDefinitions.RequestTypeId) {
            return null;
        }

        return new NvimRequest {
            Type = type,
            MsgId = listItems[1].AsUInt32(),
            Method = listItems[2].AsString(),
            Params = [.. listItems[3].AsEnumerable()]
        };
    }
}