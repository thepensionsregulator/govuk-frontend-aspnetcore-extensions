using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    /// <summary>
    /// A property value converter which ensures that ModelsBuilder models represent a block list as an <see cref="OverridableBlockListModel" />.
    /// </summary>
    public class OverridableBlockListPropertyValueConverter(
        IProfilingLogger _proflog,
        BlockEditorConverter _blockConverter,
        IContentTypeService _contentTypeService,
        IEnumerable<IPropertyValueFormatter> _propertyValueFormatters,
        IApiElementBuilder _apiElementBuilder,
        BlockListPropertyValueConstructorCache _constructorCache,
        IPublishedValueFallback _publishedValueFallback)
        : BlockListPropertyValueConverter(_proflog, _blockConverter, _contentTypeService, _apiElementBuilder, _constructorCache)
    {
        /// <inheritdoc />
        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            var baseType = base.GetPropertyValueType(propertyType);
            return baseType == typeof(BlockListModel) ? typeof(OverridableBlockListModel) : baseType;
        }

        /// <inheritdoc />
        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
        {
            var baseModel = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview);
            return baseModel is BlockListModel ? new OverridableBlockListModel(_publishedValueFallback, (BlockListModel)baseModel) { PropertyValueFormatters = _propertyValueFormatters } : baseModel;
        }

        /// <inheritdoc />
        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.Snapshot;
    }
}
