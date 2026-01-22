using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;

namespace NvimPluginHost;

// Taken from
// https://www.codeproject.com/Articles/1194332/Resolving-Assemblies-in-NET-Core

/// <summary>
/// Resolves all the dependent assemblies that are required for execution of the assembly
/// in a given path.
/// </summary>
internal sealed class AssemblyResolver : IDisposable {

    private readonly CompositeCompilationAssemblyResolver _assemblyResolver;
    private readonly DependencyContext? _dependencyContext;
    private readonly AssemblyLoadContext? _loadContext;

    public Assembly Assembly { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public AssemblyResolver(string path) {
        Assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
        _dependencyContext = DependencyContext.Load(Assembly);

        string? directoryName = Path.GetDirectoryName(path);
        if (directoryName is null) {
            throw new InvalidOperationException($"Cannot Get Directory Name for {path}");
        }

        ICompilationAssemblyResolver[] resolvers = [
             new AppBaseCompilationAssemblyResolver(directoryName),
             new ReferenceAssemblyPathResolver(),
             new PackageCompilationAssemblyResolver()
        ];

        _assemblyResolver = new CompositeCompilationAssemblyResolver(resolvers);

        _loadContext = AssemblyLoadContext.GetLoadContext(Assembly);
        if (_loadContext is not null) {
            _loadContext.Resolving += OnResolving;
        } else {
            throw new InvalidOperationException($"Cannot get the load context of this assembly {path}");
        }
    }


    public void Dispose() {
        if (_loadContext is not null) {
            _loadContext.Resolving -= OnResolving;
        }
    }

    private Assembly? OnResolving(AssemblyLoadContext context, AssemblyName name) {
        bool NamesMatch(RuntimeLibrary runtime) {
            return string.Equals(runtime.Name, name.Name,
              StringComparison.OrdinalIgnoreCase);
        }

        if (_dependencyContext is null) {
            return null;
        }

        RuntimeLibrary? library = _dependencyContext.RuntimeLibraries.FirstOrDefault(NamesMatch);
        if (library is not null) {
            CompilationLibrary wrapper = new(
              library.Type,
              library.Name,
              library.Version,
              library.Hash,
              library.RuntimeAssemblyGroups.SelectMany(g => g.AssetPaths),
              library.Dependencies,
              library.Serviceable);

            List<string> assemblies = [];
            _ = _assemblyResolver.TryResolveAssemblyPaths(wrapper, assemblies);
            if (assemblies.Count > 0) {
                return _loadContext?.LoadFromAssemblyPath(assemblies[0]);
            }
        }

        return null;
    }
}