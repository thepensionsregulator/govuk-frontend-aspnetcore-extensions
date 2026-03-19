using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        public virtual TagBuilder GenerateTprRelatedLinks(TprRelatedLinks tprRelatedLinks)
        {
            Guard.ArgumentNotNull(nameof(tprRelatedLinks), tprRelatedLinks);
            Guard.ArgumentValid(nameof(tprRelatedLinks), $"{nameof(tprRelatedLinks.HeadingContent)} cannot be null", tprRelatedLinks.HeadingContent != null);

            var outer = new TagBuilder("nav");
            if (tprRelatedLinks.RelatedLinksAttributes != null) { outer.MergeAttributes(tprRelatedLinks.RelatedLinksAttributes); }
            outer.MergeCssClass("tpr-related-links");

            var heading = new TagBuilder("h2");
            if (tprRelatedLinks.HeadingAttributes != null) { heading.MergeAttributes(tprRelatedLinks.HeadingAttributes); }
            heading.MergeCssClass("govuk-heading-m");
            heading.InnerHtml.AppendHtml(tprRelatedLinks.HeadingContent!);
            outer.InnerHtml.AppendHtml(heading);

            var list = new TagBuilder("ul");
            list.MergeCssClass("govuk-list");
            outer.InnerHtml.AppendHtml(list);

            foreach (var link in tprRelatedLinks.Links)
            {
                var li = new TagBuilder("li");
                var a = new TagBuilder("a");
                a.MergeAttributes(link.Attributes);
                a.MergeCssClass("govuk-link");
                a.InnerHtml.AppendHtml(link.Content);
                li.InnerHtml.AppendHtml(a);
                list.InnerHtml.AppendHtml(li);
            }

            return outer;
        }
    }
}
