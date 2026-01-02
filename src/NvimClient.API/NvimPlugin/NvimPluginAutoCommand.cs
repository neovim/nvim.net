using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.API.NvimPlugin.Attributes;

namespace NvimClient.API.NvimPlugin;

internal class NvimPluginAutocmd : NvimPluginExport {
    private NvimAutocmdAttribute Attribute { get; }

    public NvimPluginAutocmd(MethodInfo method, string? pluginPath, object? pluginInstance, NvimAutocmdAttribute attribute) : base(attribute.Name ?? method.Name, method, pluginPath, pluginInstance) {
        List<int> evalParameterIndices = new(capacity: Method.GetParameters().Length);
        Dictionary<Type, Action<int, object>> attributeVisitors = new() {
            {
              typeof(NvimEvalAttribute),
              (index, attr) => evalParameterIndices.Add(index)
            }
        };
        VisitParameters(new Dictionary<Type, Action<int>>(), attributeVisitors);

        List<ArgumentConverter> argumentConverters = [];
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

        Attribute = attribute;
        ArgumentConverters = argumentConverters;
    }


    public override string HandlerName =>
      $"{PluginPath}:autocmd:{Name}:{Attribute.Pattern}";

    internal override Dictionary<string, object> GetSpec() {
        Dictionary<string, string> opts = [];

        if (!string.IsNullOrEmpty(Attribute.Group)) {
            opts["group"] = Attribute.Group;
        }

        if (!string.IsNullOrEmpty(Attribute.Pattern)) {
            opts["pattern"] = Attribute.Pattern;
        }

        if (Attribute.AllowNested) {
            opts["nested"] = "1";
        }

        AddEvalOption(opts);

        return new Dictionary<string, object>
               {
             {"type", "autocmd"},
             {"name", Name},
             {"sync", Sync ? "1" : "0"},
             {"opts", opts}
           };
    }
}