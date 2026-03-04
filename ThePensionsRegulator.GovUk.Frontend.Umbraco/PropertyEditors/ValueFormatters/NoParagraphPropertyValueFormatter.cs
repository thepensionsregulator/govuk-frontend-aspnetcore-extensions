using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    /// <summary>
    /// Apply GOV.UK typography classes to HTML from the Umbraco rich text editor, and remove the paragraph element if there is only one.
    /// </summary>
    public class NoParagraphPropertyValueFormatter : RichTextPropertyValueFormatterBase, IPropertyValueFormatter
    {
        /// <inheritdoc />
        public virtual bool IsFormatter(IPublishedPropertyType propertyType)
        {
            return (propertyType.Alias == PropertyAliases.Hint && (propertyType.ContentType?.CompositionAliases.Contains(ElementTypeAliases.Hint) ?? false)) ||
                (propertyType.Alias == PropertyAliases.Hint && propertyType.ContentType?.Alias == ElementTypeAliases.Task) ||
                (propertyType.Alias == PropertyAliases.DetailsText && propertyType.ContentType?.Alias == ElementTypeAliases.Details) ||
                (propertyType.Alias == PropertyAliases.InsetText && propertyType.ContentType?.Alias == ElementTypeAliases.InsetText) ||
                (propertyType.Alias == PropertyAliases.WarningText && propertyType.ContentType?.Alias == ElementTypeAliases.WarningText) ||
                (propertyType.Alias == PropertyAliases.PhaseBannerText && (propertyType.ContentType?.CompositionAliases.Contains(ElementTypeAliases.PhaseBanner) ?? false)) ||
                (propertyType.Alias == PropertyAliases.NotificationBannerHeading && propertyType.ContentType?.Alias == ElementTypeAliases.NotificationBanner) ||
                (propertyType.Alias == PropertyAliases.SummaryListItemValue && propertyType.ContentType?.Alias == ElementTypeAliases.SummaryListItem);
        }

        /// <summary>Applies GOV.UK classes and removes a single wrapping paragraph if present.</summary>
        /// <returns>An <see cref="IHtmlEncodedString"/>.</returns>
        /// <remarks>
        /// This property type should return <see cref="IHtmlEncodedString"/> but accept <c>string</c> as well so that
        /// it is possible to provide a string of HTML to <see cref="OverridablePublishedElement.OverrideValue(string, object)"/>.
        /// </remarks>
        public object FormatValue(object value) => RemoveWrappingParagraphIfNoClass(ApplyGovUkTypographyToRichText(value));
    }
}
