using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NvimClient.API.NvimPlugin.Attributes;
using NvimClient.NvimMsgpack;

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

    protected void AddEvalOption(Dictionary<string, string> opts)
    {
      var evalExpressions = string.Join(",", Method.GetParameters().Select(
          param =>
            param.GetCustomAttribute<NvimEvalAttribute>()?.Value)
        .Where(eval => eval != null));
      if (!string.IsNullOrEmpty(evalExpressions))
      {
        opts["eval"] = $"[{evalExpressions}]";
      }
    }

    /// <summary>
    /// Allows the types and attributes of plugin export parameters
    /// to be enumerated and simultaneously validated.
    /// </summary>
    /// <param name="typeVisitors">
    /// A dictionary of handlers for the parameter types.
    /// </param>
    /// <param name="attributeVisitors">
    /// A dictionary of handlers for the parameter attributes.
    /// </param>
    protected void VisitParameters(
      IReadOnlyDictionary<Type, Action<int>> typeVisitors,
      IReadOnlyDictionary<Type, Action<int, object>> attributeVisitors)
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
        var isValidAPIType = NvimTypesMap.IsValidType(param.Type);
        var isValidPluginType =
          typeVisitors.TryGetValue(param.Type, out var visitor);
        if (!isValidAPIType && !isValidPluginType)
        {
          throw new Exception(
            $"Plugin export has invalid parameter type \"{param.Type.Name}\"");
        }

        if (visitor != null
            // If there is not a visitor for the specific type,
            // try to use the "object" visitor as a default
            || typeVisitors.TryGetValue(typeof(object), out visitor))
        {
          visitor(param.Index);
        }

        foreach (var attribute in param.Attributes)
        {
          if (attributeVisitors.TryGetValue(attribute.GetType(),
            out var attributeVisitor))
          {
            attributeVisitor(param.Index, attribute);
          }
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
