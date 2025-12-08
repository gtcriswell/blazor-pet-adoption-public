using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Shared
{
    public static class NavigationManagerExtensions
    {
        public static string? QueryString(this NavigationManager nav, string key)
        {
            Uri uri = nav.ToAbsoluteUri(nav.Uri);
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query = QueryHelpers.ParseQuery(uri.Query);

            return query.TryGetValue(key, out Microsoft.Extensions.Primitives.StringValues value)
                ? value.ToString()
                : null;
        }
    }
}