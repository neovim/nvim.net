using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MsgPack.Serialization;
using NvimClient.NvimMsgpack;
using NvimClient.NvimMsgpack.Models;
using NvimClient.NvimProcess;

namespace NvimClient
{
  public class NvimAPIGenerator
  {
    private const int OldestSupportedAPILevel = 4;

    private static IEnumerable<NvimFunction> GetNonDeprecatedFunctions(
      IEnumerable<NvimFunction> functions) => functions.Where(function =>
      !function.DeprecatedSince.HasValue
      || function.DeprecatedSince >= OldestSupportedAPILevel);

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

    public static void GenerateCSharpFile(string outputPath)
    {
      var apiMetadata = GetAPIMetadata();
      var csharpClass = GenerateCSharpClass(apiMetadata);
      File.WriteAllText(outputPath, csharpClass);
    }

    private static string GenerateCSharpClass(NvimAPIMetadata apiMetadata) => @"
using System.Threading.Tasks;
using MsgPack;
using NvimClient.NvimMsgpack.Models;

namespace NvimClient.API
{
  public partial class NvimAPI
  {
" + GenerateNvimMethods(
      GetNonDeprecatedFunctions(apiMetadata.Functions)
        .Where(function => !function.Method), "nvim_") + @"
" + GenerateNvimTypes(apiMetadata) + @"
  }
}";

    private static string GenerateNvimTypes(NvimAPIMetadata apiMetadata)
    {
      return string.Join("", apiMetadata.Types.Select(type =>
      {
        var name = "Nvim" + StringUtil.ConvertToCamelCase(type.Key, true);
        return $@"
  public class {name}
  {{
    private readonly NvimAPI _api;
    public {name}(NvimAPI api) => _api = api;
    {
    GenerateNvimMethods(
      GetNonDeprecatedFunctions(apiMetadata.Functions)
        .Where(function => function.Method
                           && function.Name.StartsWith(type.Value.Prefix)),
      type.Value.Prefix)
    }
  }}";
      }));
    }

    private static string GenerateNvimMethods(
      IEnumerable<NvimFunction> functions, string prefixToRemove) =>
      string.Join("", functions.Select(function =>
      {
        if (!function.Name.StartsWith(prefixToRemove))
        {
          throw new Exception(
            $"Function {function.Name} does not "
            + $"have expected prefix \"{prefixToRemove}\"");
        }
        var camelCaseName =
          StringUtil.ConvertToCamelCase(
            function.Name.Substring(prefixToRemove.Length), true);
        var sendAccess = function.Method ? "_api." : string.Empty;
        var returnType = NvimTypesMap.GetCSharpType(function.ReturnType);
        var genericTypeParam =
          returnType == "void" ? string.Empty : $"<{returnType}>";
        var parameters = function.Parameters.Select(param =>
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
        return $@"
    public Task{genericTypeParam} {camelCaseName}({string.Join(", ",
          parameters.Select(param =>
            $"{NvimTypesMap.GetCSharpType(param.Type)} {param.Name}"))}) =>
      {sendAccess}SendAndReceive{genericTypeParam}(new NvimRequest
      {{
        Method = ""{function.Name}"",
        Arguments = new MessagePackObject(new MessagePackObject[]
        {{
            {string.Join(", ",
              parameters.Select(param =>
                $"MessagePackObject.FromObject({param.Name})"))}
        }})
      }});
";
      }));
  }
}
