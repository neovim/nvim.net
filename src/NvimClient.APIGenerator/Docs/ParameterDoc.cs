using System.Collections.Generic;

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
}