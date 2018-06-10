using System.Text;

namespace NvimClient
{
  /// <summary>
  /// Provides methods for manipulating strings.
  /// </summary>
  public static class StringUtil
  {
    /// <summary>
    /// Converts a camelCase string to snake_case.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    public static string ConvertToSnakeCase(string str)
    {
      if (string.IsNullOrEmpty(str))
      {
        return str;
      }

      var enumerator = str.GetEnumerator();
      enumerator.MoveNext();
      var firstChar     = enumerator.Current;
      var stringBuilder = new StringBuilder(str.Length * 2);

      AppendWithUnderscores(firstChar, false);

      void AppendWithUnderscores(char previousChar, bool previousCharsUpper)
      {
        if (!enumerator.MoveNext())
        {
          stringBuilder.Append(char.ToLower(previousChar));
          return;
        }

        var currentChar       = enumerator.Current;
        var currentCharUpper  = !char.IsLower(currentChar);
        var previousCharUpper = !char.IsLower(previousChar);
        if (currentCharUpper && !previousCharUpper)
        {
          stringBuilder.Append(previousChar);
          stringBuilder.Append('_');
          stringBuilder.Append(char.ToLower(currentChar));

          if (!enumerator.MoveNext())
          {
            return;
          }
        }
        else if (!currentCharUpper && previousCharUpper && previousCharsUpper)
        {
          stringBuilder.Append('_');
          stringBuilder.Append(char.ToLower(previousChar));
          stringBuilder.Append(currentChar);

          if (!enumerator.MoveNext())
          {
            return;
          }
        }
        else
        {
          stringBuilder.Append(char.ToLower(previousChar));
        }

        AppendWithUnderscores(enumerator.Current,
          currentCharUpper && previousCharUpper);
      }

      return stringBuilder.ToString();
    }

    /// <summary>
    /// Converts a snake_case string to camelCase.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <param name="capitalizeFirstChar">
    /// Whether or not the first character should be capitalized.
    /// </param>
    public static string ConvertToCamelCase(string str,
      bool capitalizeFirstChar)
    {
      if (string.IsNullOrEmpty(str))
      {
        return str;
      }

      var enumerator    = str.GetEnumerator();
      var stringBuilder = new StringBuilder(str.Length);

      AppendFirstChar();

      void AppendFirstChar()
      {
        if (!enumerator.MoveNext())
        {
          return;
        }

        var currentChar  = enumerator.Current;
        var isUnderscore = currentChar == '_';
        if (!isUnderscore)
        {
          stringBuilder.Append(capitalizeFirstChar
            ? char.ToUpper(currentChar)
            : char.ToLower(currentChar));
          return;
        }

        AppendFirstChar();
      }

      AppendWithUpperChars(false);

      void AppendWithUpperChars(bool previousUnderscore)
      {
        if (!enumerator.MoveNext())
        {
          return;
        }

        var currentChar  = enumerator.Current;
        var isUnderscore = currentChar == '_';
        if (!isUnderscore)
        {
          stringBuilder.Append(previousUnderscore
            ? char.ToUpper(currentChar)
            : char.ToLower(currentChar));
        }

        AppendWithUpperChars(isUnderscore);
      }

      return stringBuilder.ToString();
    }
  }
}
