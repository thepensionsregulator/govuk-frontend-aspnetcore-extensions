using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprHeaderBarContext
    {
        private (AttributeDictionary Attributes, string? Href, string? AlternativeText)? _logo;
        private (AttributeDictionary Attributes, IHtmlContent? Label, bool AllowHtml)? _label;
        private (AttributeDictionary Attributes, IHtmlContent? Content, bool AllowHtml)? _content;
        private (AttributeDictionary Attributes, bool ShowSearch, string? FormId, string? ActionPath, string? AutocompleteUrl, string? PlaceholderText, string? SearchAriaLabel, string? SearchInputName)? _search;
        private TprHeaderMenuContext? _headerMenuContext;

        public AttributeDictionary? LogoAttributes => _logo?.Attributes;
        public string? LogoHref => _logo?.Href;
        public string? LogoAlternativeText => _logo?.AlternativeText;
        public AttributeDictionary? LabelAttributes => _label?.Attributes;
        public IHtmlContent? Label => _label?.Label;
        public bool LabelAllowHtml => _label?.AllowHtml ?? false;
        public AttributeDictionary? ContentAttributes => _content?.Attributes;
        public IHtmlContent? Content => _content?.Content;
        public bool ContentAllowHtml => _content?.AllowHtml ?? false;
        public bool ShowSearch => _search?.ShowSearch ?? false;
        public AttributeDictionary? SearchAttributes => _search?.Attributes;
        public string? SearchFormId => _search?.FormId;
        public string? ActionPath => _search?.ActionPath;
        public string? AutoCompleteUrl => _search?.AutocompleteUrl;
        public string? SearchPlaceholderText => _search?.PlaceholderText;
        public string? SearchAriaLabel => _search?.SearchAriaLabel;
        public string? SearchInputName => _search?.SearchInputName ?? "query";
        public bool? DisplayHeaderMenu => _headerMenuContext != null;
        public TprHeaderMenuContext? TprHeaderMenuContext => _headerMenuContext;
        public string? HeaderMenuAriaLabel => _headerMenuContext?.HeaderMenuAriaLabel;
        public string? HeaderMenuItemAriaLabel => _headerMenuContext?.HeaderMenuItemAriaLabel;
        public string? MobileMenuNoJsNavPage => _headerMenuContext?.MobileMenuNoJsNavPage;
        public string? HeaderMenuToggleClosed => _headerMenuContext?.HeaderMenuToggleClosed;
        public string? HeaderMenuToggleOpen => _headerMenuContext?.HeaderMenuToggleOpen;


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

        public void SetSearch(AttributeDictionary attributes, bool showSearch, string? formId, string? actionPath, string? autocompleteUrl, string? placeholderText, string? ariaLabel, string? inputName)
        {
            if (_search != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprHeaderSearchTagHelper.TagName,
                TprHeaderBarTagHelper.TagName);
            }

            _search = (attributes, showSearch, formId, actionPath, autocompleteUrl, placeholderText, ariaLabel, inputName);
        }
        public void SetHeaderMenu(TprHeaderMenuContext tprHeaderMenuContext)
        {
            if (_headerMenuContext != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprHeaderMenuTagHelper.TagName,
                    TprHeaderBarTagHelper.TagName
                    );
            }
            _headerMenuContext = tprHeaderMenuContext;
        }
    }
}