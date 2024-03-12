using GovUk.Frontend.Umbraco.PropertyEditors.ValueConverters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core.Blocks;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Macros;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Templates;
using Umbraco.Cms.Core.Web;

namespace ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueConverters
{
    /// <summary>
    /// A property value converter for rich text properties using TinyMCE which does the built-in conversion and then applies any 
    /// <see cref="IPropertyValueFormatter"/> instances registered with the dependency injection container.
    /// </summary>
    /// <remarks>The property editors supported by this property value converter are all identical.
    /// They exist to apply different property value formatters depending on the property.</remarks>
    public class TprRichTextEditorPropertyValueConverter : GovUkRichTextEditorPropertyValueConverter
    {
        private readonly List<string> _propertyEditorAliases = new();

        public TprRichTextEditorPropertyValueConverter(
            IUmbracoContextAccessor umbracoContextAccessor,
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
                propertyValueFormatters,
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
            _propertyEditorAliases.AddRange(GovUkPropertyEditorAliases());
            _propertyEditorAliases.Add(PropertyEditorAliases.TprHeaderFooterRichText);
        }

        /// <inheritdoc />
        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return _propertyEditorAliases.Contains(propertyType.EditorAlias);
        }
    }
}
