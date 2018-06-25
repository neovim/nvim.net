using System;
using System.Collections.Generic;
using MsgPack;

namespace NvimClient.NvimMsgpack
{
  public static class NvimTypesMap
  {
    private static Dictionary<string, string> _nvimTypesMap =>
      new Dictionary<string, string>
      {
        {"Array",      "object[]"},
        {"Boolean",    "bool"},
        {"Dictionary", "IDictionary"},
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

      string[] SplitCollection(string str) => str.Split('(', ',', ')');

      if (nvimType.StartsWith("ArrayOf(") && nvimType.EndsWith(")"))
      {
        var elementType = SplitCollection(nvimType)[1];
        return GetCSharpType(elementType) + "[]";
      }

      if (nvimType.StartsWith("DictionaryOf(") && nvimType.EndsWith(")"))
      {
        var split     = SplitCollection(nvimType);
        var keyType   = GetCSharpType(split[1]);
        var valueType = GetCSharpType(split[2]);
        return $"IDictionary<{keyType}, {valueType}>";
      }

      return "Nvim" + nvimType;
    }
  }
}
