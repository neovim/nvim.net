using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NvimClient.APIGenerator.Doxygen;
using NvimClient.APIGenerator.Properties.Models;
using NvimClient.Models.Nvim;

namespace NvimClient.APIGenerator;

internal static class Program {
    private static int Main(string[] args) {
        if (args.Length is 0 or > 2) {
            string message = """
              Generates a C# class file containing wrappers for Neovim API functions and events.
              Usage: NvimClient.APIGenerator.exe [outdir] [nvim src]"

              Where:

              [outdir] is the output directory where the generated C# class files
              will reside.

              [nvim src] the src directory inside the Neovim source directory.
              """;
            Console.WriteLine(message);
            return 1;
        }

        string outputDir = args.First();
        bool ok = Directory.Exists(outputDir);
        if (!ok) {
            Console.Write("Directory ");
            ConsoleUtils.RedWrite(outputDir);
            Console.Write(" does not exist");
            return 1;
        }


        string? nvimSrcDirectory = args.ElementAtOrDefault(1);
        if (nvimSrcDirectory is null) {
            Console.WriteLine("No neovim source provided");
            return 0;
        } else {
            Console.Write("Neovim Source read from: ");
            ConsoleUtils.BlueWriteLine("{0}", nvimSrcDirectory);
        }

        NvimAPIMetadata? mdata = MetaDataProvider.GetAPIMetadata();
        if (mdata is null) {
            Console.WriteLine("Could not retreive API meta data");
            return 1;
        }

        Console.WriteLine("==== Metadata ====");
        mdata.PrettyPrint();

        Dictionary<string, CSDocumentation> docsDictionary = GenerateDoxygen(nvimSrcDirectory);

        NvimAPIGenerator generator = new(outputDir, mdata, docsDictionary);
        generator.GenerateCSharpFile();
        return 0;
    }

    /// <summary>
    /// Generate doxygen documentation from a newovim source directory.
    /// </summary>
    ///
    /// <param name="nvimSrcDirectory">
    ///     The rool location of the nvim source code. In other words the directory that
    ///     contains the .git directory of the neovim repository.
    /// </param>
    private static Dictionary<string, CSDocumentation> GenerateDoxygen(string nvimSrcDirectory) {

        DoxygenGenerator gen = new();

        gen.GenerateDoxy(nvimSrcDirectory);

        Console.Write("Generated Files in: ");
        ConsoleUtils.BlueWriteLine("{0}", gen.XMLOutputDirectory);

        DoxygenParser docs = new(gen.XMLOutputDirectory);
        List<CSDocumentation> doxygenFunctionDocs = docs.ParseDoxygenDocumentation();

        gen.CleanUpTemporaryFiles();

        Console.WriteLine("Parsed {0} Functions", doxygenFunctionDocs.Count);


        Dictionary<string, CSDocumentation>? docsDictionary = doxygenFunctionDocs.ToDictionary(static functionDoc => functionDoc.FunctionName, static funcDoc => funcDoc);
        //if (docsDictionary is null) {
        //    Console.WriteLine("Could not retreive source code documentation");
        //    return 1;
        //}

        return docsDictionary;

    }
}