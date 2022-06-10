#nullable enable
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    /// <summary>
    /// Copied from GovUk.Frontend.AspNetCore
    /// </summary>
    internal static class TagHelperAttributeListExtensions
    {
        public static AttributeDictionary ToAttributeDictionary(this TagHelperAttributeList? list)
        {
            var attributeDictionary = new AttributeDictionary();

            if (list != null)
            {
                foreach (var attribute in list)
                {
                    attributeDictionary.Add(
                        attribute.Name,
                        attribute.ValueStyle == HtmlAttributeValueStyle.Minimized ? string.Empty : attribute.Value.ToString());
                }
            }

            return attributeDictionary;
        }
    }
}
