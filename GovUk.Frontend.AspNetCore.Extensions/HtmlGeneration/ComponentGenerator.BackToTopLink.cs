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

            var tagBuilder = new TagBuilder(BackToTopLinkElement);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.MergeCssClass("govuk-back-to-top-link");
            tagBuilder.Attributes.Add("href", href);
            tagBuilder.InnerHtml.AppendHtml(content);

            return tagBuilder;
        }
    }
}
