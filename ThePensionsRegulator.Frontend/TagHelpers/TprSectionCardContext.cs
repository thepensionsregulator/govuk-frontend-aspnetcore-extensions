using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprSectionCardContext
    {
        private (AttributeDictionary Attributes, IHtmlContent? Title, bool AllowHtml, string? Url)? _title;
        private (AttributeDictionary Attributes, IHtmlContent? Content, bool AllowHtml)? _body;
        private (AttributeDictionary Attributes, IHtmlContent? Content, bool AllowHtml)? _content;

        public AttributeDictionary? CardAttributes { get; set; }
        public AttributeDictionary? TitleAttributes => _title?.Attributes;
        public IHtmlContent? Title => _title?.Title;
        public string? TitleUrl => _title?.Url;
        public bool TitleAllowHtml => _title?.AllowHtml ?? false;
        public AttributeDictionary? BodyAttributes => _body?.Attributes;
        public AttributeDictionary? ContentAttributes => _content?.Attributes;
        public IHtmlContent? Content => _content?.Content ??_body?.Content;
        public bool ContentAllowHtml => _content != null ? _content.Value.AllowHtml : (_body?.AllowHtml ?? false);

        public void SetTitle(AttributeDictionary attributes, IHtmlContent? title, bool allowHtml, string url)
        {
            if (_title != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprSectionCardTitleTagHelper.TagName,
                    TprSectionCardTagHelper.TagName);
            }
            _title = (attributes, title, allowHtml, url);
        }

        public void SetBody(AttributeDictionary bodyAttributes, IHtmlContent? content, bool allowHtml)
        {
            if (_body != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprSectionCardBodyTagHelper.TagName,
                    TprSectionCardTagHelper.TagName);
            }
            _body = (bodyAttributes, content, true);
        }

        public void SetContent(AttributeDictionary contentAttributes, IHtmlContent? content, bool allowHtml)
        {
            if(_content != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprSectionCardContentTagHelper.TagName,
                     TprSectionCardTagHelper.TagName);
            }
            _content = (contentAttributes, content, allowHtml);
        }
    }
}
