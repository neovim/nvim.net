using System;
using System.Collections.Generic;

namespace NvimClient.NvimMsgpack
{
  public static class NvimTypesMap
  {
    private static Dictionary<string, string> _nvimTypesMap =>
      new Dictionary<string, string>
      {
        {"Array",      "MessagePackObject[]"},
        {"Boolean",    "bool"},
        {"Dictionary", "MessagePackObject"},
        {"Float",      "double"},
        {"Integer",    "long"},
        {"Object",     "object"},
        {"String",     "string"},
        {"void",       "void"}
      };

    public static string GetCSharpType(string nvimType)
    {
      if (_nvimTypesMap.TryGetValue(nvimType, out var csharpType))
      {
        return csharpType;
      }

      var splitCollection =
        (Func<string, string[]>) (str => str.Split('(', ',', ')'));
      if (nvimType.StartsWith("ArrayOf(") && nvimType.EndsWith(")"))
      {
        var elementType = splitCollection(nvimType)[1];
        return GetCSharpType(elementType) + "[]";
      }

      if (nvimType.StartsWith("DictionaryOf(") && nvimType.EndsWith(")"))
      {
        var split     = nvimType.Split('(', ',', ')');
        var keyType   = GetCSharpType(split[1]);
        var valueType = GetCSharpType(split[2]);
        return $"Dictionary<{keyType}, {valueType}>";
      }

      return "Nvim" + nvimType;
    }
  }
}
