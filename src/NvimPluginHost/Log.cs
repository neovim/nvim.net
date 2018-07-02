using System;
using System.IO;

namespace NvimPluginHost
{
  internal static class Log
  {
    private static readonly StreamWriter _writer;

    static Log()
    {
      var logFile = Environment.GetEnvironmentVariable("NVIM_DOTNET_LOG_FILE");
      if (logFile != null)
      {
        _writer = new StreamWriter(logFile, true) {AutoFlush = true};
      }
    }

    public static void WriteLine(string text) => _writer?.WriteLine(text);
    public static void Write(string text) => _writer?.Write(text);
  }
}
