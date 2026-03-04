using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    /// <summary>
    /// Apply the configured <see cref="IContextAwareHostUpdater"/> to links in HTML from the Umbraco rich text editor
    /// </summary>
    public class HostNameInRichTextEditorPropertyValueFormatter : IPropertyValueFormatter
    {
        private readonly string? _hostName;
        private readonly IContextAwareHostUpdater _contextAwareHostUpdater;

        public HostNameInRichTextEditorPropertyValueFormatter(IHttpContextAccessor httpContextAccessor, IContextAwareHostUpdater contextAwareHostUpdater)
        {
            if (httpContextAccessor == null) { throw new ArgumentNullException(nameof(httpContextAccessor)); }
            _hostName = httpContextAccessor.HttpContext?.Request?.Host.Host;
            if (string.IsNullOrEmpty(_hostName))
            {
                throw new ArgumentException($"{nameof(httpContextAccessor)} must return a context with an HTTP request and a hostname", nameof(httpContextAccessor));
            }
            _contextAwareHostUpdater = contextAwareHostUpdater ?? throw new ArgumentNullException(nameof(contextAwareHostUpdater));
        }

        /// <inheritdoc />
        public bool IsFormatter(IPublishedPropertyType propertyType) => propertyType.EditorAlias == Constants.PropertyEditors.Aliases.RichText;

        /// <inheritdoc />
        /// <remarks>
        /// This property type should return <see cref="IHtmlEncodedString"/> but accept <c>string</c> as well so that
        /// it is possible to provide a string of HTML to <see cref="OverridablePublishedElement.OverrideValue(string, object)"/>.
        /// </remarks>
        public object FormatValue(object value)
        {
            if (value is null) { return string.Empty; }
            var html = value is IHtmlEncodedString encoded ? encoded.ToHtmlString() : value.ToString();
            if (string.IsNullOrWhiteSpace(html)) { return string.Empty; }
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var links = document.DocumentNode.SelectNodes("//a[@href and @href!='' and normalize-space(@href) != ' ']");
            if (links != null)
            {
                foreach (var link in links)
                {
                    var attribute = link.Attributes["href"];
                    attribute.Value = _contextAwareHostUpdater.UpdateHost(attribute.Value, _hostName!);
                }
            }
            return new HtmlEncodedString(document.DocumentNode.OuterHtml);
        }
    }
}
