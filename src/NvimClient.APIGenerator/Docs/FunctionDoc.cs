using System.Collections.Generic;

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
    public required IEnumerable<ParameterDoc> Parameters { get; set; }

    /// <summary>
    /// Documentation of the return value of the function
    /// </summary>
    public required IEnumerable<IDocElement> Return { get; set; }

    /// <summary>
    /// Additional notes of the function documentation
    /// </summary>
    public required IEnumerable<IDocElement> Notes { get; set; }
}