using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;
//using ComponentGenerator = ThePensionsRegulator.Frontend.HtmlGeneration.ComponentGenerator;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprTimelineItemTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TimelineElement)]
    public class TprTimelineTagHelper : TagHelper
    {
        internal const string TagName = "tpr-timeline";

        private readonly ITprHtmlGenerator _htmlGenerator;
        
        [HtmlAttributeName("datesize")]
        public string? DateSize { get; set; } = string.Empty; 


        /// <summary>
        /// Creates a new <see cref="TprFooterBarTagHelper"/>.
        /// </summary>
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

            using (context.SetScopedContextItem(timelineContext))
            {
                await output.GetChildContentAsync();
            }

            timelineContext.DateSize = DateSize;

            timelineContext.ThrowIfIncomplete();

            var tagBuilder = _htmlGenerator.GenerateTprTimeline(
                output.Attributes.ToAttributeDictionary(),
                timelineContext.Tasks,
                timelineContext.DateSize
            );

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
