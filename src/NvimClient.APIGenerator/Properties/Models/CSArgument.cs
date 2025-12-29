using NvimClient.NvimMsgpack;
using NvimClient.NvimMsgpack.Models;

namespace NvimClient.APIGenerator.Properties.Models;

public record CSArgument {
    public required string ArgumentType { get; set; }

    public required string ArgumentName { get; set; }

    public static CSArgument FromNvimParameter(NvimParameter param) {
        return new CSArgument() {
            ArgumentName = param.ArgumentName,
            ArgumentType = NvimTypesMap.GetCSharpType(param.ArgumentType)
        };
    }
}