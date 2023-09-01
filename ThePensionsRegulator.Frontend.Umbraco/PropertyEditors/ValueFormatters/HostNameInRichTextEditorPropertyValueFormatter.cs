using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Umbraco.PropertyEditors;
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
        private readonly List<string> _propertyEditorAliases = new List<string> {
            Constants.PropertyEditors.Aliases.TinyMce,
            GovUk.Frontend.Umbraco.PropertyEditorAliases.GovUkInlineRichText,
            GovUk.Frontend.Umbraco.PropertyEditorAliases.GovUkInlineInverseRichText,
            PropertyEditorAliases.TprHeaderFooterRichText
        };

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
        public bool IsFormatter(IPublishedPropertyType propertyType) => _propertyEditorAliases.Contains(propertyType.EditorAlias);

        /// <inheritdoc />
        /// <remarks>
        /// This property type should return <see cref="IHtmlEncodedString"/> but accept <c>string</c> as well so that
        /// it is possible to provide a string of HTML to <see cref="OverridablePublishedElement.OverrideValue(string, object)"/>.
        /// </remarks>
        public object FormatValue(object value)
        {
            if (value is null) { return string.Empty; }
            var html = value is IHtmlEncodedString encoded ? encoded.ToHtmlString() : value.ToString();
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var links = document.DocumentNode.SelectNodes("//a");
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
