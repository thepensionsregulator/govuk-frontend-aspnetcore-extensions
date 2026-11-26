using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.GovUk.Frontend;
using ComponentGenerator = ThePensionsRegulator.Frontend.HtmlGeneration.ComponentGenerator;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.TimelineItemElement)]
    public class TprTimelineItemTagHelper : TagHelper
    {
        internal const string TagName = "tpr-timeline-item";

        [HtmlAttributeName("date")]
        public string? Date { get; set; } = string.Empty;

        [HtmlAttributeName("heading")]
        public string? Heading { get; set; } = string.Empty;

        [HtmlAttributeName("attributes")]
        public AttributeDictionary? Attributes { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskListContext = context.GetContextItem<TprTimelineContext>();

            var itemContext = new TprTimelineItemContext();

            using (context.SetScopedContextItem(itemContext))
            {
                await output.GetChildContentAsync();
            }

            var attributes = output.Attributes.ToAttributeDictionary();

            if (Attributes is not null)
            {
                foreach (var attribute in Attributes)
                {
                    attributes[attribute.Key] = attribute.Value;
                }
            }

            taskListContext.AddItem(new TprTimelineItem
            {
                Attributes = attributes,
                Heading = Heading,
                DateTime = Date,
                Content = itemContext.HtmlContent
            });

            output.SuppressOutput();
        }
    }
}
