using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TimelineElement = "div";
        internal const string TimelineItemElement = "li";

        public virtual TagBuilder GenerateTprTimeline(
            AttributeDictionary? attributes,
            IEnumerable<TprTimelineItem> items)
        {
            Guard.ArgumentNotNull(nameof(items), items);
            Guard.ArgumentValid(nameof(items), "A Timeline must contain at least one item", items.Any());

            var timelineTagBuilder = new TagBuilder(TimelineElement);
            if (attributes is not null) { timelineTagBuilder.MergeAttributes(attributes); }
            timelineTagBuilder.MergeCssClass("dwp-timeline");

            var innerItems = new TagBuilder("ol");
            innerItems.MergeCssClass("dwp-timeline__items");

            foreach (var item in items)
            {
                Guard.ArgumentValid(nameof(item), "Date cannot be null or empty", item.DateTime != null);
                Guard.ArgumentValid(nameof(item), "Heading cannot be null or empty", item.Heading != null);

                var itemBuilder = new TagBuilder(TimelineItemElement);
                itemBuilder.MergeCssClass("dwp-timeline__item");

                if (item.Attributes is not null) { itemBuilder.MergeAttributes(item.Attributes); }
                innerItems.InnerHtml.AppendHtml(itemBuilder);

                itemBuilder.InnerHtml.AppendHtml(BuildDateTime(item));
                itemBuilder.InnerHtml.AppendHtml(BuildHeading(item));

                if (item.Content != null)
                {
                    itemBuilder.InnerHtml.AppendHtml(item.Content);
                }
            }

            timelineTagBuilder.InnerHtml.AppendHtml(innerItems);

            return timelineTagBuilder;
        }

        private static TagBuilder BuildDateTime(TprTimelineItem item)
        {
            var timelineItemDateTime = new TagBuilder("p");
            timelineItemDateTime.MergeCssClass("dwp-timeline__datetime");
            timelineItemDateTime.InnerHtml.AppendHtml(item.DateTime!.ToString());
            return timelineItemDateTime;
        }

        private static TagBuilder BuildHeading(TprTimelineItem item) {
            var timelineItemHeading = new TagBuilder("h2");
            timelineItemHeading.MergeCssClass("dwp-timeline__heading");
            timelineItemHeading.InnerHtml.AppendHtml(item.Heading!.ToString());
            return timelineItemHeading;
        }

    }
}
