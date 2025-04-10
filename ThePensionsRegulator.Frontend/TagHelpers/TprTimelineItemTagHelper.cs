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
using System.Runtime.CompilerServices;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.TimelineItemElement)]
    public class TprTimelineItemTagHelper : TagHelper
    {
        internal const string TagName = "tpr-timeline-item";

        /// <summary>
        /// Additional attributes to add to the generated <c>a</c> element where <c>.govuk-link</c> is applied.
        /// </summary>
        //[HtmlAttributeName(DictionaryAttributePrefix = LinkAttributesPrefix)]

        //public IDictionary<string, string?>? LinkAttributes { get; set; } = new Dictionary<string, string?>();

        /// <summary>
        /// A link to the task.
        /// </summary>
        [HtmlAttributeName("date")]
        public string? Date { get; set; } = string.Empty; 

        [HtmlAttributeName("heading")]
        public string? Heading { get; set; } = string.Empty; 

        [HtmlAttributeName("line-colour")]
        public string? LineColour { get; set; } = string.Empty;
        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskListContext = context.GetContextItem<TprTimelineContext>();

            var itemContext = new TprTimelineItemContext();

            using (context.SetScopedContextItem(itemContext))
            {
                await output.GetChildContentAsync();
            }            

            taskListContext.AddItem(new TprTimelineItem
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                Heading = Heading,
                DateTime = Date,
                Content = itemContext.HtmlContent,
                LineColour = LineColour
            });

            output.SuppressOutput();
        }
    }
}
