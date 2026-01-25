using System;

namespace NvimClient;

/// <summary>
/// A class that provides utilities for writing to console with colors
/// </summary>
public static class ConsoleUtils {
    /// <summary>
    /// A general method for writing to console using a specified color. Similar to
    /// <see cref="Console.Write(string, object[])"/> but with an aditional color
    /// argumen
    /// </summary>
    public static void ColorWrite(ConsoleColor color, string format, params object?[]? arg) {
        ConsoleColor a = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(format, arg);
        Console.ForegroundColor = a;
    }

    /// <summary>
    /// A general method for writing to console using a specified color. Similar to
    /// <see cref="Console.WriteLine(string, object[])"/> but with an aditional color
    /// argumen
    /// </summary>
    public static void ColorWriteLine(ConsoleColor color, string format, params object?[]? arg) {
        ConsoleColor a = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(format, arg);
        Console.ForegroundColor = a;
    }

    /// <summary>
    /// A <see cref="Console.Write(string, object[])"/> implementation that writes
    /// with <see cref="ConsoleColor.Red"/> color to the console.
    /// </summary>
    public static void RedWrite(string format, params object?[]? arg) {
        ConsoleColor a = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(format, arg);
        Console.ForegroundColor = a;
    }

    /// <summary>
    /// A <see cref="Console.WriteLine(string, object[])"/> implementation that writes
    /// with <see cref="ConsoleColor.Red"/> color to the console.
    /// </summary>
    public static void RedWriteLine(string format, params object?[]? arg) {
        ConsoleColor a = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(format, arg);
        Console.ForegroundColor = a;
    }

    /// <summary>
    /// A <see cref="Console.WriteLine(string, object[])"/> implementation that writes
    /// with <see cref="ConsoleColor.Blue"/> color to the console.
    /// </summary>
    public static void BlueWriteLine(string format, params object?[]? arg) {
        ConsoleColor a = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(format, arg);
        Console.ForegroundColor = a;
    }
}