using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs
{
  public class ParameterDoc
  {
    public string Name { get; set; }
    public IEnumerable<IDocElement> Description { get; set; }
  }
}
