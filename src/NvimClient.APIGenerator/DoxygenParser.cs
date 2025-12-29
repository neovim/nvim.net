using NvimClient.APIGenerator.Docs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NvimClient.APIGenerator;

/// <summary>
///   Generates and parses XML documentation from Doxygen.
/// </summary>
public sealed class DoxygenParser : IDisposable {
    public const string DoxygenFilterArgument = "--doxygen-filter";
    private readonly string _nvimSrcDirectory;
    private readonly string _tempOutputDirectory;

    public DoxygenParser(string nvimSrcDirectory) {
        _nvimSrcDirectory = nvimSrcDirectory;
        _tempOutputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        _ = Directory.CreateDirectory(_tempOutputDirectory);
    }

    public void Dispose() {
        try {
            Directory.Delete(_tempOutputDirectory, true);
        } catch {
            Console.WriteLine($"Failed to delete temporary directory: {_tempOutputDirectory}");
        }
    }

    public static IEnumerable<string?> GetXMLFileNamesFromDoxygenCFilesIndex(XDocument indexDocument) {

        //Get all compounds of kind file. Where the filename ends in .c but
        //select the refid
        IEnumerable<string?> xmlFilenames = indexDocument
          .Descendants("compound")
          .Where(static node => node.Attribute("kind")?.Value == "file")
          .Where(static node => node.Element("name")?.Value.EndsWith(".c", StringComparison.Ordinal) ?? false)
          .Select(static node => node.Attribute("refid")?.Value);

        return xmlFilenames;
    }

    public static IEnumerable<XElement> GetNonStaticFunctionDefinitions(XDocument document) {

        static bool NonStaticFunctionSelector(XElement element) {
            bool isFunc = element.Attribute("kind")?.Value == "function";
            bool notStatic = element.Attribute("static")?.Value == "no";

            return isFunc && notStatic;
        }

        return document.Descendants("memberdef").Where(NonStaticFunctionSelector);
    }

    internal IEnumerable<FunctionDoc> ParseDoxygenDocumentation() {
        //Inside the temp directory there will be an xml directory that contains all
        //the documentation
        string xmlDocsDirectory = Path.Combine(_tempOutputDirectory, "xml");
        XDocument indexXml = XDocument.Load(Path.Combine(xmlDocsDirectory, "index.xml"));
        IEnumerable<string?> xmlFilenames = GetXMLFileNamesFromDoxygenCFilesIndex(indexXml);

        foreach (string? xmlFilename in xmlFilenames) {
            if (xmlFilename is null) {
                continue;
            }
            string local_file = Path.Combine(xmlDocsDirectory, xmlFilename + ".xml");
            Console.WriteLine("Processing File: {0} will read file {1}", xmlFilename, local_file);

            XDocument docXml;
            try {
                docXml = XDocument.Load(local_file);
            } catch {
                Console.WriteLine("Could not load file {0} trying conversion...", local_file);
                UTF8Encoder.ConvertIso88591FileToUtf8(local_file, local_file);
                docXml = XDocument.Load(local_file);
                Console.WriteLine("Reloading with conversion successfull!");
            }

            IEnumerable<XElement> doxFunctionDocs = GetNonStaticFunctionDefinitions(docXml);

            //Omit Dict(cmd) for now. TODO: see what this does

            IEnumerable<FunctionDoc> functionDocs = doxFunctionDocs.Where(ele => {
                //Ommit empty types
                XElement? typeElement = ele.Element("type");
                if (typeElement is null) {
                    return false;
                }
                return !string.IsNullOrWhiteSpace(typeElement.Value);
            }).Select(ele => FunctionDoc.FromXElement(ele, local_file));

            foreach (FunctionDoc functionDoc in functionDocs) {
                yield return functionDoc;
            }
        }
    }

    public static IEnumerable<IDocElement> GetDocElements(IEnumerable<XNode>? nodes) {
        if (nodes == null) {
            yield break;
        }

        foreach (XNode node in nodes) {
            switch (node) {
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
    internal static void FilterDoxygenInput(string filePath) {
        using FileStream fileStream = File.OpenRead(filePath);
        using StreamReader streamReader = new(fileStream);
        string? line;
        while ((line = streamReader.ReadLine()) is not null) {
            Console.WriteLine(
              Regex.Replace(line, @"^(ArrayOf|DictionaryOf)(\(.*?\))",
                static m => m.Groups[1]
                     + string.Join('_',
                       Regex.Split(m.Groups[2].Value, @"[^\w]+")))
            );
        }
    }

    /// <summary>
    /// Generates documentation by invoking the doxygen executable on the nvim
    /// source
    /// </summary>
    public void CallDoxygenDocumentationGenerationProcess() {
        string doxygenConfig = GetDoxygenConfig();

        Console.WriteLine("Configuration Read is: {0}", doxygenConfig);
        string inputDirectory = Path.Combine(_nvimSrcDirectory, "src/nvim/api");

        //the - argument tells Doxygen to read its configuration from standard input
        //instead of a file. We also take over the standard input and write the configuration
        //template with the items replaced.
        ProcessStartInfo doxy_process = new(fileName: "doxygen", arguments: "-") {
            RedirectStandardInput = true
        };

        Process? process = Process.Start(doxy_process);
        if (process is null) {
            Console.WriteLine("Could not start doxygen process!");
            return;
        }


        using (StreamWriter processStandardInput = process.StandardInput) {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string filter = $"dotnet \\\"{assemblyLocation}\\\" {DoxygenFilterArgument}";
            processStandardInput.Write(
                    doxygenConfig,
                    _tempOutputDirectory,
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
        Console.WriteLine("Reading configuration from: {0}", configName);
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(configName);
        if (stream is null) {
            throw new InvalidOperationException("Could not retreive Manifest resource stream of the the executing assembly");
        }
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}