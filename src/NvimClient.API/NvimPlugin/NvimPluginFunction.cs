using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.API.NvimPlugin.Attributes;
using NvimClient.API.NvimPlugin.Parameters;

namespace NvimClient.API.NvimPlugin
{
  internal class NvimPluginFunction : NvimPluginExport
  {
    internal NvimPluginFunction(MethodInfo method, string pluginPath,
      object pluginInstance, NvimFunctionAttribute attribute) : base(
      attribute.Name ?? method.Name, method, pluginPath, pluginInstance)
    {
      var functionParameterIndices =
        new List<int>(Method.GetParameters().Length);
      var parameterVisitors = new Dictionary<Type, Action<int>>
      {
        {
          typeof(NvimRange),
          index => RangeParameterIndex = index
        },
        {
          typeof(object),
          index => { functionParameterIndices.Add(index); }
        }
      };
      VisitParameters(parameterVisitors);

      var argumentConverters = new List<ArgumentConverter>
      {
        nvimArg => functionParameterIndices.Zip(
          (object[]) nvimArg, (index, arg) =>
            new PluginArgument
            {
              Value = arg,
              Index = index
            })
      };
      if (RangeParameterIndex.HasValue)
      {
        argumentConverters.Add(arg =>
        {
          var range = ((object[]) arg).Cast<long>().ToArray();
          return new[]
          {
            new PluginArgument
            {
              Index = RangeParameterIndex.Value,
              Value = new NvimRange
              {
                FirstLine = range[0],
                LastLine  = range[1]
              }
            }
          };
        });
      }

      ArgumentConverters = argumentConverters;
      Attribute = attribute;
    }

    public override string HandlerName => $"{PluginPath}:function:{Name}";

    private int? RangeParameterIndex { get; set; }
    private NvimFunctionAttribute Attribute { get; }

    internal override Dictionary<string, object> GetSpec()
    {
      var opts = new Dictionary<string, string>();

      if (RangeParameterIndex.HasValue)
      {
        opts["range"] = string.Empty;
      }

      return new Dictionary<string, object>
      {
        {"type", "function"},
        {"name", Name},
        {"sync", Sync ? "1" : "0"},
        {"opts", opts}
      };
    }
  }
}
