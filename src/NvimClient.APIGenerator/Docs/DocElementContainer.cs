using System.Collections.Generic;

namespace NvimClient.APIGenerator.Docs;

/// <summary>
/// A container of <see cref="IDoxygenElement"/>
/// </summary>
internal abstract class DocElementContainer : IDoxygenElement {
    /// <summary>
    /// The items contained within the Container.
    /// </summary>
    public IEnumerable<IDoxygenElement> Children { get; }


    /// <summary>
    /// Construct the elements that contain the items in the element
    /// </summary>
    protected DocElementContainer(IEnumerable<IDoxygenElement> children) {
        Children = children;
    }

    public List<string> StringContents() {
        throw new System.NotImplementedException();
    }
}