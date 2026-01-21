using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.APIGenerator.Doxygen;
using NvimClient.APIGenerator.Properties.Models;

namespace NvimClient.APIGenerator;

internal static class Program {
    private static int Main(string[] args) {
        if (args.Length is 0 or > 2) {
            string? projectName = Assembly.GetExecutingAssembly().GetName().Name;
            string message = """
              Generates a C# class file containing wrappers for Neovim API functions and events.
              Usage: NvimClient.APIGenerator.exe [output.cs] [nvim src]"

              Where:

              [output.cs] is the output directory where the generated C# class files
              will reside.

              [nvim src] the src directory inside the Neovim source directory.
              """;
            Console.WriteLine(message);
            return 1;
        }

        string outputPath = args.First();
        string? nvimSrcDirectory = args.ElementAtOrDefault(1);
        if (nvimSrcDirectory is null) {
            Console.WriteLine("No neovim source provided");
            return 0;
        } else {
            Console.Write("Neovim Source read from: ");
            ConsoleUtils.BlueWriteLine("{0}", nvimSrcDirectory);
        }

        NvimMsgpack.Models.NvimAPIMetadata? mdata = MetaDataProvider.GetAPIMetadata();
        if (mdata is null) {
            Console.WriteLine("Could not retreive API meta data");
            return 1;
        }

        Console.WriteLine("==== Metadata ====");
        mdata.Print();

        DoxygenGenerator gen = new();

        gen.GenerateDoxy(nvimSrcDirectory);

        Console.Write("Generated Files in: ");
        ConsoleUtils.BlueWriteLine("{0}", gen.XMLOutputDirectory);

        DoxygenParser docs = new(gen.XMLOutputDirectory);
        List<CSDocumentation> doxygenFunctionDocs = docs.ParseDoxygenDocumentation();

        gen.CleanUpTemporaryFiles();

        Console.WriteLine("Parsed {0} Functions", doxygenFunctionDocs.Count);

        IEnumerable<IGrouping<string, CSDocumentation>> duplicates = doxygenFunctionDocs.GroupBy(static functionDoc => functionDoc.FunctionName)
        .Where(static g => g.Count() > 1);

        Dictionary<string, CSDocumentation>? docsDictionary = doxygenFunctionDocs.ToDictionary(static functionDoc => functionDoc.FunctionName, static funcDoc => funcDoc);
        if (docsDictionary is null) {
            Console.WriteLine("Could not retreive source code documentation");
            return 1;
        }
        NvimAPIGenerator generator = new(mdata, docsDictionary);
        generator.GenerateCSharpFile();
        return 0;
    }
}