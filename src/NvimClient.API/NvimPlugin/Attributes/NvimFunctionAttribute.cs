using System;

namespace NvimClient.NvimPlugin
{
  [AttributeUsage(AttributeTargets.Method)]
  public class NvimFunctionAttribute : Attribute
  {
    public string Name  { get; set; }
    public string Eval  { get; set; }
    public string Range { get; set; }
  }
}
