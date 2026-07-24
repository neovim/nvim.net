using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Nvim.Client.Generator.Api;

/// <summary>
/// Applies Doxygen documentation to Neovim API definitions.
/// </summary>
public static class DoxygenDocumentation
{
  private static readonly Regex ReferencePattern = new(
    @"\|\{(?<codedParameter>[A-Za-z_][A-Za-z0-9_]*)\}\|"
      + @"|\{(?<parameter>[A-Za-z_][A-Za-z0-9_]*)\}"
      + @"|\|(?<function>[A-Za-z_][A-Za-z0-9_]*)\(\)\|"
      + @"|\[(?<function>[A-Za-z_][A-Za-z0-9_]*)\(\)\]",
    RegexOptions.CultureInvariant
  );

  /// <summary>
  /// Adds documentation from a generated Doxygen index to matching functions.
  /// </summary>
  public static NeovimApiModel Apply(
    NeovimApiModel definition,
    string indexPath
  )
  {
    var documentation = ReadMemberDefinitions(indexPath)
      .Where(member => (string?)member.Attribute("kind") == "function")
      .Select(member =>
        (
          Name: member.Element("name")!.Value,
          Documentation: ReadDocumentation(member)
        )
      )
      .GroupBy(item => item.Name, StringComparer.Ordinal)
      .ToImmutableDictionary(
        group => group.Key,
        group =>
          group
            .MaxBy(item => DocumentationScore(item.Documentation))!
            .Documentation,
        StringComparer.Ordinal
      );

    return definition with
    {
      Methods = definition
        .Methods.Select(function =>
          documentation.TryGetValue(function.Name, out var value)
            ? function with
            {
              Documentation = value,
            }
            : function
        )
        .ToImmutableArray(),
    };
  }

  /// <summary>
  /// Reads all compound files referenced by a Doxygen index.
  /// </summary>
  private static IEnumerable<XElement> ReadMemberDefinitions(string indexPath)
  {
    var root = XDocument.Load(indexPath).Root!;
    var directory = Path.GetDirectoryName(indexPath)!;

    return root.Elements("compound")
      .Select(compound =>
        Path.Combine(directory, compound.Attribute("refid")!.Value + ".xml")
      )
      .SelectMany(file => XDocument.Load(file).Descendants("memberdef"));
  }

  private static int DocumentationScore(Documentation documentation) =>
    documentation.Summary.Length * 100
    + documentation.Parameters.Count * 10
    + documentation.Returns.Length;

  private static Documentation ReadDocumentation(XElement member)
  {
    var summary = ReadDescription(member.Element("briefdescription"));
    var detailed = member.Element("detaileddescription");

    if (summary.IsDefaultOrEmpty)
      summary = ReadDetailedSummary(detailed);

    var parameters = (detailed?.Descendants("parameteritem") ?? [])
      .Select(item =>
        (
          Names: item.Descendants("parametername")
            .Select(name => name.Value.Trim())
            .Where(name => !string.IsNullOrWhiteSpace(name)),
          Description: ReadDescription(item.Element("parameterdescription"))
        )
      )
      .Where(item => !item.Description.IsDefaultOrEmpty)
      .SelectMany(item =>
        item.Names.Select(name => (Name: name, item.Description))
      )
      .GroupBy(item => item.Name, StringComparer.Ordinal)
      .ToImmutableDictionary(
        group => group.Key,
        group => group.First().Description,
        StringComparer.Ordinal
      );

    var returns = ReadDescription(
      detailed
        ?.Descendants("simplesect")
        .FirstOrDefault(section =>
          (string?)section.Attribute("kind") == "return"
        )
    );

    return new Documentation(summary, parameters, returns);
  }

  private static ImmutableArray<DocumentationNode> ReadDetailedSummary(
    XElement? detailed
  ) =>
    ReadParagraphs(
      detailed
        ?.Elements("para")
        .Select(paragraph =>
          paragraph
            .Nodes()
            .Where(node =>
              node
                is not XElement
                {
                  Name.LocalName: "parameterlist" or "simplesect",
                }
            )
        )
        ?? []
    );

  private static ImmutableArray<DocumentationNode> ReadDescription(
    XElement? description
  ) =>
    ReadParagraphs(
      description?.Elements("para").Select(paragraph => paragraph.Nodes()) ?? []
    );

