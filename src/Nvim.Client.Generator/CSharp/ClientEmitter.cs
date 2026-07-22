using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Nvim.Client.Generator.Api;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Nvim.Client.Generator.CSharp;

internal sealed record ManagedMethod(
  RpcMethod Source,
  string Name,
  ImmutableArray<string> ParameterNames
);

internal sealed class ClientEmitter(SyntaxGenerator Syntax)
{
  internal (SyntaxNode Interface, SyntaxNode Impl) Emit(
    IReadOnlyList<ManagedMethod> methods,
    XmlDocumentationEmitter documentation
  )
  {
    var contractMethods = new SyntaxNode[methods.Count];
    var methodDeclarations = new SyntaxNode[methods.Count];

    for (var index = 0; index < methods.Count; index++)
    {
      var method = methods[index];
      var signature = MethodDeclarationFor(method);

      contractMethods[index] = signature.WithXmlDocumentation(
        documentation.ForMethod(method)
      );

      var implementation = Syntax.WithModifiers(
        Syntax.WithAccessibility(signature, Accessibility.Public),
        DeclarationModifiers.Async
      );

      var body = ConvertToExpressionBody(implementation, method);

      methodDeclarations[index] = body.WithXmlDocumentation(
        "/// <inheritdoc />\n"
      );
    }

    return new(
      Syntax.SourceFile(
        [
          "System.Collections.Generic",
          "System.Threading",
          "System.Threading.Tasks",
        ],
        Syntax.WithModifiers(
          Syntax.InterfaceDeclaration(
            nameof(INvimClient),
            accessibility: Accessibility.Public,
            members: contractMethods
          ),
          DeclarationModifiers.Partial
        )
      ),
      Syntax.SourceFile(
        [
          "System.Collections.Generic",
          "System.Linq",
          "System.Threading",
          "System.Threading.Tasks",
        ],
        Syntax.ClassDeclaration(
          nameof(NvimClient),
          accessibility: Accessibility.Public,
          modifiers: DeclarationModifiers.Partial,
          members: methodDeclarations
        )
      )
    );
  }

  private SyntaxNode MethodDeclarationFor(ManagedMethod method) =>
    Syntax.MethodDeclaration(
      method.Name,
      ParametersFor(method).Append(CancellationParameter()),
      returnType: ReturnTypeFor(method.Source),
      accessibility: Accessibility.NotApplicable
    );

  private MethodDeclarationSyntax ConvertToExpressionBody(
    SyntaxNode method,
    ManagedMethod source
  ) =>
    ((MethodDeclarationSyntax)method)
      .WithExpressionBody(
        ArrowExpressionClause(RequestInvocationExpression(source))
      )
      .WithBody(null)
      .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

  private ExpressionSyntax MethodInvocationExpression(ManagedMethod method)
  {
    var arguments = CollectionExpression(
      SeparatedList<CollectionElementSyntax>(
        ParametersAsEncodedArguments(method)
          .Select(argument =>
            (CollectionElementSyntax)ExpressionElement(argument)
          )
      )
    );

    return (ExpressionSyntax)
      Syntax.InvocationExpression(
        Syntax.IdentifierName(nameof(NvimClient.RequestAsync)),
        Syntax.LiteralExpression(method.Source.Name),
        arguments,
        Syntax.IdentifierName("cancellationToken")
      );
  }

  private ExpressionSyntax RequestInvocationExpression(ManagedMethod method)
  {
    var awaited = AwaitExpression(MethodInvocationExpression(method));

    return method.Source.ReturnType is RpcType.Void
      ? awaited
      : awaited.DecodeAs(method.Source.ReturnType);
  }

  private IEnumerable<ExpressionSyntax> ParametersAsEncodedArguments(
    ManagedMethod method
  ) =>
    method.Source.Parameters.Select(
      (parameter, index) =>
        SyntaxFactory
          .IdentifierName(method.ParameterNames[index])
          .EncodeAs(parameter.Type)
    );

  private IEnumerable<SyntaxNode> ParametersFor(ManagedMethod method) =>
    method.Source.Parameters.Select(
      (parameter, index) =>
        Syntax.ParameterDeclaration(
          method.ParameterNames[index],
          parameter.Type.ToManagedType()
        )
    );

  private SyntaxNode CancellationParameter() =>
    Syntax.ParameterDeclaration(
      "cancellationToken",
      IdentifierName(nameof(CancellationToken)),
      DefaultExpression(IdentifierName(nameof(CancellationToken)))
    );

  private static TypeSyntax ReturnTypeFor(RpcMethod method) =>
    method.ReturnType is RpcType.Void
      ? IdentifierName(nameof(Task))
      : GenericName(Identifier(nameof(Task)))
        .WithTypeArgumentList(
          TypeArgumentList(
            SingletonSeparatedList(method.ReturnType.ToManagedType())
          )
        );
}
