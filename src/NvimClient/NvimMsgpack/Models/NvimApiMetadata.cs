using System.Collections.Generic;

namespace NvimClient.NvimMsgpack.Models
{
  public class NvimAPIMetadata
  {
    public NvimVersion Version { get; set; }
    public NvimFunction[] Functions { get; set; }
    public NvimUIEvent[] UIEvents { get; set; }
    public Dictionary<string, NvimType> Types { get; set; }
    public Dictionary<string, NvimErrorType> ErrorTypes { get; set; }
  }
}
