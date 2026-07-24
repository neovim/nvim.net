using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Nvim.Client.Generator.Api;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Nvim.Client.Generator.CSharp;

internal sealed class UiEventEmitter(SyntaxGenerator syntax)
{
  internal (SyntaxNode EventTypes, SyntaxNode Factory) Emit(
    IEnumerable<UiEvent> events
  )
  {
    var currentEvents = events.ToArray();

    return new(
      syntax.SourceFile(
        ["System.Collections.Generic"],
        currentEvents.Select(EventDeclaration).ToArray()
      ),
      syntax
        .SourceFile(
          ["System.Collections.Generic"],
          syntax.ClassDeclaration(
            nameof(NvimUiEventFactory),
            accessibility: Accessibility.Internal,
            modifiers: DeclarationModifiers.Static
              | DeclarationModifiers.Partial,
            members:
            [
              DecodeMethod,
              DispatchMethodFor(currentEvents),
              .. currentEvents.Select(CreatorMethodFor),
            ]
          )
        )
        .WithLeadingTrivia(ParseLeadingTrivia("#nullable enable\n"))
    );
  }

  private static SyntaxNode EventDeclaration(UiEvent @event) =>
    RecordDeclaration(
        SyntaxKind.RecordDeclaration,
        Token(SyntaxKind.RecordKeyword),
        ManagedNames.PascalIdentifierFor(@event.Name)
      )
      .AddModifiers(
        Token(SyntaxKind.PublicKeyword),
        Token(SyntaxKind.SealedKeyword)
      )
      .WithParameterList(
        ParameterList(
          SeparatedList(
            @event.Parameters.Select(parameter =>
              Parameter(
                  Identifier(ManagedNames.PascalIdentifierFor(parameter.Name))
                )
                .WithType(parameter.Type.ToManagedType())
            )
          )
        )
      )
      .WithBaseList(
        BaseList(
          SingletonSeparatedList<BaseTypeSyntax>(
            SimpleBaseType(IdentifierName(nameof(NvimUiEvent)))
          )
        )
      )
      .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
      .WithXmlDocumentation(
        $"""
        /// <summary>
        /// Represents the <c>{@event.Name}</c> UI redraw event.
        /// <see href="https://neovim.io/doc/user/api-ui-events.html">
        /// See the Neovim UI event documentation for more.
        /// </see>
        /// </summary>

        """
      );

  private SyntaxNode DecodeMethod =>
    syntax.MethodDeclaration(
      nameof(UiEventDecoder.Decode),
      [
        syntax.ParameterDeclaration(
          "batch",
          ReadOnlyListOf(IdentifierName(nameof(NvimValue)))
        ),
        syntax.ParameterDeclaration(
          "events",
          ReadOnlyListOf(IdentifierName(nameof(NvimUiEvent))),
          refKind: RefKind.Ref
        ),
      ],
      returnType: PredefinedType(Token(SyntaxKind.VoidKeyword)),
      accessibility: Accessibility.NotApplicable,
      modifiers: DeclarationModifiers.Static | DeclarationModifiers.Partial,
      statements:
      [
        syntax.AssignmentStatement(
          IdentifierName("events"),
          syntax.InvocationExpression(
            syntax.MemberAccessExpression(
              IdentifierName(nameof(UiEventDecoder)),
              nameof(UiEventDecoder.Decode)
            ),
            IdentifierName("batch"),
            IdentifierName("Create")
          )
        ),
      ]
    );

  private SyntaxNode DispatchMethodFor(IReadOnlyList<UiEvent> events) =>
    (
      (MethodDeclarationSyntax)
        syntax.MethodDeclaration(
          "Create",
          [
            syntax.ParameterDeclaration(
              "name",
              PredefinedType(Token(SyntaxKind.StringKeyword))
            ),
            syntax.ParameterDeclaration(
              "values",
              ReadOnlyListOf(IdentifierName(nameof(NvimValue)))
            ),
          ],
          returnType: NullableType(IdentifierName(nameof(NvimUiEvent))),
          accessibility: Accessibility.Private,
          modifiers: DeclarationModifiers.Static
        )
    )
      .WithExpressionBody(ArrowExpressionClause(DispatchExpression(events)))
      .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
      .WithBody(null);

