using System.Text.RegularExpressions;
using System.Text;

namespace Core.Extentions
{
    public static class StringExtentions
    {
        public static string ToSnakeCase(this string input, char separator = '_')
        {
            string pattern = @"[A-Z][a-z]*";
            MatchCollection matches = Regex.Matches(input, pattern);
            string [] words = new string [matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                words [i] = matches [i].Value.ToUpper();
            }

            return string.Join(separator, words);
        }

        public static string? ToCamelCase(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return char.ToLowerInvariant(str [0]) + str[1..];
        }

        public static int? ToIntNullable(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return int.TryParse(str, out var val) ? val : null;
        }

        public static int ToInt(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default;
            }
            return int.TryParse(str, out var val) ? val : default;
        }

        public static byte [] ConvertStringToByteArray(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static bool IsNullOrEmpty(this string? str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
