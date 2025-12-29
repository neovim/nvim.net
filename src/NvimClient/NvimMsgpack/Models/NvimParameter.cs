using System;
using MsgPack.Serialization;

namespace NvimClient.NvimMsgpack.Models;

public record NvimParameter {
    [MessagePackMember(0)]
    public required string ArgumentType { get; set; }

    [MessagePackMember(1)]
    public required string ArgumentName { get; set; }

    public void Print() {
        Console.WriteLine("Type: {0}", ArgumentType);
        Console.WriteLine("Name: {0}", ArgumentName);
    }
}