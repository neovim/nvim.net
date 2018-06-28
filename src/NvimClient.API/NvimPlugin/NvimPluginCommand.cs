using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.NvimPlugin.Attributes;

namespace NvimClient.API.NvimPlugin
{
  internal class NvimPluginCommand : NvimPluginExport
  {
    public NvimPluginCommand(MethodInfo method, string pluginPath,
      object pluginInstance, NvimCommandAttribute attribute) : base(method,
      pluginPath, pluginInstance)
    {
      Attribute = attribute;
      Method    = method;
      Name      = attribute.Name ?? method.Name;
    }

    internal NvimCommandAttribute Attribute { get; }
    internal MethodInfo           Method    { get; }
    internal string               Name      { get; }

    public override string HandlerName => $"{PluginPath}:command:{Name}";

    public override Func<object[], object> Handler => args =>
    {
      var functionArguments = (object[]) args[0];
      var range = ((object[]) args[1]).Cast<long>().ToArray();
      return Method.Invoke(PluginInstance,
        new object[] {range, functionArguments});
    };

    internal override Dictionary<string, object> GetSpec()
    {
      var opts = new Dictionary<string, string>();

      if (!string.IsNullOrEmpty(Attribute.NArgs))
      {
        opts["nargs"] = Attribute.NArgs;
      }

      if (!string.IsNullOrEmpty(Attribute.Range))
      {
        opts["range"] = Attribute.Range == "." ? string.Empty : Attribute.Range;
      }
      else if (!string.IsNullOrEmpty(Attribute.Count))
      {
        opts["count"] = Attribute.Count;
      }

      if (Attribute.Bang)
      {
        opts["bang"] = string.Empty;
      }

      if (Attribute.Register)
      {
        opts["register"] = string.Empty;
      }

      if (!string.IsNullOrEmpty(Attribute.Eval))
      {
        opts["eval"] = Attribute.Eval;
      }

      if (!string.IsNullOrEmpty(Attribute.Addr))
      {
        opts["addr"] = Attribute.Addr;
      }

      if (Attribute.Bar)
      {
        opts["bar"] = string.Empty;
      }

      if (!string.IsNullOrEmpty(Attribute.Complete))
      {
        opts["complete"] = Attribute.Complete;
      }

      return new Dictionary<string, object>
             {
               {"type", "command"},
               {"name", Name},
               {"sync", Sync ? "1" : "0"},
               {"opts", opts}
             };
    }
  }
}
