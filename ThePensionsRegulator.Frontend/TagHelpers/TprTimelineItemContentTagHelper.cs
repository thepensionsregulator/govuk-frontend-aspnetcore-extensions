using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using System.Collections.Generic;
using ComponentGenerator = ThePensionsRegulator.Frontend.HtmlGeneration.ComponentGenerator;
using System;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics;
using Microsoft.AspNetCore.Html;

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
                }
            }

            taskContext.HtmlContent = htmlContent;
            output.SuppressOutput();
        }

    }
}

