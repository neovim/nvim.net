using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using MsgPack.Serialization;
using NvimClient.APIGenerator.Docs;
using NvimClient.NvimMsgpack;
using NvimClient.NvimMsgpack.Models;
using NvimClient.NvimProcess;

namespace NvimClient
{
  public class NvimAPIGenerator
  {
    private static Dictionary<string, FunctionDoc> _functionDocs;
    private const int OldestSupportedAPILevel = 4;

    private static bool IsDeprecated<T>(T functionOrEvent)
      where T : NvimFunctionEventBase =>
      functionOrEvent.DeprecatedSince < OldestSupportedAPILevel;

    public static NvimAPIMetadata GetAPIMetadata()
    {
      var process = Process.Start(
        new NvimProcessStartInfo(StartOption.ApiInfo | StartOption.Headless));

      var context = new SerializationContext();
      context.DictionarySerlaizationOptions.KeyTransformer =
        StringUtil.ConvertToSnakeCase;
      var serializer = context.GetSerializer<NvimAPIMetadata>();
      var apiMetadata = serializer.Unpack(process.StandardOutput.BaseStream);
      return apiMetadata;
    }

    public static void GenerateCSharpFile(string outputPath,
      IEnumerable<FunctionDoc> functionDocs)
    {
      _functionDocs = functionDocs?.ToDictionary(functionDoc => functionDoc.Function,
        funcDoc => funcDoc);
      var apiMetadata = GetAPIMetadata();

      // Filter out functions only callable from Lua.
      apiMetadata.Functions = apiMetadata.Functions.Where(f => !f.Parameters.Where(p => p.Type == "LuaRef").Any()).ToArray();

      var csharpClass = GenerateCSharpClass(apiMetadata);
      File.WriteAllText(outputPath, csharpClass);
    }

    private static string GenerateCSharpClass(NvimAPIMetadata apiMetadata) => @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MsgPack;
using NvimClient.NvimMsgpack.Models;

namespace NvimClient.API
{
  public partial class NvimAPI
  {
" +
    GenerateNvimUIEvents(
      apiMetadata.UIEvents.Where(uiEvent => !IsDeprecated(uiEvent))) + @"
" + GenerateNvimMethods(
      apiMetadata.Functions.Where(function =>
        !IsDeprecated(function) && !function.Method),
      "nvim_", false) + @"
" + GenerateNvimTypes(apiMetadata) + @"
" + GenerateNvimUIEventArgs(
      apiMetadata.UIEvents.Where(uiEvent => !IsDeprecated(uiEvent))) + @"
    private void CallUIEventHandler(string eventName, object[] args)
    {
      switch (eventName)
      {
  " + GenerateNvimUIEventCalls(
        apiMetadata.UIEvents.Where(uiEvent => !IsDeprecated(uiEvent))) + @"
      }
    }

