#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprFooterBarElement = "footer";
        public const string FooterLogoDefaultAlt = "Go to The Pensions Regulator website";
        internal const string FooterLogoDefaultHref = "https://www.thepensionsregulator.gov.uk";
        internal static string CopyrightDefaultContent = $"{DateTimeOffset.UtcNow.Year} The Pensions Regulator";

        public virtual TagBuilder GenerateTprFooterBar(
            string? logoHref,
            string logoAlt,
            IHtmlContent? content,
            string copyright,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(logoAlt), logoAlt);

            var tagBuilder = new TagBuilder(TprFooterBarElement);
            if (attributes != null) tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("tpr-footer");
            tagBuilder.MergeAttribute("role", "contentinfo");

            var footerContent = new TagBuilder("div");
            footerContent.MergeCssClass("govuk-width-container");

            var logoContainer = new TagBuilder("div");
            logoContainer.MergeCssClass("tpr-footer__footer-logo");

            var logoIsLinked = !string.IsNullOrEmpty(logoHref);
            var logoElement = new TagBuilder(logoIsLinked ? "a" : "span");
            if (logoIsLinked) { logoElement.Attributes.Add("href", logoHref); }

            var logo = new TagBuilder("img");
            logo.TagRenderMode = TagRenderMode.SelfClosing;
            logo.Attributes.Add("src", "/_content/GovUK.Frontend.AspNetCore.Extensions/tpr/tpr-logo-footer.svg");
            logo.Attributes.Add("alt", logoAlt);
            logo.Attributes.Add("width", "126");
            logo.Attributes.Add("height", "47");
            logoElement.InnerHtml.AppendHtml(logo);

            logoContainer.InnerHtml.AppendHtml(logoElement);
            footerContent.InnerHtml.AppendHtml(logoContainer);

            var hasContent = !(content as TagHelperContent)?.IsEmptyOrWhiteSpace ?? true;
            var hasCopyright = !string.IsNullOrWhiteSpace(copyright);

            if (hasContent || hasCopyright)
            {
                var contentContainer = new TagBuilder("div");
                contentContainer.MergeCssClass("tpr-footer__content-container");

                var contentElement = new TagBuilder("div");
                contentElement.MergeCssClass("govuk-body");
                contentElement.MergeCssClass("tpr-footer__content");
                if (hasContent)
                {
                    contentElement.InnerHtml.AppendHtml(content!);
                }
                else
                {
                    contentElement.MergeCssClass("tpr-footer__content--empty");
                }
                contentContainer.InnerHtml.AppendHtml(contentElement);

                if (hasCopyright)
                {
                    var copyrightElement = new TagBuilder("p");
                    copyrightElement.MergeCssClass("govuk-body");
                    copyrightElement.MergeCssClass("tpr-footer__copyright");
                    copyrightElement.InnerHtml.Append("© " + copyright);
                    contentContainer.InnerHtml.AppendHtml(copyrightElement);
                }
                footerContent.InnerHtml.AppendHtml(contentContainer);
            }

            tagBuilder.InnerHtml.AppendHtml(footerContent);

            return tagBuilder;
        }
    }
}
