using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    internal class TprHeaderBarContext
    {
        private (AttributeDictionary Attributes, string? Href, string? AlternativeText)? _logo;
        private (AttributeDictionary Attributes, IHtmlContent? Label, bool AllowHtml)? _label;
        private (AttributeDictionary Attributes, IHtmlContent? Content, bool AllowHtml)? _content;

        public AttributeDictionary? LogoAttributes => _logo?.Attributes;
        public string? LogoHref => _logo?.Href;
        public string? LogoAlternativeText => _logo?.AlternativeText;
        public AttributeDictionary? LabelAttributes => _label?.Attributes;
        public IHtmlContent? Label => _label?.Label;
        public bool LabelAllowHtml => _label?.AllowHtml ?? false;
        public AttributeDictionary? ContentAttributes => _content?.Attributes;
        public IHtmlContent? Content => _content?.Content;
        public bool ContentAllowHtml => _content?.AllowHtml ?? false;

        public void SetLogo(AttributeDictionary attributes, string? href, string? alternativeText)
        {
            if (_logo != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprHeaderBarLogoTagHelper.TagName,
                    TprHeaderBarTagHelper.TagName);
            }

            _logo = (attributes, href, alternativeText);
        }

        public void SetLabel(AttributeDictionary attributes, IHtmlContent? label, bool allowHtml)
        {
            if (_label != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprHeaderBarLabelTagHelper.TagName,
                    TprHeaderBarTagHelper.TagName);
            }

            _label = (attributes, label, allowHtml);
        }

        public void SetContent(AttributeDictionary attributes, IHtmlContent htmlContent, bool allowHtml)
        {
            if (_content != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprHeaderBarContentTagHelper.TagName,
                TprHeaderBarTagHelper.TagName);
            }

            _content = (attributes, htmlContent, allowHtml);
        }
    }
}