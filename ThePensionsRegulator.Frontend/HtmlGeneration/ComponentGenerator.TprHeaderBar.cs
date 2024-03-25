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
            tagBuilder.InnerHtml.AppendHtml(headerContent);

            return tagBuilder;
        }
    }
}
