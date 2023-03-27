using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Generates a TPR header bar component
    /// </summary>
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprHeaderBarLogoTagHelper.TagName, TprHeaderBarLabelTagHelper.TagName, TprHeaderBarContentTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TprHeaderBarElement)]
    public class TprHeaderBarTagHelper : TagHelper
    {
        internal const string TagName = "tpr-header-bar";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TprHeaderBarTagHelper"/>.
        /// </summary>
        public TprHeaderBarTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprHeaderBarTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = new TprHeaderBarContext();

            using (context.SetScopedContextItem(barContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateTprHeaderBar(new TprHeaderBar
            {
                HeaderBarAttributes = output.Attributes.ToAttributeDictionary(),
                LogoAttributes = barContext.LogoAttributes,
                LogoHref = barContext.LogoHref ?? ComponentGenerator.HeaderLogoDefaultHref,
                LogoAlternativeText = barContext.LogoAlternativeText ?? ComponentGenerator.HeaderLogoDefaultAlt,
                LabelAttributes = barContext.LabelAttributes,
                Label = barContext.Label ?? new HtmlString(ComponentGenerator.DefaultHeaderLabel),
                LabelAllowHtml = barContext.LabelAllowHtml,
                ContentAttributes = barContext.ContentAttributes,
                Content = barContext.Content,
                ContentAllowHtml = barContext.ContentAllowHtml
            });

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
