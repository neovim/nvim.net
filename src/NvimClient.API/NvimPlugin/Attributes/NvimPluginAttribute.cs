using System;

namespace NvimClient.NvimPlugin.Attributes
{
  public class NvimPluginAttribute : Attribute
  {
    public string Name { get; set; }
    public string Version { get; set; }
    public string Website { get; set; }
    public string License { get; set; }
    public string Logo    { get; set; }
  }
}
