using System;
using System.Linq;
using System.Reflection;

namespace NvimClient.APIGenerator
{
  internal static class Program
  {
    private static int Main(string[] args)
    {
      var outputPath = args.SingleOrDefault();
      if (string.IsNullOrEmpty(outputPath))
      {
        var projectName = Assembly.GetExecutingAssembly().GetName().Name;
        Console.WriteLine(
          "Generates a C# class file containing wrappers for Neovim API " +
          "functions and events.\n\n" +
          $"Usage: dotnet run --project {projectName}.csproj output.cs\n\n" +
          "output.cs\t\toutput path of the generated C# class file");
        return 1;
      }

      NvimAPIGenerator.GenerateCSharpFile(outputPath);
      return 0;
    }
  }
}
