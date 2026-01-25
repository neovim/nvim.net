using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NvimClient.Models.Nvim;

/// <summary>
/// NVIM API metadata
/// </summary>
public class NvimAPIMetadata {
    public const int OLDEST_SUPPORTED_API_LEVEL = 4;

    [MessagePackMember(0)]
    public required NvimVersion Version { get; set; }

    [MessagePackMember(1)]
    public required NvimFunction[] Functions { get; set; }

    [MessagePackMember(2)]
    public required NvimUIEvent[] UIEvents { get; set; }

    [MessagePackMember(3)]
    public List<string>? UIOptions { get; set; }

    [MessagePackMember(4)]
    public required Dictionary<string, NvimErrorType> ErrorTypes { get; set; }

    [MessagePackMember(5)]
    public required Dictionary<string, NvimType> Types { get; set; }

    private static void PrintSectionTitle(string title) {
        Console.Write("========== ");
        ConsoleUtils.ColorWrite(ConsoleColor.Blue, title);
        Console.Write(" ==========\n");
    }

    public void PrettyPrint() {
        Console.WriteLine("Version: {0}", Version);
        PrintSectionTitle("Functions");
        foreach (NvimFunction f in Functions) {
            Console.Write("Nvim Function:   ");
            ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0,-30}", f.Name);
            if (f.DeprecatedSince is null) {
                ConsoleUtils.ColorWriteLine(ConsoleColor.DarkGreen, "{0,10}", " Active");
            } else {
                ConsoleUtils.ColorWrite(ConsoleColor.Red, "{0,15}", " Deprecated ");
                Console.WriteLine("Since Api Level {0}", f.DeprecatedSince.Value);
            }
        }
        Console.WriteLine();

        PrintSectionTitle("Nvim UI Events");
        foreach (NvimUIEvent e in UIEvents) {
            Console.Write("Nvim UI Event:   ");
            ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0,-25}", e.Name);
            Console.WriteLine("Since Api Level {0}", e.Since);
        }
        Console.WriteLine();

        if (UIOptions is not null) {
            PrintSectionTitle("Nvim UI Options");
            foreach (string s in UIOptions) {
                ConsoleUtils.ColorWriteLine(ConsoleColor.Green, s);
            }

            Console.WriteLine();
        }

        PrintSectionTitle("ErrorCodes");
        foreach (KeyValuePair<string, NvimErrorType> a in ErrorTypes) {
            ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0,-15}", a.Key);
            ConsoleUtils.ColorWriteLine(ConsoleColor.DarkYellow, " Value: {0,-15}", a.Value.Id);
        }
        Console.WriteLine();

        PrintSectionTitle("NvimTypes");
        foreach (KeyValuePair<string, NvimType> a in Types) {
            ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0,-15}", a.Key);
            ConsoleUtils.ColorWriteLine(ConsoleColor.DarkYellow, " Value: {0,-15}", a.Value.Id);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("======== Nvim API Metadata Finish =========");
        Console.WriteLine();
        Console.WriteLine();
    }


    /// <summary>
    /// Returns the <cref="NvimUIEvent"/> that are suppored for the current
    /// SuppurderAPI level;
    /// </summary>
    public IEnumerable<NvimUIEvent> SupportedUIEvents() {
        return UIEvents.Where(static uiEvent => uiEvent.IsActive(OLDEST_SUPPORTED_API_LEVEL));
    }

    /// <summary>
    /// Returns the <cref="NvimFunction"/> that are suppored for the current
    /// SuppurderAPI level;
    /// </summary>
    public IEnumerable<NvimFunction> AvailableFunctions() {
        return Functions.Where(static function => !function.Method);
    }

    /// <summary>
    /// Returns the <cref="NvimFunction"/> that are methods for a given nvim type and
    /// Supported level;
    /// </summary>
    public IEnumerable<NvimFunction> SupportedMethods(NvimType t) {
        return Functions.Where(function => function.IsActive(OLDEST_SUPPORTED_API_LEVEL) && function.Method && function.Name.StartsWith(t.Prefix, StringComparison.Ordinal));
    }
}