using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Nvim.Client.Generator.Api;

namespace Nvim.Client.Generator.CSharp;

internal static class ManagedNames
{
  internal static string MethodNameFor(string name) =>
    PascalIdentifierFor(
      name.StartsWith("nvim_", StringComparison.Ordinal)
        ? name.Substring(5)
        : name
    );

  internal static ImmutableArray<string> ToParameterNames(
    this IEnumerable<RpcParameter> parameters
  )
  {
    var used = new HashSet<string>(StringComparer.Ordinal);
    var result = ImmutableArray.CreateBuilder<string>();
    foreach (var parameter in parameters)
    {
      var root = PascalIdentifierFor(parameter.Name);
      var name = root;
      for (var suffix = 1; !used.Add(name); suffix++)
        name = root + suffix.ToString(CultureInfo.InvariantCulture);
      result.Add(name);
    }

    return result.ToImmutable();
  }

  internal static string PascalIdentifierFor(string name)
  {
    var parts = name.Split('_', StringSplitOptions.RemoveEmptyEntries);
    var value = string.Concat(
      parts.Select(part => char.ToUpperInvariant(part[0]) + part[1..])
    );
    return string.IsNullOrWhiteSpace(value) ? "Generated"
      : SyntaxFacts.GetKeywordKind(value) != SyntaxKind.None ? "@" + value
      : value;
  }
}
