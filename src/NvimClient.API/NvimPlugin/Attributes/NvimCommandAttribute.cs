using System;

namespace NvimClient.API.NvimPlugin.Attributes
{
  [AttributeUsage(AttributeTargets.Method)]
  public class NvimCommandAttribute : Attribute
  {
    public string Addr { get; set; }
    public string Complete { get; set; }
    public long Count { get; set; }
    public string Eval { get; set; }
    public string Range { get; set; }
    public bool Register { get; set; }
    public string NArgs { get; set; }
    public string Name { get; set; }
    public bool Bar { get; set; }
  }
}
