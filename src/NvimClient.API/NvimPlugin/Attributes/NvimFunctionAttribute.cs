using System;

namespace NvimClient.NvimPlugin
{
  public class NvimFunctionAttribute : Attribute
  {
    public string Name  { get; set; }
    public string Eval  { get; set; }
    public string Range { get; set; }
    public bool   Sync  { get; set; }
  }
}
