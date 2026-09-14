using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Core
{
    /// <summary>
    /// A wrapper for <see cref="IPublishedElement"/> which allows property values to be overriden.
    /// </summary>
    public class OverridablePublishedElement : IPublishedElement, IOverridablePublishedElement
    {
        private readonly IPublishedElement _publishedElement;

        // Umbraco caches this object instance at PropertyCacheLevel.Element, so it can be reused by multiple requests.
        // Request overrides must therefore live in the scoped store below rather than on this cached object.
        // This dictionary is only used when the wrapper was created without an HTTP context, such as in a unit test.
        private readonly Dictionary<string, object> _propertyValuesNoHttpContext = new();
        private readonly IHttpContextAccessor? _httpContextAccessor;
        private IEnumerable<IPropertyValueFormatter>? _propertyValueFormatters;

        /// <summary>
        /// Creates an overridable element for non-HTTP usage. For HTTP request handling, use <see cref="OverridablePublishedElement(IPublishedElement, IHttpContextAccessor)"/>.
        /// </summary>
        public OverridablePublishedElement(IPublishedElement publishedElement) : this(publishedElement, null)
        {
        }

        /// <summary>
        /// Creates an overridable element that resolves request-scoped override values through the supplied HTTP context accessor.
        /// </summary>
        /// <param name="publishedElement">The element to wrap.</param>
        /// <param name="httpContextAccessor">The accessor used to find the current request-scoped override store.</param>
        internal OverridablePublishedElement(IPublishedElement publishedElement, IHttpContextAccessor? httpContextAccessor)
        {
            _publishedElement = publishedElement;
            _httpContextAccessor = httpContextAccessor;
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
                    foreach (var alias in _propertyValuesNoHttpContext.Keys)
                    {
                        var propertyType = GetProperty(alias)?.PropertyType;
                        if (propertyType is not null)
                        {
                            _propertyValuesNoHttpContext[alias] = _propertyValueFormatters.ApplyFormatters(propertyType, _propertyValuesNoHttpContext[alias]);
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
            var propertyValues = GetPropertyValues();

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
            var propertyValues = GetPropertyValues();

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
            var propertyValues = GetPropertyValues();

            var key = alias.ToUpperInvariant();
            if (propertyValues.ContainsKey(key))
            {
                return (T)propertyValues[key];
            }

            return _publishedElement != null ? _publishedElement.Value(publishedValueFallback, alias, culture, segment, fallback, defaultValue) : default;
        }

        private IDictionary<string, object> GetPropertyValues()
        {
            // This object instance is element-cached, but overrides are request data. A scoped store gives every
            // request its own values and prevents concurrent requests from sharing or clearing each other's overrides.
            var requestStore = _httpContextAccessor?.HttpContext?.RequestServices.GetService<IOverridablePublishedElementValueStore>();

            return requestStore?.Get(this)
                ?? _propertyValuesNoHttpContext;
        }
    }
}
