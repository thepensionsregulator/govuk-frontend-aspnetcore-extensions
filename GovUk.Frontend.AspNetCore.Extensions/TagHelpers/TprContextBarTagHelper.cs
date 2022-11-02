using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Generates a TPR context bar component
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.TprContextBarElement)]
    public class TprContextBarTagHelper : TagHelper
    {
        internal const string TagName = "tpr-context-bar";

        private const string Context1AttributeName = "context-1";
        private const string Context2AttributeName = "context-2";
        private const string Context3AttributeName = "context-3";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TprContextBarTagHelper"/>.
        /// </summary>
        public TprContextBarTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprContextBarTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// The text displayed at the left of the context bar.
        /// </summary>
        [HtmlAttributeName(Context1AttributeName)]
        public string? Context1 { get; set; }

        /// <summary>
        /// The text displayed in the middle of the context bar.
        /// </summary>
        [HtmlAttributeName(Context2AttributeName)]
        public string? Context2 { get; set; }

        /// <summary>
        /// The text displayed at the right of the context bar.
        /// </summary>
        [HtmlAttributeName(Context3AttributeName)]
        public string? Context3 { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrWhiteSpace(Context1) || !string.IsNullOrWhiteSpace(Context2) || !string.IsNullOrWhiteSpace(Context3))
            {
                var tagBuilder = _htmlGenerator.GenerateTprContextBar(Context1, Context2, Context3, output.Attributes.ToAttributeDictionary());

                output.TagName = tagBuilder.TagName;
                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Clear();
                output.MergeAttributes(tagBuilder);
                output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            }
        }
    }
}
