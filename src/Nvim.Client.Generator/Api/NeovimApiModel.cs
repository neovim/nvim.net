using System.Collections.Immutable;

namespace Nvim.Client.Generator.Api;

public sealed record NeovimApiModel(
  Compatibility Version,
  ImmutableArray<RpcMethod> Methods,
  ImmutableArray<UiEvent> UiEvents,
  ImmutableArray<HandleType> Extensions
);

public sealed record Compatibility(int ApiLevel, int Compatible);

public sealed record RpcMethod(
  string Name,
  ImmutableArray<RpcParameter> Parameters,
  RpcType ReturnType,
  bool IsMethod,
  int Since,
  int? DeprecatedSince,
  Documentation? Documentation = null
);

public sealed record UiEvent(
  string Name,
  ImmutableArray<RpcParameter> Parameters,
  int Since,
  int? DeprecatedSince
);

public sealed record RpcParameter(string Name, RpcType Type);

public sealed record HandleType(string Name, sbyte Tag, string Prefix);

public enum RpcPrimitive
{
  Value,
  Boolean,
  Integer,
  Float,
  String,
}

public abstract record RpcType
{
  private RpcType() { }

  public sealed record Primitive(RpcPrimitive Kind) : RpcType;

  public sealed record Array(RpcType Element, int? Length = null) : RpcType;

  public sealed record Dictionary(RpcType Key, RpcType Value) : RpcType;

  public sealed record Handle(string Name) : RpcType;

  public sealed record LuaRef : RpcType;

  public sealed record Void : RpcType;
}

public sealed record Documentation(
  ImmutableArray<DocumentationNode> Summary,
  ImmutableDictionary<string, ImmutableArray<DocumentationNode>> Parameters,
  ImmutableArray<DocumentationNode> Returns
);

public abstract record DocumentationNode
{
  private DocumentationNode() { }

  public sealed record Text(string Value) : DocumentationNode;

  public sealed record InlineCode(string Value) : DocumentationNode;

  public sealed record ParameterReference(string Name, bool IsCode = false)
    : DocumentationNode;

  public sealed record FunctionReference(string Name) : DocumentationNode;

  public sealed record Paragraph(ImmutableArray<DocumentationNode> Children)
    : DocumentationNode;

  public sealed record List(
    bool Ordered,
    ImmutableArray<DocumentationNode> Items
  ) : DocumentationNode;
}

public sealed record GenerationDiagnostic(string Code, string Message);

public sealed record GeneratedFile(string RelativePath, string Content);

public sealed record GenerationResult(
  ImmutableArray<GeneratedFile> Files,
  ImmutableArray<GenerationDiagnostic> Diagnostics
);

public interface ISourceEmitter
{
  GenerationResult Emit(NeovimApiModel definition);
}
