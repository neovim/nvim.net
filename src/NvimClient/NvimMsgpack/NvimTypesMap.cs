using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NvimClient.NvimMsgpack;

public static class NvimTypesMap {
    /// <summary>
    /// A global map between the Nvim Types, and the CSharp types both in name and type
    /// </summary>
    private static readonly (string NvimTypeName, string CSharpTypeName, Type CSharpType)[] _types =
    [
        ("Array",           "object[]",            typeof(object[])),
        ("Boolean",         "bool",                typeof(bool)),
        ("Dictionary",      "IDictionary",         typeof(IDictionary)),
        ("Float",           "double",              typeof(double)),
        ("Integer",         "long",                typeof(long)),
        ("Object",          "object",              typeof(object)),
        ("String",          "string",              typeof(string)),
        ("void",            "void",                typeof(void))
    ];

    private static Dictionary<string, string> _nvimTypesMap {
        get {
            return _types.ToDictionary(static type => type.NvimTypeName, static type => type.CSharpTypeName);
        }
    }

    private static readonly HashSet<Type> _validCSharpTypes =
      [.. _types.Select(static type => type.CSharpType)];

    public static string GetCSharpType(string nvimType) {
        if (_nvimTypesMap.TryGetValue(nvimType, out string? csharpType)) {
            return csharpType;
        }

        Match arrayRegexMatch = Regex.Match(nvimType, @"^ArrayOf\((?:(?<ElementType>.+), (?<Size>\d+)|(?<ElementType>.+))\)$");
        if (arrayRegexMatch.Success) {
            string elementType = arrayRegexMatch.Groups["ElementType"].Value;
            return GetCSharpType(elementType) + "[]";
        }

        Match dictionaryRegexMatch = Regex.Match(nvimType, @"^DictionaryOf\((?<KeyType>.+), (?<ValueType>.+)?\)$");
        if (dictionaryRegexMatch.Success) {
            string keyType = GetCSharpType(dictionaryRegexMatch.Groups["KeyType"].Value);
            string valueType = GetCSharpType(dictionaryRegexMatch.Groups["ValueType"].Value);
            return $"IDictionary<{keyType}, {valueType}>";
        }

        return "Nvim" + nvimType;
    }

    public static bool IsValidType(Type type) {
        return _validCSharpTypes.Contains(type) || IsValidArrayType(type) || IsValidGenericType(type);
    }

    public static bool IsValidArrayType(Type type) {
        if (!type.IsArray) {
            return false;
        }
        Type? elementType = type.GetElementType();
        if (elementType is null) {
            return false;
        }

        //We can now check the simple type. Or recurse if it is a nested
        //array.
        return IsValidType(elementType);
    }

    public static bool IsValidGenericType(Type type) {
        if (!type.IsGenericType) {
            return false;
        }
        Type theType = type.GetGenericTypeDefinition();
        if (theType != typeof(IDictionary<,>)) {
            return false;
        }

        //Check if each of the arguments is of valid type here
        Type[] a = type.GetGenericArguments();
        foreach (Type t in a) {
            bool isvalid = IsValidType(t);
            if (!isvalid) {
                return false;
            }
        }

        return true;
    }
}