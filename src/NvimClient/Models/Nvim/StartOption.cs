using System;
using System.Collections.Generic;

namespace NvimClient.Models.Nvim;

/// <summary>
///     Neovim starup arguments. Actual string values are decorated
///     as attributes.
/// </summary>
///
/// <remarks>
///     This is a flag type enum. Meaning that multiple enum values may be present
///     in a single enum instance.
/// </remarks>
[Flags]
public enum StartOption {
    None = 0,

    /// <summary>
    /// Use stdin/stdout as a msgpack-rpc channel
    /// </summary>
    Embed = 1,

    /// <summary>
    /// Don't start a user interface
    /// </summary>
    Headless = 2,

    /// <summary>
    /// Write msgpack-encoded API metadata to stdout
    /// </summary>
    ApiInfo = 4

}

public static class StartOptionExtensions {

    /// <summary>
    /// Converts a <see cref="StartOption"/> to a list of arguments for nvim
    /// </summary>
    public static IReadOnlyList<string> ToNvimStringArguments(this StartOption options) {
        if (options == StartOption.None) {
            return [];
        }

        List<string> result = new(capacity: 3);

        // Deterministic order
        if ((options & StartOption.Embed) != 0) {
            result.Add("--embed");
        }

        if ((options & StartOption.Headless) != 0) {
            result.Add("--headless");
        }

        if ((options & StartOption.ApiInfo) != 0) {
            result.Add("--api-info");
        }

        // detect unknown bits (helps catch bugs when enum evolves)
        StartOption known = StartOption.Embed | StartOption.Headless | StartOption.ApiInfo;

        //We do that by accumulating all the flags in the line above. Then using
        //two's complement if the bitwise AND produces even one non zero bit then
        //we have an unknown flag.

        StartOption unknown = options & ~known;
        if (unknown is not 0) {
            throw new ArgumentOutOfRangeException(nameof(options), options, $"Unknown StartOption bits: {unknown}");
        }

        return result;
    }
}