  private static ImmutableArray<DocumentationNode> ReadParagraphs(
    IEnumerable<IEnumerable<XNode>> paragraphs
  )
  {
    var parsed = paragraphs
      .Select(ReadNodes)
      .Where(paragraph => !paragraph.IsDefaultOrEmpty)
      .ToImmutableArray();

    return parsed is [var paragraph]
      ? paragraph
      : parsed
        .Select(paragraph =>
          (DocumentationNode)new DocumentationNode.Paragraph(paragraph)
        )
        .ToImmutableArray();
  }

  /// <summary>
  /// Converts Doxygen XML nodes into the documentation AST.
  /// </summary>
  private static ImmutableArray<DocumentationNode> ReadNodes(
    IEnumerable<XNode> nodes
  )
  {
    var source = nodes.ToArray();

    return source
      .SelectMany(
        (node, index) =>
          node switch
          {
            XText text when !string.IsNullOrWhiteSpace(text.Value) => ReadText(
              source,
              index,
              text
            ),
            XElement { Name.LocalName: "computeroutput" } code =>
            [
              new DocumentationNode.InlineCode(
                NormalizeCodeLiteral(Normalize(code.Value))
              ),
            ],
            XElement { Name.LocalName: "ref" }
              when BracketedFunctionName(source, index) is { } function =>
            [
              new DocumentationNode.FunctionReference(function),
            ],
            XElement { Name.LocalName: "para" } paragraph =>
            [
              new DocumentationNode.Paragraph(ReadNodes(paragraph.Nodes())),
            ],
            XElement { Name.LocalName: "itemizedlist" } list =>
            [
              ReadList(list, ordered: false),
            ],
            XElement { Name.LocalName: "orderedlist" } list =>
            [
              ReadList(list, ordered: true),
            ],
            XElement element => ReadNodes(element.Nodes()),
            _ => [],
          }
      )
      .ToImmutableArray();
  }

  private static ImmutableArray<DocumentationNode> ReadText(
    XNode[] source,
    int index,
    XText text
  )
  {
    var value = Normalize(text.Value);

    if (BracketedFunctionName(source, index + 1) is not null)
      value = value[..^1].TrimEnd();

    if (BracketedFunctionName(source, index - 1) is not null)
      value = value[1..].TrimStart();

    return string.IsNullOrWhiteSpace(value) ? [] : ParseText(value);
  }

  private static string? BracketedFunctionName(XNode[] source, int index)
  {
    if (
      index <= 0
      || index + 1 >= source.Length
      || source[index] is not XElement { Name.LocalName: "ref" } reference
      || source[index - 1] is not XText before
      || source[index + 1] is not XText after
      || !before.Value.TrimEnd().EndsWith("[", StringComparison.Ordinal)
      || !after.Value.TrimStart().StartsWith("]", StringComparison.Ordinal)
    )
      return null;

    var value = Normalize(reference.Value);

    return value.EndsWith("()", StringComparison.Ordinal) ? value[..^2] : null;
  }

  private static DocumentationNode.List ReadList(XElement list, bool ordered) =>
    new(
      ordered,
      list.Elements("listitem")
        .Select(ReadDescription)
        .Where(item => !item.IsDefaultOrEmpty)
        .Select(item =>
          (DocumentationNode)new DocumentationNode.Paragraph(item)
        )
        .ToImmutableArray()
    );

  /// <summary>
  /// Splits text into plain text, parameter references and function references.
  /// </summary>
  private static ImmutableArray<DocumentationNode> ParseText(string text)
  {
    var result = ImmutableArray.CreateBuilder<DocumentationNode>();
    var offset = 0;

    foreach (Match match in ReferencePattern.Matches(text))
    {
      if (match.Index > offset)
        result.Add(new DocumentationNode.Text(text[offset..match.Index]));

      var codedParameter = match.Groups["codedParameter"];
      var parameter = match.Groups["parameter"];

      result.Add(
        codedParameter.Success
          ? new DocumentationNode.ParameterReference(codedParameter.Value, true)
        : parameter.Success
          ? new DocumentationNode.ParameterReference(parameter.Value, false)
        : new DocumentationNode.FunctionReference(
          match.Groups["function"].Value
        )
      );

      offset = match.Index + match.Length;
    }

    if (offset < text.Length)
      result.Add(new DocumentationNode.Text(text[offset..]));

    return result.ToImmutable();
  }

  private static string NormalizeCodeLiteral(string value)
  {
    value = Regex.Replace(value, @"\|([^|\r\n]+)\|", "$1");
    value = Regex.Replace(value, @"\[([^\]\r\n]+)\]\([^)]*\)", "$1");

    return Regex.Replace(value, @"\{([A-Za-z_][A-Za-z0-9_]*)\}", "$1");
  }

  private static string Normalize(string value) =>
    string.Join(
      " ",
      value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
    );
}
