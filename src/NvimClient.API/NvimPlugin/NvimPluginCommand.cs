using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NvimClient.API.NvimPlugin.Attributes;
using NvimClient.API.NvimPlugin.Parameters;

namespace NvimClient.API.NvimPlugin;

internal class NvimPluginCommand : NvimPluginExport {
    private static readonly string[] AllowedNArgsValues = ["0", "1", "*", "?", "+"];

    private int? BangParameterIndex { get; set; }
    private int? CountParameterIndex { get; set; }
    private int? RangeParameterIndex { get; set; }
    private int? RegisterParameterIndex { get; set; }
    private readonly NvimCommandAttribute Attribute;

    public override string HandlerName => $"{PluginPath}:command:{Name}";

    public NvimPluginCommand(MethodInfo method, string? pluginPath, object? pluginInstance, NvimCommandAttribute attribute) : base(attribute.Name ?? method.Name, method, pluginPath, pluginInstance) {

        if (!string.IsNullOrEmpty(attribute.NArgs) && !AllowedNArgsValues.Contains(attribute.NArgs)) {
            throw new InvalidOperationException($"The value of {nameof(attribute.NArgs)} is invalid");
        }

        int? functionParameterIndex = null;
        Dictionary<Type, Action<int>> parameterVisitors = new() {
            {typeof(NvimBang), index => BangParameterIndex = index},
            {typeof(NvimCount), index => CountParameterIndex = index},
            {typeof(NvimRange), index => RangeParameterIndex = index},
            {typeof(NvimRegister), index => RegisterParameterIndex = index},
            {typeof(object), index => functionParameterIndex = index}
        };

        List<int> evalParameterIndices = new(Method.GetParameters().Length);
        Dictionary<Type, Action<int, object>> attributeVisitors = new() {
            {
              typeof(NvimEvalAttribute),
              (index, attr) => evalParameterIndices.Add(index)
            }
        };

        VisitParameters(parameterVisitors, attributeVisitors);

        if (CountParameterIndex.HasValue && RangeParameterIndex.HasValue) {
            throw new ArgumentException("Range and Count parameters are mutually exclusive");
        }


        if (functionParameterIndex is null) {
            throw new InvalidOperationException("Function Parameter Index is null");
        }

        List<ArgumentConverter> argumentConverters = [];
        ParameterInfo? functionParameter = functionParameterIndex.HasValue ? Method.GetParameters()[functionParameterIndex.Value] : null;

        if (functionParameter is null) {
            throw new InvalidOperationException("Could not locate function parameter");
        }

        Type paramType = functionParameter.ParameterType;


        if (paramType == typeof(string)) {
            if (attribute.NArgs is not "1" and not "?") {
                throw new InvalidOperationException($"Parameter \"{functionParameter.Name}\" of type string is only allowed when {nameof(attribute.NArgs)} is \"1\" or \"?\"");
            }

            argumentConverters.Add(arg => [
                  new PluginArgument {
                    Value = ((object[]) arg).First(),
                    Index = functionParameterIndex.Value
                  }
              ]);
        } else if (paramType == typeof(string[])) {
            argumentConverters.Add(arg => [
                  new PluginArgument {
                      Value = ((object[]) arg).Cast<string>().ToArray(),
                      Index = functionParameterIndex.Value
                  }
              ]);
        } else if (paramType is not null) {
            throw new InvalidOperationException($"Parameter \"{functionParameter.Name}\" must be of type string or string[]");
        }

        if (RangeParameterIndex.HasValue) {
            argumentConverters.Add(arg => {
                long[] range = [.. ((object[])arg).Cast<long>()];
                return [
                  new PluginArgument {
                    Index = RangeParameterIndex.Value,
                    Value = new NvimRange
                    {
                      FirstLine = range[0],
                      LastLine  = range[1]
                    }
                  }
                ];
            });
        } else if (CountParameterIndex.HasValue) {
            argumentConverters.Add(arg => [
                new PluginArgument {
                  Index = CountParameterIndex.Value,
                  Value = new NvimCount((long) arg)
                }
            ]);
        }

        if (BangParameterIndex.HasValue) {
            argumentConverters.Add(arg => [
                new PluginArgument {
                  Index = BangParameterIndex.Value,
                  Value = new NvimBang(Convert.ToBoolean(arg,CultureInfo.InvariantCulture))
                }
            ]);
        }

        if (RegisterParameterIndex.HasValue) {
            argumentConverters.Add(arg => [
                new PluginArgument {
                  Index = RegisterParameterIndex.Value,
                  Value = new NvimRegister((string) arg)
                }
            ]);
        }

        if (evalParameterIndices.Count is not 0) {
            argumentConverters.Add(nvimArg => evalParameterIndices.Zip(
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

        if (!string.IsNullOrEmpty(Attribute.NArgs)) {
            opts["nargs"] = Attribute.NArgs;
        }

        if (RangeParameterIndex.HasValue) {
            opts["range"] = Attribute.Range ?? string.Empty;
        } else if (CountParameterIndex.HasValue) {
            if (Attribute.Count is null) {
                throw new InvalidOperationException($"The Atribute property {nameof(Attribute.Count)} is null");
            }
            string? str = Attribute.Count.ToString();
            if (str is null) {
                throw new InvalidOperationException($"Could not convert Count to string");
            }
            opts["count"] = str;
        }

        if (BangParameterIndex.HasValue) {
            opts["bang"] = string.Empty;
        }

        if (Attribute.Register is not null && Attribute.Register.Value) {
            opts["register"] = string.Empty;
        }

        if (!string.IsNullOrEmpty(Attribute.Eval)) {
            opts["eval"] = Attribute.Eval;
        }

        if (!string.IsNullOrEmpty(Attribute.Addr)) {
            opts["addr"] = Attribute.Addr;
        }

        if (Attribute.Bar is not null && Attribute.Bar.Value) {
            opts["bar"] = string.Empty;
        }

        if (!string.IsNullOrEmpty(Attribute.Complete)) {
            opts["complete"] = Attribute.Complete;
        }

        AddEvalOption(opts);

        return new Dictionary<string, object>
        {
      {"type", "command"},
      {"name", Name},
      {"sync", Sync ? "1" : "0"},
      {"opts", opts}
    };
    }
}