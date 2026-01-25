using MsgPack;
using MsgPack.Serialization;
using NvimClient.Models.MsgPack;
using System.Collections.Generic;
using System.Linq;

namespace NvimClient.Models.Nvim;

public record NvimNotification {
    [MessagePackMember(0)]
    public byte Type { get; set; }

    [MessagePackMember(1)]
    public required string Method { get; set; }

    [MessagePackMember(2)]
    public required MessagePackObject[] Params { get; set; }

    /// <summary>
    /// Just a property that makes <see cref="Params" displayable
    /// as a human readble string
    /// </summary>
    public string ParamsString() {
        if (Params is null) {
            return "null";
        } else {
            return "[" + string.Join(", ", Params.Select(static p => p.ToString())) + "]";
        }
    }

    public static NvimNotification? FromMessagePackObject(MessagePackObject obj) {
        if (!obj.IsArray) {
            return null;
        }

        IList<MessagePackObject> list_items = obj.AsList();

        //The nvim response consists of 3 or 4 elements
        if (list_items.Count is not 3) {
            return null;
        }

        byte Type = list_items[0].AsByte();

        if (Type is not MsgPackDefinitions.NotificationTypeId) {
            return null;
        }

        NvimNotification n = new() {
            Type = Type,
            Method = list_items[1].AsString(),
            Params = [.. list_items[2].AsEnumerable()]
        };
        return n;
    }
}