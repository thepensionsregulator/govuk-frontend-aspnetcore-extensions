using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Blocks;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Templates;

namespace ThePensionsRegulator.Umbraco.Core.PropertyEditors.ValueConverters
{
    /// <summary>
    /// A property value converter for rich text properties using TinyMCE which does the built-in conversion and then applies any 
    /// <see cref="IPropertyValueFormatter"/> instances registered with the dependency injection container.
    /// </summary>
    public class RichTextEditorPropertyValueConverter : RteBlockRenderingValueConverter
    {
        private readonly IEnumerable<IPropertyValueFormatter> _propertyValueFormatters;

        public RichTextEditorPropertyValueConverter(
            HtmlLocalLinkParser linkParser,
            HtmlUrlParser urlParser,
            HtmlImageSourceParser imageSourceParser,
            IEnumerable<IPropertyValueFormatter> propertyValueFormatters,
            IApiRichTextElementParser apiRichTextElementParser,
            IApiRichTextMarkupParser apiRichTextMarkupParser,
            IPartialViewBlockEngine partialViewBlockEngine,
            BlockEditorConverter blockEditorConverter,
            IJsonSerializer jsonSerializer,
            IApiElementBuilder apiElementBuilder,
            RichTextBlockPropertyValueConstructorCache richTextBlockConstructorCache,
            ILogger<RteBlockRenderingValueConverter> logger,
            BlockEditorVarianceHandler blockEditorVarianceHandler,
            IVariationContextAccessor variationContextAccessor,

            IOptionsMonitor<DeliveryApiSettings> deliveryApiSettings) :
            base(linkParser,
                urlParser,
                imageSourceParser,
                apiRichTextElementParser,
                apiRichTextMarkupParser,
                partialViewBlockEngine,
                blockEditorConverter,
                jsonSerializer,
                apiElementBuilder,
                richTextBlockConstructorCache,
                logger,
                variationContextAccessor,
                blockEditorVarianceHandler,
                deliveryApiSettings)
        {
            _propertyValueFormatters = propertyValueFormatters ?? throw new ArgumentNullException(nameof(propertyValueFormatters));
        }

        /// <inheritdoc />
        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
        {
            object? value = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview)
                                 ?? new HtmlEncodedString(string.Empty);

            return _propertyValueFormatters.ApplyFormatters(propertyType, value);
        }

        /// <inheritdoc />
        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias == Constants.PropertyEditors.Aliases.RichText;
        }
    }


}
