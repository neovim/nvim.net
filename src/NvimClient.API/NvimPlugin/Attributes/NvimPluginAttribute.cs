using System;

namespace NvimClient.API.NvimPlugin.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class NvimPluginAttribute : Attribute {
    public required string Name { get; set; }
    public required string Version { get; set; }
    public string? Website { get; set; }
    public string? License { get; set; }
    public string? Logo { get; set; }
}