    private object GetExtensionType(MessagePackExtendedTypeObject msgPackExtObj)
    {
      switch (msgPackExtObj.TypeCode)
      {
" + GenerateNvimTypeCases(apiMetadata.Types) + @"
        default:
          throw new SerializationException(
            $""Unknown extension type id {msgPackExtObj.TypeCode}"");
      }
    }
  }
}";

    private static string GenerateNvimUIEventCalls(
      IEnumerable<NvimUIEvent> uiEvents) =>
      string.Join("", uiEvents.Select(uiEvent =>
      {
        var camelCaseName = StringUtil.ConvertToCamelCase(uiEvent.Name, true);
        var eventArgs = uiEvent.Parameters.Any()
          ? $@"new {camelCaseName}EventArgs
          {{
{
              string.Join(",\n", uiEvent.Parameters.Select((param, index) =>
              {
                var name = StringUtil.ConvertToCamelCase(param.Name, true);
                var type = NvimTypesMap.GetCSharpType(param.Type);
                return $@"            {name} = ({type}) args[{index}]";
              }))
            }
          }}"
          : "EventArgs.Empty";
        return $@"
      case ""{uiEvent.Name}"":
          {camelCaseName}?.Invoke(this, {eventArgs});
          break;
";
      }));

    private static string GenerateNvimUIEvents(
      IEnumerable<NvimUIEvent> uiEvents) =>
      string.Join("\n",
        uiEvents.Select(uiEvent =>
        {
          var camelCaseName = StringUtil.ConvertToCamelCase(uiEvent.Name, true);
          var genericTypeParam = uiEvent.Parameters.Any()
            ? $"<{camelCaseName}EventArgs>"
            : string.Empty;
          return $"    public event EventHandler{genericTypeParam} {camelCaseName};";
        }));

    private static string GenerateNvimUIEventArgs(
      IEnumerable<NvimUIEvent> uiEvents) =>
      string.Join("", uiEvents.Where(uiEvent => uiEvent.Parameters.Any())
                              .Select(uiEvent =>
      {
        var eventName = StringUtil.ConvertToCamelCase(uiEvent.Name, true);
        return $@"
  public class {eventName}EventArgs : EventArgs
  {{
{
      string.Join("", uiEvent.Parameters.Select(param => {
        var type = NvimTypesMap.GetCSharpType(param.Type);
        var paramName = StringUtil.ConvertToCamelCase(param.Name, true);
        return $"    public {type} {paramName} {{ get; set; }}\n";
      }))
}
  }}";
      }));

    private static string GenerateNvimTypes(NvimAPIMetadata apiMetadata)
    {
      return string.Join("", apiMetadata.Types.Select(type =>
      {
        var name = "Nvim" + StringUtil.ConvertToCamelCase(type.Key, true);
        return $@"
  public class {name}
  {{
    private readonly NvimAPI _api;
    private readonly MessagePackExtendedTypeObject _msgPackExtObj;
    internal {name}(NvimAPI api, MessagePackExtendedTypeObject msgPackExtObj)
    {{
      _api = api;
      _msgPackExtObj = msgPackExtObj;
    }}
    {
    GenerateNvimMethods(
      apiMetadata.Functions.Where(function =>
        !IsDeprecated(function) && function.Method
        && function.Name.StartsWith(type.Value.Prefix)),
      type.Value.Prefix, true)
    }
  }}";
      }));
    }

    private static string GenerateNvimMethods(
      IEnumerable<NvimFunction> functions, string prefixToRemove,
      bool isVirtualMethod) =>
      string.Join("", functions.Select(function =>
      {
        if (!function.Name.StartsWith(prefixToRemove))
        {
          throw new Exception(
            $"Function {function.Name} does not "
            + $"have expected prefix \"{prefixToRemove}\"");
        }

        FunctionDoc doc = null;
        _functionDocs?.TryGetValue(function.Name, out doc);
        var camelCaseName =
          StringUtil.ConvertToCamelCase(
            function.Name.Substring(prefixToRemove.Length), true);
        var sendAccess = function.Method ? "_api." : string.Empty;
        var returnType = NvimTypesMap.GetCSharpType(function.ReturnType);
        var genericTypeParam =
          returnType == "void" ? string.Empty : $"<{returnType}>";
        var parameters =
          (isVirtualMethod ? function.Parameters.Skip(1) : function.Parameters)
          .Select(param =>
          new
          {
            param.Type,
            // Prefix every parameter name with the verbatim identifier `@`
            // to prevent them from being interpreted as keywords.
            // In the future, it might be worth considering adding a list
            // of all C# keywords and only adding the prefix to parameter
            // names that are in the list.
            Name = "@" + StringUtil.ConvertToCamelCase(param.Name, false)
          }).ToArray();

        return $@"{string.Join(string.Empty,
          GetDocElement("summary", doc?.Summary).Concat(
            doc?.Parameters
              .Where(param =>
                function.Parameters.Any(p => p.Name == param.Name)
                && (!isVirtualMethod
                    || param.Name != function.Parameters.First().Name))
              .SelectMany(param =>
                GetDocElement("param", param.Description,
                  $@"name=""{
                    StringUtil.ConvertToCamelCase(param.Name, false)}"""))
            ?? Enumerable.Empty<string>()).Concat(
            GetDocElement("returns", doc?.Return)).Concat(
            GetDocElement("remarks", doc?.Notes)).Select(docLine => $@"
    /// {docLine}"))}
    public Task{genericTypeParam} {camelCaseName}({string.Join(", ",
          parameters.Select(param =>
            $"{NvimTypesMap.GetCSharpType(param.Type)} {param.Name}"))}) =>
      {sendAccess}SendAndReceive{genericTypeParam}(new NvimRequest
      {{
        Method = ""{function.Name}"",
        Arguments = GetRequestArguments(
          {string.Join(", ",
            (isVirtualMethod ? new[] {"_msgPackExtObj"} : Enumerable.Empty<string>())
            .Concat(parameters.Select(param => param.Name)))})
      }});
";
      }));

    private static IEnumerable<string> GetDocElement(string tag,
      IEnumerable<IDocElement> elements, string tagAttributes = null)
    {
      if (elements == null)
      {
        yield break;
      }

      using (var lineEnumerator =
        elements.SelectMany(GetDocLines).GetEnumerator())
      {
        if (!lineEnumerator.MoveNext())
        {
          // Do not output a tag when it would be empty
          yield break;
        }

        yield return string.IsNullOrEmpty(tagAttributes)
          ? $"<{tag}>"
          : $"<{tag} {tagAttributes}>";
        do
        {
          yield return lineEnumerator.Current;
        } while (lineEnumerator.MoveNext());

        yield return $"</{tag}>";
      }
    }

    private static IEnumerable<string> GetDocLines(IDocElement element)
    {
      switch (element)
      {
        case Paragraph paragraph:
          yield return "<para>";
          foreach (var child in paragraph.Children.SelectMany(GetDocLines))
          {
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
          foreach (var listItemLines in list.Children.Select(GetDocLines))
          {
            yield return "<item><description>";
            foreach (var line in listItemLines)
            {
              yield return line;
            }

            yield return "</description></item>";
          }

          yield return "</list>";
          yield break;
        default:
          yield return SecurityElement.Escape(element.ToString());
          yield break;
      }
    }

    private static string
      GenerateNvimTypeCases(Dictionary<string, NvimType> apiMetadata) =>
      string.Join(string.Empty, apiMetadata.Select(type => $@"
        case {type.Value.Id}:
          return new Nvim{
            StringUtil.ConvertToCamelCase(type.Key, true)
            }(this, msgPackExtObj);")
      );
  }
}
