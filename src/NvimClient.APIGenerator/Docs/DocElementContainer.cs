using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs
{
  internal abstract class DocElementContainer : IDocElement
  {
    protected DocElementContainer(IEnumerable<IDocElement> children) =>
      Children = children;

    public IEnumerable<IDocElement> Children { get; }
  }
}
