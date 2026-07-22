using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using MessagePack;

namespace Nvim.Client.Generator.Api;

/// <summary>
/// Reads the MessagePack payload emitted by <c>nvim --api-info</c>.
/// </summary>
public static partial class MetadataReader
{
  private static readonly MessagePackSerializerOptions SerializerOptions =
    MessagePackSerializerOptions.Standard.WithSecurity(
      MessagePackSecurity.UntrustedData
    );

  [StringSyntax(StringSyntaxAttribute.Regex)]
  private const string GenericArgumentsPattern =
    @"\A(?:\s*(?<argument>(?:[^(),]+|(?<open>\()|(?<-open>\))|(?(open),|(?!)))+)(?(open)(?!))\s*(?:,(?=\s*\S)|(?=\z)))+\z";

  [GeneratedRegex(
    GenericArgumentsPattern,
    RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace
  )]
  private static partial Regex GenericArgumentsRegex();

  /// <summary>
  /// Reads Neovim API metadata into the generator model.
  /// </summary>
  public static NeovimApiModel Read(ReadOnlyMemory<byte> metadata)
  {
    var wire = MessagePackSerializer.Deserialize<MetadataPayload>(
      metadata,
      SerializerOptions
    );

    if (
      wire
      is not {
        Version: { } version,
        Methods: { } functions,
        UiEvents: { } events,
        HandleTypes: { } extensions,
      }
    )
      throw Invalid("root", wire?.ToString() ?? "null");

    // Neovim emits non-null entries and required fields. Missing entries are
    // dropped.
    return new NeovimApiModel(
      new Compatibility(version.Level, version.Compatible),
      functions
        .OfType<RpcMethodPayload>()
        .Select(MapFunction)
        .ToImmutableArray(),
      events.OfType<UiEventPayload>().Select(MapEvent).ToImmutableArray(),
      extensions
        .Where(static extension => extension.Value is not null)
        .Select(static extension =>
          MapExtension(extension.Key, extension.Value!)
        )
        .ToImmutableArray()
    );
  }

  private static RpcMethod MapFunction(RpcMethodPayload function) =>
    new(
      function.Name!,
      MapParameters(function.Parameters!),
      ParseType(function.ReturnType!),
      function.Method,
      function.Since,
      function.DeprecatedSince
    );

  private static UiEvent MapEvent(UiEventPayload apiEvent) =>
    new(
      apiEvent.Name!,
      MapParameters(apiEvent.Parameters!),
      apiEvent.Since,
      apiEvent.DeprecatedSince
    );

  private static HandleType MapExtension(
    string name,
    HandleTypePayload extension
  ) => new(name, checked((sbyte)extension.ExtensionTag), extension.Prefix!);

  private static ImmutableArray<RpcParameter> MapParameters(
    RpcParameterPayload?[] parameters
  ) =>
    parameters
      .OfType<RpcParameterPayload>()
      .Select(MapParameter)
      .ToImmutableArray();

  private static RpcParameter MapParameter(RpcParameterPayload parameter) =>
    new(parameter.Name!, ParseType(parameter.Type!));

  /// <summary>
  /// Maps a Neovim type name to the generator type model.
  /// Unknown names are kept as extension types.
  /// </summary>
  public static RpcType ParseType(string name) =>
    name switch
    {
      "void" => new RpcType.Void(),
      "Boolean" => new RpcType.Primitive(RpcPrimitive.Boolean),
      "Integer" => new RpcType.Primitive(RpcPrimitive.Integer),
      "Float" => new RpcType.Primitive(RpcPrimitive.Float),
      "String" => new RpcType.Primitive(RpcPrimitive.String),
      "Object" => new RpcType.Primitive(RpcPrimitive.Value),
      "LuaRef" => new RpcType.LuaRef(),
      "Array" => new RpcType.Array(new RpcType.Primitive(RpcPrimitive.Value)),
      "Dictionary" or "Dict" => new RpcType.Dictionary(
        new RpcType.Primitive(RpcPrimitive.Value),
        new RpcType.Primitive(RpcPrimitive.Value)
      ),
      _ => ParseConstructedOrExtension(name),
    };

  private static RpcType ParseConstructedOrExtension(string name)
  {
    var opening = name.IndexOf('(');

    if (opening <= 0 || !name.EndsWith(')'))
      return new RpcType.Handle(name);

    var constructor = name[..opening];
    var content = name[(opening + 1)..^1];

    return constructor switch
    {
      "ArrayOf" => ParseArray(ParseArguments(content)),
      "DictionaryOf" => ParseDictionary(ParseArguments(content)),
      _ => new RpcType.Handle(name),
    };
  }

  private static RpcType ParseArray(string[] arguments) =>
    arguments switch
    {
      [var element] => new RpcType.Array(ParseType(element)),
      [var element, var size]
        when int.TryParse(size, out var length) && length >= 0 =>
        new RpcType.Array(ParseType(element), length),
      _ => throw Invalid("array type", $"({string.Join(",", arguments)})"),
    };

  private static RpcType ParseDictionary(string[] arguments) =>
    arguments is [var key, var value]
      ? new RpcType.Dictionary(ParseType(key), ParseType(value))
      : throw Invalid("dictionary type", $"({string.Join(",", arguments)})");

  /// <summary>
  /// Maps args like:
  /// <list type="table">
  /// <listheader>
  /// <term>Input</term>
  /// <description>Arguments</description>
  /// </listheader>
  /// <item>
  /// <term><c>Integer</c></term>
  /// <description><c>["Integer"]</c></description>
  /// </item>
  /// <item>
  /// <term><c>Integer, 4</c></term>
  /// <description><c>["Integer", "4"]</c></description>
  /// </item>
  /// <item>
  /// <term><c>ArrayOf(Integer), 4</c></term>
  /// <description><c>["ArrayOf(Integer)", "4"]</c></description>
  /// </item>
  /// <item>
  /// <term><c>DictionaryOf(String, ArrayOf(Integer))</c></term>
  /// <description>
  /// <c>["DictionaryOf(String, ArrayOf(Integer))"]</c>
  /// </description>
  /// </item>
  /// <item>
  /// <term><c>ArrayOf(DictionaryOf(String, ArrayOf(Integer)))</c></term>
  /// <description>
  /// <c>["ArrayOf(DictionaryOf(String, ArrayOf(Integer)))"]</c>
  /// </description>
  /// </item>
  /// </list>
  /// </summary>
  private static string[] ParseArguments(string content)
  {
    var match = GenericArgumentsRegex().Match(content);

    if (!match.Success)
      throw Invalid("type", content);

    return match
      .Groups["argument"]
      .Captures.Cast<Capture>()
      .Select(capture => RequiredArgument(capture.Value))
      .ToArray();
  }

  private static string RequiredArgument(string value) =>
    !string.IsNullOrWhiteSpace(value)
      ? value.Trim()
      : throw Invalid("type", value);

  private static InvalidOperationException Invalid(
    string element,
    string content
  ) => new($"Invalid Neovim API metadata {element}, {content}.");
}
