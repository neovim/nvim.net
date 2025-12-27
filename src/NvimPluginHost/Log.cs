using System;
using System.IO;

namespace NvimPluginHost;

/// <summary>
/// A Basic logging class
/// </summary>
internal static class Log {
    /// <summary>
    /// The object that writes the logs
    /// </summary>
    private static readonly StreamWriter? _writer;

    static Log() {
        string? logFile = Environment.GetEnvironmentVariable("NVIM_DOTNET_LOG_FILE");
        if (logFile is not null) {
            _writer = new StreamWriter(path: logFile, append: true) { AutoFlush = true };
        }
    }

    /// <summary>
    /// Writes a line to the log
    /// </summary>
    public static void WriteLine(ReadOnlySpan<char> text) {
        _writer?.WriteLine(text);
    }

    /// <summary>
    /// Writes a string to the log
    /// </summary>
    public static void Write(ReadOnlySpan<char> text) {
        _writer?.Write(text);
    }
}