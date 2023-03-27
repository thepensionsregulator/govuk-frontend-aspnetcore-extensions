using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Generates a TPR context bar component
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprContextBarContext1TagHelper.TagName, TprContextBarContext2TagHelper.TagName, TprContextBarContext3TagHelper.TagName)]
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
        [Obsolete($"Use {TprContextBarContext1TagHelper.TagName} child tag helper")]
        [HtmlAttributeName(Context1AttributeName)]
        public string? Context1 { get; set; }

        /// <summary>
        /// The text displayed in the middle of the context bar.
        /// </summary>
        [Obsolete($"Use {TprContextBarContext2TagHelper.TagName} child tag helper")]
        [HtmlAttributeName(Context2AttributeName)]
        public string? Context2 { get; set; }

        /// <summary>
        /// The text displayed at the right of the context bar.
        /// </summary>
        [Obsolete($"Use {TprContextBarContext3TagHelper.TagName} child tag helper")]
        [HtmlAttributeName(Context3AttributeName)]
        public string? Context3 { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = new TprContextBarContext();

            using (context.SetScopedContextItem(barContext))
            {
                await output.GetChildContentAsync();
            }

            var context1Content = barContext.Context1Content != null ? barContext.Context1Content : new HtmlString(Context1);
            var context2Content = barContext.Context2Content != null ? barContext.Context2Content : new HtmlString(Context2);
            var context3Content = barContext.Context3Content != null ? barContext.Context3Content : new HtmlString(Context3);

            if (!string.IsNullOrWhiteSpace(context1Content.ToString()) || !string.IsNullOrWhiteSpace(context2Content.ToString()) || !string.IsNullOrWhiteSpace(context3Content.ToString()))
            {
                var tagBuilder = _htmlGenerator.GenerateTprContextBar(new TprContextBar
                {
                    ContextBarAttributes = output.Attributes.ToAttributeDictionary(),
                    Context1Attributes = barContext.Context1Attributes,
                    Context1Content = context1Content,
                    Context1AllowHtml = barContext.Context1AllowHtml,
                    Context2Attributes = barContext.Context2Attributes,
                    Context2Content = context2Content,
                    Context2AllowHtml = barContext.Context2AllowHtml,
                    Context3Attributes = barContext.Context3Attributes,
                    Context3Content = context3Content,
                    Context3AllowHtml = barContext.Context3AllowHtml
                });

                output.TagName = tagBuilder.TagName;
                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Clear();
                output.MergeAttributes(tagBuilder);
                output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            }
        }
    }
}
