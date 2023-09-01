using System.Text;

namespace Smartclause.SDK.Tools
{
    public static class StringExtensions
    {
        public static string PascalToCamelCase(this string source)
        {
            if (source is null) return null;

            if (source.Length == 0) return string.Empty;

            StringBuilder builder = new();
            builder.Append(char.ToLowerInvariant(source[0]));
            builder.Append(source, 1, source.Length - 1);

            return builder.ToString();
        }
    }
}