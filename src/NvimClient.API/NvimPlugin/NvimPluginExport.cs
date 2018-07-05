using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NvimClient.API.NvimPlugin.Attributes;

namespace NvimClient.API.NvimPlugin
{
  public abstract class NvimPluginExport
  {
    protected NvimPluginExport(string name, MethodInfo method,
      string pluginPath, object pluginInstance)
    {
      Name           = name;
      Method         = method;
      PluginPath     = pluginPath;
      PluginInstance = pluginInstance;
      Sync = method.GetCustomAttribute<AsyncStateMachineAttribute>() == null
             && method.ReturnType != typeof(Task)
             && (!method.ReturnType.IsGenericType
                 || method.ReturnType.GetGenericTypeDefinition()
                 != typeof(Task<>));
    }

    protected IReadOnlyCollection<ArgumentConverter> ArgumentConverters
    {
      get;
      set;
    }

    internal        bool       Sync           { get; }
    public abstract string     HandlerName    { get; }
    protected       string     PluginPath     { get; }
    private         object     PluginInstance { get; }
    public          MethodInfo Method         { get; }
    public          string     Name           { get; }

    public Func<object[], object> Handler =>
      args => Method.Invoke(PluginInstance,
        ConvertPluginArguments(args).ToArray());

    internal abstract Dictionary<string, object> GetSpec();

    internal void Register(NvimAPI nvim) =>
      nvim.RegisterHandler(HandlerName, Handler);

    private IEnumerable<object>
      ConvertPluginArguments(IEnumerable<object> nvimArguments) =>
      ArgumentConverters.Zip(nvimArguments, (converter, arg) => converter(arg))
        .SelectMany(arg => arg).OrderBy(arg => arg.Index)
        .Select(arg => arg.Value);

    protected void VisitParameters(
      IReadOnlyDictionary<Type, Action<int>> typeVisitors)
    {
      foreach (var param in Method.GetParameters()
        .Select((param, index) =>
          new
          {
            Index      = index,
            Type       = param.ParameterType,
            Attributes = param.GetCustomAttributes()
          }))
      {
        if (typeVisitors.TryGetValue(param.Type, out var visitor) ||
            typeVisitors.TryGetValue(typeof(object), out visitor))
        {
          visitor(param.Index);
        }
      }
    }

    protected struct PluginArgument
    {
      public int    Index { get; set; }
      public object Value { get; set; }
    }

    /// <summary>
    ///   Converts Nvim RPC arguments to plugin arguments.
    /// </summary>
    /// <param name="nvimArgument">Argument from Nvim.</param>
    /// <returns>Arguments to invoke the plugin method with.</returns>
    protected delegate IEnumerable<PluginArgument> ArgumentConverter(
      object nvimArgument);
  }
}
