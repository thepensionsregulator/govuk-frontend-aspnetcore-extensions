using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco
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
        /// Property value formatters which may be applied when a property is overridden with a new value.
        /// </summary>
        /// <remarks>
        /// This should remain internal and is intended to be set by <see cref="OverridableBlockListPropertyValueConverter"/> to pass down via <see cref="OverridableBlockListModel"/>,
        /// because the property value converter is the nearest place that can inject the property value formatters registered with the dependency injection container.
        /// </remarks>
        internal IEnumerable<IPropertyValueFormatter>? PropertyValueFormatters { get; set; }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        public IPublishedContentType ContentType => _publishedElement.ContentType;

        /// <summary>
        /// Gets the unique key of the published element.
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
        /// <param name="alias">The property alias.</param>
        /// <returns>The property identified by the alias.</returns>
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
        /// <param name="alias">The property alias.</param>
        /// <param name="value">The new property value.</param>
        public void OverrideValue(string alias, object value)
        {
            // Apply property value formatters so that any automatic changes that would have been applied
            // by a property value converter that supports property value formatters will also be applied to the new value.
            if (PropertyValueFormatters is not null && PropertyValueFormatters.Any())
            {
                var propertyType = GetProperty(alias)?.PropertyType;
                if (propertyType is not null)
                {
                    value = PropertyValueFormatters.ApplyFormatters(propertyType, value);
                }
            }

            // Set the new value
            var key = alias.ToUpperInvariant();
            if (_propertyValues.ContainsKey(key))
            {
                _propertyValues[key] = value;
            }
            else
            {
                _propertyValues.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the value of a content's property identified by its alias, converted to a specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="alias">The property alias.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        /// <param name="fallback">Optional fallback strategy.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <remarks>
        /// The value comes a value passed to <see cref="OverrideValue"/>, or from the <see cref="IPublishedProperty"/> field <c>Value</c> ie it is suitable for use when rendering content.
        /// 
        /// If no property with the specified alias exists, or if the property has no value, or if it could not be converted, returns <c>default(T)</c>.
        /// 
        /// The alias is case-insensitive.
        /// </remarks>
        public T? Value<T>(string alias, string? culture = null, string? segment = null, Fallback fallback = default, T? defaultValue = default)
        {
            var key = alias.ToUpperInvariant();
            if (_propertyValues.ContainsKey(key))
            {
                return (T)_propertyValues[key];
            }

            return _publishedElement != null ? _publishedElement.Value(alias, culture, segment, fallback, defaultValue) : default;
        }
    }
}
