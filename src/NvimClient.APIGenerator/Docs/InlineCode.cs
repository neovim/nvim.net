using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs;

internal class InlineCode : IDoxygenElement {
    private readonly string _code;

    public InlineCode(string code) => _code = code;

    public List<string> StringContents() {
        throw new System.NotImplementedException();
    }

    public override string ToString() => _code;
}