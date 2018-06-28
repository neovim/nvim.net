using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.NvimPlugin;

namespace NvimClient.API.NvimPlugin
{
  internal class NvimPluginFunction : NvimPluginExport
  {
    internal NvimPluginFunction(MethodInfo method, string pluginPath,
      object pluginInstance, NvimFunctionAttribute attribute) : base(method,
      pluginPath, pluginInstance)
    {
      Attribute = attribute;
      Method    = method;
      Name      = attribute.Name ?? method.Name;
    }

    public override string HandlerName => $"{PluginPath}:function:{Name}";

    internal MethodInfo            Method    { get; }
    internal string                Name      { get; }
    private  NvimFunctionAttribute Attribute { get; }

    public override Func<object[], object> Handler => args =>
    {
      var functionArguments = (object[]) args.First();
      return Method.Invoke(PluginInstance, functionArguments);
    };

    internal override Dictionary<string, object> GetSpec()
    {
      var opts = new Dictionary<string, string>();

      if (!string.IsNullOrEmpty(Attribute.Eval))
      {
        opts["eval"] = Attribute.Eval;
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
