using System.Collections.Generic;
using System.Text;

namespace NvimClient.APIGenerator.Properties.Models;

/// <summary>
/// Represents a C Sharp field
/// </summary>
public record CSProperty {
    public required List<string> Specifiers { get; set; }
    public required string Type { get; set; }
    public required string Name { get; set; }

    public string? Value { get; set; }

    public string ToCode() {
        StringBuilder sb = new();
        foreach (string s in Specifiers) {
            _ = sb.Append(s).Append(' ');
        }

        _ = sb.Append(Type).Append(' ').Append(Name).Append(" { get; set; }");

        return sb.ToString();
    }
}