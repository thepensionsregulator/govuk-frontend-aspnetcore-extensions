using HtmlAgilityPack;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    /// <summary>
    /// Apply .tpr-table class to HTML tables from the Umbraco rich text editor.
    /// </summary>
    public class TprTablePropertyValueFormatter : IPropertyValueFormatter
    {
        public virtual bool IsFormatter(IPublishedPropertyType propertyType) => Constants.PropertyEditors.Aliases.RichText.Equals(propertyType.EditorAlias);

        /// <inheritdoc />
        /// <remarks>
        /// This property type should return <see cref="IHtmlEncodedString"/> but accept <c>string</c> as well so that
        /// it is possible to provide a string of HTML to <see cref="OverridablePublishedElement.OverrideValue(string, object)"/>.
        /// </remarks>
        public object FormatValue(object value)
        {
            var richTextHtml = value is IHtmlEncodedString html ? html.ToHtmlString() : value as string;

            if (!string.IsNullOrWhiteSpace(richTextHtml))
            {
                var document = new HtmlDocument();
                document.LoadHtml(richTextHtml);

                var tables = document.DocumentNode.SelectNodes("//table");
                if (tables is not null)
                {
                    foreach (var table in tables)
                    {
                        table.AddClass(TprClassNames.Table);
                    }
                }
                richTextHtml = document.DocumentNode.OuterHtml;
            }
            return new HtmlEncodedString(richTextHtml ?? "");
        }
    }
}
