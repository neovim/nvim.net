using NvimClient.NvimMsgpack.Models;
using System.Collections.Generic;
using System.Text;

namespace NvimClient.APIGenerator.Models;

/// <summary>
/// Represents a C Sharp field
/// </summary>
public record CSEventDeclaration {
    public required List<string> Specifiers { get; set; }
    public string? Parameters { get; set; }
    public required string Name { get; set; }

    public string ToCode() {
        StringBuilder sb = new();
        foreach (string s in Specifiers) {
            _ = sb.Append(s).Append(' ');
        }

        _ = sb.Append("event").Append(' ');

        if(Parameters is null) {
            _ = sb.Append("EventHandler").Append(' ').Append(Name).Append(';');
        } else {
            _ = sb.Append("EventHandler").Append('<').Append(Parameters).Append('>').Append(' ');
            _ = sb.Append(Name).Append(';');
        }


        return sb.ToString();
    }


    public static CSEventDeclaration FromNvimEvent(NvimUIEvent fn) {
        string name = StringUtil.ConvertToCamelCase(fn.Name, capitalizeFirstChar: true);
        string param;
        if(fn.Parameters is not null && fn.Parameters.Length > 0) {
            param = $"<{name}EventArgs>";
        } else {
           param = string.Empty;
        }

        return new CSEventDeclaration() {
            Specifiers = [
                "public"
            ],
            Parameters = param,
            Name = name
        };
    }
}