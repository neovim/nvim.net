using System.Globalization;
using System.Text;

namespace NvimClient;

/// <summary>
/// Provides methods for manipulating strings.
/// </summary>
public static class StringUtil {
    /// <summary>
    /// Converts a camelCase string to snake_case.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    public static string ConvertToSnakeCase(string str) {
        if (string.IsNullOrEmpty(str)) {
            return str;
        }

        System.CharEnumerator enumerator = str.GetEnumerator();
        _ = enumerator.MoveNext();
        char firstChar = enumerator.Current;
        StringBuilder stringBuilder = new(str.Length * 2);

        AppendWithUnderscores(firstChar, false);

        void AppendWithUnderscores(char previousChar, bool previousCharsUpper) {
            if (!enumerator.MoveNext()) {
                _ = stringBuilder.Append(char.ToLower(previousChar, CultureInfo.InvariantCulture));
                return;
            }

            char currentChar = enumerator.Current;
            bool currentCharUpper = !char.IsLower(currentChar);
            bool previousCharUpper = !char.IsLower(previousChar);
            if (currentCharUpper && !previousCharUpper) {
                _ = stringBuilder.Append(previousChar);
                _ = stringBuilder.Append('_');
                _ = stringBuilder.Append(char.ToLower(currentChar, CultureInfo.InvariantCulture));

                if (!enumerator.MoveNext()) {
                    return;
                }
            } else if (!currentCharUpper && previousCharUpper && previousCharsUpper) {
                _ = stringBuilder.Append('_');
                _ = stringBuilder.Append(char.ToLower(previousChar, CultureInfo.InvariantCulture));
                _ = stringBuilder.Append(currentChar);

                if (!enumerator.MoveNext()) {
                    return;
                }
            } else {
                _ = stringBuilder.Append(char.ToLower(previousChar, CultureInfo.InvariantCulture));
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
      bool capitalizeFirstChar) {
        if (string.IsNullOrEmpty(str)) {
            return str;
        }

        System.CharEnumerator enumerator = str.GetEnumerator();
        StringBuilder stringBuilder = new(str.Length);

        AppendFirstChar();

        void AppendFirstChar() {
            if (!enumerator.MoveNext()) {
                return;
            }

            char currentChar = enumerator.Current;
            bool isUnderscore = currentChar == '_';
            if (!isUnderscore) {
                _ = stringBuilder.Append(capitalizeFirstChar ? char.ToUpper(currentChar, CultureInfo.InvariantCulture) : char.ToLower(currentChar, CultureInfo.InvariantCulture));
                return;
            }

            AppendFirstChar();
        }

        AppendWithUpperChars(false);

        void AppendWithUpperChars(bool previousUnderscore) {
            if (!enumerator.MoveNext()) {
                return;
            }

            char currentChar = enumerator.Current;
            bool isUnderscore = currentChar == '_';
            if (!isUnderscore) {
                _ = stringBuilder.Append(previousUnderscore ? char.ToUpper(currentChar, CultureInfo.InvariantCulture) : char.ToLower(currentChar, CultureInfo.InvariantCulture));
            }

            AppendWithUpperChars(isUnderscore);
        }

        return stringBuilder.ToString();
    }
}