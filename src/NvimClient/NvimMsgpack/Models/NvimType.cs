namespace NvimClient.NvimMsgpack.Models;

public record NvimType {
    public required int Id { get; set; }
    public required string Prefix { get; set; }
}