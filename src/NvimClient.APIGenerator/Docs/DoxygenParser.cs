using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NvimClient.APIGenerator.Docs
{
  /// <summary>
  ///   Generates and parses XML documentation from Doxygen.
  /// </summary>
  internal class DoxygenParser : IDisposable
  {
    public const string DoxygenFilterArgument = "--doxygen-filter";
    private readonly string _nvimSrcDirectory;
    private readonly string _tempOutputDirectory;

    public DoxygenParser(string nvimSrcDirectory)
    {
      _nvimSrcDirectory = nvimSrcDirectory;
      _tempOutputDirectory =
        Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(_tempOutputDirectory);
    }

    public void Dispose()
    {
      try
      {
        Directory.Delete(_tempOutputDirectory, true);
      }
      catch
      {
        Console.WriteLine(
          $"Failed to delete temporary directory: {_tempOutputDirectory}");
      }
    }

    internal IEnumerable<FunctionDoc> GetDocumentation()
    {
      GenerateDocumentation();

      var xmlDocsDirectory = Path.Combine(_tempOutputDirectory, "xml");
      var indexXml =
        XDocument.Load(Path.Combine(xmlDocsDirectory, "index.xml"));
      var xmlFilenames = indexXml
        .Descendants("compound")
        .Where(node =>
          node.Attribute("kind")?.Value == "file")
        .Where(node =>
          node.Element("name")?.Value.EndsWith(".c")
          ?? false).Select(node =>
          node.Attribute("refid")?.Value);
      foreach (var xmlFilename in xmlFilenames)
      {
        var docXml =
          XDocument.Load(Path.Combine(xmlDocsDirectory, xmlFilename + ".xml"));
        var functionDocs = docXml.Descendants("memberdef")
          .Where(memberDef =>
            memberDef.Attribute("kind")?.Value == "function"
            && memberDef.Attribute("static")?.Value == "no").Select(
            memberDef => new FunctionDoc
            {
              Function = memberDef.Element("name").Value,
              Summary = GetDocElements(memberDef.Element("detaileddescription")
                .Elements("para")
                .Where(para => para.Element("parameterlist") == null)),
              Parameters = memberDef
                             .Descendants("parameterlist")
                             .FirstOrDefault()
                             ?.Elements("parameteritem").Select(
                               param => new ParameterDoc
                               {
                                 Name = param.Descendants("parametername")
                                   .First().Value,
                                 Description =
                                   GetDocElements(param
                                     .Descendants("parameterdescription")
                                     .First().Nodes())
                               }) ?? Enumerable.Empty<ParameterDoc>(),
              Return = GetDocElements(memberDef.Element("detaileddescription")
                ?.Descendants("simplesect").FirstOrDefault(simplesect =>
                  simplesect.Attribute("kind")?.Value == "return")
                ?.Nodes()),
              Notes = GetDocElements(memberDef.Element("detaileddescription")
                ?.Descendants("simplesect").Where(simplesect =>
                  simplesect.Attribute("kind")?.Value == "note"))
            }
          );
        foreach (var functionDoc in functionDocs)
        {
          yield return functionDoc;
        }
      }
    }

    private static IEnumerable<IDocElement> GetDocElements(
      IEnumerable<XNode> nodes)
    {
      if (nodes == null)
      {
        yield break;
      }

      foreach (var node in nodes)
      {
        switch (node)
        {
          case XElement element when element.Name == "computeroutput":
            yield return new InlineCode(element.Value);
            yield break;
          case XElement element when element.Name == "para":
            yield return new Paragraph(GetDocElements(element.Nodes()));
            yield break;
          case XElement element when element.Name == "itemizedlist":
            yield return new DocList(DocListType.ItemizedList,
              GetDocElements(element.Elements("listitem")));
            yield break;
          case XElement element when element.Name == "orderedlist":
            yield return new DocList(DocListType.OrderedList,
              GetDocElements(element.Elements("listitem")));
            yield break;
          case XElement element:
            yield return new Text(element.Value);
            yield break;
          case XText text:
            yield return new Text(text.Value);
            yield break;
        }
      }
    }

    /// <summary>
    ///   Filters the source file to fix macros that confuse Doxygen.
    /// </summary>
    /// <param name="filePath">The path of the file to read from.</param>
    /// <remarks>
    ///   Ported from the <c>filter_source</c> function in
    ///   <c>neovim/scripts/gen_api_vimdoc.py</c>.
    /// </remarks>
    internal static void FilterDoxygenInput(string filePath)
    {
      using (var fileStream = File.OpenRead(filePath))
      {
        using (var streamReader = new StreamReader(fileStream))
        {
          string line;
          while ((line = streamReader.ReadLine()) != null)
          {
            Console.WriteLine(
              Regex.Replace(line, @"^(ArrayOf|DictionaryOf)(\(.*?\))",
                m => m.Groups[1]
                     + string.Join('_',
                       Regex.Split(m.Groups[2].Value, @"[^\w]+")))
            );
          }
        }
      }
    }

    private void GenerateDocumentation()
    {
      var doxygenConfig = GetDoxygenConfig();
      var inputDirectory = Path.Combine(_nvimSrcDirectory, "src/nvim/api");
      var process = Process.Start(new ProcessStartInfo("doxygen", "-")
      {
        RedirectStandardInput = true
      });
      using (var processStandardInput = process.StandardInput)
      {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        processStandardInput.Write(doxygenConfig, _tempOutputDirectory,
          inputDirectory,
          $"dotnet \"{assemblyLocation}\" {DoxygenFilterArgument}");
      }

      process.WaitForExit();
    }

    private static string GetDoxygenConfig()
    {
      const string configName = nameof(NvimClient) + "." + nameof(APIGenerator)
                                + ".doxygen.config";
      using (var stream = Assembly.GetExecutingAssembly()
        .GetManifestResourceStream(configName))
      {
        using (var reader = new StreamReader(stream))
        {
          return reader.ReadToEnd();
        }
      }
    }
  }
}
