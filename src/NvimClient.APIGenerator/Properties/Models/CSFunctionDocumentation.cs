using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NvimClient.APIGenerator.Properties.Models;

public record CSDocumentation {
    public required string FunctionName { get; set; }
    public string? Summary { get; set; }
    public string[]? ParameterReferences { get; set; }

    public static void WriteIdentation(StringBuilder sb, int identationLevel) {
        int identationWidth = 4;
        for (int i = 0; i < identationLevel; i++) {
            for (int j = 0; j < identationWidth; j++) {
                _ = sb.Append(' ');
            }
        }
    }

    public string? ToCode(int identationLevel) {
        if (Summary is null) {
            return null;
        }
        StringBuilder sb = new();
        string[] lines = Summary.Split('\n');
        foreach (string l in lines) {
            string l1 = l.Replace("\r", string.Empty);
            WriteIdentation(sb, identationLevel);
            _ = sb.Append("/// ").Append(l1).Append('\n');
        }

        if (ParameterReferences is not null && ParameterReferences.Length > 0) {
            Console.WriteLine("Adding {0} Parameters to function documentation for function {1}", ParameterReferences.Length, FunctionName);

            WriteIdentation(sb, identationLevel);
            _ = sb.Append("///\n");

            WriteIdentation(sb, identationLevel);
            _ = sb.Append("///\n");

            foreach (string l in ParameterReferences) {
                string l1 = l.Replace("\r", string.Empty);
                WriteIdentation(sb, identationLevel);
                _ = sb.Append("/// ").Append(l1).Append('\n');
            }
        } else {
            Console.WriteLine("No Parameter to Add to documentation for function {0}", FunctionName);
        }

        return sb.ToString();
    }


    /// <summary>
    /// Converts the Doxygen XML documentation to Csharp doxygen documentation
    /// </summary>
    ///
    /// <param name="memberdef">
    ///     The XML elemenet that is to be conveted this is a memberdef item
    /// </param>
    public static CSDocumentation FromMemberDefXElement(XElement memberdef) {
        //Console.WriteLine(element.Value);

        XElement? detailed_description = memberdef.Element("detaileddescription");
        if (detailed_description is null) {
            throw new InvalidOperationException("Expecting a \"detaileddescription\" tag but none was found");
        }

        XElement summary = ParseDetailedDescription(detailed_description);

        Console.WriteLine("Detailed Description:");
        Console.WriteLine(summary.ToString());


        XElement? paramlist = GetParameterListElement(detailed_description);

        List<string> paramReferences = [];
        if (paramlist is not null) {


            //Popoulate any parameters documentation for the function. Param gets
            //the parameter name and parameter type
            foreach (XElement param in memberdef.Descendants("param")) {
                XElement? param_name = param.Element("declname");
                XElement? param_type = param.Element("type");

                if (param_name is not null && param_type is not null) {
                    //Csharp code is the following --> <param name="name">description</param>
                    XElement? desc = GetParameterDescriptionByName(paramlist, param_name.Value);
                    //In the following path detaileddescription/para/parameterlist we can get information
                    if (desc is not null) {
                        XElement csparam = new("param");
                        csparam.SetAttributeValue("name", param_name.Value);
                        csparam.Add(desc);
                        string s = csparam.ToString();
                        paramReferences.Add(s);
                    }

                }

            }
        }



        return new CSDocumentation() {
            FunctionName = memberdef.Element("name")!.Value,
            Summary = summary.ToString(),
            ParameterReferences = [.. paramReferences]
        };
    }

    public static XElement? GetParameterListElement(XElement detailedDescription) {
        foreach (XElement para_element in detailedDescription.Descendants("para")) {
            foreach (XElement el in para_element.Elements()) {
                if (el.Name.LocalName is "parameterlist") {
                    return el;
                }
            }
        }
        return null;
    }

    public static XElement? GetParameterDescriptionByName(XElement paramlist, string paramname) {
        foreach (XElement parameter_item in paramlist.Elements()) {
            XElement? paramnamelist = parameter_item.Element("parameternamelist");
            if (paramnamelist is null) {
                continue;
            }
            string? inner_name = paramnamelist.Element("parametername")?.Value;

            if (inner_name is null) {
                continue;
            }
            if (inner_name == paramname) {
                return parameter_item.Element("paramaeterdescription");
            }
        }
        return null;
    }

    /// <summary>
    /// Parses a detailed description doxygen ekement into a C Sharp documentation
    /// item summary.
    /// </summary>
    private static XElement ParseDetailedDescription(XElement detailedDescription) {
        XElement result = new(name: "Summary");

        int nu_elements = 0;
        foreach (XElement e in detailedDescription.Elements()) {
            if (e.Name.LocalName is "para") {
                XElement? p = ConvertParagraph(e);
                if (p is not null) {
                    result.Add(p);
                }
            }
            nu_elements++;
        }

        if (nu_elements is 0) {
            result.SetValue(detailedDescription.Value);
        }

        return result;
    }


