using System;

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
    [Argument("--embed")]
    Embed = 1,

    /// <summary>
    /// Don't start a user interface
    /// </summary>
    [Argument("--headless")]
    Headless = 2,

    /// <summary>
    /// Write msgpack-encoded API metadata to stdout
    /// </summary>
    [Argument("--api-info")]
    ApiInfo = 4
}
