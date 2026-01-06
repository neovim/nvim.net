using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs;

/// <summary>
/// Raw text documentation
/// </summary>
internal class Text : IDoxygenElement {
    /// <summary>
    /// The actual text
    /// </summary>
    private readonly string _text;

    /// <summary>
    /// Constructor that receives the text as a string
    /// </summary>
    public Text(string text) {
        _text = text;
    }

    public List<string> StringContents() {
        return [_text];
    }

    /// <summary>
    /// An override to return the text instead of the object name
    /// </summary>
    public override string ToString() => _text;
}