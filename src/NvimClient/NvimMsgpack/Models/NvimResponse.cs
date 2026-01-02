using MsgPack;
using MsgPack.Serialization;
using System.Collections.Generic;

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

    public static NvimResponse? FromMessagePackObject(MessagePackObject obj) {
        if (!obj.IsArray) {
            return null;
        }

        IList<MessagePackObject> list_items = obj.AsList();

        // The nvim response consists of 4 elements
        if (list_items.Count is not 4) {
            return null;
        }

        byte Type = list_items[0].AsByte();

        if (Type is not MsgPackDefinitions.ResponseTypeId or MsgPackDefinitions.NotificationTypeId) {
            return null;
        }

        NvimResponse r = new() {
            Type = Type,
            MsgId = list_items[1].AsUInt32(),
            Error = list_items[2],
            Result = list_items[3],
        };

        return r;
    }
}
