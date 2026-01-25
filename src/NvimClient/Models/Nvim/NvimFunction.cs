using MsgPack.Serialization;
using System.Linq;

namespace NvimClient.Models.Nvim;

/// <summary>
/// Defines an nvim function as described by the Nvim metadata
/// </summary>
public record NvimFunction {

    /// <summary>
    /// Function Name
    /// </summary>
    [MessagePackMember(1)]
    public required string Name { get; set; }

    /// <summary>
    /// The parameters that this function receive
    /// </summary>
    [MessagePackMember(4)]
    public required NvimParameter[] Parameters { get; set; }

    /// <summary>
    /// Valid Since
    /// </summary>
    [MessagePackMember(0)]
    public int Since { get; set; }

    /// <summary>
    /// Deprecated since
    /// </summary>
    [MessagePackMember(5)]
    public int? DeprecatedSince { get; set; }


    [MessagePackMember(3)]
    public bool Method { get; set; }

    /// <summary>
    /// The type that the string returns
    /// </summary>
    [MessagePackMember(2)]
    public required string ReturnType { get; set; }

    /// <summary>
    /// Just a parameter that makes parameters displayable in a human readble
    /// way
    /// </summary>
    public string ParametersDisplay {
        get {
            if (Parameters is null) {
                return "null";
            } else {
                return "[" + string.Join(", ", Parameters.Select(static p => p.ToString())) + "]";
            }
        }
    }

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
}