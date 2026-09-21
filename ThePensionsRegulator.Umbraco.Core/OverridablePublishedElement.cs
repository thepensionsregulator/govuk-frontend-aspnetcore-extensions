using Microsoft.Extensions.DependencyInjection;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Core
{
    /// <summary>
    /// A wrapper for <see cref="IPublishedElement"/> which allows property values to be overridden.
    /// </summary>
    public class OverridablePublishedElement : IPublishedElement, IOverridablePublishedElement
    {
        private readonly IPublishedElement _publishedElement;
        private IOverridablePublishedElementValueStore? _overridablePublishedElementValueStore;
        private IEnumerable<IPropertyValueFormatter>? _propertyValueFormatters;

        // Resolved on first use so that the obsolete constructor defers the StaticServiceProvider call.
        private IOverridablePublishedElementValueStore ValueStore
            => _overridablePublishedElementValueStore
               ??= StaticServiceProvider.Instance.GetRequiredService<IOverridablePublishedElementValueStore>();

        [Obsolete("Use the constructor that accepts IOverridablePublishedElementValueStore to avoid depending on Umbraco's StaticServiceProvider.")]
        public OverridablePublishedElement(IPublishedElement publishedElement)
        {
            _publishedElement = publishedElement ?? throw new ArgumentNullException(nameof(publishedElement));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OverridablePublishedElement"/> class.
        /// </summary>
        /// <param name="publishedElement">The published element to wrap.</param>
        /// <param name="overridablePublishedElementValueStore">A request-scoped store for overridden property values.</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        /// <remarks>
        /// Umbraco caches this object instance at PropertyCacheLevel.Element, so it can be reused by multiple requests.
        /// Request overrides must therefore live in the request-scoped store <see cref="IOverridablePublishedElementValueStore"/> 
        /// rather than on this cached object instance.
        /// </remarks>
        public OverridablePublishedElement(IPublishedElement publishedElement, IOverridablePublishedElementValueStore overridablePublishedElementValueStore)
        {
            _publishedElement = publishedElement ?? throw new ArgumentNullException(nameof(publishedElement));
            _overridablePublishedElementValueStore = overridablePublishedElementValueStore ?? throw new ArgumentNullException(nameof(overridablePublishedElementValueStore));
        }

        /// <summary>
        /// Property value formatters which may be applied when a property is overridden with a new value.
        /// </summary>
        /// <remarks>
        /// This should remain internal and is intended to be set by <see cref="OverridableBlockListPropertyValueConverter"/> or <see cref="OverridableBlockGridPropertyValueConverter"/> 
        /// to pass down via <see cref="OverridableBlockListModel"/> or <see cref="OverridableBlockGridModel"/>,
        /// because the property value converter is the nearest place that can inject the property value formatters registered with the dependency injection container.
        /// </remarks>
        internal IEnumerable<IPropertyValueFormatter>? PropertyValueFormatters
        {
            get => _propertyValueFormatters;
            set
            {
                _propertyValueFormatters = value;

                if (_propertyValueFormatters is not null)
                {
                    var propertyValues = ValueStore.Get(this);
                    foreach (var alias in new List<string>(propertyValues.Keys))
                    {
                        var propertyType = GetProperty(alias)?.PropertyType;
                        if (propertyType is not null)
                        {
                            propertyValues[alias] = _propertyValueFormatters.ApplyFormatters(propertyType, propertyValues[alias]);
                        }
                    }
                }
            }
        }

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
            var propertyValues = ValueStore.Get(this);

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
            if (propertyValues.ContainsKey(key))
            {
                propertyValues[key] = value;
            }
            else
            {
                propertyValues.Add(key, value);
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
            var propertyValues = ValueStore.Get(this);

            var key = alias.ToUpperInvariant();
            if (propertyValues.ContainsKey(key))
            {
                return (T)propertyValues[key];
            }

            return _publishedElement != null ? _publishedElement.Value(alias, culture, segment, fallback, defaultValue) : default;
        }

        /// <summary>
        /// Gets the value of a content's property identified by its alias, converted to a specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
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
        public T? Value<T>(IPublishedValueFallback publishedValueFallback, string alias, string? culture = null, string? segment = null, Fallback fallback = default, T? defaultValue = default)
        {
            var propertyValues = ValueStore.Get(this);

            var key = alias.ToUpperInvariant();
            if (propertyValues.ContainsKey(key))
            {
                return (T)propertyValues[key];
            }

            return _publishedElement != null ? _publishedElement.Value(publishedValueFallback, alias, culture, segment, fallback, defaultValue) : default;
        }
    }
}
