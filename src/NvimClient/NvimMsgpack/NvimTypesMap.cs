using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NvimClient.NvimMsgpack
{
  public static class NvimTypesMap
  {
    private static readonly
      (string NvimTypeName, string CSharpTypeName, Type CSharpType)[] _types =
      {
        ("Array",           "object[]",            typeof(object[])),
        ("Boolean",         "bool",                typeof(bool)),
        ("Dictionary",      "IDictionary",         typeof(IDictionary)),
        ("Float",           "double",              typeof(double)),
        ("Integer",         "long",                typeof(long)),
        ("Object",          "object",              typeof(object)),
        ("String",          "string",              typeof(string)),
        ("void",            "void",                typeof(void))
      };

    private static Dictionary<string, string> _nvimTypesMap =>
      _types.ToDictionary(type => type.NvimTypeName, type => type.CSharpTypeName);

    private static readonly HashSet<Type> _validCSharpTypes =
      new HashSet<Type>(_types.Select(type => type.CSharpType));

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

    public static bool IsValidType(Type type) =>
      _validCSharpTypes.Contains(type)
      || type.IsArray && IsValidType(type.GetElementType())
      || type.IsGenericType
      && type.GetGenericTypeDefinition() == typeof(IDictionary<,>)
      && type.GetGenericArguments().All(IsValidType);
  }
}
