using Umbraco.Cms.Core.Dictionary;

namespace GovUk.Frontend.Umbraco.ModelBinding
{
    internal static class CultureDictionaryExtensions
    {
        public static string? ReadValue(this ICultureDictionary cultureDictionary, string key)
        {
            var value = cultureDictionary[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value;
        }
    }
}
