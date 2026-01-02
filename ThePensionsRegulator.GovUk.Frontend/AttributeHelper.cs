using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Web;

namespace ThePensionsRegulator.GovUk.Frontend
{
    /// <summary>
    /// Copied from GovUk.Frontend.AspNetCore
    /// </summary>
    public static class AttributeHelper
    {
        /// <summary>
        /// Creates an <see cref="AttributeDictionary"/> from a <see cref="TagHelperAttributeList"/>.
        /// </summary>
        /// <param name="list">The <see cref="TagHelperAttributeList"/> to retrieve attributes from.</param>
        public static AttributeDictionary ToAttributeDictionary(this TagHelperAttributeList? list)
        {
            var attributeDictionary = new AttributeDictionary();

            if (list != null)
            {
                foreach (var attribute in list)
                {
                    attributeDictionary.Add(
                        attribute.Name,
                        attribute.ValueStyle == HtmlAttributeValueStyle.Minimized ?
                            string.Empty :
                            attribute.Value is HtmlString htmlString ? HttpUtility.HtmlDecode(htmlString.Value) :
                            (attribute.Value ?? string.Empty).ToString());
                }
            }

            return attributeDictionary;
        }

        internal static AttributeDictionary MergeAttribute(
        this AttributeDictionary attributes,
        string key,
        object value)
        {
            if (value == null)
            {
                return attributes;
            }

            var newValue = AttributeValueToString(value);
            string? mergedValue;

            if (attributes.ContainsKey(key))
            {
                if (key == "class" || key == "aria-describedby")
                {
                    mergedValue = attributes[key] + " " + newValue;
                }
                else
                {
                    throw new InvalidOperationException($"Don't know how to merge attributes with key '{key}'.");
                }
            }
            else
            {
                mergedValue = newValue;
            }

            attributes[key] = mergedValue;

            return attributes;
        }

        internal static AttributeDictionary ToAttributeDictionary(this IDictionary<string, string?>? dictionary)
        {
            var attributeDictionary = new AttributeDictionary();

            if (dictionary != null)
            {
                foreach (var kvp in dictionary)
                {
                    attributeDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            return attributeDictionary;
        }

        internal static string? AttributeValueToString(object value) => value switch
        {
            bool b => b ? "true" : "false",
            _ => value.ToString()
        };
    }
}
