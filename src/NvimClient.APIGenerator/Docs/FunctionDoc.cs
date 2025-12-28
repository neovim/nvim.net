using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NvimClient.APIGenerator.Docs;

/// <summary>
/// Represents the documentation of the function
/// </summary>
public class FunctionDoc {
    /// <summary>
    /// The function name
    /// </summary>
    public required string Function { get; set; }

    /// <summary>
    /// The summary of the function
    /// </summary>
    public required IEnumerable<IDocElement> Summary { get; set; }

    /// <summary>
    /// Documentation about the function parameters
    /// </summary>
    public required IEnumerable<ParameterDoc>? Parameters { get; set; }

    /// <summary>
    /// Documentation of the return value of the function
    /// </summary>
    public required IEnumerable<IDocElement>? Return { get; set; }

    /// <summary>
    /// Additional notes of the function documentation
    /// </summary>
    public required IEnumerable<IDocElement>? Notes { get; set; }

    public static FunctionDoc FromXElement(XElement memberDef) {

        IEnumerable<XElement>? doc_containers = memberDef.Element("detaileddescription")?.Elements("para").Where(static para => para.Element("parameterlist") == null);

        IEnumerable<ParameterDoc>? doc_parameters = memberDef.Descendants("parameterlist").FirstOrDefault()?.Elements("parameteritem").Select(ParameterDoc.FromXElement);

        XElement? doc_return = memberDef.Element("detaileddescription")?.Descendants("simplesect").FirstOrDefault(static simplesect => simplesect.Attribute("kind")?.Value == "return");

        IEnumerable<XElement>? doc_notes = memberDef.Element("detaileddescription")?.Descendants("simplesect").Where(static simplesect => simplesect.Attribute("kind")?.Value == "note");

        return new FunctionDoc() {
            Function = memberDef.Element("name")!.Value,
            Summary = DoxygenParser.GetDocElements(doc_containers),
            Parameters = doc_parameters,
            Return = DoxygenParser.GetDocElements([doc_return]),
            Notes = DoxygenParser.GetDocElements(doc_notes),
        };
        //Return = GetDocElements(memberDef.Element("detaileddescription")
        //?.Descendants("simplesect").FirstOrDefault(simplesect =>
        //  simplesect.Attribute("kind")?.Value == "return")
        //?.Nodes()),
        //Notes = GetDocElements(memberDef.Element("detaileddescription")
        //?.Descendants("simplesect").Where(simplesect =>
        //  simplesect.Attribute("kind")?.Value == "note"))

    }
}