using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Nvim.Client.Generator.Api;

namespace Nvim.Client.Generator.CSharp;

/// <summary>
/// Emits C# XML documentation for generated Neovim client methods.
/// </summary>
internal sealed class XmlDocumentationEmitter
{
  private readonly IReadOnlyDictionary<string, string> _methods;

  /// <summary>
  /// Builds the source-to-managed method mapping used by documentation links.
  /// </summary>
  internal XmlDocumentationEmitter(IEnumerable<ManagedMethod> methods) =>
    _methods = methods.ToDictionary(
      method => method.Source.Name,
      method => method.Name,
      StringComparer.Ordinal
    );

  /// <summary>
  /// Emits the full documentation block for a generated method.
  /// </summary>
  internal string ForMethod(ManagedMethod method)
  {
    var sourceMethod = method.Source;
    var xmlDocs = sourceMethod.Documentation;
    var context = new DocumentationContext(
      sourceMethod
        .Parameters.Select(
          (parameter, index) =>
            (parameter.Name, Emitted: method.ParameterNames[index])
        )
        .ToDictionary(
          parameter => parameter.Name,
          parameter => parameter.Emitted,
          StringComparer.Ordinal
        ),
      _methods
    );
    var output = new StringBuilder();

    AddElement(
      output,
      "summary",
      xmlDocs?.Summary ?? [],
      $"Invokes Neovim RPC method <c>{Escape(sourceMethod.Name)}</c>.",
      context
    );

    for (var index = 0; index < sourceMethod.Parameters.Length; index++)
    {
      var parameter = sourceMethod.Parameters[index];
      var name = method.ParameterNames[index];

      AddElement(
        output,
        "param",
        xmlDocs is not null
        && xmlDocs.Parameters.TryGetValue(parameter.Name, out var description)
          ? description
          : [],
        $"The <paramref name=\"{Escape(name)}\" /> argument.",
        context,
        $" name=\"{Escape(name)}\""
      );
    }

    AddElement(
      output,
      "param",
      [],
      "A token that cancels the RPC request.",
      context,
      " name=\"cancellationToken\""
    );

    if (sourceMethod.ReturnType is not RpcType.Void)
      AddElement(
        output,
        "returns",
        xmlDocs?.Returns ?? [],
        "The Neovim RPC result.",
        context
      );

    return output.ToString();
  }

  internal static string Summary(string text) =>
    $"/// <summary>\n/// {text}\n/// </summary>\n";

  private static void AddElement(
    StringBuilder output,
    string methodName,
    ImmutableArray<DocumentationNode> docNodes,
    string fallbackDoc,
    DocumentationContext context,
    string attributes = ""
  )
  {
    output.Append("/// <").Append(methodName).Append(attributes).Append(">\n");
    AddContent(output, docNodes, fallbackDoc, context);
    output.Append("/// </").Append(methodName).Append(">\n");
  }

  private static void AddContent(
    StringBuilder output,
    ImmutableArray<DocumentationNode> nodes,
    string fallback,
    DocumentationContext context
  )
  {
    var lines = nodes.IsDefaultOrEmpty
      ? new[] { fallback }
      : Render(nodes, context).ToArray();
    var count = lines.Length;

    while (count > 0 && lines[count - 1].Length == 0)
      count--;

    for (var index = 0; index < count; index++)
      output
        .Append(lines[index].Length == 0 ? "///" : "/// " + lines[index])
        .Append('\n');
  }

  /// <summary>
  /// Renders inline nodes onto the current line and block nodes separately.
  /// </summary>
  private static IEnumerable<string> Render(
    ImmutableArray<DocumentationNode> nodes,
    DocumentationContext context
  )
  {
    var line = string.Empty;

    foreach (var node in nodes)
    {
      if (RenderInline(node, context) is { } inline)
      {
        AppendInline(ref line, inline);
        continue;
      }

      if (Flush(ref line) is { } prefix)
        yield return prefix;

      switch (node)
      {
        case DocumentationNode.Paragraph paragraph:
          foreach (var rendered in Render(paragraph.Children, context))
            yield return rendered;

          yield return string.Empty;
          break;

        case DocumentationNode.List list:
          foreach (var rendered in RenderList(list, context))
            yield return rendered;
          break;
      }
    }

    if (Flush(ref line) is { } remainder)
      yield return remainder;
  }

