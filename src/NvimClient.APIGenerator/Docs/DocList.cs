using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs;

internal class DocList : DocElementContainer {
    public DocListType ListType { get; }

    public DocList(DocListType itemizedList, IEnumerable<IDoxygenElement> children) : base(children) {
        ListType = itemizedList;
    }

}