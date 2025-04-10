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
        internal const string TimelineItemContentElement = "div";

        public virtual TagBuilder GenerateTprTimeline(
            AttributeDictionary? attributes,
            IEnumerable<TprTimelineItem> items,
            string dateSize,
            bool hideTail)
        {
            Guard.ArgumentNotNull(nameof(items), items);
            Guard.ArgumentValid(nameof(items), "A Timeline must contain at least one item", items.Any());

            var timelineTagBuilder = new TagBuilder(TimelineElement);
            if (attributes is not null) { timelineTagBuilder.MergeAttributes(attributes); }
            timelineTagBuilder.MergeCssClass("tpr-timeline");

            
            if (!string.IsNullOrWhiteSpace(dateSize))
            {
                timelineTagBuilder.AddCssClass("tpr-timeline--" + dateSize.ToLower());
            }

            if (hideTail)
            {
                timelineTagBuilder.AddCssClass("tpr-timeline--hide-tail");
            }
            
            var innerItems = new TagBuilder("ol");
            innerItems.MergeCssClass("tpr-timeline__items");

            foreach (var item in items)
            {
                Guard.ArgumentValid(nameof(item), "Date cannot be null or empty", item.DateTime != null);
                Guard.ArgumentValid(nameof(item), "Heading cannot be null or empty", item.Heading != null);

                var itemBuilder = new TagBuilder(TimelineItemElement);
                itemBuilder.MergeCssClass("tpr-timeline__item");
                if(!string.IsNullOrWhiteSpace(item.LineColour))
                {
                    itemBuilder.AddCssClass("tpr-timeline__item--" + item.LineColour.ToLower());
                }
                var itemContentBuilder = new TagBuilder(TimelineItemContentElement);
                itemContentBuilder.AddCssClass("tpr-timeline__item-content");
                
                if (item.Attributes is not null) { itemContentBuilder.MergeAttributes(item.Attributes); }
                itemBuilder.InnerHtml.AppendHtml(itemContentBuilder);
                innerItems.InnerHtml.AppendHtml(itemBuilder);

                itemContentBuilder.InnerHtml.AppendHtml(BuildDateTime(item));
                itemContentBuilder.InnerHtml.AppendHtml(BuildHeading(item));

                if (item.Content != null)
                {
                    itemContentBuilder.InnerHtml.AppendHtml(item.Content);
                }
            }

            timelineTagBuilder.InnerHtml.AppendHtml(innerItems);

            return timelineTagBuilder;
        }

        private static TagBuilder BuildDateTime(TprTimelineItem item)
        {
            var timelineItemDateTime = new TagBuilder("p");
            timelineItemDateTime.MergeCssClass("tpr-timeline__datetime");
            timelineItemDateTime.InnerHtml.AppendHtml(item.DateTime!.ToString());
            return timelineItemDateTime;
        }

        private static TagBuilder BuildHeading(TprTimelineItem item) {
            var timelineItemHeading = new TagBuilder("h2");
            timelineItemHeading.MergeCssClass("tpr-timeline__heading");
            timelineItemHeading.InnerHtml.AppendHtml(item.Heading!.ToString());
            return timelineItemHeading;
        }

    }
}
