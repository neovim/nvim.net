using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.API.NvimPlugin.Attributes;
using NvimClient.API.NvimPlugin.Parameters;

namespace NvimClient.API.NvimPlugin;

internal class NvimPluginFunction : NvimPluginExport {

    public override string HandlerName => $"{PluginPath}:function:{Name}";
    private int? RangeParameterIndex { get; set; }
    private NvimFunctionAttribute Attribute { get; }

    internal NvimPluginFunction(MethodInfo method, string? pluginPath, object? pluginInstance, NvimFunctionAttribute attribute) : base(attribute.Name ?? method.Name, method, pluginPath, pluginInstance) {
        List<int> functionParameterIndices = new(Method.GetParameters().Length);

        Dictionary<Type, Action<int>> parameterVisitors = new() {
                {
                  typeof(NvimRange),
                  index => RangeParameterIndex = index
                },
                {
                  typeof(object),
                  functionParameterIndices.Add
                }
        };

        List<int> evalParameterIndices = new(Method.GetParameters().Length);
        Dictionary<Type, Action<int, object>> attributeVisitors = new() {
              {
                typeof(NvimEvalAttribute),
                (index, attr) => evalParameterIndices.Add(index)
              }
        };
        VisitParameters(parameterVisitors, attributeVisitors);

        List<ArgumentConverter> argumentConverters =
        [
          nvimArg => functionParameterIndices.Zip( (object[]) nvimArg, (index, arg) =>
              new PluginArgument
              {
                Value = arg,
                Index = index
              })
        ];

        if (RangeParameterIndex.HasValue) {
            argumentConverters.Add(arg => {
                long[] range = [.. ((object[])arg).Cast<long>()];
                return [
                    new PluginArgument {
                        Index = RangeParameterIndex.Value,
                        Value = new NvimRange {
                            FirstLine = range[0],
                            LastLine  = range[1]
                        }
                    }
                ];
            });
        }

        if (evalParameterIndices.Count is not 0) {
            argumentConverters.Add(
              nvimArg => evalParameterIndices.Zip(
                (object[])nvimArg, (index, arg) =>
                 new PluginArgument {
                     Value = arg,
                     Index = index
                 })
            );
        }

        ArgumentConverters = argumentConverters;
        Attribute = attribute;
    }


    internal override Dictionary<string, object> GetSpec() {
        Dictionary<string, string> opts = [];

        if (RangeParameterIndex.HasValue) {
            opts["range"] = string.Empty;
        }

        AddEvalOption(opts);

        return new Dictionary<string, object> {
            {"type", "function"},
            {"name", Name},
            {"sync", Sync ? "1" : "0"},
            {"opts", opts}
        };
    }
}