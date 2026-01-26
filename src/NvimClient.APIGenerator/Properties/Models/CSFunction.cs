using NvimClient.Models.MsgPack;
using NvimClient.Models.Nvim;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NvimClient.APIGenerator.Properties.Models;

/// <summary>
/// Represents a C Sharp field
/// </summary>
public record CSFunction {
    public CSDocumentation? Documentation { get; set; }
    public CSFunctionAttributeDescription? Attribute { get; set; }
    public required List<string> Specifiers { get; set; }
    public required string ReturnType { get; set; }
    public required string Name { get; set; }
    public required List<CSArgument> Arguments { get; set; }
    public required string Code { get; set; }

    public string? Value { get; set; }

    public static void WriteIdentation(StringBuilder sb, int identationLevel) {
        int identationWidth = 4;
        for (int i = 0; i < identationLevel; i++) {
            for (int j = 0; j < identationWidth; j++) {
                _ = sb.Append(' ');
            }
        }
    }

    /// <summary>
    /// Converts the <see cref="CSFunction"/> object to code
    /// </summary>
    public string ToCode(int identationLevel) {
        StringBuilder sb = new();

        if (Documentation is not null && Documentation.Summary is not null) {
            string doc = Documentation.ToCode(identationLevel)!;
            _ = sb.Append(doc);
        }


        if (Attribute is not null) {
            WriteIdentation(sb, identationLevel);
            _ = sb.Append(Attribute.ToCode()).Append('\n');
        }

        WriteIdentation(sb, identationLevel);
        foreach (string s in Specifiers) {
            _ = sb.Append(s).Append(' ');
        }

        _ = sb.Append(ReturnType).Append(' ').Append(Name).Append('(');
        for (int i = 0; i < Arguments.Count; i++) {
            _ = sb.Append(Arguments[i].ArgumentType).Append(' ');
            if (CSKeywordChecker.IsKeyword(Arguments[i].ArgumentName)) {
                _ = sb.Append('@');
            }
            _ = sb.Append(Arguments[i].ArgumentName);
            if (i != Arguments.Count - 1) {
                _ = sb.Append(',').Append(' ');
            }
        }

        _ = sb.Append(") {\n");
        identationLevel++;
        string[] lines = Code.Split('\n');
        //Naive approach to write the code as indented block. This will break if the
        //code contains \n characters. We re-append the \n because it was removed from
        //the split operation.
        foreach (string line in lines) {
            WriteIdentation(sb, identationLevel);
            _ = sb.Append(line).Append('\n');
        }
        identationLevel--;
        WriteIdentation(sb, identationLevel);
        _ = sb.Append("}\n");

        return sb.ToString();
    }

    public static CSFunction FromNvimFunction(NvimFunction fn, string? prefixToRemove, bool isVirtualMethod) {
        string name;

        if (prefixToRemove is null) {
            name = StringUtil.ConvertToCamelCase(fn.Name, true);
        } else {
            if (fn.Name.StartsWith(prefixToRemove, System.StringComparison.Ordinal)) {
                name = StringUtil.ConvertToCamelCase(fn.Name[prefixToRemove.Length..], true);
            } else {
                name = StringUtil.ConvertToCamelCase(fn.Name, true);
            }
        }

        StringBuilder sb = new();
        for (int i = 0; i < fn.Parameters.Length; i++) {
            string arg_name = fn.Parameters[i].ArgumentName;
            if (CSKeywordChecker.IsKeyword(arg_name)) {
                _ = sb.Append('@');
            }
            _ = sb.Append(arg_name);
            if (fn.Parameters[i].ArgumentType is "Dictionary" or "Dict") {
                _ = sb.Append(".ToMessagePackObject()");
            }
            if (i != fn.Parameters.Length - 1) {
                _ = sb.Append(',');
            }
        }


        string ret = NvimTypesMap.GetCSharpType(fn.ReturnType);
        CSObjectDeclaration request_object = new() {
            ObjectType = nameof(NvimRequest),
            ObjectName = "req",
            InitializerList = new() {
                { nameof(NvimRequest.Method), $"\"{fn.Name}\"" },
                { nameof(NvimRequest.Params), $"[{sb}]" },
            }
        };

        //if(fn.Parameters)


        string code;
        if (ret is "void") {
            code = $$"""
            {{request_object.ToCode(0)}}
            return SendAndReceive(req);
            """;
        } else {
            code = $$"""
            {{request_object.ToCode(0)}}
            return SendAndReceive<{{ret}}>(req);
            """;
        }


        CSFunctionAttributeDescription? potentially_deprecated = null;
        if (fn.DeprecatedSince is not null) {
            potentially_deprecated = new CSFunctionAttributeDescription() {
                AttributeName = "Obsolete"
            };
        }

        return new CSFunction() {
            Attribute = potentially_deprecated,
            Specifiers = [
                "public"
            ],
            ReturnType = ret is "void" ? "Task" : $"Task<{ret}>",
            Name = name,
            Arguments = [.. fn.Parameters.Select(CSArgument.FromNvimParameter)],
            Code = code
        };

    }

    ///<summary>
    ///     Generates a method for an nvim type.
    ///</summary>
    public static CSFunction FromNvimTypeMethod(NvimType t, NvimFunction fn, bool isVirtualMethod) {
        string name;
        string prefixToRemove = t.Prefix;
        if (prefixToRemove is not null) {
            if (!fn.Name.StartsWith(prefixToRemove, System.StringComparison.Ordinal)) {
                throw new System.InvalidOperationException($"Function {fn.Name} does not have expected prefix \"{prefixToRemove}\"");
            }
            name = StringUtil.ConvertToCamelCase(fn.Name[prefixToRemove.Length..], true);
        } else {
            name = StringUtil.ConvertToCamelCase(fn.Name, true);
        }

        StringBuilder sb = new();
        _ = sb.Append("_msgPackExtObj, ");
        //Ommit the first parameter. Assume it's the type itself
        for (int i = 1; i < fn.Parameters.Length; i++) {
            _ = sb.Append('@').Append(fn.Parameters[i].ArgumentName);
            if (i != fn.Parameters.Length - 1) {
                _ = sb.Append(", ");
            }
        }

        CSObjectDeclaration request_object = new() {
            ObjectType = "NvimRequest",
            ObjectName = "req",
            InitializerList = new() {
                { nameof(NvimRequest.Method), $"\"{fn.Name}\"" },
                { nameof(NvimRequest.Params), $"[{sb}]" },
            }
        };

        string csreturn = NvimTypesMap.GetCSharpType(fn.ReturnType);

        string code;
        if (csreturn is not "void") {
            code = $$"""
            {{request_object.ToCode(0)}}
            return _api.SendAndReceive<{{csreturn}}>(req);
            """;
        } else {
            code = $$"""
            {{request_object.ToCode(0)}}
            return _api.SendAndReceive<string>(req);
            """;
        }


        CSFunctionAttributeDescription? potentially_deprecated = null;
        if (fn.DeprecatedSince is not null) {
            potentially_deprecated = new CSFunctionAttributeDescription() {
                AttributeName = "Obsolete"
            };
        }


        return new CSFunction() {
            Attribute = potentially_deprecated,
            Specifiers = [
                "public"
            ],
            ReturnType = csreturn is "void" ? "Task" : $"Task<{csreturn}>",
            Name = name,
            //Ommit the first argument
            Arguments = [.. fn.Parameters[1..].Select(CSArgument.FromNvimParameter)],
            Code = code
        };

    }
}