namespace NvimClient.NvimMsgpack.Models;

/// <summary>
/// Defines the Nvim Error type. This is required for the deserialisation schema
/// of the metadata
/// </summary>
public record NvimErrorType {
    public required int Id { get; set; }
}