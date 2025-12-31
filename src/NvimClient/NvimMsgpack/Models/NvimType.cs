namespace NvimClient.NvimMsgpack.Models;

/// <summary>
/// Nvim types as required by the metadata.
/// </summary>
public record NvimType {
    public required int Id { get; set; }
    public required string Prefix { get; set; }
}