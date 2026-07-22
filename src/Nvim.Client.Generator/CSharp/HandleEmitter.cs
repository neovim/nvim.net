using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using Nvim.Client.Generator.Api;

namespace Nvim.Client.Generator.CSharp;

internal sealed class HandleEmitter(SyntaxGenerator syntax)
{
  internal SyntaxNode Emit(IEnumerable<HandleType> handles) =>
    syntax.SourceFile(["System"], handles.Select(HandleDeclaration).ToArray());

  private SyntaxNode HandleDeclaration(HandleType handle)
  {
    var name = ManagedNames.PascalIdentifierFor(handle.Name);
    var extension = syntax.IdentifierName("extension");
    var extensionType = syntax.IdentifierName(nameof(NvimExtension));

    return syntax
      .StructDeclaration(
        name,
        accessibility: Accessibility.Public,
        modifiers: DeclarationModifiers.ReadOnly,
        members:
        [
          syntax
            .PropertyDeclaration(
              "Extension",
              extensionType,
              Accessibility.Public,
              DeclarationModifiers.ReadOnly
            )
            .WithSummary("Gets the underlying MessagePack extension value."),
          syntax
            .ConstructorDeclaration(
              name,
              [syntax.ParameterDeclaration("extension", extensionType)],
              Accessibility.Public,
              statements:
              [
                syntax.ExpressionStatement(
                  syntax.InvocationExpression(
                    syntax.MemberAccessExpression(
                      syntax.IdentifierName(nameof(ArgumentNullException)),
                      nameof(ArgumentNullException.ThrowIfNull)
                    ),
                    extension
                  )
                ),
                syntax.IfStatement(
                  syntax.ValueNotEqualsExpression(
                    syntax.MemberAccessExpression(
                      extension,
                      nameof(NvimExtension.Tag)
                    ),
                    syntax.LiteralExpression(handle.Tag)
                  ),
                  [
                    syntax.ThrowStatement(
                      syntax.ObjectCreationExpression(
                        syntax.IdentifierName(nameof(ArgumentException)),
                        syntax.LiteralExpression(
                          $"Expected {handle.Name} extension tag."
                        )
                      )
                    ),
                  ]
                ),
                syntax.AssignmentStatement(
                  syntax.IdentifierName("Extension"),
                  extension
                ),
              ]
            )
            .WithSummary(
              "Creates a validated handle from a MessagePack extension value."
            ),
        ]
      )
      .WithSummary($"Represents Neovim {handle.Name} handle values.");
  }
}
