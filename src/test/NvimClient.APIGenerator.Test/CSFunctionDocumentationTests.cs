using Microsoft.VisualStudio.TestTools.UnitTesting;
using NvimClient.APIGenerator.Properties.Models;
using System.Xml.Linq;

namespace NvimClient.APIGenerator.Test;


[TestClass]
public class CSFunctionDocumentationTests {

    [TestMethod]
    public void ConvertCodeElement_TypicalElement_ShouldConvertCorrectly() {
        XElement input = XElementProvider.GetSourceElement();
        XElement a = CSDocumentation.ConvertCodeElement(input);
        System.Console.WriteLine(a.ToString());
        Assert.AreEqual("Code", a.Name.LocalName);
        Assert.IsTrue(a.HasElements);
        Assert.IsFalse(a.HasElements);
    }

    [TestMethod]
    public void ConvertMemberDef_SampleMemberDef_ShouldConvertCorrectly() {
        XElement input = XElementProvider.GetFullMemberDefElement();
        CSDocumentation doc = CSDocumentation.FromMemberDefXElement(input);
        System.Console.WriteLine(doc);
        Assert.IsNull(doc.Summary);
        Assert.IsNotNull(doc.Summary);
    }

    [TestMethod]
    public void ConvertMemberDef_ArtificialySimpleDef_ShouldConvertCorrectly() {
        XElement input = XElementProvider.GetBasicMemberdefElement()!;
        CSDocumentation doc = CSDocumentation.FromMemberDefXElement(input);
        System.Console.WriteLine(doc);
        Assert.IsNull(doc.Summary);
        Assert.IsNotNull(doc.Summary);
    }

    [TestMethod]
    public void ConvertPara_ParaWithTextAndList_ShouldConvertCorrectly() {
        XElement input = XElementProvider.ParaWithTextAndList();
        XElement? para = CSDocumentation.ConvertParagraph(input);

        Assert.IsNotNull(para);
        Assert.IsFalse(para.IsEmpty);
        System.Console.WriteLine(para.ToString());
        Assert.IsFalse(true);

    }

    [TestMethod]
    public void DeNestElement() {
        string source = """
            <para>
                <para>
                    <para>
                        This was originally a deeply nested element
                    </para>
                </para>
            </para>
            """;

        XElement input = XElement.Parse(source);
        XElement actual = CSDocumentation.DeNestShallowElement(input);
        System.Console.WriteLine(actual.ToString());

        string expect_source = """
            <para>
                This was originally a deeply nested element
            </para>
            """;
        XElement expected = XElement.Parse(expect_source);

        Assert.IsTrue(XNode.DeepEquals(actual, expected));

    }


}