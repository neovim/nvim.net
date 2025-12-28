using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NvimClient.APIGenerator.Docs;

/// <summary>
/// Documentation that documents a parameter
/// </summary>
public class ParameterDoc {
    /// <summary>
    /// The parameter name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The parameter description
    /// </summary>
    public required IEnumerable<IDocElement> Description { get; set; }

    public static ParameterDoc FromXElement(XElement param) {
        IEnumerable<XNode> parameter_nodes = param.Descendants("parameterdescription").First().Nodes();
        return new ParameterDoc {
            Name = param.Descendants("parametername").First().Value,
            Description = DoxygenParser.GetDocElements(parameter_nodes)
        };
    }
}