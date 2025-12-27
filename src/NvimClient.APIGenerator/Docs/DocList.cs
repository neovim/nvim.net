using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs;

internal class DocList : DocElementContainer {
    public DocListType ListType { get; }

    public DocList(DocListType itemizedList, IEnumerable<IDocElement> children) : base(children) {
        ListType = itemizedList;
    }

}
