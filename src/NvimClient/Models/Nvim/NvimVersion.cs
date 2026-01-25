namespace NvimClient.Models.Nvim;

/// <summary>
/// Represents the version information that Neovim Exposes
/// </summary>
public record NvimVersion {
    /// <summary>
    /// The major version
    /// </summary>
    public int Major { get; set; }

    /// <summary>
    /// The minor version
    /// </summary>
    public int Minor { get; set; }

    /// <summary>
    /// The patch version
    /// </summary>
    public int Patch { get; set; }

    /// <summary>
    /// Indicates if this version of nvim is prerelease
    /// </summary>
    public bool IsPrerelease { get; set; }

    /// <summary>
    /// The api level that this nvim supports
    /// </summary>
    public int ApiLevel { get; set; }

    /// <summary>
    /// The api level to which this nvim is compatible
    /// </summary>
    public int ApiCompatible { get; set; }

    /// <summary>
    /// Indicates if the api that this nvim instance uses is prrelease api
    /// </summary>
    public bool ApiPrerelease { get; set; }

    /// <summary>
    /// The build number
    /// </summary>
    public string? Build { get; set; }
}