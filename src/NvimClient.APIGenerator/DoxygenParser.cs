using NvimClient.APIGenerator.Docs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace NvimClient.APIGenerator;

/// <summary>
///   Call the doxygen process and Generates and generates XML documentation.
///   This class also provides methods to parse generated XML documentation
///   from Doxygen.
/// </summary>
public sealed class DoxygenParser {

    private readonly string _xmlDirectory;

    public DoxygenParser(string xmlDocsDirectory) {
        _xmlDirectory = xmlDocsDirectory;
    }



    ///<summary>
    ///     Parse the complete doxygen documentation
    ///</summary>
    ///
    ///<returns>
    ///     A <see cref="List{FunctionDoc}"/> that contains the documentation of
    ///     the functions.
    ///</returns>
    internal List<FunctionDoc> ParseDoxygenDocumentation() {
        //Inside the temp directory there will be an xml directory that contains all
        //the documentation
        string pa = Path.Combine(_xmlDirectory, "xml", "index.xml");
        Console.WriteLine("Reading doxygen files from {0}", pa);
        XDocument indexXml = XDocument.Load(pa);
        IEnumerable<string?> xmlFilenames = GetXMLFileNamesFromDoxygenCFilesIndex(indexXml);

        List<FunctionDoc> results = [];

        foreach (string? xmlFilename in xmlFilenames) {
            if (xmlFilename is null) {
                continue;
            }
            string local_file = Path.Combine(_xmlDirectory, "xml", $"{xmlFilename}.xml");
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

            results.AddRange(functionDocs);
        }

        return results;
    }


    ///<summary>
    ///     Retreives the XML files that constitute the complete documentation set
    ///     of this project.
    ///
    ///     <paramref name="indexDocument">
    ///         The <see cref="XDocument"/> of the index.xml file as loaded.
    ///     </paramref>
    ///</summary>
    public static IEnumerable<string?> GetXMLFileNamesFromDoxygenCFilesIndex(XDocument indexDocument) {

        //Get inside all compound nodes and get chilred. Select the file kind
        //where the source filename ends in .c but select the refid which is
        //the name of the xml file that needs to be parsed.
        IEnumerable<string?> xmlFilenames = indexDocument
          .Descendants("compound")
          .Where(static node => node.Attribute("kind")?.Value == "file")
          .Where(static node => node.Element("name")?.Value.EndsWith(".c", StringComparison.Ordinal) ?? false)
          .Select(static node => node.Attribute("refid")?.Value);

        return xmlFilenames;
    }

    ///<summary>
    ///     Retrieves all the <see langword="public"/> c functions from the given
    ///     <see cref="XDocument"/>.
    ///</summary>
    public static IEnumerable<XElement> GetNonStaticFunctionDefinitions(XDocument document) {
        return document.Descendants("memberdef").Where(IsNonStaticFunction);
    }

    ///<summary>
    ///     Indicdes if the <see cref="XElement"/> represents a non static function.
    ///     In C static functions are <see langword="private"/> functions.
    ///</summary>
    private static bool IsNonStaticFunction(XElement element) {
        bool isFunc = element.Attribute("kind")?.Value == "function";
        bool notStatic = element.Attribute("static")?.Value == "no";

        return isFunc && notStatic;
    }

    public static IEnumerable<IDoxygenElement> GetDocElements(IEnumerable<XNode>? nodes) {
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

}