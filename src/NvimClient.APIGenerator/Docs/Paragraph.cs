using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs
{
  internal class Paragraph : DocElementContainer
  {
    public Paragraph(IEnumerable<IDocElement> children) : base(children)
    {
    }
  }
}
