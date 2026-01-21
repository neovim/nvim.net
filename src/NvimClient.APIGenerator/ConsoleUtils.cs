using System;

namespace NvimClient.APIGenerator;

public static class ConsoleUtils {
    public static void ColorWrite(ConsoleColor color, string format, params object?[]? arg) {
        ConsoleColor a = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(format, arg);
        Console.ForegroundColor = a;
    }

    public static void RedWrite(string format, params object?[]? arg) {
        ConsoleColor a = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(format, arg);
        Console.ForegroundColor = a;
    }

    public static void RedWriteLine(string format, params object?[]? arg) {
        ConsoleColor a = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(format, arg);
        Console.ForegroundColor = a;
    }

    public static void BlueWriteLine(string format, params object?[]? arg) {
        ConsoleColor a = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(format, arg);
        Console.ForegroundColor = a;
    }
}