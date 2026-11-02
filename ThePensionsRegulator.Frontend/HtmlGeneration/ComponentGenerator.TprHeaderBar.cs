using Microsoft.AspNetCore.Mvc.Rendering;
using ThePensionsRegulator.GovUk.Frontend.Caching;


namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprHeaderBarElement = "div";
        internal const string DefaultHeaderLabel = "Making workplace pensions work";
        internal const string HeaderLogoDefaultAlt = "The Pensions Regulator home page";
        internal const string HeaderLogoDefaultHref = "https://www.thepensionsregulator.gov.uk";

        public virtual TagBuilder GenerateTprHeaderBar(TprHeaderBar tprHeaderBar)
        {

            var tagBuilder = new TagBuilder(TprHeaderBarElement);
            if (tprHeaderBar.HeaderBarAttributes != null) { tagBuilder.MergeAttributes(tprHeaderBar.HeaderBarAttributes); }
            tagBuilder.AddCssClass("tpr-header");

            var headerContent = new TagBuilder("div");
            headerContent.AddCssClass("govuk-width-container");
            headerContent.AddCssClass("tpr-header__inner");

            var logoIsLinked = !string.IsNullOrEmpty(tprHeaderBar.LogoHref);
            var logoElement = new TagBuilder(logoIsLinked ? "a" : "span");
            if (logoIsLinked)
            {
                logoElement.AddCssClass("govuk-link-image");
                logoElement.Attributes.Add("href", tprHeaderBar.LogoHref);
            }
            logoElement.AddCssClass("tpr-header__logo");

            var pictureElement = new TagBuilder("picture");

            var sourceElement = new TagBuilder("source");
            sourceElement.Attributes.Add("srcset", $"/ThePensionsRegulator.Frontend/img/tpr-logo-footer.svg?{CachingConstants.StaticAssetVersionQueryParamName}={TprFrontendVersion}");
            sourceElement.Attributes.Add("media", "(forced-colors: active) and (prefers-color-scheme: light)");
            pictureElement.InnerHtml.AppendHtml(sourceElement);

            var screenLogo = new TagBuilder("img");
            screenLogo.TagRenderMode = TagRenderMode.SelfClosing;
            if (tprHeaderBar.LogoAttributes != null) { screenLogo.MergeAttributes(tprHeaderBar.LogoAttributes); }
            screenLogo.Attributes.Add("src", $"/ThePensionsRegulator.Frontend/img/tpr-logo-header.svg?{CachingConstants.StaticAssetVersionQueryParamName}={TprFrontendVersion}");
            screenLogo.Attributes.Add("alt", tprHeaderBar.LogoAlternativeText);
            screenLogo.Attributes.Add("width", "180");
            screenLogo.Attributes.Add("height", "75");
            screenLogo.AddCssClass("tpr-header__logo-img--screen");

            pictureElement.InnerHtml.AppendHtml(screenLogo);
            logoElement.InnerHtml.AppendHtml(pictureElement);

            var printLogo = new TagBuilder("img");
            printLogo.TagRenderMode = TagRenderMode.SelfClosing;
            printLogo.Attributes.Add("src", $"/ThePensionsRegulator.Frontend/img/tpr-logo-footer.svg?{CachingConstants.StaticAssetVersionQueryParamName}={TprFrontendVersion}");
            printLogo.Attributes.Add("alt", tprHeaderBar.LogoAlternativeText);
            printLogo.Attributes.Add("width", "180");
            printLogo.Attributes.Add("height", "75");
            printLogo.AddCssClass("tpr-header__logo-img--print");
            logoElement.InnerHtml.AppendHtml(printLogo);

            headerContent.InnerHtml.AppendHtml(logoElement);

            if (tprHeaderBar.Label != null && !string.IsNullOrWhiteSpace(tprHeaderBar.Label.ToString()))
            {
                var labelElement = new TagBuilder("p");
                if (tprHeaderBar.LabelAttributes != null) { labelElement.MergeAttributes(tprHeaderBar.LabelAttributes); }
                labelElement.AddCssClass("govuk-body");
                labelElement.AddCssClass("tpr-header__label");
                if (tprHeaderBar.LabelAllowHtml)
                {
                    labelElement.InnerHtml.AppendHtml(tprHeaderBar.Label);
                }
                else
                {
                    labelElement.InnerHtml.Append(tprHeaderBar.Label.ToString()!);
                }
                headerContent.InnerHtml.AppendHtml(labelElement);
            }

            if (tprHeaderBar.Content != null && !string.IsNullOrWhiteSpace(tprHeaderBar.Content.ToString()))
            {
                var contentElement = new TagBuilder("div");
                if (tprHeaderBar.ContentAttributes != null) { contentElement.MergeAttributes(tprHeaderBar.ContentAttributes); }
                contentElement.AddCssClass("govuk-body");
                contentElement.AddCssClass("tpr-header__content");
                headerContent.InnerHtml.AppendHtml(contentElement);
                if (tprHeaderBar.ContentAllowHtml)
                {
                    contentElement.InnerHtml.AppendHtml(tprHeaderBar.Content);
                }
                else
                {
                    contentElement.InnerHtml.Append(tprHeaderBar.Content.ToString()!);
                }
            }
            else if (tprHeaderBar.ShowSearch)
            {

                var headerSearch = GenerateTprHeaderSearch(tprHeaderBar, tprHeaderBar.SearchFormId);
                headerContent.InnerHtml.AppendHtml(headerSearch);
            }
            if (tprHeaderBar?.DisplayHeaderMenu ?? false)
            {
                var navDiv = new TagBuilder("div");
                navDiv.AddCssClass("tpr-mobile-menu__container");
                headerContent.InnerHtml.AppendHtml(navDiv);

                var mobileMenu = new TprHeaderMenu
                {

                    Attributes = tprHeaderBar.HeaderMenuAttributes,
                    HeaderMenuItems = tprHeaderBar.HeaderMenuItems,
                };

                var headerMenu = GenerateTprHeaderMenu(mobileMenu, tprHeaderBar);
                navDiv.InnerHtml.AppendHtml(headerMenu);

                tagBuilder.InnerHtml.AppendHtml(headerContent);

                var headerMenuNav = GenerateTprHeaderNav(mobileMenu, tprHeaderBar);
                tagBuilder.InnerHtml.AppendHtml(headerMenuNav);
            }
            else
            {
                tagBuilder.InnerHtml.AppendHtml(headerContent);
            }

            return tagBuilder;
        }

        public virtual TagBuilder GenerateTprHeaderSearch(TprHeaderBar tprHeaderBar, string? formId = null)
        {
            var divTag = new TagBuilder("div");
            if (tprHeaderBar.SearchAttributes != null) { divTag.MergeAttributes(tprHeaderBar.SearchAttributes); }
            divTag.AddCssClass("tpr-header-search");

            var form = new TagBuilder("form");
            if (!string.IsNullOrEmpty(tprHeaderBar.ActionPath))
            {
                form.Attributes.Add("action", tprHeaderBar.ActionPath);
            }

            if (!string.IsNullOrEmpty(formId))
            {
                form.Attributes.Add("id", formId);
            }

            form.Attributes.Add("method", "get");

            var searchField = new TagBuilder("div");
            searchField.AddCssClass("tpr-header-search__field");

            var autoComplete = new TagBuilder("div");
            autoComplete.AddCssClass("tpr-header-search-autocomplete");

            searchField.InnerHtml.AppendHtml(autoComplete);

            var autoCompleteContainer = new TagBuilder("div");
            autoCompleteContainer.AddCssClass("tpr-autocomplete-container");
            if (!string.IsNullOrEmpty(tprHeaderBar.AutoCompleteUrl))
            {
                autoCompleteContainer.Attributes.Add("data-autocomplete-url", tprHeaderBar.AutoCompleteUrl);
            }
            autoComplete.InnerHtml.AppendHtml(autoCompleteContainer);

            var searchInput = new TagBuilder("input");
            if (!string.IsNullOrEmpty(tprHeaderBar.SearchPlaceholderText))
            {
                searchInput.Attributes.Add("placeholder", tprHeaderBar.SearchPlaceholderText);
            }
            searchInput.Attributes.Add("type", "search");
            if (!string.IsNullOrEmpty(tprHeaderBar.SearchInputName))
            {
                searchInput.Attributes.Add("name", tprHeaderBar.SearchInputName);
            }
            searchInput.AddCssClass("govuk-input");
            searchInput.AddCssClass("tpr-header-search__input");

            autoCompleteContainer.InnerHtml.AppendHtml(searchInput);

            var button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.AddCssClass("tpr-header-search__button");
            button.AddCssClass("govuk-input");
            if (!string.IsNullOrEmpty(tprHeaderBar.SearchAriaLabel))
            {
                button.Attributes.Add("aria-label", tprHeaderBar.SearchAriaLabel);
            }

            var buttonSvg = new TagBuilder("svg");
            buttonSvg.Attributes.Add("viewBox", "0 0 21 6");
            buttonSvg.Attributes.Add("xmlns", "http://www.w3.org/2000/svg");
            buttonSvg.Attributes.Add("version", "1.1");
            buttonSvg.Attributes.Add("focusable", "false");
            buttonSvg.AddCssClass("tpr-header-search__button-image");

            var svgTag1 = new TagBuilder("g");
            svgTag1.Attributes.Add("stroke", "none");
            svgTag1.Attributes.Add("stroke-width", "1");
            svgTag1.Attributes.Add("fill", "none");
            svgTag1.Attributes.Add("fill-rule", "evenodd");
            buttonSvg.InnerHtml.AppendHtml(svgTag1);

            var svgTag2 = new TagBuilder("g");
            svgTag2.Attributes.Add("transform", "translate(1.000000, -6.000000)");
            svgTag2.AddCssClass("tpr-header-search__button-icon");
            svgTag1.InnerHtml.AppendHtml(svgTag2);

            var path1 = new TagBuilder("path");
            path1.Attributes.Add("d", "m 15.176041,6.8528646 c 0,-3.289 -2.683,-5.95400005 -5.9910002,-5.95400005 -3.311,0 -5.993,2.66500005 -5.993,5.95400005 0,3.2870004 2.683,5.9520004 5.993,5.9520004 3.3080002,0 5.9910002,-2.665 5.9910002,-5.9520004 z m -11.0660002,0.065 c 0,-2.806 2.279,-5.076 5.092,-5.076 2.8110002,0 5.0880002,2.271 5.0880002,5.076 0,2.804 -2.277,5.0750004 -5.0880002,5.0750004 -2.813,0 -5.092,-2.2720004 -5.092,-5.0750004 z");
            svgTag2.InnerHtml.AppendHtml(path1);

            var path2 = new TagBuilder("path");
            path2.Attributes.Add("d", "m 2.1965269,15.514568 -1.822,-1.822 4.037,-4.0380003 c 0,0 0.096,0.7650003 0.58,1.2470003 0.482,0.484 1.242,0.576 1.242,0.576 z");
            svgTag2.InnerHtml.AppendHtml(path2);

            button.InnerHtml.AppendHtml(buttonSvg);

            searchField.InnerHtml.AppendHtml(button);

            form.InnerHtml.AppendHtml(searchField);

            divTag.InnerHtml.AppendHtml(form);

            return divTag;
        }
    }
}
