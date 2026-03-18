using System.Threading.Tasks;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.TprAddressLookupElement)]
    public class TprAddressLookupTagHelper : TagHelper
    {
        internal const string TagName = "tpr-address-lookup";
        internal const string DescribedByAttributeName = "described-by";
        internal const string SameAsPrimaryCheckboxLabelAttributeName = "same-as-primary-checkbox-label";

        private readonly ITprHtmlGenerator _htmlGenerator;

        internal TprAddressLookupTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public TprAddressLookupTagHelper() : this(null)
        {

        }

        [HtmlAttributeName(DescribedByAttributeName)]
        public string? DescribedBy { get; set; }

        [EditorRequired()]
        [HtmlAttributeName(SameAsPrimaryCheckboxLabelAttributeName)]
        public string SameAsPrimaryCheckboxLabel { get; set; } = null!;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var addressLookupContext = new TprAddressLookupContext();

            IHtmlContent childContent;
            using (context.SetScopedContextItem(addressLookupContext))
            {
                childContent = await output.GetChildContentAsync();
            }

            addressLookupContext.ThrowIfNotComplete();


            var tagBuilder = _htmlGenerator.GenerateTprAddressLookup(
                addressLookupContext.IsLegendPageHeading,
                addressLookupContext.LegendAttributes,
                addressLookupContext.Legend,
                childContent,
                DescribedBy,
                SameAsPrimaryCheckboxLabel);

            output.TagName = tagBuilder.TagName;
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}