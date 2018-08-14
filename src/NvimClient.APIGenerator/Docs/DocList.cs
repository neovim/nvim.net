using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs
{
  internal class DocList : DocElementContainer
  {
    public DocList(DocListType itemizedList, IEnumerable<IDocElement> children)
      : base(children) => ListType = itemizedList;

    public DocListType ListType { get; }
  }
}
