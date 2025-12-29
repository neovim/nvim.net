using System;
namespace NvimClient.NvimMsgpack.Models;

/// <summary>
/// Represents an nvim function or event
/// </summary>
public abstract class NvimFunctionEventBase {
    public required string Name { get; set; }
    public NvimParameter[] Parameters { get; set; }
    public int Since { get; set; }
    public int? DeprecatedSince { get; set; }

    /// <summary>
    /// Indicates if the function is deprecated based on the given <paramref name="apilevel"/>
    /// </summary>
    public bool IsDeprecated(int apilevel) {
        return DeprecatedSince < apilevel;
    }

    /// <summary>
    /// Indicates if the function is active based on the given <paramref name="apilevel"/>
    /// </summary>
    public bool IsActive(int apilevel) {
        return DeprecatedSince >= apilevel;
    }

    public void Print() {
        Console.WriteLine("Name: {0}", Name);
        Console.WriteLine("Parameters:");
        if (Parameters is null) {
            Console.WriteLine("null");
        } else {
            foreach (NvimParameter p in Parameters) {
                p.Print();
            }
        }
        Console.WriteLine("Since: {0}", Since);
    }
}