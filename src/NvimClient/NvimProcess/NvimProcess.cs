using System.Diagnostics;

namespace NvimClient.NvimProcess
{
  /// <summary>
  ///   Provides Start methods similar to those in <see cref="Process" />
  ///   for starting Nvim processes.
  /// </summary>
  public static class NvimProcess
  {
    /// <summary>
    ///   Starts a new Nvim process.
    /// </summary>
    /// <param name="startInfo">The </param>
    /// <returns></returns>
    public static Process Start(NvimProcessStartInfo startInfo) =>
      Process.Start(startInfo);

    /// <summary>
    ///   Starts a new Nvim process.
    /// </summary>
    /// <param name="nvimPath">
    ///   The path to the nvim executable. If null, the PATH will be searched.
    /// </param>
    /// <param name="arguments">Arguments to pass to the process.</param>
    /// <param name="startOptions">
    ///   Options for starting the process.
    /// </param>
    /// <returns></returns>
    public static Process Start(
      string nvimPath, string arguments,
      StartOption startOptions = StartOption.None) => Start(
      new NvimProcessStartInfo(nvimPath, arguments, startOptions));
  }
}
