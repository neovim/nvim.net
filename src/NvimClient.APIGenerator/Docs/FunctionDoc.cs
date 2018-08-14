using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs
{
  public class FunctionDoc
  {
    public string Function { get; set; }
    public IEnumerable<IDocElement> Summary { get; set; }
    public IEnumerable<ParameterDoc> Parameters { get; set; }
    public IEnumerable<IDocElement> Return { get; set; }
    public IEnumerable<IDocElement> Notes { get; set; }
  }
}
