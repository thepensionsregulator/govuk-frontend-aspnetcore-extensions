using GovUk.Frontend.Umbraco.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.Umbraco.TagHelpers
{
    [HtmlTargetElement("govuk-date-input-options")]
    [RestrictChildren("govuk-date-input")]
    public class GovUkDateInputOptionsTagHelper : TagHelper
    {
        private readonly IDateInputHtmlEnhancer _htmlEnhancer;

        public GovUkDateInputOptionsTagHelper(IDateInputHtmlEnhancer htmlEnhancer)
        {
            _htmlEnhancer = htmlEnhancer ?? throw new ArgumentNullException(nameof(htmlEnhancer));
        }

        /// <summary>
        /// Gets or sets whether to show the day field.
        /// </summary>
        [HtmlAttributeName("show-day")]
        public bool ShowDay { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to the show the year field.
        /// </summary>
        [HtmlAttributeName("show-year")]
        public bool ShowYear { get; set; } = true;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!ShowDay && !ShowYear)
            {
                throw new InvalidOperationException("At least one of 'show-day' or 'show-year' must be true.");
            }

            // Grab the HTML that would've been rendered by the child tag helper.
            var html = (await output.GetChildContentAsync()).GetContent();
            output.SuppressOutput();

            html = _htmlEnhancer.EnhanceHtml(html, ShowDay, ShowYear);

            // Output the child HTML with any modifications made
            output.Content.AppendHtml(html);
        }
    }
}
