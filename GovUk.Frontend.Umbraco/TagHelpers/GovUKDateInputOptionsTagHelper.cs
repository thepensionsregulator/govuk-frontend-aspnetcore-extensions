using GovUk.Frontend.Umbraco.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

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

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Grab the HTML that would've been rendered by the child tag helper.
            var html = (await output.GetChildContentAsync()).GetContent();
            output.SuppressOutput();

            html = _htmlEnhancer.EnhanceHtml(html, ShowDay);

            // Output the child HTML with any modifications made
            output.Content.AppendHtml(html);
        }
    }
}
