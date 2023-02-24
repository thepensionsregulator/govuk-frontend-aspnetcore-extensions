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
            if (attributes != null) { outer.MergeAttributes(attributes); }
            outer.MergeCssClass("tpr-back-to-top");

            var middle = new TagBuilder("div");
            middle.MergeCssClass("govuk-width-container");

            var inner = new TagBuilder("div");
            inner.MergeCssClass("tpr-back-to-top__inner");

            var link = new TagBuilder(BackToTopLinkElement);
            link.MergeCssClass("govuk-link");
            link.Attributes.Add("href", href);

            outer.InnerHtml.AppendHtml(middle);
            middle.InnerHtml.AppendHtml(inner);
            inner.InnerHtml.AppendHtml(link);
            link.InnerHtml.AppendHtml(content);

            return outer;
        }
    }
}
