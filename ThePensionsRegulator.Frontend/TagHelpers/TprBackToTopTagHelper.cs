using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    /// <summary>
    /// Generates a GOV.UK back link component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.BackToTopLinkElement)]
    public class TprBackToTopTagHelper : TagHelper
    {
        internal const string TagName = "tpr-back-to-top";

        private static readonly HtmlString _defaultContent = new HtmlString(ComponentGenerator.BackToTopLinkDefaultContent);

        private const string HrefAttributeName = "href";

        private string _href = ComponentGenerator.BackToTopLinkDefaultHref;

        private readonly ITprHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TprBackToTopTagHelper"/>.
        /// </summary>
        public TprBackToTopTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprBackToTopTagHelper(ITprHtmlGenerator? htmlGenerator)
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

            var tagBuilder = _htmlGenerator.GenerateTprBackToTop(Href, content, output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
