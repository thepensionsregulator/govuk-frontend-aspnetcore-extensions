using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;

namespace ThePensionsRegulator.Umbraco.BlockLists
{
    /// <summary>
    /// A property value converter which ensures that ModelsBuilder models represent a block list as an <see cref="OverridableBlockListModel" />.
    /// </summary>
    public class OverridableBlockListPropertyValueConverter : BlockListPropertyValueConverter
    {
        private readonly IEnumerable<IPropertyValueFormatter> _propertyValueFormatters;

        public OverridableBlockListPropertyValueConverter(IProfilingLogger proflog, BlockEditorConverter blockConverter, IContentTypeService contentTypeService, IEnumerable<IPropertyValueFormatter> propertyValueFormatters)
            : base(proflog, blockConverter, contentTypeService)
        {
            _propertyValueFormatters = propertyValueFormatters ?? throw new ArgumentNullException(nameof(propertyValueFormatters));
        }

        /// <inheritdoc />
        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            var baseType = base.GetPropertyValueType(propertyType);
            return baseType == typeof(BlockListModel) ? typeof(OverridableBlockListModel) : baseType;
        }

        public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview)
        {
            // When the value is overridden using IOverridablePublishedElement.OverrideValue the converted type may be supplied, so just pass it through.
            if (source is BlockListModel || source is OverridableBlockListModel) { return source; }
            return base.ConvertSourceToIntermediate(owner, propertyType, source, preview);
        }

        /// <inheritdoc />
        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
        {
            // When the value is overridden using IOverridablePublishedElement.OverrideValue the converted type may be supplied, so just pass it through.
            if (inter is BlockListModel || inter is OverridableBlockListModel) { return inter; }
            var baseModel = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview);
            return baseModel is BlockListModel ? new OverridableBlockListModel((BlockListModel)baseModel) { PropertyValueFormatters = _propertyValueFormatters } : baseModel;
        }
    }
}
