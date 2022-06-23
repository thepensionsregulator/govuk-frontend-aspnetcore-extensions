using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Generates a link that cancels the current operation.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.BackToMenuElement)]
    public class BackToMenuTagHelper : TagHelper
    {
        internal const string TagName = "govuk-back-to-menu";

        private static readonly HtmlString _defaultContent = new HtmlString(ComponentGenerator.BackToMenuDefaultContent);

        private const string HrefAttributeName = "href";

        private string _href = ComponentGenerator.BackToMenuDefaultHref;

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="BackToMenuTagHelper"/>.
        /// </summary>
        public BackToMenuTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal BackToMenuTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// The <c>href</c> attribute for the link.
        /// </summary>
        /// <remarks>
        /// The default is <c>&quot;#content&quot;</c>.
        /// Cannot be <c>null</c> or empty.
        /// </remarks>
        [HtmlAttributeName(HrefAttributeName)]
        public string Href
        {
            get => _href;
            set => _href = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            IHtmlContent content = _defaultContent;

            if (output.TagMode == TagMode.StartTagAndEndTag)
            {
                content = await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateBackToMenu(Href, content, output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
