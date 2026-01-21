using System.Diagnostics;
using NvimClient.NvimProcess;

namespace NvimClient.Test;

/// <summary>
///   A helper class that provides Start methods similar to those in
///   <see cref="Process" /> for starting Nvim processes with less code.
/// </summary>
public static class NvimProcess {
    /// <summary>
    ///   Starts a new Nvim process.
    /// </summary>
    ///
    /// <param name="startInfo">
    ///     The
    /// </param>
    ///
    /// <returns>
    ///     A new Process that is associated with the process resource, or null
    ///     if no process resource is started. Note that a new process that's
    ///     started alongside already running instances of the same process will
    ///     be independent from the others. In addition, Start may return a non-null
    ///     Process with its HasExited property already set to true. In this case,
    ///     the started process may have activated an existing instance of itself
    ///     and then exited.
    /// </returns>
    public static Process? Start(NvimProcessStartInfo startInfo) {
        return Process.Start(startInfo.ProcessStartInfo);
    }

    /// <summary>
    ///   Starts a new Nvim process.
    /// </summary>
    ///
    /// <param name="nvimPath">
    ///   The path to the nvim executable. If null, the PATH will be searched.
    /// </param>
    ///
    /// <param name="arguments">
    ///     Arguments to pass to the process.
    /// </param>
    ///
    /// <param name="startOptions">
    ///   Options for starting the process.
    /// </param>
    ///
    /// <returns>
    ///     A new Process that is associated with the process resource, or null
    ///     if no process resource is started. Note that a new process that's
    ///     started alongside already running instances of the same process will
    ///     be independent from the others. In addition, Start may return a non-null
    ///     Process with its HasExited property already set to true. In this case,
    ///     the started process may have activated an existing instance of itself
    ///     and then exited.
    /// </returns>
    //public static Process? Start(string? nvimPath, string arguments, StartOption startOptions = StartOption.None) {
    //    NvimProcessStartInfo startInfo = new(nvimPath, arguments, startOptions);
    //    return Start(startInfo);
    //}
}