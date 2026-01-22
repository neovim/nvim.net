using NvimClient.APIGenerator.Docs;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace NvimClient.APIGenerator.Doxygen;

public sealed class DocumentationConverter {

    ///<summary>
    ///    Composes a CSharp element documentation from <see cref="IDocElement"/>
    ///    Doxygen elements
    ///</summary>
    private static IEnumerable<string> GetDocElement(string tag, IEnumerable<IDoxygenElement> elements, string? tagAttributes = null) {


        //This class aims to replace this thing
        //
        //           return $@"{string.Join(string.Empty, GetDocElement("summary", doc?.Summary).Concat(doc?.Parameters.Where(param => function.Parameters.Any(p => p.ArgumentName == param.Name) && (!isVirtualMethod || param.Name != function.Parameters.First().ArgumentName))
        //             .SelectMany(param => GetDocElement("param", param.Description, $@"name=""{StringUtil.ConvertToCamelCase(param.Name, false)}""")) ?? Enumerable.Empty<string>()).Concat(GetDocElement("returns", doc?.Return)).Concat(GetDocElement("remarks", doc?.Notes)).Select(docLine => $@"
        //   /// {docLine}"))}



        if (elements == null) {
            yield break;
        }

        using IEnumerator<string> lineEnumerator = elements.SelectMany(GetDocLines).GetEnumerator();
        if (!lineEnumerator.MoveNext()) {
            // Do not output a tag when it would be empty
            yield break;
        }

        yield return string.IsNullOrEmpty(tagAttributes)
          ? $"<{tag}>"
          : $"<{tag} {tagAttributes}>";
        do {
            yield return lineEnumerator.Current;
        } while (lineEnumerator.MoveNext());

        yield return $"</{tag}>";
    }

    private static IEnumerable<string> GetDocLines(IDoxygenElement element) {

        switch (element) {
            case Paragraph paragraph:
                yield return "<para>";
                foreach (string child in paragraph.Children.SelectMany(GetDocLines)) {
                    yield return child;
                }
                yield return "</para>";
                yield break;
            case InlineCode inlineCode:
                yield return
                  $"<c>${SecurityElement.Escape(inlineCode.ToString())}</c>";
                yield break;
            case DocList list:
                yield return
                  "<list type=\""
                  + (list.ListType == DocListType.ItemizedList ? "bullet" : "number")
                  + "\">";
                foreach (IEnumerable<string> listItemLines in list.Children.Select(GetDocLines)) {
                    yield return "<item><description>";
                    foreach (string line in listItemLines) {
                        yield return line;
                    }

                    yield return "</description></item>";
                }

                yield return "</list>";
                yield break;
            default:
                yield return SecurityElement.Escape(element.ToString()) ?? "";
                yield break;
        }
    }


}