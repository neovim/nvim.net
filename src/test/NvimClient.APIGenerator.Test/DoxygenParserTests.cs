using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NvimClient.APIGenerator.Test;


[TestClass]
public class DoxygenParserTests {
#pragma warning disable CA1707

    [TestMethod]
    public void GetXMLFileNamesFromDoxygenCFilesIndex_SampleDoxygenIndex_ShouldHaveTheCorrectNumberOfCFiles() {
        XDocument indexXml = XDocument.Load("NvimDoxygen/xml/index.xml");
        IEnumerable<string?> xmlFilenames = DoxygenParser.GetXMLFileNamesFromDoxygenCFilesIndex(indexXml);

        List<string?> xmlList = [.. xmlFilenames];

        Assert.AreNotEqual(0, xmlList.Count);
        Assert.HasCount(12, xmlList);
    }

    [TestMethod]
    public void GetNonStaticFunctionDefinitions_SampleDoxygenIndex_ShouldParseCorrectly() {
        XDocument indexXml = XDocument.Load("NvimDoxygen/xml/autocmd_8c.xml");
        IEnumerable<XElement> xmlFilenames = DoxygenParser.GetNonStaticFunctionDefinitions(indexXml);

        List<XElement> xmlList = [.. xmlFilenames];

        Assert.AreNotEqual(0, xmlList.Count);
        Assert.HasCount(8, xmlList);
    }

#pragma warning restore CA1707
}