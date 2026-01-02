using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TimelineElement = "ol";
        internal const string TimelineItemElement = "li";
        internal const string TimelineItemContentElement = "div";

        public virtual TagBuilder GenerateTprTimeline(
            AttributeDictionary? attributes,
            IEnumerable<TprTimelineItem> items,
            bool hideTail,
            int headingLevel,
            string? ariaTitle)
        {
            Guard.ArgumentNotNull(nameof(items), items);
            Guard.ArgumentValid(nameof(items), "A Timeline must contain at least one item", items.Any());

            var timelineTagBuilder = new TagBuilder(TimelineElement);
            if (attributes is not null) { timelineTagBuilder.MergeAttributes(attributes); }
            timelineTagBuilder.MergeCssClass("tpr-timeline");
            var accessibleTitle = "Timeline";
            if (!string.IsNullOrWhiteSpace(ariaTitle))
            {
                accessibleTitle = ariaTitle;
            }
            timelineTagBuilder.Attributes.Add("aria-label", accessibleTitle);

            if (hideTail)
            {
                timelineTagBuilder.AddCssClass("tpr-timeline--hide-tail");
            }

            foreach (var item in items)
            {
                Guard.ArgumentValid(nameof(item), "Date cannot be null or empty", item.DateTime != null);
                // Guard.ArgumentValid(nameof(item), "Heading cannot be null or empty", item.Heading != null);

                var itemBuilder = new TagBuilder(TimelineItemElement);
                var itemContentBuilder = new TagBuilder(TimelineItemContentElement);
                itemContentBuilder.AddCssClass("tpr-timeline__item-content");

                if (item.Attributes is not null) { itemBuilder.MergeAttributes(item.Attributes); }

                itemBuilder.MergeCssClass("tpr-timeline__item");
                itemBuilder.InnerHtml.AppendHtml(itemContentBuilder);
                timelineTagBuilder.InnerHtml.AppendHtml(itemBuilder);

                itemContentBuilder.InnerHtml.AppendHtml(BuildDateTime(item));
                if (!string.IsNullOrWhiteSpace(item.Heading))
                {
                    itemContentBuilder.InnerHtml.AppendHtml(BuildHeading(item, headingLevel));
                }

                if (item.Content != null)
                {
                    itemContentBuilder.InnerHtml.AppendHtml(item.Content);
                }
            }

            return timelineTagBuilder;
        }

        private static TagBuilder BuildDateTime(TprTimelineItem item)
        {
            var timelineItemDateTime = new TagBuilder("p");
            timelineItemDateTime.MergeCssClass("tpr-timeline__datetime");
            timelineItemDateTime.InnerHtml.AppendHtml(item.DateTime!.ToString());
            return timelineItemDateTime;
        }

        private static TagBuilder BuildHeading(TprTimelineItem item, int headingLevel)
        {
            var timelineItemHeading = new TagBuilder($"h{headingLevel}");
            timelineItemHeading.MergeCssClass("tpr-timeline__heading");
            timelineItemHeading.InnerHtml.AppendHtml(item.Heading!.ToString());
            return timelineItemHeading;
        }

    }
}
