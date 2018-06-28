using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NvimClient.NvimPlugin;
using NvimClient.NvimPlugin.Attributes;

namespace NvimClient.API.NvimPlugin
{
  public static class PluginHost
  {
    private const string PluginHostName = "dotnet";

    public static async Task
      RegisterPlugin<T>(NvimAPI api, string pluginPath) =>
      await RegisterPlugin(api, pluginPath, typeof(T));

    public static async Task RegisterPlugin(NvimAPI api, string pluginPath,
      Type pluginType)
    {
      var pluginAttribute =
        pluginType.GetCustomAttribute<NvimPluginAttribute>();
      if (pluginAttribute == null)
      {
        throw new Exception(
          $"Type \"{pluginType}\" must have the NvimPlugin attribute");
      }

      var pluginInstance = Activator.CreateInstance(pluginType, api);
      var methodsDictionary =
        new Dictionary<string, Dictionary<string, object>>();
      var exports = GetPluginExports(pluginType, pluginPath, pluginInstance)
        .ToArray();
      foreach (var export in exports)
      {
        export.Register(api);
        if (export is NvimPluginFunction function)
        {
          methodsDictionary[function.Name] =
            new Dictionary<string, object>
            {
              {"async", !function.Sync},
              {"nargs", function.Method.GetParameters().Length}
            };
        }
      }

      await RegisterPlugin(api, pluginPath, exports);

      var version = new Version(pluginAttribute.Version);
      await api.SetClientInfo(pluginAttribute.Name ?? pluginType.Name,
        new Dictionary<string, int>
        {
          {"major", version.Major},
          {"minor", version.Minor},
          {"patch", version.Build}
        }, "plugin", methodsDictionary,
        new Dictionary<string, string>
        {
          {"website", pluginAttribute.Website},
          {"license", pluginAttribute.License},
          {"logo", pluginAttribute.Logo}
        });

      var channelID = (long) (await api.GetApiInfo())[0];
      await api.CallFunction("remote#host#Register",
        new object[]
        {
          PluginHostName, "*", channelID
        });
    }

    public static IReadOnlyCollection<Dictionary<string, object>>
      GetPluginSpecs(Type type) => GetPluginExports(type, null, null)
      .Select(x => x.GetSpec()).ToArray();

    public static NvimPluginExport[] RegisterPluginExports(NvimAPI api, string pluginPath,
      Type pluginType)
    {
      var pluginInstance = Activator.CreateInstance(pluginType, api);
      var exports = GetPluginExports(pluginType, pluginPath, pluginInstance)
        .ToArray();
      foreach (var export in exports)
      {
        export.Register(api);
      }

      return exports;
    }

    private static IEnumerable<NvimPluginExport> GetPluginExports(
      Type pluginType, string pluginPath, object pluginInstance)
    {
      foreach (var method in pluginType.GetMethods())
      {
        var functionAttribute =
          method.GetCustomAttribute<NvimFunctionAttribute>();
        if (functionAttribute != null)
        {
          yield return new NvimPluginFunction(method, pluginPath,
            pluginInstance, functionAttribute);
        }

        var commandAttribute =
          method.GetCustomAttribute<NvimCommandAttribute>();
        if (commandAttribute != null)
        {
          yield return new NvimPluginCommand(method, pluginPath, pluginInstance,
            commandAttribute);
        }

        var autocmdAttribute =
          method.GetCustomAttribute<NvimAutocmdAttribute>();
        if (autocmdAttribute != null)
        {
          yield return new NvimPluginAutocmd(method, pluginPath, pluginInstance,
            autocmdAttribute);
        }
      }
    }

    private static async Task RegisterPlugin(NvimAPI api, string pluginPath,
      IEnumerable<NvimPluginExport> exports)
    {
      await api.CallFunction("remote#host#RegisterPlugin",
        new object[]
        {
          PluginHostName, pluginPath,
          exports.Select(export => export.GetSpec()).ToArray()
        });
    }
  }
}
