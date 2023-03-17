using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    internal class TprFooterBarContext
    {
        private (AttributeDictionary Attributes, string? Href, string? AlternativeText)? _logo;
        private (AttributeDictionary Attributes, IHtmlContent? Copyright, bool AllowHtml)? _copyright;
        private (AttributeDictionary Attributes, IHtmlContent? Content, bool AllowHtml)? _content;

        public AttributeDictionary? LogoAttributes => _logo?.Attributes;
        public string? LogoHref => _logo?.Href;
        public string? LogoAlternativeText => _logo?.AlternativeText;
        public AttributeDictionary? CopyrightAttributes => _copyright?.Attributes;
        public IHtmlContent? Copyright => _copyright?.Copyright;
        public bool CopyrightAllowHtml => _copyright?.AllowHtml ?? false;
        public AttributeDictionary? ContentAttributes => _content?.Attributes;
        public IHtmlContent? Content => _content?.Content;
        public bool ContentAllowHtml => _content?.AllowHtml ?? false;

        public void SetLogo(AttributeDictionary attributes, string? href, string? alternativeText)
        {
            if (_logo != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprFooterBarLogoTagHelper.TagName,
                    TprFooterBarTagHelper.TagName);
            }

            _logo = (attributes, href, alternativeText);
        }

        public void SetCopyright(AttributeDictionary attributes, IHtmlContent? copyright, bool allowHtml)
        {
            if (_copyright != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprFooterBarCopyrightTagHelper.TagName,
                    TprFooterBarTagHelper.TagName);
            }

            _copyright = (attributes, copyright, allowHtml);
        }

        public void SetContent(AttributeDictionary attributes, IHtmlContent htmlContent, bool allowHtml)
        {
            if (_content != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprFooterBarContentTagHelper.TagName,
                TprFooterBarTagHelper.TagName);
            }

            _content = (attributes, htmlContent, allowHtml);
        }
    }
}