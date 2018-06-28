using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NvimClient.API.NvimPlugin
{
  public abstract class NvimPluginExport
  {
    protected NvimPluginExport(MethodInfo method, string pluginPath,
      object pluginInstance)
    {
      PluginPath     = pluginPath;
      PluginInstance = pluginInstance;
      Sync = method.GetCustomAttribute<AsyncStateMachineAttribute>() == null
             && method.ReturnType != typeof(Task)
             && (!method.ReturnType.IsGenericType
                 || method.ReturnType.GetGenericTypeDefinition()
                 != typeof(Task<>));
    }

    internal        bool                   Sync           { get; }
    public abstract string                 HandlerName    { get; }
    protected       string                 PluginPath     { get; }
    protected       object                 PluginInstance { get; }
    public abstract Func<object[], object> Handler        { get; }

    internal abstract Dictionary<string, object> GetSpec();

    internal void Register(NvimAPI nvim) =>
      nvim.RegisterHandler(HandlerName, Handler);
  }
}
