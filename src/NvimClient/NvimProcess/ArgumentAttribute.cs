using System;

namespace NvimClient.NvimProcess
{
  internal class ArgumentAttribute : Attribute
  {
    public ArgumentAttribute(string flag) => Flag = flag;

    /// <summary>
    ///   The nvim command line flag that is associated with the enum value.
    /// </summary>
    public string Flag { get; }
  }
}
