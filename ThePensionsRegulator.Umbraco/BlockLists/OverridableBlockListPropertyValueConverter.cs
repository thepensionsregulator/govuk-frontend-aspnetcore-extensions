using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;

namespace ThePensionsRegulator.Umbraco.BlockLists
{
    public class OverridableBlockListPropertyValueConverter : BlockListPropertyValueConverter, IPropertyValueConverter
    {
        public OverridableBlockListPropertyValueConverter(IProfilingLogger proflog, BlockEditorConverter blockConverter, IContentTypeService contentTypeService) : base(proflog, blockConverter, contentTypeService)
        {
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            var baseType = base.GetPropertyValueType(propertyType);
            return baseType == typeof(BlockListModel) ? typeof(OverridableBlockListModel) : baseType;
        }

        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
        {
            var baseModel = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview);
            return baseModel is BlockListModel ? new OverridableBlockListModel((BlockListModel)baseModel) : baseModel;
        }
    }
}
