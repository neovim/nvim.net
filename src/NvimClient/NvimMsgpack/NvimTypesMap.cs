using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

      var arrayRegexMatch = Regex.Match(nvimType,
        @"^ArrayOf\((?:(?<ElementType>.+), (?<Size>\d+)|(?<ElementType>.+))\)$");
      if (arrayRegexMatch.Success)
      {
        var elementType = arrayRegexMatch.Groups["ElementType"].Value;
        return GetCSharpType(elementType) + "[]";
      }

      var dictionaryRegexMatch = Regex.Match(nvimType,
        @"^DictionaryOf\((?<KeyType>.+), (?<ValueType>.+)?\)$");
      if (dictionaryRegexMatch.Success)
      {
        var keyType =
          GetCSharpType(dictionaryRegexMatch.Groups["KeyType"].Value);
        var valueType =
          GetCSharpType(dictionaryRegexMatch.Groups["ValueType"].Value);
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
