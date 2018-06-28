using System;
using System.Collections.Generic;
using System.Reflection;
using NvimClient.NvimPlugin.Attributes;

namespace NvimClient.API.NvimPlugin
{
  internal class NvimPluginAutocmd : NvimPluginExport
  {
    public NvimPluginAutocmd(MethodInfo method, string pluginPath,
      object pluginInstance, NvimAutocmdAttribute attribute) : base(method,
      pluginPath, pluginInstance)
    {
      Attribute = attribute;
      Method    = method;
      Name      = attribute.Name ?? method.Name;
    }

    private NvimAutocmdAttribute Attribute { get; }
    private MethodInfo           Method    { get; }
    private string               Name      { get; }

    public override string HandlerName =>
      $"{PluginPath}:autocmd:{Name}:{Attribute.Pattern}";

    public override Func<object[], object> Handler =>
      args => Method.Invoke(PluginInstance, args);

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

      if (!string.IsNullOrEmpty(Attribute.Eval))
      {
        opts["eval"] = Attribute.Eval;
      }

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
