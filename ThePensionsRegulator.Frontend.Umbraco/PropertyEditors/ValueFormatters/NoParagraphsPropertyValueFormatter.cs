using GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using HtmlAgilityPack;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    /// <summary>
    /// Apply GOV.UK typography classes to HTML from the Umbraco rich text editor, and remove all paragraph elements.
    /// </summary>
    public class NoParagraphsPropertyValueFormatter : TinyMCEPropertyValueFormatterBase, IPropertyValueFormatter
    {
        /// <inheritdoc />
        public bool IsFormatter(IPublishedPropertyType propertyType) => PropertyEditorAliases.TprHeaderFooterRichText.Equals(propertyType.EditorAlias);

        /// <inheritdoc />
        /// <remarks>
        /// This property type should return <see cref="IHtmlEncodedString"/> but accept <c>string</c> as well so that
        /// it is possible to provide a string of HTML to <see cref="OverridablePublishedElement.OverrideValue(string, object)"/>.
        /// </remarks>
        public object FormatValue(object value) => RemoveWrappingParagraphs(ApplyGovUkTypographyToTinyMCE(value));

        private static IHtmlEncodedString RemoveWrappingParagraphs(IHtmlEncodedString html)
        {
            if (!string.IsNullOrWhiteSpace(html.ToHtmlString()))
            {
                var document = new HtmlDocument();
                document.LoadHtml(html.ToHtmlString());

                var paragraphs = document.DocumentNode.Elements("p").ToList();
                for (var i = 0; i < paragraphs.Count; i++)
                {
                    foreach (var child in paragraphs[i].ChildNodes)
                    {
                        document.DocumentNode.InsertBefore(child, paragraphs[i]);
                    }
                    paragraphs[i].Remove();
                }
                html = new HtmlEncodedString(document.DocumentNode.OuterHtml);
            }
            return html;
        }
    }
}
