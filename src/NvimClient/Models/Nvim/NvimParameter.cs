using MsgPack.Serialization;

namespace NvimClient.Models.Nvim;

/// <summary>
/// Nvim Parameters as defined by the metadata
/// </summary>
public record NvimParameter {
    /// <summary>
    /// The type of the argument
    /// </summary>
    [MessagePackMember(0)]
    public required string ArgumentType { get; set; }

    /// <summary>
    /// The name of the argument
    /// </summary>
    [MessagePackMember(1)]
    public required string ArgumentName { get; set; }
}