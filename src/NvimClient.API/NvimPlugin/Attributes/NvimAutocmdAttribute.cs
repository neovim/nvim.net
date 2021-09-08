using System;

namespace NvimClient.API.NvimPlugin.Attributes
{
  [AttributeUsage(AttributeTargets.Method)]
  public class NvimAutocmdAttribute : Attribute
  {
    public NvimAutocmdAttribute(string name) => Name = name;

    public string Name { get; set; }
    public string Group { get; set; }
    public bool AllowNested { get; set; }
    public string Pattern { get; set; }
  }
}
