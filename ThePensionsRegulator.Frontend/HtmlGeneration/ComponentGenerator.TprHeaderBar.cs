using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;


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
            tagBuilder.MergeCssClass("tpr-header");

            var headerContent = new TagBuilder("div");
            headerContent.MergeCssClass("govuk-width-container");
            headerContent.MergeCssClass("tpr-header__inner");

            var logoIsLinked = !string.IsNullOrEmpty(tprHeaderBar.LogoHref);
            var logoElement = new TagBuilder(logoIsLinked ? "a" : "span");
            if (logoIsLinked)
            {
                logoElement.MergeCssClass("govuk-link-image");
                logoElement.Attributes.Add("href", tprHeaderBar.LogoHref);
            }
            logoElement.MergeCssClass("tpr-header__logo");

            var screenLogo = new TagBuilder("img");
            screenLogo.TagRenderMode = TagRenderMode.SelfClosing;
            if (tprHeaderBar.LogoAttributes != null) { screenLogo.MergeAttributes(tprHeaderBar.LogoAttributes); }
            screenLogo.Attributes.Add("src", "/_content/ThePensionsRegulator.Frontend/tpr/tpr-logo-header.svg");
            screenLogo.Attributes.Add("alt", tprHeaderBar.LogoAlternativeText);
            screenLogo.Attributes.Add("width", "180");
            screenLogo.Attributes.Add("height", "75");
            screenLogo.MergeCssClass("tpr-header__logo-img--screen");
            logoElement.InnerHtml.AppendHtml(screenLogo);

            var printLogo = new TagBuilder("img");
            printLogo.TagRenderMode = TagRenderMode.SelfClosing;
            printLogo.Attributes.Add("src", "/_content/ThePensionsRegulator.Frontend/tpr/tpr-logo-footer.svg");
            printLogo.Attributes.Add("alt", tprHeaderBar.LogoAlternativeText);
            printLogo.Attributes.Add("width", "180");
            printLogo.Attributes.Add("height", "75");
            printLogo.MergeCssClass("tpr-header__logo-img--print");
            logoElement.InnerHtml.AppendHtml(printLogo);

            headerContent.InnerHtml.AppendHtml(logoElement);

            if (tprHeaderBar.Label != null && !string.IsNullOrWhiteSpace(tprHeaderBar.Label.ToString()))
            {
                var labelElement = new TagBuilder("p");
                if (tprHeaderBar.LabelAttributes != null) { labelElement.MergeAttributes(tprHeaderBar.LabelAttributes); }
                labelElement.MergeCssClass("govuk-body");
                labelElement.MergeCssClass("tpr-header__label");
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
                contentElement.MergeCssClass("govuk-body");
                contentElement.MergeCssClass("tpr-header__content");
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
            else if (tprHeaderBar.DisplaySearchBar)
            {

                var headerSearch = GenerateTprHeaderSearch(tprHeaderBar);
                headerContent.InnerHtml.AppendHtml(headerSearch);
            }

            tagBuilder.InnerHtml.AppendHtml(headerContent);

            return tagBuilder;
        }

        public virtual TagBuilder GenerateTprHeaderSearch(TprHeaderBar tprHeaderBar)
        {
            var divTag = new TagBuilder("div");
            if (tprHeaderBar.SearchAttributes != null) { divTag.MergeAttributes(tprHeaderBar.SearchAttributes); }
            divTag.AddCssClass("tpr-header-search");

            var form = new TagBuilder("form");
            form.Attributes.Add("action", $"{tprHeaderBar.ActionPath}");
            form.Attributes.Add("id", "tpr-header-search__form");
            form.Attributes.Add("method", "get");

            var searchField = new TagBuilder("div");
            searchField.AddCssClass("tpr-header-search__field");

            var autoComplete = new TagBuilder("div");
            autoComplete.AddCssClass("tpr-header-search-autocomplete");
            searchField.InnerHtml.AppendHtml(autoComplete);

            var autoCompleteContainer = new TagBuilder("div");
            autoCompleteContainer.AddCssClass("tpr-autocomplete-container");
            autoCompleteContainer.Attributes.Add("autocomplete-url", tprHeaderBar.AutoCompleteUrl);
            autoCompleteContainer.Attributes.Add("placeholder", tprHeaderBar.SearchPlaceholderText);
            autoComplete.InnerHtml.AppendHtml(autoCompleteContainer);

            var searchInput = new TagBuilder("input");
            searchInput.Attributes.Add("aria-label", tprHeaderBar.SearchAriaLabel);
            searchInput.Attributes.Add("placeholder", tprHeaderBar.SearchPlaceholderText);
            searchInput.Attributes.Add("type", "search");
            searchInput.Attributes.Add("name", "query");
            searchInput.AddCssClass("govuk-input");
            searchInput.AddCssClass("tpr-header-search__input");

            autoCompleteContainer.InnerHtml.AppendHtml(searchInput);

            var button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.AddCssClass("tpr-header-search__button");
            button.AddCssClass("govuk-input");
            button.Attributes.Add("aria-label", tprHeaderBar.SearchAriaLabel);

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
            svgTag2.Attributes.Add("transform", "translate(1.000000, -5.000000)");
            svgTag2.Attributes.Add("fill", "#434343");
            svgTag1.InnerHtml.AppendHtml(svgTag2);

            var path1 = new TagBuilder("path");
            path1.Attributes.Add("d", "m 15.176041,6.8528646 c 0,-3.289 -2.683,-5.95400005 -5.9910002,-5.95400005 -3.311,0 -5.993,2.66500005 -5.993,5.95400005 0,3.2870004 2.683,5.9520004 5.993,5.9520004 3.3080002,0 5.9910002,-2.665 5.9910002,-5.9520004 z m -11.0660002,0.065 c 0,-2.806 2.279,-5.076 5.092,-5.076 2.8110002,0 5.0880002,2.271 5.0880002,5.076 0,2.804 -2.277,5.0750004 -5.0880002,5.0750004 -2.813,0 -5.092,-2.2720004 -5.092,-5.0750004 z");
            path1.AddCssClass("si-glyph-fill");
            svgTag2.InnerHtml.AppendHtml(path1);

            var path2 = new TagBuilder("path");
            path2.Attributes.Add("d", "m 2.1965269,15.514568 -1.822,-1.822 4.037,-4.0380003 c 0,0 0.096,0.7650003 0.58,1.2470003 0.482,0.484 1.242,0.576 1.242,0.576 z");
            path2.AddCssClass("si-glyph-fill");
            svgTag2.InnerHtml.AppendHtml(path2);

            button.InnerHtml.AppendHtml(buttonSvg);

            searchField.InnerHtml.AppendHtml(button);

            form.InnerHtml.AppendHtml(searchField);

            divTag.InnerHtml.AppendHtml(form);

            return divTag;
        }
    }
}
