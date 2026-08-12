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
using Umbraco.Cms.Core.Services;
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
            HtmlLocalLinkParser _linkParser,
            HtmlUrlParser _urlParser,
            HtmlImageSourceParser _imageSourceParser,
            IEnumerable<IPropertyValueFormatter> _propertyValueFormatters,
            IApiRichTextElementParser _apiRichTextElementParser,
            IApiRichTextMarkupParser _apiRichTextMarkupParser,
            IPartialViewBlockEngine _partialViewBlockEngine,
            BlockEditorConverter _blockEditorConverter,
            IJsonSerializer _jsonSerializer,
            IApiElementBuilder _apiElementBuilder,
            RichTextBlockPropertyValueConstructorCache _richTextBlockConstructorCache,
            ILogger<RteBlockRenderingValueConverter> _logger,
            BlockEditorVarianceHandler _blockEditorVarianceHandler,
            IVariationContextAccessor _variationContextAccessor,
            IOptionsMonitor<DeliveryApiSettings> _deliveryApiSettings,
            ILanguageService _languageService,
            IPropertyRenderingContextAccessor _propertyRenderingContextAccessor) :
            base(_linkParser,
                _urlParser,
                _imageSourceParser,
                _apiRichTextElementParser,
                _apiRichTextMarkupParser,
                _partialViewBlockEngine,
                _blockEditorConverter,
                _jsonSerializer,
                _apiElementBuilder,
                _richTextBlockConstructorCache,
                _logger,
                _variationContextAccessor,
                _blockEditorVarianceHandler,
                _deliveryApiSettings,
                _languageService,
                _propertyRenderingContextAccessor)
        {
            this._propertyValueFormatters = _propertyValueFormatters ?? throw new ArgumentNullException(nameof(_propertyValueFormatters));
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

        /// <inheritdoc />
        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.None;
    }


}
