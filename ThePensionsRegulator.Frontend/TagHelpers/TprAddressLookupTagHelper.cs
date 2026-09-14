using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprAddressLookupTagHelper : TagHelper
    {
        internal const string TagName = "tpr-address-lookup";

        private readonly ITprHtmlGenerator _htmlGenerator;

        [HtmlAttributeName("legend")]
        public string Legend { get; set; } = "Your address";

        [HtmlAttributeName("hint")]
        public string Hint { get; set; } = "Enter your address";

        [HtmlAttributeName("search-button-text")]
        public string SearchButtonText { get; set; } = "Find address";

        [HtmlAttributeName("error-message")]
        public string ErrorMessage { get; set; } = "Enter your address";

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; } = default!;

        [HtmlAttributeName("address-line-1-label")]
        public string? AddressLine1Label { get; set; }

        [HtmlAttributeName("address-line-2-label")]
        public string? AddressLine2Label { get; set; }

        [HtmlAttributeName("address-line-3-label")]
        public string? AddressLine3Label { get; set; }

        [HtmlAttributeName("post-town-label")]
        public string? PostTownLabel { get; set; }

        [HtmlAttributeName("post-county-label")]
        public string? PostCountyLabel { get; set; }

        [HtmlAttributeName("postcode-label")]
        public string? PostcodeLabel { get; set; }

        [HtmlAttributeName("country-label")]
        public string? CountryLabel { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        internal TprAddressLookupTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public TprAddressLookupTagHelper() : this(null)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!typeof(TprAddress).IsAssignableFrom(For.ModelExplorer.ModelType))
            {
                throw new InvalidOperationException($"The 'for' attribute must be of type '{typeof(TprAddress).FullName}'.");
            }

            var addressLookupContent = GetContent();

            var result = _htmlGenerator.GenerateTprAddressLookup(For, ViewContext.ModelState, addressLookupContent);

            output.TagName = result.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            output.MergeAttributes(result);
            output.Content.SetHtmlContent(result.InnerHtml);
        }

        private TprAddressLookupContent GetContent()
        {
            var content = new TprAddressLookupContent();
            content.AddressLine1Label = AddressLine1Label ?? content.AddressLine1Label;
            content.AddressLine2Label = AddressLine2Label ?? content.AddressLine2Label;
            content.AddressLine3Label = AddressLine3Label ?? content.AddressLine3Label;
            content.PostTownLabel = PostTownLabel ?? content.PostTownLabel;
            content.PostCountyLabel = PostCountyLabel ?? content.PostCountyLabel;
            content.PostcodeLabel = PostcodeLabel ?? content.PostcodeLabel;
            content.CountryLabel = CountryLabel ?? content.CountryLabel;
            return content;
        }
    }
}