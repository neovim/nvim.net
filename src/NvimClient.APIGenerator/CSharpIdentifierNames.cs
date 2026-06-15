using System.Text.RegularExpressions;

namespace NvimClient
{
  internal static class CSharpIdentifierNames
  {
    private static readonly Regex SnakeCaseWordStart = new("(^|_)(.)");
    private static readonly Regex SnakeCaseWordSeparator = new("_(.)");

    public static string FromSnakeCaseToPascalCase(string name) =>
      SnakeCaseWordStart.Replace(
        name,
        match => match.Groups[2].Value.ToUpperInvariant()
      );

    public static string FromSnakeCaseToCamelCase(string name) =>
      SnakeCaseWordSeparator.Replace(
        name,
        match => match.Groups[1].Value.ToUpperInvariant()
      );
  }
}
