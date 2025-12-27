using System;
using System.Linq;
using System.Reflection;
using NvimClient.APIGenerator.Docs;

namespace NvimClient.APIGenerator;

internal static class Program {
    private static int Main(string[] args) {
        if (args.Length is 0 or > 2) {
            string projectName = Assembly.GetExecutingAssembly().GetName().Name;
            Console.WriteLine(
              "Generates a C# class file containing wrappers for Neovim API "
            + "functions and events.\n\n"
            + $"Usage: dotnet run --project {projectName}.csproj output.cs [nvim src]\n\n"
            + "output.cs\t\toutput path of the generated C# class file" +
              "nvim src\t\tpath to the Neovim source directory for generating documentation");
            return 1;
        }

        if (args.First() == DoxygenParser.DoxygenFilterArgument) {
            DoxygenParser.FilterDoxygenInput(args[1]);
            return 0;
        }

        string outputPath = args.First();
        string nvimSrcDirectory = args.ElementAtOrDefault(1);
        using DoxygenParser docs = string.IsNullOrEmpty(nvimSrcDirectory) ? null : new DoxygenParser(nvimSrcDirectory);
        NvimAPIGenerator.GenerateCSharpFile(outputPath, docs?.GetDocumentation());
        return 0;
    }
}
