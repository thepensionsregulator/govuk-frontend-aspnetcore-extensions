#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprHeaderElement = "div";
        internal const string DefaultHeaderLabel = "Making workplace pensions work";
        public const string HeaderLogoDefaultAlt = "Go to The Pensions Regulator website";
        internal const string HeaderLogoDefaultHref = "https://www.thepensionsregulator.gov.uk";

        public virtual TagBuilder GenerateTprHeader(
            string? logoHref,
            string logoAlt,
            string? label,
            IHtmlContent? content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(logoAlt), logoAlt);

            var tagBuilder = new TagBuilder(TprHeaderElement);
            if (attributes != null) tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("tpr-header");

            var headerContent = new TagBuilder("div");
            headerContent.MergeCssClass("govuk-width-container");
            headerContent.MergeCssClass("tpr-header__inner");

            var logoIsLinked = !string.IsNullOrEmpty(logoHref);
            var logoElement = new TagBuilder(logoIsLinked ? "a" : "span");
            if (logoIsLinked) { logoElement.Attributes.Add("href", logoHref); }
            logoElement.MergeCssClass("tpr-header__logo");
            var logo = new TagBuilder("img");
            logo.TagRenderMode = TagRenderMode.SelfClosing;
            logo.Attributes.Add("src", "/_content/GovUK.Frontend.AspNetCore.Extensions/tpr/tpr-logo-header.svg");
            logo.Attributes.Add("alt", logoAlt);
            logo.Attributes.Add("width", "180");
            logo.Attributes.Add("height", "75");
            logoElement.InnerHtml.AppendHtml(logo);
            headerContent.InnerHtml.AppendHtml(logoElement);

            if (!string.IsNullOrEmpty(label))
            {
                var labelElement = new TagBuilder("p");
                labelElement.MergeCssClass("govuk-body");
                labelElement.MergeCssClass("tpr-header__label");
                labelElement.InnerHtml.Append(label);
                headerContent.InnerHtml.AppendHtml(labelElement);
            }

            if (!string.IsNullOrEmpty(content?.ToString()?.Trim()))
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
