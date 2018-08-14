using System;

namespace NvimClient.API.NvimPlugin.Attributes
{
  /// <summary>
  /// When applied to a parameter, this attribute specifies
  /// an expression that will be evaluated by Nvim and passed
  /// as an argument when the parameter's method is called.
  /// </summary>
  [AttributeUsage(AttributeTargets.Parameter)]
  public class NvimEvalAttribute : Attribute
  {
    public NvimEvalAttribute(string value) => Value = value;
    public string Value { get; }
  }
}
