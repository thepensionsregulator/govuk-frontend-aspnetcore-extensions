using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Blocks;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Macros;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Templates;
using Umbraco.Cms.Core.Web;

namespace ThePensionsRegulator.Umbraco.PropertyEditors.ValueConverters
{
    /// <summary>
    /// A property value converter for rich text properties using TinyMCE which does the built-in conversion and then applies any 
    /// <see cref="IPropertyValueFormatter"/> instances registered with the dependency injection container.
    /// </summary>
    public class RichTextEditorPropertyValueConverter : RteMacroRenderingValueConverter
    {
        private readonly IEnumerable<IPropertyValueFormatter> _propertyValueFormatters;

        public RichTextEditorPropertyValueConverter(IUmbracoContextAccessor umbracoContextAccessor,
            IMacroRenderer macroRenderer,
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
            ILogger<RteMacroRenderingValueConverter> macroLogger,
            IOptionsMonitor<DeliveryApiSettings> deliveryApiSettings) :
            base(umbracoContextAccessor,
                macroRenderer,
                linkParser,
                urlParser,
                imageSourceParser,
                apiRichTextElementParser,
                apiRichTextMarkupParser,
                partialViewBlockEngine,
                blockEditorConverter,
                jsonSerializer,
                apiElementBuilder,
                richTextBlockConstructorCache,
                macroLogger,
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
        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.Snapshot;
    }


}
