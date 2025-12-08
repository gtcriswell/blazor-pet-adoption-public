using System.Web;

namespace Shared
{
    public static class Utilities
    {

        public static string BuildUrl(string baseUrl, params Dictionary<object, object>[] dicts)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("Base URL cannot be null or empty.", nameof(baseUrl));
            }

            Dictionary<string, string> combined = [];

            // Merge all dictionaries
            foreach (Dictionary<object, object> dict in dicts)
            {
                if (dict == null)
                {
                    continue;
                }

                foreach (KeyValuePair<object, object> kv in dict)
                {
                    combined[kv.Key.ToTrimmedString()] = kv.Value.ToTrimmedString();  // overwrite duplicate keys
                }
            }

            // Build final query string
            string query = string.Join("&",
                combined
                    .Where(kv => kv.Value != null)
                    .Select(kv => $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(kv.Value)}")
            );

            // Combine with base URL
            return string.IsNullOrWhiteSpace(query)
                ? baseUrl
                : $"{baseUrl}?{query}";
        }
    }
}
