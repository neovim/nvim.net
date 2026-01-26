using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NvimClient.Models.Nvim;

/// <summary>
///     NVIM API metadata
/// </summary>
public class NvimAPIMetadata {
    /// <summary>
    /// The oldest supported nvim api level
    /// </summary>
    public const int OLDEST_SUPPORTED_API_LEVEL = 4;

    /// <summary>
    /// The nvim vesion that provided the metadata contained in this class
    /// </summary>
    [MessagePackMember(0)]
    public required NvimVersion Version { get; set; }

    /// <summary>
    /// An array of the available nvim functions
    /// </summary>
    [MessagePackMember(1)]
    public required NvimFunction[] Functions { get; set; }

    /// <summary>
    /// An array of the available nvim ui events
    /// </summary>
    [MessagePackMember(2)]
    public required NvimUIEvent[] UIEvents { get; set; }

    /// <summary>
    /// An array of the available nvim ui options
    /// </summary>
    [MessagePackMember(3)]
    public List<string>? UIOptions { get; set; }

    /// <summary>
    /// A mapping of the available nvim errors
    /// </summary>
    [MessagePackMember(4)]
    public required Dictionary<string, NvimErrorType> ErrorTypes { get; set; }

    /// <summary>
    /// A mapping of the nvim types that are exposed by nvim
    /// </summary>
    [MessagePackMember(5)]
    public required Dictionary<string, NvimType> Types { get; set; }

    private static void PrintSectionTitle(string title) {
        Console.Write("========== ");
        ConsoleUtils.ColorWrite(ConsoleColor.Blue, title);
        Console.Write(" ==========\n");
    }

    /// <summary>
    /// Pretty prints this metadata. This function uses colors
    /// </summary>
    public void PrettyPrint() {
        Console.WriteLine("Version: {0}", Version);
        PrintSectionTitle("Functions");
        foreach (NvimFunction f in Functions) {

            if (f.Name is "nvim_get_autocmds") {
                Console.WriteLine(f);
                Console.WriteLine(f);
                Console.WriteLine(f);
                Console.WriteLine(f);
                Console.WriteLine(f);
            }


            Console.Write("Nvim Function:   ");
            ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0,-30}", f.Name);
            if (f.DeprecatedSince is null) {

                if (f.Parameters.Length > 0) {
                    Console.Write("[ ");
                    for (int i = 0; i < f.Parameters.Length; i++) {
                        NvimParameter par = f.Parameters[i];
                        if (i != f.Parameters.Length - 1) {
                            ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0}, ", par.ArgumentType);
                        } else {
                            ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0}", par.ArgumentType);
                        }
                    }
                    Console.Write(" ] ");
                    Console.Write("Returns: ");
                    ConsoleUtils.ColorWrite(ConsoleColor.Yellow, f.ReturnType);
                } else {
                    Console.Write("[ ");
                    ConsoleUtils.ColorWrite(ConsoleColor.Red, "None");
                    Console.Write(" ] ");
                    Console.Write("Returns: ");
                    ConsoleUtils.ColorWrite(ConsoleColor.Yellow, f.ReturnType);
                }



                ConsoleUtils.ColorWrite(ConsoleColor.DarkGreen, " Active");
                Console.Write(" Since Api Level ");
                ConsoleUtils.ColorWriteLine(ConsoleColor.Blue, " {0,-5}", f.Since);
            } else {
                ConsoleUtils.ColorWrite(ConsoleColor.Red, "{0,12}", " Deprecated ");
                Console.WriteLine("Since Api Level {0}", f.DeprecatedSince.Value);
            }
        }
        Console.WriteLine();

        PrintSectionTitle("Nvim UI Events");

        foreach (NvimUIEvent e in UIEvents) {
            Console.Write("Nvim UI Event:   ");
            ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0,-25}", e.Name);
            if (e.Parameters.Length > 0) {
                Console.Write("[ ");
                for (int i = 0; i < e.Parameters.Length; i++) {
                    NvimParameter par = e.Parameters[i];
                    if (i != e.Parameters.Length - 1) {
                        ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0}, ", par.ArgumentType);
                    } else {
                        ConsoleUtils.ColorWrite(ConsoleColor.Green, "{0}", par.ArgumentType);
                    }
                }
                Console.Write(" ] ");
            } else {
                Console.Write("[ ");
                ConsoleUtils.ColorWrite(ConsoleColor.Red, "None");
                Console.Write(" ] ");
            }
            Console.WriteLine("Active Since Api Level {0}", e.Since);
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
    /// Returns the <see cref="NvimUIEvent"/> that are suppored for the current
    /// SuppurderAPI level;
    /// </summary>
    public IEnumerable<NvimUIEvent> SupportedUIEvents() {
        return UIEvents.Where(static uiEvent => uiEvent.IsActive(OLDEST_SUPPORTED_API_LEVEL));
    }

    /// <summary>
    /// Returns the <see cref="NvimFunction"/> that are suppored for the current
    /// SuppurderAPI level;
    /// </summary>
    public IEnumerable<NvimFunction> AvailableFunctions() {
        return Functions.Where(static function => !function.Method);
    }

    /// <summary>
    /// Returns the <see cref="NvimFunction"/> that are methods for a given nvim type and
    /// Supported level;
    /// </summary>
    public IEnumerable<NvimFunction> SupportedMethods(NvimType t) {
        return Functions.Where(function => function.IsActive(OLDEST_SUPPORTED_API_LEVEL) && function.Method && function.Name.StartsWith(t.Prefix, StringComparison.Ordinal));
    }
}