using System.Collections.Generic;

namespace NvimClient.NvimMsgpack.Models;

/// <summary>
/// NVIM API metadata
/// </summary>
public class NvimAPIMetadata {
    public required NvimVersion Version { get; set; }
    public required NvimFunction[] Functions { get; set; }
    public required NvimUIEvent[] UIEvents { get; set; }
    public required Dictionary<string, NvimType> Types { get; set; }
    public required Dictionary<string, NvimErrorType> ErrorTypes { get; set; }
}