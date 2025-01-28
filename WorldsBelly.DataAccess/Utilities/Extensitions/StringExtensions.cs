using System;

namespace WorldsBelly.DataAccess.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceLast(this string str, string find, string replace)
        {
            var lastIndex = str.LastIndexOf(find, StringComparison.Ordinal);

            return lastIndex == -1 ? str : str.Remove(lastIndex, find.Length).Insert(lastIndex, replace);
        }

        public static bool IsNotNull(this string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }

        public static bool IsNull(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }
    }
}
