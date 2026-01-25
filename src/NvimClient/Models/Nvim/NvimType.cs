namespace NvimClient.Models.Nvim;

/// <summary>
/// Nvim types as required by the metadata.
/// </summary>
public record NvimType {
    public required int Id { get; set; }

    ///<summary>
    ///     The Prefix for the methods that this type calls
    ///</summary>
    public required string Prefix { get; set; }
}