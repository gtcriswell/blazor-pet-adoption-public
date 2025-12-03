using System.Text;

namespace Shared
{
    public static class CacheHelper
    {

        public const string animals = "animals";
        public const string orgs = "orgs";

        public static string GetKey(string name, params object[] args)
        {
            StringBuilder keyBuilder = new();
            foreach (object arg in args)
            {
                _ = keyBuilder.Append($"{arg}_");
            }

            return $"{name}_{keyBuilder}";
        }
    }
}
