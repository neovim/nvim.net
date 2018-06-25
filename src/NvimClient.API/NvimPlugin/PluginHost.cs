using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NvimClient.API;
using NvimClient.NvimPlugin.Attributes;

namespace NvimClient.NvimPlugin
{
  public static class PluginHost
  {
    private const string PluginHostName = "nvim-dotnet";

    public static async Task RegisterPlugin<T>(NvimAPI api)
    {
      var pluginAttribute = typeof(T).GetCustomAttribute<NvimPluginAttribute>();
      if (pluginAttribute == null)
      {
        throw new Exception(
          $"Type \"{typeof(T)}\" must have the NvimPlugin attribute");
      }

      var pluginInstance = Activator.CreateInstance(typeof(T), api);
      var exposedFunctions = typeof(T).GetMethods()
        .Select(method => new
                          {
                            Method = method,
                            Attribute =
                              method
                                .GetCustomAttribute<NvimFunctionAttribute>()
                          }).Where(method => method.Attribute != null);
      var methodsDictionary = new Dictionary<string, Dictionary<string, object>>();
      foreach (var exposedFunction in exposedFunctions)
      {
        await RegisterFunction(api, pluginInstance, exposedFunction.Method,
          exposedFunction.Attribute);
        methodsDictionary[exposedFunction.Method.Name] =
          new Dictionary<string, object>
          {
            {"async", exposedFunction.Attribute.Sync},
            {"nargs", exposedFunction.Method.GetParameters().Length}
          };
      }

      await api.SetClientInfo(pluginAttribute.Name ?? typeof(T).Name,
        pluginAttribute.Version, "plugin", methodsDictionary,
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

    private static async Task RegisterFunction(NvimAPI api,
      object pluginInstance, MethodBase method, NvimFunctionAttribute attribute)
    {
      var pluginPath = method.DeclaringType.Name;
      api.AddRequestHandler($"{pluginPath}:function:{method.Name}",
        args =>
        {
          var functionArguments = (object[]) args.First();
          var returnValue = method.Invoke(pluginInstance, functionArguments);
          return returnValue;
        });
      var opts = new Dictionary<string, string>();
      if (!string.IsNullOrEmpty(attribute.Eval))
      {
        opts["eval"] = attribute.Eval;
      }
      await api.CallFunction("remote#host#RegisterPlugin",
        new object[]
        {
          PluginHostName, pluginPath,
          new[]
          {
            new Dictionary<string, object>
            {
              {"type", "function"},
              {"name", attribute.Name ?? method.Name},
              {"sync", attribute.Sync ? "1" : "0"},
              {"opts", opts}
            }
          }
        });
    }
  }
}
