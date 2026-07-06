using HtmlAgilityPack;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    /// <summary>
    /// Apply .tpr-table class to HTML tables from the Umbraco rich text editor.
    /// Optionally injects CSV download forms for no-JS support when EnableTableCsvDownload is true.
    /// </summary>
    public class TprTablePropertyValueFormatter : IPropertyValueFormatter
    {
        private readonly IOptions<TprFrontendOptions> _tprOptions;
        private readonly IAntiforgery _antiforgery;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TableHtmlCsvFormGenerator _formGenerator;

        public TprTablePropertyValueFormatter(
            IOptions<TprFrontendOptions> tprOptions,
            IAntiforgery antiforgery,
            IHttpContextAccessor httpContextAccessor)
        {
            _tprOptions = tprOptions;
            _antiforgery = antiforgery;
            _httpContextAccessor = httpContextAccessor;
            _formGenerator = new TableHtmlCsvFormGenerator();
        }

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

                // Inject CSV download forms for no-JS support if enabled
                if (_tprOptions.Value.EnableTableCsvDownload && _httpContextAccessor.HttpContext != null)
                {
                    var tokens = _antiforgery.GetAndStoreTokens(_httpContextAccessor.HttpContext);
                    richTextHtml = _formGenerator.AddCsvDownloadForms(richTextHtml, tokens.RequestToken!);
                }
            }
            return new HtmlEncodedString(richTextHtml ?? "");
        }
    }
}
