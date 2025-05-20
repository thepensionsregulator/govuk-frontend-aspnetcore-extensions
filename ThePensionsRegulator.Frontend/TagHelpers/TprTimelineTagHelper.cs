using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using System;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprTimelineItemTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TimelineElement)]
    public class TprTimelineTagHelper : TagHelper
    {
        internal const string TagName = "tpr-timeline";
        internal const int TimelineMinHeadingLevel = 1;
        internal const int TimelineMaxHeadingLevel = 6;
        internal const int TimelineDefaultHeadingLevel = 2;
        private int _headingLevel = TimelineDefaultHeadingLevel;

        private readonly ITprHtmlGenerator _htmlGenerator;
        
        [HtmlAttributeName("hide-tail")] //     
        public bool? HideTail {get; set;} = false;

        [HtmlAttributeName("heading-level")]
        public int HeadingLevel
        {
            get => _headingLevel;
            set
            {
                if (value < TimelineMinHeadingLevel ||
                    value > TimelineMaxHeadingLevel)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingLevel)} must be between {TimelineMinHeadingLevel} and {TimelineMaxHeadingLevel}.");
                }

                _headingLevel = value;
            }
        }
        
        [HtmlAttributeName("aria-title")]
        public string? AriaTitle { get; set; } = string.Empty; 

        public TprTimelineTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprTimelineTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var timelineContext = new TprTimelineContext();

            if(HideTail.HasValue)
            {
                timelineContext.HideTail = HideTail.Value;
            }
            
            timelineContext.HeadingLevel = HeadingLevel;

            if (!string.IsNullOrWhiteSpace(AriaTitle))
            {
                timelineContext.AriaTitle = AriaTitle;
            }

            using (context.SetScopedContextItem(timelineContext))
            {
                await output.GetChildContentAsync();
            }

            timelineContext.ThrowIfIncomplete();

            var tagBuilder = _htmlGenerator.GenerateTprTimeline(
                output.Attributes.ToAttributeDictionary(),
                timelineContext.Tasks,
                timelineContext.HideTail,
                timelineContext.HeadingLevel,
                timelineContext.AriaTitle
            );

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
