using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Nvim.Client.Generator.Api;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Nvim.Client.Generator.CSharp;

internal static class RpcTypeMapping
{
  internal static TypeSyntax ToManagedType(this RpcType type) =>
    type switch
    {
      RpcType.Primitive primitive => ManagedPrimitiveType(primitive.Kind),
      RpcType.Array array => GenericType(
        nameof(IReadOnlyList<NvimValue>),
        array.Element.ToManagedType()
      ),
      RpcType.Dictionary => GenericType(
        nameof(IReadOnlyList<NvimValue>),
        IdentifierName(nameof(NvimMapEntry))
      ),
      RpcType.Handle handle => IdentifierName(
        ManagedNames.PascalIdentifierFor(handle.Name)
      ),
      _ => throw Unsupported(type),
    };

  internal static ExpressionSyntax EncodeAs(
    this ExpressionSyntax value,
    RpcType type
  ) =>
    type switch
    {
      RpcType.Primitive => value,
      RpcType.Array array => New(
        IdentifierName(nameof(NvimArray)),
        Select(value, item => item.EncodeAs(array.Element))
      ),
      RpcType.Dictionary => New(IdentifierName(nameof(NvimMap)), value),
      RpcType.Handle => Member(value, "Extension"),
      _ => throw Unsupported(type),
    };

  internal static ExpressionSyntax DecodeAs(
    this ExpressionSyntax value,
    RpcType type
  ) =>
    type switch
    {
      RpcType.Primitive => CastExpression(type.ToManagedType(), value),
      RpcType.Array array => Call(
        Select(
          Member(
            CastExpression(IdentifierName(nameof(NvimArray)), value),
            nameof(NvimArray.Items)
          ),
          item => item.DecodeAs(array.Element)
        ),
        nameof(Enumerable.ToArray)
      ),
      RpcType.Dictionary => Member(
        CastExpression(IdentifierName(nameof(NvimMap)), value),
        nameof(NvimMap.Entries)
      ),
      RpcType.Handle => New(
        type.ToManagedType(),
        CastExpression(IdentifierName(nameof(NvimExtension)), value)
      ),
      _ => throw Unsupported(type),
    };

  internal static bool ContainsLuaRef(this RpcType type) =>
    type switch
    {
      RpcType.LuaRef => true,
      RpcType.Array array => array.Element.ContainsLuaRef(),
      RpcType.Dictionary dictionary => dictionary.Key.ContainsLuaRef()
        || dictionary.Value.ContainsLuaRef(),
      _ => false,
    };

  private static GenericNameSyntax GenericType(
    string name,
    TypeSyntax argument
  ) =>
    GenericName(Identifier(name))
      .WithTypeArgumentList(TypeArgumentList(SingletonSeparatedList(argument)));

  private static ObjectCreationExpressionSyntax New(
    TypeSyntax type,
    params ExpressionSyntax[] arguments
  ) =>
    ObjectCreationExpression(type)
      .WithArgumentList(
        ArgumentList(SeparatedList(arguments.Select(Argument)))
      );

  private static MemberAccessExpressionSyntax Member(
    ExpressionSyntax receiver,
    string name
  ) =>
    MemberAccessExpression(
      SyntaxKind.SimpleMemberAccessExpression,
      receiver is CastExpressionSyntax
        ? ParenthesizedExpression(receiver)
        : receiver,
      IdentifierName(name)
    );

  private static InvocationExpressionSyntax Call(
    ExpressionSyntax receiver,
    string method,
    params ExpressionSyntax[] arguments
  ) =>
    InvocationExpression(
      Member(receiver, method),
      ArgumentList(SeparatedList(arguments.Select(Argument)))
    );

  private static InvocationExpressionSyntax Select(
    ExpressionSyntax source,
    Func<ExpressionSyntax, ExpressionSyntax> selector
  )
  {
    var item = IdentifierName("item");

    return Call(
      source,
      nameof(Enumerable.Select),
      SimpleLambdaExpression(Parameter(item.Identifier), selector(item))
    );
  }

  private static TypeSyntax ManagedPrimitiveType(RpcPrimitive primitive) =>
    IdentifierName(
      primitive switch
      {
        RpcPrimitive.Value => nameof(NvimValue),
        RpcPrimitive.Boolean => nameof(NvimBoolean),
        RpcPrimitive.Integer => nameof(NvimInteger),
        RpcPrimitive.Float => nameof(NvimFloat),
        RpcPrimitive.String => nameof(NvimString),
        _ => throw new ArgumentOutOfRangeException(nameof(primitive)),
      }
    );

  private static InvalidOperationException Unsupported(RpcType type) =>
    new($"Unsupported RPC type: {type}.");
}
