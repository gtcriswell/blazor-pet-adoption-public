namespace Shared
{
    public static class ObjectExtensions
    {
        public static int ToInt(this string? value, int defaultValue = 0)
        {
            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        public static string ToPhoneNumber(this string? value, string defaultValue = "")
        {
            if (value == null)
            {
                return defaultValue;
            }

            string str = value.ToString();

            if (string.IsNullOrWhiteSpace(str))
            {
                return defaultValue;
            }

            string numeric = new(str.Where(char.IsDigit).ToArray());

            if (numeric.Length > 10)
            {
                str = str[..10];
            }

            if (numeric.Length == 10)
            {
                string formatted = $"({numeric[..3]}) {numeric.Substring(3, 3)}-{numeric.Substring(6, 4)}";
                return formatted;
            }

            return str.Trim();
        }

        public static string ToTrimmedString(this object? value, string defaultValue = "")
        {
            if (value == null)
            {
                return defaultValue;
            }

            string? str = value.ToString();

            return string.IsNullOrWhiteSpace(str) ? defaultValue : str.Trim();
        }
    }
}
