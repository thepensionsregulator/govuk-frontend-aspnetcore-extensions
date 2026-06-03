using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
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

        private readonly ITprHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TprContextBarTagHelper"/>.
        /// </summary>
        public TprContextBarTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprContextBarTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = new TprContextBarContext();

            using (context.SetScopedContextItem(barContext))
            {
                await output.GetChildContentAsync();
            }

            if (!string.IsNullOrWhiteSpace(barContext.Context1Content?.ToString()) ||
                !string.IsNullOrWhiteSpace(barContext.Context2Content?.ToString()) ||
                !string.IsNullOrWhiteSpace(barContext.Context3Content?.ToString()))
            {
                var tagBuilder = _htmlGenerator.GenerateTprContextBar(new TprContextBar
                {
                    ContextBarAttributes = output.Attributes.ToAttributeDictionary(),
                    Context1Attributes = barContext.Context1Attributes,
                    Context1Content = barContext.Context1Content,
                    Context1AllowHtml = barContext.Context1AllowHtml,
                    Context2Attributes = barContext.Context2Attributes,
                    Context2Content = barContext.Context2Content,
                    Context2AllowHtml = barContext.Context2AllowHtml,
                    Context3Attributes = barContext.Context3Attributes,
                    Context3Content = barContext.Context3Content,
                    Context3AllowHtml = barContext.Context3AllowHtml
                });

                output.TagName = tagBuilder.TagName;
                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Clear();
                output.MergeAttributes(tagBuilder);
                output.Content.SetHtmlContent(tagBuilder.InnerHtml);
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}
