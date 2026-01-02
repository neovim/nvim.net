using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NvimClient.API.NvimPlugin.Attributes;

namespace NvimClient.API.NvimPlugin;

public static class PluginHost {
    private const string PluginHostName = "dotnet";

    public static async Task RegisterPlugin<T>(NvimAPI api, string pluginPath) {
        await RegisterPlugin(api, pluginPath, typeof(T));
    }

    public static async Task RegisterPlugin(NvimAPI api, string pluginPath, Type pluginType) {
        NvimPluginAttribute? pluginAttribute = pluginType.GetCustomAttribute<NvimPluginAttribute>();
        if (pluginAttribute is null) {
            throw new InvalidOperationException($"Type \"{pluginType}\" must have the NvimPlugin attribute");
        }

        object? pluginInstance = Activator.CreateInstance(pluginType, api);
        if (pluginInstance is null) {
            throw new InvalidOperationException($"Could not create an instance of the plugin {pluginPath}");
        }
        Dictionary<string, Dictionary<string, object>> methodsDictionary = [];
        NvimPluginExport[] exports = [.. GetPluginExports(pluginType, pluginPath, pluginInstance)];
        foreach (NvimPluginExport export in exports) {
            export.Register(api);
            if (export is NvimPluginFunction function) {
                methodsDictionary[function.Name] = new Dictionary<string, object> {
                    {"async", !function.Sync},
                    {"nargs", function.Method.GetParameters().Length}
                  };
            }
        }

        await RegisterPlugin(api, pluginPath, exports);

        Version version = new(pluginAttribute.Version);

        await api.SetClientInfo(pluginAttribute.Name ?? pluginType.Name,
          new Dictionary<string, int>
          {
            {"major", version.Major},
            {"minor", version.Minor},
            {"patch", version.Build}
          }, "plugin", methodsDictionary,
          new Dictionary<string, string>
          {
            {"website", pluginAttribute.Website is null ? "null" : pluginAttribute.Website},
            {"license", pluginAttribute.License is null ? "null" : pluginAttribute.License},
            {"logo", pluginAttribute.Logo is null ? "null" : pluginAttribute.Logo}
          });

        long channelID = (long)(await api.GetApiInfo())[0];
        _ = await api.CallFunction("remote#host#Register",
          [
            PluginHostName, "*", channelID
          ]);
    }

    public static IReadOnlyCollection<Dictionary<string, object>> GetPluginSpecs(Type type) {
        return [.. GetPluginExports(type, null, null).Select(static x => x.GetSpec())];
    }

    public static NvimPluginExport[] RegisterPluginExports(NvimAPI api, string pluginPath,
      Type pluginType) {
        object? pluginInstance = Activator.CreateInstance(pluginType, api);
        NvimPluginExport[] exports = [.. GetPluginExports(pluginType, pluginPath, pluginInstance)];
        foreach (NvimPluginExport export in exports) {
            export.Register(api);
        }

        return exports;
    }

    private static IEnumerable<NvimPluginExport> GetPluginExports(Type pluginType, string? pluginPath, object? pluginInstance) {
        foreach (MethodInfo method in pluginType.GetMethods()) {
            NvimFunctionAttribute? functionAttribute = method.GetCustomAttribute<NvimFunctionAttribute>();
            if (functionAttribute is not null) {
                yield return new NvimPluginFunction(method, pluginPath, pluginInstance, functionAttribute);
            }

            NvimCommandAttribute? commandAttribute = method.GetCustomAttribute<NvimCommandAttribute>();
            if (commandAttribute != null) {
                yield return new NvimPluginCommand(method, pluginPath, pluginInstance, commandAttribute);
            }

            NvimAutocmdAttribute? autocmdAttribute = method.GetCustomAttribute<NvimAutocmdAttribute>();
            if (autocmdAttribute != null) {
                yield return new NvimPluginAutocmd(method, pluginPath, pluginInstance, autocmdAttribute);
            }
        }
    }

    private static async Task RegisterPlugin(NvimAPI api, string pluginPath, IEnumerable<NvimPluginExport> exports) {
        _ = await api.CallFunction("remote#host#RegisterPlugin",
          [
            PluginHostName,
            pluginPath,
            exports.Select(static export => export.GetSpec()).ToArray()
          ]);
    }
}