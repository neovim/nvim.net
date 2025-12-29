using NvimClient.NvimMsgpack;
using NvimClient.NvimMsgpack.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NvimClient.APIGenerator.Properties.Models;

/// <summary>
/// Represents a C Sharp field
/// </summary>
public record CSFunction {
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

    public string ToCode(int identationLevel) {
        StringBuilder sb = new();

        WriteIdentation(sb, identationLevel);

        foreach (string s in Specifiers) {
            _ = sb.Append(s).Append(' ');
        }

        _ = sb.Append(ReturnType).Append(' ').Append(Name).Append('(');
        for (int i = 0; i < Arguments.Count; i++) {
            _ = sb.Append(Arguments[i].ArgumentType).Append(' ').Append('@').Append(Arguments[i].ArgumentName);
            if (i != Arguments.Count - 1) {
                _ = sb.Append(',');
            }
        }

        _ = sb.Append(") {\n");
        identationLevel++;
        WriteIdentation(sb, identationLevel);
        _ = sb.Append(Code).Append('\n');
        identationLevel--;
        WriteIdentation(sb, identationLevel);
        _ = sb.Append("}\n");

        return sb.ToString();
    }

    public static CSFunction FromNvimMethod(NvimFunction fn, string? prefixToRemove, bool isVirtualMethod) {
        string name;
        if (prefixToRemove is not null) {
            if (!fn.Name.StartsWith(prefixToRemove, System.StringComparison.Ordinal)) {
                throw new System.InvalidOperationException($"Function {fn.Name} does not have expected prefix \"{prefixToRemove}\"");
            }
            name = StringUtil.ConvertToCamelCase(fn.Name[prefixToRemove.Length..], true);
        } else {
            name = StringUtil.ConvertToCamelCase(fn.Name, true);
        }

        string code = $$"""
            NvimRequest req = new() {
                Method = {{fn.Name}},
                Arguments = []
            };
            SendAndReceive<string>(req);
            """;

        return new CSFunction() {
            Specifiers = [
                "public"
            ],
            ReturnType = NvimTypesMap.GetCSharpType(fn.ReturnType),
            Name = name,
            Arguments = [.. fn.Parameters.Select(CSArgument.FromNvimParameter)],
            Code = code
        };

        //            public string Exec() {NvimRequest req = new() {
        //    Method = nvim_exec,
        //    Arguments = []
        //};

    }
}