using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs;

internal class Paragraph : DocElementContainer {
    public Paragraph(IEnumerable<IDoxygenElement> children) : base(children) {
    }
}