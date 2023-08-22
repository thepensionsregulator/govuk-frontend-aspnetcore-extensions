using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Web;

namespace ThePensionsRegulator.Umbraco.PropertyEditors.ValueConverters
{
    /// <summary>
    /// A property value converter for properties using the multi-URL picker which does the built-in conversion and then applies any 
    /// <see cref="IPropertyValueFormatter"/> instances registered with the dependency injection container.
    /// </summary>
    public class MultiUrlPickerPropertyValueConverter : MultiUrlPickerValueConverter
    {
        private readonly IEnumerable<IPropertyValueFormatter> _propertyValueFormatters;

        public MultiUrlPickerPropertyValueConverter(
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            IProfilingLogger proflog,
            IJsonSerializer jsonSerializer,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedUrlProvider publishedUrlProvider,
            IEnumerable<IPropertyValueFormatter> propertyValueFormatters)
            : base(publishedSnapshotAccessor, proflog, jsonSerializer, umbracoContextAccessor, publishedUrlProvider)
        {
            _propertyValueFormatters = propertyValueFormatters ?? throw new ArgumentNullException(nameof(propertyValueFormatters));
        }

        /// <inheritdoc />
        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
        {
            var value = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview);

            return value is null ? null : _propertyValueFormatters.ApplyFormatters(propertyType, value);
        }
    }
}
