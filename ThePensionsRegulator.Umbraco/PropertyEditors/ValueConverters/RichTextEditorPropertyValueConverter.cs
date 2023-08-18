using Umbraco.Cms.Core.Macros;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Templates;
using Umbraco.Cms.Core.Web;

namespace ThePensionsRegulator.Umbraco.PropertyEditors.ValueConverters
{
    /// <summary>
    /// A property value converter for rich text fields using TinyMCE which does the built-in conversion and then applies any 
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
            IEnumerable<IPropertyValueFormatter> propertyValueFormatters) :
            base(umbracoContextAccessor, macroRenderer, linkParser, urlParser, imageSourceParser)
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
    }
}