  private SwitchExpressionSyntax DispatchExpression(
    IEnumerable<UiEvent> events
  ) =>
    SwitchExpression(IdentifierName("name"))
      .WithArms(
        SeparatedList(
          events
            .Select(@event =>
              SwitchExpressionArm(
                ConstantPattern(
                  LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal(@event.Name)
                  )
                ),
                (ExpressionSyntax)
                  syntax.InvocationExpression(
                    IdentifierName(
                      "Create" + ManagedNames.PascalIdentifierFor(@event.Name)
                    ),
                    IdentifierName("values")
                  )
              )
            )
            .Append(
              SwitchExpressionArm(
                DiscardPattern(),
                LiteralExpression(SyntaxKind.NullLiteralExpression)
              )
            )
        )
      );

  private SyntaxNode CreatorMethodFor(UiEvent @event)
  {
    var name = ManagedNames.PascalIdentifierFor(@event.Name);
    var values = IdentifierName("values");

    return syntax.MethodDeclaration(
      "Create" + name,
      [
        syntax.ParameterDeclaration(
          "values",
          ReadOnlyListOf(IdentifierName(nameof(NvimValue)))
        ),
      ],
      returnType: IdentifierName(name),
      accessibility: Accessibility.Private,
      modifiers: DeclarationModifiers.Static,
      statements:
      [
        syntax.ExpressionStatement(
          syntax.InvocationExpression(
            syntax.MemberAccessExpression(
              IdentifierName(nameof(UiEventDecoder)),
              nameof(UiEventDecoder.RequireArity)
            ),
            values,
            syntax.LiteralExpression(@event.Parameters.Length)
          )
        ),
        syntax.ReturnStatement(
          syntax.ObjectCreationExpression(
            IdentifierName(name),
            @event
              .Parameters.Select(
                (parameter, index) =>
                  DecodeExpression(
                    parameter.Type,
                    (ExpressionSyntax)
                      syntax.ElementAccessExpression(
                        values,
                        syntax.LiteralExpression(index)
                      )
                  )
              )
              .ToArray()
          )
        ),
      ]
    );
  }

  private ExpressionSyntax DecodeExpression(
    RpcType type,
    ExpressionSyntax value
  ) =>
    type switch
    {
      RpcType.Primitive => RequireExpression(type.ToManagedType(), value),
      RpcType.Handle => (ExpressionSyntax)
        syntax.ObjectCreationExpression(
          type.ToManagedType(),
          RequireExpression(IdentifierName(nameof(NvimExtension)), value)
        ),
      RpcType.Array array => (ExpressionSyntax)
        syntax.InvocationExpression(
          syntax.MemberAccessExpression(
            IdentifierName(nameof(UiEventDecoder)),
            nameof(UiEventDecoder.RequireArray)
          ),
          value,
          SimpleLambdaExpression(
            Parameter(Identifier("item")),
            DecodeExpression(array.Element, IdentifierName("item"))
          )
        ),
      RpcType.Dictionary => (ExpressionSyntax)
        syntax.InvocationExpression(
          syntax.MemberAccessExpression(
            IdentifierName(nameof(UiEventDecoder)),
            nameof(UiEventDecoder.RequireMap)
          ),
          value
        ),
      _ => RequireExpression(IdentifierName(nameof(NvimValue)), value),
    };

  private ExpressionSyntax RequireExpression(
    TypeSyntax type,
    ExpressionSyntax value
  ) =>
    (ExpressionSyntax)
      syntax.InvocationExpression(
        syntax.MemberAccessExpression(
          IdentifierName(nameof(UiEventDecoder)),
          syntax.GenericName(nameof(UiEventDecoder.Require), type)
        ),
        value
      );

  private static TypeSyntax ReadOnlyListOf(TypeSyntax element) =>
    GenericName(Identifier(nameof(IReadOnlyList<NvimValue>)))
      .WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList(element)));
}
