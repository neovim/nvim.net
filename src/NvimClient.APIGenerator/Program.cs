using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.APIGenerator.Docs;
using NvimClient.APIGenerator.Doxygen;
using NvimClient.APIGenerator.Properties.Models;

namespace NvimClient.APIGenerator;

internal static class Program {
    private static int Main(string[] args) {
        if (args.Length is 0 or > 2) {
            string? projectName = Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine(
              "Generates a C# class file containing wrappers for Neovim API functions and events.\n"
            + "Usage: NvimClient.APIGenerator.exe output.cs [nvim src]\n\n"
            + "output.cs\t\toutput path of the generated C# class file" +
              "nvim src\t\tpath to the Neovim source directory for generating documentation");
            return 1;
        }

        if (args.First() == "--doxygen-filter") {
            DoxygenParser.FilterDoxygenInput(args[1]);
            return 0;
        }

        string outputPath = args.First();
        string? nvimSrcDirectory = args.ElementAtOrDefault(1);
        if (nvimSrcDirectory is null) {
            Console.WriteLine("No neovim source provided");
            return 0;
        } else {
            Console.WriteLine("Neovim Source read from: {0}", nvimSrcDirectory);
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

        Console.WriteLine("Generated Files in {0}", gen.XMLOutputDirectory);

        DoxygenParser docs = new(gen.XMLOutputDirectory);
        List<CSDocumentation> doxygenFunctionDocs = docs.ParseDoxygenDocumentation();


        gen.CleanUpTemporaryFiles();

        Console.WriteLine("Parsed {0} Functions", doxygenFunctionDocs.Count);

        // foreach (FunctionDoc f in a) {
        //     Console.WriteLine("Process Function {0}", f.Function);
        // }

        IEnumerable<IGrouping<string, CSDocumentation>> duplicates = doxygenFunctionDocs.GroupBy(static functionDoc => functionDoc.FunctionName)
        .Where(static g => g.Count() > 1);

        //if (duplicates.Any()) {
        //    foreach (IGrouping<string, CSDocumentation> group in duplicates) {
        //        Console.WriteLine($"Duplicate key: {group.Key}");
        //        foreach (CSDocumentation doc in group) {
        //            Console.WriteLine($"  {doc.FunctionName} {doc.DoxygenFileOrigin}");
        //        }
        //    }
        //}

        Console.WriteLine(doxygenFunctionDocs.First());

        Dictionary<string, CSDocumentation>? b = doxygenFunctionDocs.ToDictionary(static functionDoc => functionDoc.FunctionName, static funcDoc => funcDoc);
        if (b is null) {
            Console.WriteLine("Could not retreive source code documentation");
            return 1;
        }
        NvimAPIGenerator generator = new(mdata, b);
        generator.GenerateCSharpFile();
        return 0;
    }
}