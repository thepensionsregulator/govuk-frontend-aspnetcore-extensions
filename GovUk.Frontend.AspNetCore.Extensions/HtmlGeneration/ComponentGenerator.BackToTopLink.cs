#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string BackToTopLinkDefaultContent = "Back to top";
        internal const string BackToTopLinkElement = "a";
        internal const string BackToTopLinkDefaultHref = "#";

        public virtual TagBuilder GenerateBackToTopLink(
            string href,
            IHtmlContent content,
            AttributeDictionary? attributes)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(href), href);
            Guard.ArgumentNotNull(nameof(content), content);

            var outer = new TagBuilder("div");
            outer.MergeCssClass("tpr-back-to-top");

            var inner = new TagBuilder("div");
            inner.MergeCssClass("govuk-width-container");

            var link = new TagBuilder(BackToTopLinkElement);
            if (attributes != null) link.MergeAttributes(attributes);
            link.MergeCssClass("govuk-link");
            link.Attributes.Add("href", href);

            outer.InnerHtml.AppendHtml(inner);
            inner.InnerHtml.AppendHtml(link);
            link.InnerHtml.AppendHtml(content);

            return outer;
        }
    }
}
