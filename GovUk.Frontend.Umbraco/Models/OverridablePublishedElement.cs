using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Models
{
    /// <summary>
    /// A wrapper for <see cref="IPublishedElement"/> which allows property values to be overriden.
    /// </summary>
    public class OverridablePublishedElement : IPublishedElement, IOverridablePublishedElement
    {
        private readonly IPublishedElement _publishedElement;
        private readonly Dictionary<string, object> _propertyValues = new();

        public OverridablePublishedElement(IPublishedElement publishedElement) => _publishedElement = publishedElement;

        /// <summary>
        /// Gets the content type
        /// </summary>
        public IPublishedContentType ContentType => _publishedElement.ContentType;

        /// <summary>
        /// Gets the unique key of the published element
        /// </summary>
        public Guid Key => _publishedElement.Key;

        /// <summary>
        /// Gets the properties of the element.
        /// </summary>
        /// <remarks>
        /// Contains one <see cref="IPublishedProperty"/> for each property defined by the content type, including inherited properties. Some properties have no value.
        /// </remarks>
        public IEnumerable<IPublishedProperty> Properties => _publishedElement.Properties;

        /// <summary>
        /// Gets a property identified by its alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns>The property identified by the alias</returns>
        /// <remarks>
        /// If a content type has no property with that alias, including inherited properties, returns <c>null</c>.
        /// 
        /// Otherwise return a property -- that may have no value (ie <c>HasValue</c> is <c>false</c>).
        /// 
        /// The alias is case insensitive.
        /// </remarks>
        public IPublishedProperty? GetProperty(string alias) => _publishedElement.GetProperty(alias);

        /// <summary>
        /// Sets a value for the property identified by the alias, which will be returned by <see cref="Value"/> in preference to the value saved in the Umbraco back office.
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="value"></param>
        public void OverrideValue(string alias, object value)
        {
            if (_propertyValues.ContainsKey(alias))
            {
                _propertyValues[alias] = value;
            }
            else
            {
                _propertyValues.Add(alias, value);
            }
        }

        /// <summary>
        /// Gets the value of a content's property identified by its alias, converted to a specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="alias"></param>
        /// <returns></returns>
        /// <remarks>
        /// The value comes a value passed to <see cref="OverrideValue"/>, or from the <see cref="IPublishedProperty"/> field <c>Value</c> ie it is suitable for use when rendering content.
        /// 
        /// If no property with the specified alias exists, or if the property has no value, or if it could not be converted, returns <c>default(T)</c>.
        /// 
        /// The alias is case-insensitive.
        /// </remarks>
        public T? Value<T>(string alias)
        {
            if (_propertyValues.ContainsKey(alias))
            {
                return (T)_propertyValues[alias];
            }

            return _publishedElement != null ? _publishedElement.Value<T>(alias) : default;
        }
    }
}
