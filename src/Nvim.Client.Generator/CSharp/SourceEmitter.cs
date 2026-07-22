using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using Nvim.Client.Generator.Api;

namespace Nvim.Client.Generator.CSharp;

public sealed class SourceEmitter : ISourceEmitter
{
  public GenerationResult Emit(NeovimApiModel model)
  {
    var diagnostics = ImmutableArray.CreateBuilder<GenerationDiagnostic>();
    var names = new HashSet<string>(StringComparer.Ordinal);
    var methods = ImmutableArray.CreateBuilder<ManagedMethod>();

    foreach (
      var method in model.Methods.Where(static method =>
        method.DeprecatedSince is null
      )
    )
    {
      if (string.IsNullOrWhiteSpace(method.Name))
      {
        diagnostics.Add(new("NVIMGEN003", "Function metadata has no name."));
        continue;
      }

      if (
        method.ReturnType.ContainsLuaRef()
        || method.Parameters.Any(parameter => parameter.Type.ContainsLuaRef())
      )
      {
        diagnostics.Add(
          new("NVIMGEN001", $"Skipped {method.Name}: LuaRef is not supported.")
        );
        continue;
      }

      var name = $"{ManagedNames.MethodNameFor(method.Name)}Async";
      if (!names.Add(name))
      {
        diagnostics.Add(
          new("NVIMGEN002", $"Generated member collision: {name}.")
        );
        continue;
      }

      methods.Add(new(method, name, method.Parameters.ToParameterNames()));
    }

    var managedMethods = methods.ToImmutable();

    using var workspace = new AdhocWorkspace();
    var syntax = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);

    var clientSources = new ClientEmitter(syntax).Emit(
      managedMethods,
      new XmlDocumentationEmitter(managedMethods)
    );

    var uiSources = new UiEventEmitter(syntax).Emit(
      model.UiEvents.Where(static @event => @event.DeprecatedSince is null)
    );

    GeneratedFile CreateFile(string name, SyntaxNode source) =>
      new($"Generated/{name}.g.cs", source.Format());

    return new(
      [
        CreateFile("INvimClient", clientSources.Interface),
        CreateFile("NvimClient", clientSources.Impl),
        CreateFile("NvimUiEvent", uiSources.EventTypes),
        CreateFile("NvimUiEventFactory", uiSources.Factory),
        CreateFile(
          "NvimHandles",
          new HandleEmitter(syntax).Emit(model.Extensions)
        ),
      ],
      diagnostics.ToImmutable()
    );
  }
}
