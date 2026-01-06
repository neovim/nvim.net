using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs;

/// <summary>
///     Represents a documentation element. All doxygen documentation elements
///     are of type <see cref="IDoxygenElement/>
/// </summary>
public interface IDoxygenElement {
    List<string> StringContents();
}