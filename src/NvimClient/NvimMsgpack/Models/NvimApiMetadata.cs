using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NvimClient.NvimMsgpack.Models;

/// <summary>
/// NVIM API metadata
/// </summary>
public class NvimAPIMetadata {
    public const int OldestSupportedAPILevel = 4;

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

    public void Print() {
        Console.WriteLine("Version: {0}", Version);
        Console.WriteLine("========== Functions ========== ");
        foreach (NvimFunction f in Functions) {
            Console.WriteLine(f);
        }
        Console.WriteLine();

        Console.WriteLine("========== UIEvents ========== ");
        foreach (NvimUIEvent e in UIEvents) {
            Console.WriteLine(e);
        }
        Console.WriteLine();

        if (UIOptions is not null) {
            Console.WriteLine("========== UIOptions ========== ");
            foreach (string s in UIOptions) {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }

        Console.WriteLine("========== ErrorCodes ==========");
        foreach (KeyValuePair<string, NvimErrorType> a in ErrorTypes) {
            Console.WriteLine("Key = {0}, Value = {1}", a.Key, a.Value);
        }
        Console.WriteLine();

        Console.WriteLine("========== NvimTypes ========== ");
        foreach (KeyValuePair<string, NvimType> a in Types) {
            Console.WriteLine("Key = {0}, Value = {1}", a.Key, a.Value);
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
        return UIEvents.Where(static uiEvent => uiEvent.IsActive(OldestSupportedAPILevel));
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
        return Functions.Where(function => function.IsActive(OldestSupportedAPILevel) && function.Method && function.Name.StartsWith(t.Prefix, StringComparison.Ordinal));
    }
}