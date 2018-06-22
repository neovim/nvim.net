using System;

namespace NvimClient.NvimPlugin.Attributes
{
  public class NvimAutocmdAttribute : Attribute
  {
    public NvimAutocmdAttribute(string name) => Name = name;

    public string Name        { get; set; }
    public bool   AllowNested { get; set; }
    public string Pattern     { get; set; }
    public string Eval        { get; set; }
    public bool   Sync        { get; set; }
  }
}
