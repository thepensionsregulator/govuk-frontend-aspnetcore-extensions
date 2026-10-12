using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;

namespace ThePensionsRegulator.Umbraco.Core.PropertyEditors.ValueConverters
{
    /// <summary>
    /// A property value converter for properties using the multi-URL picker which does the built-in conversion and then applies any 
    /// <see cref="IPropertyValueFormatter"/> instances registered with the dependency injection container.
    /// </summary>
    public class MultiUrlPickerPropertyValueConverter : MultiUrlPickerValueConverter
    {
        private readonly IEnumerable<IPropertyValueFormatter> _propertyValueFormatters;

        public MultiUrlPickerPropertyValueConverter(
            IProfilingLogger proflog,
            IJsonSerializer jsonSerializer,
            IPublishedUrlProvider publishedUrlProvider,
            IEnumerable<IPropertyValueFormatter> propertyValueFormatters,
            IApiContentNameProvider apiContentNameProvider,
            IApiMediaUrlProvider apiMediaUrlProvider,
            IApiContentRouteBuilder apiRouteBuilder,
            IPublishedContentCache publishedContentCache,
            IPublishedMediaCache publishedMediaCache
            )
            : base(proflog,
                  jsonSerializer,
                  publishedUrlProvider,
                  apiContentNameProvider,
                  apiMediaUrlProvider,
                  apiRouteBuilder,
                  publishedContentCache,
                  publishedMediaCache)
        {
            _propertyValueFormatters = propertyValueFormatters ?? throw new ArgumentNullException(nameof(propertyValueFormatters));
        }

        /// <inheritdoc />
        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
        {
            var value = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview);

            return value is null ? null : _propertyValueFormatters.ApplyFormatters(propertyType, value);
        }

        /// <inheritdoc />
        /// <remarks>
        /// This is set to <see cref="PropertyCacheLevel.None"/> because Umbraco is caching generated links in multi url pickers.
        /// For example, after loading /page/10000006 and then /page/10000007, any links styled as a button will still end in 10000006.
        /// </remarks>
        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.None;
    }
}
