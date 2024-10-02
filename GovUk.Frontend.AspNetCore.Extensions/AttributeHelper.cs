using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    /// <summary>
    /// Copied from GovUk.Frontend.AspNetCore
    /// </summary>
    internal static class AttributeHelper
    {
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
