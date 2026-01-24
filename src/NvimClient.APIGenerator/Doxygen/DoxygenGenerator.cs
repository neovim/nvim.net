using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace NvimClient.APIGenerator.Doxygen;

/// <summary>
/// Generates documentation by invoking the doxygen executable on the nvim
/// source code
/// </summary>
public class DoxygenGenerator {

    public const string DoxygenFilterArgument = "--doxygen-filter";
    public string XMLOutputDirectory { get; set; }


    public DoxygenGenerator() {
        XMLOutputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        _ = Directory.CreateDirectory(XMLOutputDirectory);
    }

    private static void PrintConfiguration(string config) {
        string[] lines = config.Split('\n');
        foreach (string l in lines) {
            string[] fields = l.Split('=');
            if (fields.Length is 0) {
                Console.WriteLine();
                continue;
            }

            if (fields.Length is 1) {
                ConsoleUtils.ColorWrite(ConsoleColor.DarkBlue, "{0,-25}", fields[0]);
                Console.WriteLine(" = ");
                continue;
            }

            ConsoleUtils.ColorWrite(ConsoleColor.DarkBlue, "{0,-25}", fields[0]);
            Console.Write(" = ");
            ConsoleUtils.ColorWriteLine(ConsoleColor.Yellow, "{0,-25}", fields[1]);
        }
    }

    /// <summary>
    /// Generates documentation by invoking the doxygen executable on the nvim
    /// source
    /// </summary>
    public void GenerateDoxy(string sourceRoot) {
        string doxygenConfig = GetDoxygenConfig();

        Console.WriteLine("Configuration Read is: ");
        PrintConfiguration(doxygenConfig);
        string inputDirectory = Path.Combine(sourceRoot, "src/nvim/api");

        //the - argument tells Doxygen to read its configuration from standard input
        //instead of a file. We also take over the standard input and write the configuration
        //template with the items replaced.
        //
        //The -q arguments indicates a quiet run.
        ProcessStartInfo doxy_process = new(fileName: "doxygen", arguments: "-q -") {
            RedirectStandardInput = true
        };

        Process? process = Process.Start(doxy_process);
        if (process is null) {
            Console.WriteLine("Could not start doxygen process. Make sure that the doxygen executable is found in your $PATH");
            return;
        }


        using (StreamWriter processStandardInput = process.StandardInput) {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string filter = $"dotnet \\\"{assemblyLocation}\\\" {DoxygenFilterArgument}";
            processStandardInput.Write(
                    doxygenConfig,
                    XMLOutputDirectory,
                    inputDirectory,
                    filter);
        }

        process.WaitForExit();
    }

    /// <summary>
    /// Reads the doxygen configuration
    /// </summary>
    private static string GetDoxygenConfig() {
        const string configName = $"{nameof(NvimClient)}.{nameof(APIGenerator)}.doxygen.config";
        Console.Write("Reading doxygen configuration from: ");
        ConsoleUtils.BlueWriteLine("{0}", configName);
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(configName);
        if (stream is null) {
            throw new InvalidOperationException("Could not retreive Manifest resource stream of the the executing assembly");
        }
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }


    public void CleanUpTemporaryFiles() {
        try {
            Directory.Delete(XMLOutputDirectory, true);
        } catch {
            Console.WriteLine($"Failed to delete temporary directory: {XMLOutputDirectory}");
        }
    }
}