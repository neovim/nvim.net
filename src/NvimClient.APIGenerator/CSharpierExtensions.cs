using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CSharpier.Core;
using Microsoft.CodeAnalysis;

internal static class CSharpierExtensions
{
  extension(CodeFormatterResult codeFormatterResult)
  {
    public CSharpierResult ToCSharpierResult()
    {
      Exception[] errors = codeFormatterResult
        .CompilationErrors.Where(error =>
          error.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error
        )
        .Select(error => new Exception(error.ToString()))
        .ToArray();
      return errors.Any()
        ? CSharpierResult.Failure(new(errors))
        : CSharpierResult.Success(codeFormatterResult.Code);
    }
  }
}

internal readonly record struct CSharpierResult
{
  public string Code { get; }

  public AggregateException? Exception { get; }

  [MemberNotNullWhen(true, nameof(Exception))]
  public bool IsExceptional { get; }

  private CSharpierResult(
    string code,
    AggregateException? exception,
    bool isExceptional
  )
  {
    Code = code;
    Exception = exception;
    IsExceptional = isExceptional;
  }

  public static CSharpierResult Success(string code) => new(code, null, false);

  public static CSharpierResult Failure(AggregateException exception) =>
    new(string.Empty, exception, true);
}
