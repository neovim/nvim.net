using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NvimClient.NvimProcess
{
  /// <summary>
  ///   A wrapper for <see cref="System.Diagnostics.ProcessStartInfo" />
  ///   that provides start options and properties specific to Nvim.
  /// </summary>
  public class NvimProcessStartInfo
  {
    /// <summary>
    ///   Initializes a new instance of the NvimProcessStartInfo class with
    ///   the specified start options.
    /// </summary>
    /// <param name="startOptions">The options for starting Nvim.</param>
    public NvimProcessStartInfo(StartOption startOptions) : this(null, null,
      startOptions)
    {
    }

    /// <summary>
    ///   Initializes a new instance of the NvimProcessStartInfo class that
    ///   specifies the path, arguments, and start options to use for starting
    ///   the Nvim process.
    /// </summary>
    /// <param name="nvimPath">
    ///   The path to the nvim executable. If null, the PATH will be searched.
    /// </param>
    /// <param name="arguments">The arguments to pass to Nvim.</param>
    /// <param name="startOptions">The options for starting Nvim.</param>
    public NvimProcessStartInfo(string nvimPath,
      string arguments, StartOption startOptions = StartOption.None) : this(
      new ProcessStartInfo(
        nvimPath, string.Join(" ",
          GetFlagsForOptions(startOptions).Append(arguments)
            .Where(argument => !string.IsNullOrEmpty(argument))))
      {
        CreateNoWindow = startOptions.HasFlag(StartOption.Headless) ||
                         startOptions.HasFlag(StartOption.Embed),
        RedirectStandardInput = startOptions.HasFlag(StartOption.ApiInfo) ||
                                startOptions.HasFlag(StartOption.Embed)
      })
    {
    }

    /// <summary>
    ///   Initializes a new instance of the NvimProcessStartInfo class with
    ///   the specified ProcessStartInfo.
    /// </summary>
    /// <param name="startInfo">
    ///   The start info used for starting the Nvim process.
    /// </param>
    public NvimProcessStartInfo(ProcessStartInfo startInfo)
    {
      startInfo.FileName = string.IsNullOrEmpty(startInfo.FileName)
        ? "nvim"
        : startInfo.FileName;
      ProcessStartInfo = startInfo;
    }

    /// <summary>
    ///   The underlying ProcessStartInfo that may be passed to
    ///   <see cref="Process.Start(ProcessStartInfo)" />.
    /// </summary>
    public ProcessStartInfo ProcessStartInfo { get; set; }

    /// <summary>
    ///   Gets or sets the address the Nvim RPC server will listen on.
    /// </summary>
    public string ListenAddress
    {
      get => ProcessStartInfo.Environment["NVIM_LISTEN_ADDRESS"];
      set => ProcessStartInfo.Environment["NVIM_LISTEN_ADDRESS"] = value;
    }

    public static implicit operator NvimProcessStartInfo(
      ProcessStartInfo startInfo) => new NvimProcessStartInfo(startInfo);

    public static implicit operator ProcessStartInfo(
      NvimProcessStartInfo startInfo) => startInfo.ProcessStartInfo;

    private static IEnumerable<string>
      GetFlagsForOptions(StartOption startOptions)
    {
      return Enum.GetValues(typeof(StartOption)).Cast<StartOption>()
        .Where(option =>
          option != StartOption.None && startOptions.HasFlag(option))
        .Select(option =>
          EnumUtil.GetAttribute<ArgumentAttribute>(option).Flag);
    }
  }
}
