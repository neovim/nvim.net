using System;

namespace NvimClient.NvimPlugin.Attributes
{
  public class NvimCommandAttribute : Attribute
  {
    public string Addr     { get; set; }
    public bool   Bang     { get; set; }
    public string Complete { get; set; }
    public string Count    { get; set; }
    public string Eval     { get; set; }
    public string Range    { get; set; }
    public bool   Register { get; set; }
    public string NArgs    { get; set; }
    public string Name     { get; set; }
    public bool   Sync     { get; set; }
  }
}
