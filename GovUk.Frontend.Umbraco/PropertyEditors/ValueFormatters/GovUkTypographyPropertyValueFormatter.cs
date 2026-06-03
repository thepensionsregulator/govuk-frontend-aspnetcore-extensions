using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    /// <summary>
    /// Apply GOV.UK typography classes to HTML from the Umbraco rich text editor
    /// </summary>
    public class GovUkTypographyPropertyValueFormatter : TinyMCEPropertyValueFormatterBase, IPropertyValueFormatter
    {
        /// <inheritdoc />
        public bool IsFormatter(IPublishedPropertyType propertyType) => Constants.PropertyEditors.Aliases.TinyMce.Equals(propertyType.EditorAlias);

        /// <inheritdoc />
        /// <remarks>
        /// This property type should return <see cref="IHtmlEncodedString"/> but accept <c>string</c> as well so that
        /// it is possible to provide a string of HTML to <see cref="OverridablePublishedElement.OverrideValue(string, object)"/>.
        /// </remarks>
        public object FormatValue(object value) => ApplyGovUkTypographyToTinyMCE(value);
    }
}