    /// <summary>
    /// Converts a doxygen paragraph into a
    /// </summary>
    public static XElement? ConvertParagraph(XElement para) {
        if (para.IsEmpty) {
            return null;
        }

        XElement para_result = new("para");

        foreach (XNode node in para.Nodes()) {
            if (node is XElement e) {
                if (e.Name.LocalName is "para") {
                    XElement? inner_para = ConvertParagraph(e);
                    if (inner_para is not null) {
                        para_result.Add(inner_para);
                    }
                }
                if (e.Name.LocalName is "programlisting") {
                    XElement code = ConvertCodeElement(e);
                    para_result.Add(code);
                }
                if (e.Name.LocalName is "simplesect") {
                    XElement el = ConvertSimpleSection(e);
                    para_result.Add(el);
                }
                if (e.Name.LocalName is "orderedlist") {
                    XElement el = ConvertOrderedList(e);
                    para_result.Add(el);
                }
            } else if (node is XText text) {
                para_result.Add(text);
            }
        }

        if (para_result.IsEmpty) {
            return null;
        }

        return DeNestShallowElement(para_result);
    }

    public static XElement ConvertOrderedList(XElement list) {
        XElement cslist = new("list");
        cslist.SetAttributeValue("type", "number");
        foreach (XElement listitem in list.Elements()) {
            XElement? para = listitem.Element("para");
            if (para is not null) {
                XElement? conv_para = ConvertParagraph(para);
                if (conv_para is not null) {
                    XElement it = new("item");
                    it.Add(conv_para);
                    cslist.Add(it);
                }
            }
        }
        return cslist;

    }

    public static XElement ConvertSimpleSection(XElement simplesect) {
        XElement para_result = new("para");
        foreach (XElement e in simplesect.Elements()) {
            if (e.Name.LocalName is "para") {
                XElement? inner_para = ConvertParagraph(e);
                if (inner_para is not null) {
                    para_result.Add(inner_para);
                }
            }
        }
        return para_result;
    }

    public static XElement ConvertCodeElement(XElement codeblock) {
        if (codeblock.Name.LocalName is not "programlisting") {
            throw new InvalidOperationException("Cannot Convert non program listing");
        }

        XElement result = new(name: "Code");

        bool isFirst = true;
        foreach (XElement c in codeblock.Elements()) {
            XText? t = ConvertLuaCodeLineElement(c, isFirst);
            if (t is null) {
                continue;
            }
            result.Add(t);
            isFirst = false;
        }

        return result;
    }

    /// <summary>
    /// Converts a single <c>codeline</c> element into regular source code for C#
    /// code block documentation
    /// </summary>
    public static XText? ConvertLuaCodeLineElement(XElement codeline, bool prependNewLine) {
        if (!codeline.HasElements) {
            Console.WriteLine("Codeline has no elements");
            return null;
        }

        StringBuilder sb = new();
        if (prependNewLine) {
            _ = sb.Append('\n');
        }
        _ = sb.Append("    ");
        int nu_elements = 0;
        foreach (XElement e in codeline.Elements()) {
            foreach (XNode n in e.Nodes()) {
                if (n is XText xtext) {
                    _ = sb.Append(xtext.Value);
                }

                if (n is XElement xel) {
                    if (xel.Name.LocalName is "sp") {
                        _ = sb.Append(' ');
                    }
                }
            }
            nu_elements++;
        }
        XText t = new(sb.Append('\n').ToString());
        return t;
    }

    /// <summary>
    /// Removes single layer nested elements. This denests only elements that
    /// contain only other elements otherwise it leaves them untouched.
    ///
    /// <example>
    ///     This:
    ///     <code>
    ///         <para>
    ///             <para>
    ///                 Hello
    ///             </para>
    ///         </para>
    ///     </code>
    ///     is converted to this:
    ///     <code>
    ///         <para>
    ///             Hello
    ///         </para>
    ///     </code>
    ///
    /// </exmple>
    /// </summary>
    ///
    /// <param name="element">The elemenet to be denested</para>
    public static XElement DeNestShallowElement(XElement element) {

        // Must contain only elements (no text/comments/CDATA/etc.)
        bool containsOnlyElements = element.Nodes().All(static node => node is XElement);
        if (!containsOnlyElements) {
            return element;
        }

        using IEnumerator<XElement> enumerator = element.Elements().GetEnumerator();
        if (!enumerator.MoveNext()) {
            return element; // zero children
        }

        XElement first = enumerator.Current;

        if (enumerator.MoveNext()) {
            return element; // more than one child
        }

        return DeNestShallowElement(first); // exactly one child
    }

}