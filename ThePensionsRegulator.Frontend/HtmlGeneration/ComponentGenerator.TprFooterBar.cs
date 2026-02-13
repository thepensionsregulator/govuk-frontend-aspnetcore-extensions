using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions.Caching;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprFooterBarElement = "footer";
        public const string FooterLogoDefaultAlt = "The Pensions Regulator home page";
        internal const string FooterLogoDefaultHref = "https://www.thepensionsregulator.gov.uk";
        internal static string CopyrightDefaultContent = $"{DateTimeOffset.UtcNow.Year} The Pensions Regulator";

        public virtual TagBuilder GenerateTprFooterBar(TprFooterBar tprFooterBar)
        {
            var tagBuilder = new TagBuilder(TprFooterBarElement);
            if (tprFooterBar.FooterBarAttributes != null) { tagBuilder.MergeAttributes(tprFooterBar.FooterBarAttributes); }
            tagBuilder.MergeCssClass("tpr-footer");
            tagBuilder.MergeAttribute("role", "contentinfo");

            var widthContainer = new TagBuilder("div");
            widthContainer.MergeCssClass("govuk-width-container");
            tagBuilder.InnerHtml.AppendHtml(widthContainer);

            var upperFooterContainer = new TagBuilder("div");
            upperFooterContainer.MergeCssClass("govuk-grid-row");
            widthContainer.InnerHtml.AppendHtml(upperFooterContainer);

            var logoContainer = new TagBuilder("div");
            logoContainer.MergeCssClass("govuk-grid-column-one-quarter tpr-footer__footer-logo");

            var logoIsLinked = !string.IsNullOrEmpty(tprFooterBar.LogoHref);
            var logoElement = new TagBuilder(logoIsLinked ? "a" : "span");
            if (logoIsLinked) { logoElement.Attributes.Add("href", tprFooterBar.LogoHref); }

            var logo = new TagBuilder("img");
            logo.TagRenderMode = TagRenderMode.SelfClosing;
            if (tprFooterBar.LogoAttributes != null) { logo.MergeAttributes(tprFooterBar.LogoAttributes); }
            logo.Attributes.Add("src", $"/ThePensionsRegulator.Frontend/img/tpr-logo-footer.svg?{CachingConstants.StaticAssetVersionQueryParamName}={TprFrontendVersion}");
            logo.Attributes.Add("alt", tprFooterBar.LogoAlternativeText);
            logo.Attributes.Add("width", "126");
            logo.Attributes.Add("height", "47");
            logoElement.InnerHtml.AppendHtml(logo);

            logoContainer.InnerHtml.AppendHtml(logoElement);
            upperFooterContainer.InnerHtml.AppendHtml(logoContainer);

            if (tprFooterBar.ThreeColumnLinks != null && tprFooterBar.ThreeColumnLinks?.Count > 0)
            {

                foreach (var column in tprFooterBar.ThreeColumnLinks)
                {
                    if (column.ThreeColumnFooterLinks != null && column.ThreeColumnFooterLinks.Count > 0)
                    {
                        var linksColumn = new TagBuilder("div");
                        linksColumn.AddCssClass("govuk-grid-column-one-quarter");

                        var ul = new TagBuilder("ul");
                        if (column.Attributes != null) { ul.MergeAttributes(column.Attributes); }
                        ul.AddCssClass("govuk-list");
                        ul.AddCssClass("tpr-footer__links");

                        foreach (var link in column.ThreeColumnFooterLinks)
                        {
                            var li = new TagBuilder("li");

                            var a = new TagBuilder("a");
                            a.AddCssClass("govuk-link");

                            if (link.LinkText != null)
                            {
                                a.InnerHtml.Append(link.LinkText);
                            }
                            if (link.LinkUrl != null)
                            {
                                a.Attributes.Add("href", link.LinkUrl);
                            }

                            if (link.Attributes != null)
                            {
                                a.MergeAttributes(link.Attributes);
                            }

                            li.InnerHtml.AppendHtml(a);
                            ul.InnerHtml.AppendHtml(li);
                        }

                        linksColumn.InnerHtml.AppendHtml(ul);
                        upperFooterContainer.InnerHtml.AppendHtml(linksColumn);
                    }
                }
            }

            var hasContent = (tprFooterBar.Content != null && !string.IsNullOrWhiteSpace(tprFooterBar.Content.ToString()));
            var hasCopyright = (tprFooterBar.Copyright != null && !string.IsNullOrWhiteSpace(tprFooterBar.Copyright.ToString()));

            if (hasContent || hasCopyright)
            {
                var contentContainer = new TagBuilder("div");
                contentContainer.MergeCssClass("tpr-footer__content-container");

                var contentElement = new TagBuilder("div");
                if (tprFooterBar.ContentAttributes != null) { contentElement.MergeAttributes(tprFooterBar.ContentAttributes); }
                contentElement.MergeCssClass("govuk-body");
                contentElement.MergeCssClass("tpr-footer__content");
                if (hasContent)
                {
                    if (tprFooterBar.ContentAllowHtml)
                    {

                        contentElement.InnerHtml.AppendHtml(tprFooterBar.Content!);

                    }
                    else
                    {
                        contentElement.InnerHtml.Append(tprFooterBar.Content!.ToString()!);
                    }
                }
                else
                {
                    contentElement.MergeCssClass("tpr-footer__content--empty");
                }
                contentContainer.InnerHtml.AppendHtml(contentElement);

                if (hasCopyright)
                {
                    var copyrightElement = new TagBuilder("p");
                    if (tprFooterBar.CopyrightAttributes != null) { copyrightElement.MergeAttributes(tprFooterBar.CopyrightAttributes); }
                    copyrightElement.MergeCssClass("govuk-body");
                    copyrightElement.MergeCssClass("tpr-footer__copyright");
                    copyrightElement.InnerHtml.AppendHtml("&copy; ");
                    if (tprFooterBar.CopyrightAllowHtml)
                    {
                        copyrightElement.InnerHtml.AppendHtml(tprFooterBar.Copyright!);
                    }
                    else
                    {
                        copyrightElement.InnerHtml.Append(tprFooterBar.Copyright!.ToString()!);
                    }
                    contentContainer.InnerHtml.AppendHtml(copyrightElement);
                }
                widthContainer.InnerHtml.AppendHtml(contentContainer);
            }

            return tagBuilder;
        }
    }
}
