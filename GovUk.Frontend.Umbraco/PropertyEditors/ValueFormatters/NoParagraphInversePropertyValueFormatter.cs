using GovUk.Frontend.AspNetCore.Extensions.Typography;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    /// <summary>
    /// Apply GOV.UK typography classes to HTML from the Umbraco rich text editor, and remove the paragraph element if there is only one.
    /// </summary>
    public class NoParagraphInversePropertyValueFormatter : TinyMCEPropertyValueFormatterBase, IPropertyValueFormatter
    {
        /// <inheritdoc />
        public bool IsFormatter(IPublishedPropertyType propertyType) => PropertyEditorAliases.GovUkInlineInverseRichText.Equals(propertyType.EditorAlias);

        /// <summary>Applies GOV.UK classes including inverse classes and removes a single wrapping paragraph if present.</summary>
        /// <returns>An <see cref="IHtmlEncodedString"/>.</returns>
        /// <remarks>
        /// This property type should return <see cref="IHtmlEncodedString"/> but accept <c>string</c> as well so that
        /// it is possible to provide a string of HTML to <see cref="OverridablePublishedElement.OverrideValue(string, object)"/>.
        /// </remarks>
        public object FormatValue(object value) => RemoveWrappingParagraph(
            ApplyGovUkTypographyToTinyMCE(value, new TypographyOptions { BackgroundType = BackgroundType.Dark })
        );
    }
}
