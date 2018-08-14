using System;

namespace NvimClient.API.NvimPlugin.Attributes
{
  [AttributeUsage(AttributeTargets.Method)]
  public class NvimFunctionAttribute : Attribute
  {
    public string Name  { get; set; }
  }
}
