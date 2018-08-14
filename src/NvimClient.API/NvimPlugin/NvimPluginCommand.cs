using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.API.NvimPlugin.Attributes;
using NvimClient.API.NvimPlugin.Parameters;

namespace NvimClient.API.NvimPlugin
{
  internal class NvimPluginCommand : NvimPluginExport
  {
    private static readonly string[] AllowedNArgsValues =
      {"0", "1", "*", "?", "+"};

    public NvimPluginCommand(MethodInfo method, string pluginPath,
      object pluginInstance, NvimCommandAttribute attribute) : base(
      attribute.Name ?? method.Name, method, pluginPath, pluginInstance)
    {
      if (!string.IsNullOrEmpty(attribute.NArgs) &&
          !AllowedNArgsValues.Contains(attribute.NArgs))
      {
        throw new Exception(
          $"The value of {nameof(attribute.NArgs)} is invalid");
      }

      int? functionParameterIndex = null;
      var parameterVisitors = new Dictionary<Type, Action<int>>
      {
        {typeof(NvimBang), index => BangParameterIndex = index},
        {typeof(NvimCount), index => CountParameterIndex = index},
        {typeof(NvimRange), index => RangeParameterIndex = index},
        {typeof(NvimRegister), index => RegisterParameterIndex = index},
        {typeof(object), index => functionParameterIndex = index}
      };
      var evalParameterIndices = new List<int>(Method.GetParameters().Length);
      var attributeVisitors = new Dictionary<Type, Action<int, object>>
      {
        {
          typeof(NvimEvalAttribute),
          (index, attr) => evalParameterIndices.Add(index)
        }
      };
      VisitParameters(parameterVisitors, attributeVisitors);

      if (CountParameterIndex.HasValue && RangeParameterIndex.HasValue)
      {
        throw new ArgumentException(
          "Range and Count parameters are mutually exclusive");
      }

      var argumentConverters = new List<ArgumentConverter>();
      var functionParameter = functionParameterIndex.HasValue
        ? Method.GetParameters()[functionParameterIndex.Value]
        : null;
      var paramType = functionParameter?.ParameterType;
      if (paramType == typeof(string))
      {
        if (attribute.NArgs != "1" && attribute.NArgs != "?")
        {
          throw new Exception(
            $"Parameter \"{functionParameter.Name}\" of type string is only"
            + $" allowed when {nameof(attribute.NArgs)} is \"1\" or \"?\"");
        }

        argumentConverters.Add(
          arg => new[]
          {
            new PluginArgument
            {
              Value = ((object[]) arg).First(),
              Index = functionParameterIndex.Value
            }
          });
      }
      else if (paramType == typeof(string[]))
      {
        argumentConverters.Add(
          arg => new[]
          {
            new PluginArgument
            {
              Value = ((object[]) arg).Cast<string>().ToArray(),
              Index = functionParameterIndex.Value
            }
          });
      }
      else if (paramType != null)
      {
        throw new Exception($"Parameter \"{functionParameter.Name}\" must"
                            + " be of type string or string[]");
      }

      if (RangeParameterIndex.HasValue)
      {
        argumentConverters.Add(arg =>
        {
          var range = ((object[]) arg).Cast<long>().ToArray();
          return new[]
          {
            new PluginArgument
            {
              Index = RangeParameterIndex.Value,
              Value = new NvimRange
              {
                FirstLine = range[0],
                LastLine  = range[1]
              }
            }
          };
        });
      }
      else if (CountParameterIndex.HasValue)
      {
        argumentConverters.Add(arg => new[]
        {
          new PluginArgument
          {
            Index = CountParameterIndex.Value,
            Value = new NvimCount((long) arg)
          }
        });
      }

      if (BangParameterIndex.HasValue)
      {
        argumentConverters.Add(arg => new[]
        {
          new PluginArgument
          {
            Index = BangParameterIndex.Value,
            Value = new NvimBang(Convert.ToBoolean(arg))
          }
        });
      }

      if (RegisterParameterIndex.HasValue)
      {
        argumentConverters.Add(arg => new[]
        {
          new PluginArgument
          {
            Index = RegisterParameterIndex.Value,
            Value = new NvimRegister((string) arg)
          }
        });
      }

      if (evalParameterIndices.Any())
      {
        argumentConverters.Add(
          nvimArg => evalParameterIndices.Zip(
            (object[]) nvimArg, (index, arg) =>
              new PluginArgument
              {
                Value = arg,
                Index = index
              })
        );
      }

      ArgumentConverters = argumentConverters;
      Attribute = attribute;
    }

    private int? BangParameterIndex     { get; set; }
    private int? CountParameterIndex    { get; set; }
    private int? RangeParameterIndex    { get; set; }
    private int? RegisterParameterIndex { get; set; }
    private NvimCommandAttribute Attribute              { get; }

    public override string HandlerName => $"{PluginPath}:command:{Name}";

    internal override Dictionary<string, object> GetSpec()
    {
      var opts = new Dictionary<string, string>();

      if (!string.IsNullOrEmpty(Attribute.NArgs))
      {
        opts["nargs"] = Attribute.NArgs;
      }

      if (RangeParameterIndex.HasValue)
      {
        opts["range"] = Attribute.Range ?? string.Empty;
      }
      else if (CountParameterIndex.HasValue)
      {
        opts["count"] = Attribute.Count.ToString();
      }

      if (BangParameterIndex.HasValue)
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
}
