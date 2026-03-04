using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprSectionCardContext
    {
        private (AttributeDictionary Attributes, IHtmlContent? Title, bool AllowHtml, string? Url, string? Target)? _title;
        private (AttributeDictionary Attributes, IHtmlContent? Content, bool AllowHtml)? _content;

        public AttributeDictionary? CardAttributes { get; set; }
        public AttributeDictionary? TitleAttributes => _title?.Attributes;
        public IHtmlContent? Title => _title?.Title;
        public string? TitleUrl => _title?.Url;
        public string? TitleTarget => _title?.Target;
        public bool TitleAllowHtml => _title?.AllowHtml ?? false;
        public AttributeDictionary? ContentAttributes => _content?.Attributes;
        public IHtmlContent? Content => _content?.Content;
        public bool ContentAllowHtml => _content?.AllowHtml ?? false;

        public void SetTitle(AttributeDictionary attributes, IHtmlContent? title, bool allowHtml, string? url, string? target)
        {
            if (_title != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprSectionCardTitleTagHelper.TagName,
                    TprSectionCardTagHelper.TagName);
            }
            _title = (attributes, title, allowHtml, url, target);
        }

        public void SetContent(AttributeDictionary contentAttributes, IHtmlContent? content, bool allowHtml)
        {
            if (_content != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprSectionCardContentTagHelper.TagName,
                     TprSectionCardTagHelper.TagName);
            }
            _content = (contentAttributes, content, allowHtml);
        }
    }
}
