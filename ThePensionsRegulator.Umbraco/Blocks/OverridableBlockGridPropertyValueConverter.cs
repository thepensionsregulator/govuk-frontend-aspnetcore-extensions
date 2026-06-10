using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Serialization;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    /// <summary>
    /// A property value converter which ensures that ModelsBuilder models represent a block grid as an <see cref="OverridableBlockGridModel" />.
    /// </summary>
    public class OverridableBlockGridPropertyValueConverter(
        IProfilingLogger _proflog,
        BlockEditorConverter _blockConverter,
        IJsonSerializer _jsonSerializer,
        IEnumerable<IPropertyValueFormatter> _propertyValueFormatters,
        IApiElementBuilder _apiElementBuilder,
        BlockGridPropertyValueConstructorCache _constructorCache,
        IPublishedValueFallback _publishedValueFallback)
        : BlockGridPropertyValueConverter(_proflog, _blockConverter, _jsonSerializer, _apiElementBuilder, _constructorCache)
    {
        /// <inheritdoc />
        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            var baseType = base.GetPropertyValueType(propertyType);
            return baseType == typeof(BlockGridModel) ? typeof(OverridableBlockGridModel) : baseType;
        }

        /// <inheritdoc />
        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
        {
            var baseModel = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview);
            return baseModel is BlockGridModel ? new OverridableBlockGridModel(_publishedValueFallback, (BlockGridModel)baseModel) { PropertyValueFormatters = _propertyValueFormatters } : baseModel;
        }

        /// <inheritdoc />
        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.Snapshot;
    }
}
