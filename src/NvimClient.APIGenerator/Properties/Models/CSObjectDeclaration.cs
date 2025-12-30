

using System.Collections.Generic;
using System.Text;

namespace NvimClient.APIGenerator.Properties.Models;

/// <summary>
/// Represents a Model that describe an object declaration with the goal
/// of producing code.
/// </summary>
public record CSObjectDeclaration {
    public required string ObjectType { get; set; }
    public required string ObjectName { get; set; }
    public List<string>? ConstructorArguments { get; set; }
    public Dictionary<string, string>? InitializerList { get; set; }


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

        if (ConstructorArguments is null) {
            _ = sb.Append(ObjectType).Append(' ').Append(ObjectName).Append(" = ").Append("new()");
        } else {
            _ = sb.Append(ObjectType).Append(' ').Append(ObjectName).Append(" = ").Append("new(");
            for (int i = 0; i < ConstructorArguments.Count; i++) {
                _ = sb.Append(ConstructorArguments[i]).Append(' ').Append('@').Append(ConstructorArguments[i]);
                if (i != ConstructorArguments.Count - 1) {
                    _ = sb.Append(',');
                }
            }
            _ = sb.Append(')');
        }

        if (InitializerList is null) {
            _ = sb.Append('\n');
        } else {
            _ = sb.Append(" {\n");
            identationLevel++;
            int index = 0;
            foreach (KeyValuePair<string, string> kvp in InitializerList) {
                WriteIdentation(sb, identationLevel);
                _ = sb.Append(kvp.Key).Append(" = ").Append(kvp.Value);
                if (index != InitializerList.Count - 1) {
                    _ = sb.Append(',').Append('\n');
                } else {
                    _ = sb.Append('\n');
                }
                index++;
            }

            identationLevel--;
            WriteIdentation(sb, identationLevel);
            _ = sb.Append('}').Append(';');
        }

        // foreach (string s in Specifiers) {
        //     _ = sb.Append(s).Append(' ');
        // }
        //
        // _ = sb.Append(ReturnType).Append(' ').Append(ObjectName).Append('(');
        // for (int i = 0; i < Arguments.Count; i++) {
        //     _ = sb.Append(Arguments[i].ArgumentType).Append(' ').Append('@').Append(Arguments[i].ArgumentName);
        //     if (i != Arguments.Count - 1) {
        //         _ = sb.Append(',');
        //     }
        // }
        //
        // _ = sb.Append(") {\n");
        // identationLevel++;
        // WriteIdentation(sb, identationLevel);
        // _ = sb.Append(Code).Append('\n');
        // identationLevel--;
        // WriteIdentation(sb, identationLevel);
        // _ = sb.Append("}\n");

        return sb.ToString();
    }

}