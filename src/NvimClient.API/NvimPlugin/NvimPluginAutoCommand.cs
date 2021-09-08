using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.API.NvimPlugin.Attributes;

namespace NvimClient.API.NvimPlugin
{
  internal class NvimPluginAutocmd : NvimPluginExport
  {
    private NvimAutocmdAttribute Attribute { get; }

    public NvimPluginAutocmd(MethodInfo method, string pluginPath,
      object pluginInstance, NvimAutocmdAttribute attribute) : base(
      attribute.Name ?? method.Name, method, pluginPath,
      pluginInstance)
    {
      var evalParameterIndices = new List<int>(Method.GetParameters().Length);
      var attributeVisitors = new Dictionary<Type, Action<int, object>>
      {
        {
          typeof(NvimEvalAttribute),
          (index, attr) => evalParameterIndices.Add(index)
        }
      };
      VisitParameters(new Dictionary<Type, Action<int>>(), attributeVisitors);

      var argumentConverters = new List<ArgumentConverter>();
      if (evalParameterIndices.Any())
      {
        argumentConverters.Add(
          nvimArg => evalParameterIndices.Zip(
            (object[])nvimArg, (index, arg) =>
             new PluginArgument
             {
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

    internal override Dictionary<string, object> GetSpec()
    {
      var opts = new Dictionary<string, string>();

      if (!string.IsNullOrEmpty(Attribute.Group))
      {
        opts["group"] = Attribute.Group;
      }

      if (!string.IsNullOrEmpty(Attribute.Pattern))
      {
        opts["pattern"] = Attribute.Pattern;
      }

      if (Attribute.AllowNested)
      {
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
}
