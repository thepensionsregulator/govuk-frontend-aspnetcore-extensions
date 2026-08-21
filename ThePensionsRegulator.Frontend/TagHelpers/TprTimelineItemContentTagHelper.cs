using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.GovUk.Frontend;
using ComponentGenerator = ThePensionsRegulator.Frontend.HtmlGeneration.ComponentGenerator;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.TimelineItemElement)]
    public class TprTimelineItemContentTagHelper : TagHelper
    {
        internal const string TagName = "tpr-timeline-item-content";


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskContext = context.GetContextItem<TprTimelineItemContext>();

            HtmlString? htmlContent = null;
            using (context.SetScopedContextItem(taskContext))
            {
                var content = (await output.GetChildContentAsync()).GetContent();
                if (!string.IsNullOrEmpty(content))
                {
                    htmlContent = new HtmlString(content);
                    taskContext.HtmlContent = htmlContent;
                }
            }

            output.SuppressOutput();
        }

    }
}