  /// <summary>
  /// Renders a documentation node that can appear within a text line.
  /// </summary>
  private static string? RenderInline(
    DocumentationNode node,
    DocumentationContext context
  ) =>
    node switch
    {
      DocumentationNode.Text text => RenderText(text.Value),
      DocumentationNode.InlineCode code => $"<c>{Escape(code.Value)}</c>",
      DocumentationNode.ParameterReference reference when reference.IsCode =>
        $"<c>{ParameterReference(reference.Name, context)}</c>",
      DocumentationNode.ParameterReference reference => ParameterReference(
        reference.Name,
        context
      ),
      DocumentationNode.FunctionReference reference => FunctionReference(
        reference.Name,
        context
      ),
      _ => null,
    };

  /// <summary>
  /// Renders each source list item as one XML list item, including nested lists.
  /// </summary>
  private static IEnumerable<string> RenderList(
    DocumentationNode.List list,
    DocumentationContext context
  )
  {
    if (list.Items.IsEmpty)
      yield break;

    yield return $"""<list type="{(list.Ordered ? "number" : "bullet")}">""";

    foreach (var item in list.Items)
    {
      var children = item is DocumentationNode.Paragraph paragraph
        ? paragraph.Children
        : ImmutableArray.Create(item);
      var lines = Render(children, context).ToArray();
      var count = lines.Length;

      while (count > 0 && lines[count - 1].Length == 0)
        count--;

      if (count == 0)
        continue;

      yield return "<item><description>";

      for (var index = 0; index < count; index++)
        yield return index == 0
          ? NormalizeListLabel(lines[index])
          : lines[index];

      yield return "</description></item>";
    }

    yield return "</list>";
  }

  /// <summary>
  /// Wraps field-like list labels such as <c>buf:</c> in inline code.
  /// </summary>
  private static string NormalizeListLabel(string line) =>
    Regex.Replace(line, @"^([A-Za-z_][A-Za-z0-9_]*)(?=:|\s+\()", "<c>$1</c>");

  private static string RenderText(string text) =>
    Regex.Replace(
      Regex.Replace(Escape(text), @"\|([^|\r\n]+)\|", "<c>$1</c>"),
      @"\[([^\]\r\n]+)\]\([^)]*\)",
      "<c>$1</c>"
    );

  private static string ParameterReference(
    string name,
    DocumentationContext context
  ) =>
    context.Parameters.TryGetValue(name, out var managedName)
      ? $"<paramref name=\"{Escape(managedName)}\" />"
      : $"<c>{Escape(name)}</c>";

  private static string FunctionReference(
    string name,
    DocumentationContext context
  ) =>
    context.Methods.TryGetValue(name, out var managedName)
      ? $"<see cref=\"{Escape(managedName)}\" />"
      : $"<c>{Escape(name)}()</c>";

  /// <summary>
  /// Adds spacing between inline fragments where needed.
  /// </summary>
  private static void AppendInline(ref string line, string value)
  {
    if (
      line.Length > 0
      && value.Length > 0
      && !char.IsWhiteSpace(line[^1])
      && !char.IsWhiteSpace(value[0])
      && !"([{<".Contains(line[^1])
      && !",.;:!?)]}-".Contains(value[0])
    )
      line += " ";

    line += value;
  }

  private static string? Flush(ref string line)
  {
    if (line.Length == 0)
      return null;

    var result = line;
    line = "";
    return result;
  }

  private static string Escape(string value) => SecurityElement.Escape(value)!;

  /// <summary>
  /// Maps source parameter and method names to their emitted names.
  /// </summary>
  private readonly record struct DocumentationContext(
    IReadOnlyDictionary<string, string> Parameters,
    IReadOnlyDictionary<string, string> Methods
  );
}
