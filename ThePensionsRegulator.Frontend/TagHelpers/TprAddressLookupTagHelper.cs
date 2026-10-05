using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprAddressLookupTagHelper : TagHelper
    {
        internal const string TagName = "tpr-address-lookup";

        private readonly ITprHtmlGenerator _htmlGenerator;
        private readonly ITprCountryRepository _countryRepository;

        [HtmlAttributeName("address-search-url")]
        public string SearchUrl { get; set; }

        [HtmlAttributeName("for")]
        public string For { get; set; } = default!;

        [HtmlAttributeName("content")]
        public TprAddressLookupContent? Content { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        internal TprAddressLookupTagHelper(ITprHtmlGenerator? htmlGenerator, ITprCountryRepository countryRepository)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
            _countryRepository = countryRepository;
        }

        public TprAddressLookupTagHelper(ITprCountryRepository countryRepository) : this(null, countryRepository)
        {
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var countries = _countryRepository.GetCountries();
            var addressLookupContent = Content ?? new TprAddressLookupContent();
            var addressLookupSettings = GetSettings();

            var result = _htmlGenerator.GenerateTprAddressLookup(For, ViewContext.ModelState, addressLookupContent, addressLookupSettings, await countries);

            output.TagName = result.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            output.MergeAttributes(result);
            output.Content.SetHtmlContent(result.InnerHtml);
        }

        private TprAddressLookupSettings GetSettings()
        {
            var settings = new TprAddressLookupSettings
            {
                SearchUrl = SearchUrl,
            };

            return settings;
        }
    }
}