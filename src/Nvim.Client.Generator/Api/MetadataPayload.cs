using System.Collections.Generic;
using MessagePack;

namespace Nvim.Client.Generator.Api;

[MessagePackObject(AllowPrivate = true)]
internal sealed record MetadataPayload(
  [property: Key("version")] CompatibilityPayload? Version,
  [property: Key("functions")] RpcMethodPayload?[]? Methods,
  [property: Key("ui_events")] UiEventPayload?[]? UiEvents,
  [property: Key("types")] Dictionary<string, HandleTypePayload?>? HandleTypes
);

[MessagePackObject(AllowPrivate = true)]
internal sealed record CompatibilityPayload(
  [property: Key("api_level")] int Level,
  [property: Key("api_compatible")] int Compatible
);

[MessagePackObject(AllowPrivate = true)]
internal sealed record RpcMethodPayload(
  [property: Key("name")] string? Name,
  [property: Key("parameters")] RpcParameterPayload?[]? Parameters,
  [property: Key("return_type")] string? ReturnType,
  [property: Key("method")] bool Method,
  [property: Key("since")] int Since,
  [property: Key("deprecated_since")] int? DeprecatedSince
);

[MessagePackObject(AllowPrivate = true)]
internal sealed record UiEventPayload(
  [property: Key("name")] string? Name,
  [property: Key("parameters")] RpcParameterPayload?[]? Parameters,
  [property: Key("since")] int Since,
  [property: Key("deprecated_since")] int? DeprecatedSince
);

[MessagePackObject(AllowPrivate = true)]
internal sealed record RpcParameterPayload(
  [property: Key(0)] string? Type,
  [property: Key(1)] string? Name
);

[MessagePackObject(AllowPrivate = true)]
internal sealed record HandleTypePayload(
  [property: Key("id")] int ExtensionTag,
  [property: Key("prefix")] string? Prefix
);
