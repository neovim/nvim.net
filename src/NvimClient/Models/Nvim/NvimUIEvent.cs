using MsgPack.Serialization;
using System.Linq;

namespace NvimClient.Models.Nvim;

/// <summary>
/// Represents an nvim UI Event
/// </summary>
public record NvimUIEvent {
    /// <summary>
    /// The event Name
    /// </summary>
    [MessagePackMember(2)]
    public required string Name { get; set; }

    /// <summary>
    /// The parameters that this function receive
    /// </summary>
    [MessagePackMember(0)]
    public required NvimParameter[] Parameters { get; set; }
    //public required ImmutableArray<NvimParameter> Parameters { get; set; }

    /// <summary>
    /// Valid Since
    /// </summary>
    [MessagePackMember(1)]
    public int Since { get; set; }


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
        return Since > apilevel;
    }

    /// <summary>
    /// Indicates if the function is active based on the given <paramref name="apilevel"/>
    /// </summary>
    public bool IsActive(int apilevel) {
        return Since <= apilevel;
    }
}