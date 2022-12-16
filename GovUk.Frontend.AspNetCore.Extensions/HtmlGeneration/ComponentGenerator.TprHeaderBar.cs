#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprHeaderBarElement = "div";
        internal const string DefaultHeaderLabel = "Making workplace pensions work";
        public const string HeaderLogoDefaultAlt = "The Pensions Regulator home page";
        internal const string HeaderLogoDefaultHref = "https://www.thepensionsregulator.gov.uk";

        public virtual TagBuilder GenerateTprHeaderBar(
            string? logoHref,
            string logoAlt,
            string? label,
            IHtmlContent? content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(logoAlt), logoAlt);

            var tagBuilder = new TagBuilder(TprHeaderBarElement);
            if (attributes != null) tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("tpr-header");

            var headerContent = new TagBuilder("div");
            headerContent.MergeCssClass("govuk-width-container");
            headerContent.MergeCssClass("tpr-header__inner");

            var logoIsLinked = !string.IsNullOrEmpty(logoHref);
            var logoElement = new TagBuilder(logoIsLinked ? "a" : "span");
            if (logoIsLinked) { logoElement.Attributes.Add("href", logoHref); }
            logoElement.MergeCssClass("tpr-header__logo");

            var screenLogo = new TagBuilder("img");
            screenLogo.TagRenderMode = TagRenderMode.SelfClosing;
            screenLogo.Attributes.Add("src", "/_content/GovUK.Frontend.AspNetCore.Extensions/tpr/tpr-logo-header.svg");
            screenLogo.Attributes.Add("alt", logoAlt);
            screenLogo.Attributes.Add("width", "180");
            screenLogo.Attributes.Add("height", "75");
            screenLogo.MergeCssClass("tpr-header__logo-img--screen");
            logoElement.InnerHtml.AppendHtml(screenLogo);

            var printLogo = new TagBuilder("img");
            printLogo.TagRenderMode = TagRenderMode.SelfClosing;
            printLogo.Attributes.Add("src", "/_content/GovUK.Frontend.AspNetCore.Extensions/tpr/tpr-logo-footer.svg");
            printLogo.Attributes.Add("alt", logoAlt);
            printLogo.Attributes.Add("width", "180");
            printLogo.Attributes.Add("height", "75");
            printLogo.MergeCssClass("tpr-header__logo-img--print");
            logoElement.InnerHtml.AppendHtml(printLogo);

            headerContent.InnerHtml.AppendHtml(logoElement);

            if (!string.IsNullOrEmpty(label))
            {
                var labelElement = new TagBuilder("p");
                labelElement.MergeCssClass("govuk-body");
                labelElement.MergeCssClass("tpr-header__label");
                labelElement.InnerHtml.Append(label);
                headerContent.InnerHtml.AppendHtml(labelElement);
            }

            if (!(content as TagHelperContent)?.IsEmptyOrWhiteSpace ?? true)
            {
                var contentElement = new TagBuilder("div");
                contentElement.MergeCssClass("govuk-body");
                contentElement.MergeCssClass("tpr-header__content");
                headerContent.InnerHtml.AppendHtml(contentElement);
                contentElement.InnerHtml.AppendHtml(content);
            }
            tagBuilder.InnerHtml.AppendHtml(headerContent);

            return tagBuilder;
        }
    }
}
