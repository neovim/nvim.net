using System;
using System.Collections.Generic;

namespace NvimClient.NvimProcess;

/// <summary>
/// Neovim starup arguments. Actual string values are decorated
/// as attributes.
/// </summary>
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

        // Optional: detect unknown bits (helps catch bugs when enum evolves)
        StartOption known = StartOption.Embed | StartOption.Headless | StartOption.ApiInfo;
        StartOption unknown = options & ~known;
        if (unknown != 0) {
            throw new ArgumentOutOfRangeException(nameof(options), options, $"Unknown StartOption bits: {unknown}");
        }

        return result;
    }
}