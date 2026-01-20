namespace NvimClient.APIGenerator.Properties.Models;

public record CSFunctionAttributeDescription {
    public required string AttributeName { get; init; }
    //public List<CSArgument>? Arguments { get; set; }

    public string ToCode() {
        return $"[{AttributeName}]";
    }
}