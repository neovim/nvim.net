using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NvimClient.APIGenerator.Docs;

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

        if (args.First() == DoxygenParser.DoxygenFilterArgument) {
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

        using DoxygenParser docs = new(nvimSrcDirectory);
        docs.CallDoxygenDocumentationGenerationProcess();
        IEnumerable<FunctionDoc> a = docs.ParseDoxygenDocumentation();

        // foreach (FunctionDoc f in a) {
        //     Console.WriteLine("Process Function {0}", f.Function);
        // }

        IEnumerable<IGrouping<string, FunctionDoc>> duplicates = a.GroupBy(static functionDoc => functionDoc.Function)
        .Where(static g => g.Count() > 1);

        foreach (IGrouping<string, FunctionDoc> group in duplicates) {
            Console.WriteLine($"Duplicate key: {group.Key}");
            foreach (FunctionDoc doc in group) {
                Console.WriteLine($"  {doc.Function} {doc.DoxygenFileOrigin}");
            }
        }

        Dictionary<string, FunctionDoc>? b = a?.ToDictionary(static functionDoc => functionDoc.Function, static funcDoc => funcDoc);
        if (b is null) {
            Console.WriteLine("Could not retreive source code documentation");
            return 1;
        }
        NvimAPIGenerator generator = new(mdata, b);
        generator.GenerateCSharpFile(outputPath);
        return 0;
    }
}