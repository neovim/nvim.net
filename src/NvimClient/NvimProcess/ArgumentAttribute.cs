using System;

namespace NvimClient.NvimProcess;

[AttributeUsage(AttributeTargets.Field)]
internal class ArgumentAttribute : Attribute {
    /// <summary>
    ///   The nvim command line flag that is associated with the enum value.
    /// </summary>
    public string Flag { get; }

    public ArgumentAttribute(string flag) {
        Flag = flag;
    }
}