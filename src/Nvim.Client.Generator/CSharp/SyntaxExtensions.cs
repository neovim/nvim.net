using System.Collections.Generic;
using System.Linq;
using CSharpier.Core;
using CSharpier.Core.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Nvim.Client.Generator.CSharp;

internal static class SyntaxExtensions
{
  internal static SyntaxNode SourceFile(
    this SyntaxGenerator syntax,
    IEnumerable<string> imports,
    params SyntaxNode[] declarations
  ) =>
    syntax.CompilationUnit(
      imports
        .Select(syntax.NamespaceImportDeclaration)
        .Append(syntax.NamespaceDeclaration("Nvim.Client", declarations))
    );

  internal static SyntaxNode WithXmlDocumentation(
    this SyntaxNode node,
    string documentation
  ) => node.WithLeadingTrivia(ParseLeadingTrivia(documentation));

  internal static SyntaxNode WithSummary(
    this SyntaxNode node,
    string summary
  ) => node.WithXmlDocumentation(XmlDocumentationEmitter.Summary(summary));

  internal static string Format(this SyntaxNode unit) =>
    CSharpFormatter
      .FormatAsync(
        unit.NormalizeWhitespace(" ").ToFullString(),
        new CodeFormatterOptions
        {
          EndOfLine = CSharpier.Core.EndOfLine.LF,
          IndentSize = 2,
          Width = 80,
        }
      )
      .GetAwaiter()
      .GetResult()
      .Code;
}
