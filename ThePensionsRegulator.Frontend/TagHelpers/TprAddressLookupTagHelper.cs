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

        [HtmlAttributeName("address-legend")]
        public string AddressLookupLegend { get; set; }

        [HtmlAttributeName("address-hint")]
        public string AddressLookupHint { get; set; }

        [HtmlAttributeName("search-button-text")]
        public string SearchButtonText { get; set; }

        [HtmlAttributeName("error-message")]
        public string ErrorMessage { get; set; }

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; } = default!;

        // manual entry labels and error messages
        [HtmlAttributeName("address-line-1-label")]
        public string? AddressLine1Label { get; set; }

        [HtmlAttributeName("address-line-1-required-error-message")]
        public string? AddressLine1RequiredErrorMessage { get; set; }

        [HtmlAttributeName("address-line-1-max-length-error-message")]
        public string? AddressLine1MaxLengthErrorMessage { get; set; }

        [HtmlAttributeName("address-line-2-label")]
        public string? AddressLine2Label { get; set; }

        [HtmlAttributeName("address-line-2-max-length-error-message")]
        public string? AddressLine2MaxLengthErrorMessage { get; set; }

        [HtmlAttributeName("address-line-3-label")]
        public string? AddressLine3Label { get; set; }

        [HtmlAttributeName("address-line-3-max-length-error-message")]
        public string? AddressLine3MaxLengthErrorMessage { get; set; }

        [HtmlAttributeName("post-town-label")]
        public string? PostTownLabel { get; set; }

        [HtmlAttributeName("post-town-required-error-message")]
        public string? PostTownRequiredErrorMessage { get; set; }

        [HtmlAttributeName("post-town-max-length-error-message")]
        public string? PostTownMaxLengthErrorMessage { get; set; }

        [HtmlAttributeName("county-label")]
        public string? CountyLabel { get; set; }

        [HtmlAttributeName("county-max-length-error-message")]
        public string? CountyMaxLengthErrorMessage { get; set; }

        [HtmlAttributeName("county-state-province-label")]
        public string? CountyStateProvinceLabel { get; set; }

        [HtmlAttributeName("county-state-province-max-length-error-message")]
        public string? CountyStateProvinceMaxLengthErrorMessage { get; set; }

        [HtmlAttributeName("postcode-label")]
        public string? PostcodeLabel { get; set; }

        [HtmlAttributeName("postcode-required-error-message")]
        public string? PostcodeRequiredErrorMessage { get; set; }

        [HtmlAttributeName("postcode-max-length-error-message")]
        public string? PostcodeMaxLengthErrorMessage { get; set; }

        [HtmlAttributeName("postcode-pattern-error-message")]
        public string? PostcodePatternErrorMessage { get; set; }

        [HtmlAttributeName("country-label")]
        public string? CountryLabel { get; set; }

        [HtmlAttributeName("country-required-error-message")]
        public string? CountryRequiredErrorMessage { get; set; }

        [HtmlAttributeName("building-name-or-number-label")]
        public string? BuildingNameOrNumberLabel { get; set; }

        // UK manual entry labels
        [HtmlAttributeName("uk-manual-entry-link-label")]
        public string? UkManualEntryLinkLabel { get; set; }

        [HtmlAttributeName("uk-manual-entry-legend")]
        public string? UkManualEntryLegend { get; set; }

        // international manual entry labels
        [HtmlAttributeName("international-manual-entry-label")]
        public string? InternationalManualEntryLabel { get; set; }

        [HtmlAttributeName("international-manual-entry-legend")]
        public string? InternationalManualEntryLegend { get; set; }

        // search view
        [HtmlAttributeName("find-address-button")]
        public string? FindAddressButton { get; set; }

        [HtmlAttributeName("address-not-found-message")]
        public string? AddressNotFoundMessage { get; set; }

        [HtmlAttributeName("service-unavailable-message")]
        public string? ServiceUnavailableMessage { get; set; }

        [HtmlAttributeName("search-not-confirmed-error-message")]
        public string? SearchNotConfirmedErrorMessage { get; set; }

        // results view
        [HtmlAttributeName("address-select-label")]
        public string? AddressSelectLabel { get; set; }

        [HtmlAttributeName("confirm-address-label")]
        public string? ConfirmAddressLabel { get; set; }

        [HtmlAttributeName("back-to-postcode-search-label")]
        public string? BackToPostcodeSearchLabel { get; set; }

        [HtmlAttributeName("results-not-confirmed-error-message")]
        public string? ResultsNotConfirmedErrorMessage { get; set; }

        // UK manual entry view
        [HtmlAttributeName("uk-manual-entry-not-confirmed-error-message")]
        public string? UkManualEntryNotConfirmedErrorMessage { get; set; }

        // international manual entry view
        [HtmlAttributeName("international-manual-entry-not-confirmed-error-message")]
        public string? InternationalManualEntryNotConfirmedErrorMessage { get; set; }

        // confirmed view
        [HtmlAttributeName("change-address-label")]
        public string? ChangeAddressLabel { get; set; }

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
            if (!typeof(TprAddress).IsAssignableFrom(For.ModelExplorer.ModelType))
            {
                throw new InvalidOperationException($"The 'for' attribute must be of type '{typeof(TprAddress).FullName}'.");
            }

            var countries = _countryRepository.GetCountries();
            var addressLookupContent = GetContent();
            var addressLookupSettings = GetSettings();

            var result = _htmlGenerator.GenerateTprAddressLookup(For, ViewContext.ModelState, addressLookupContent, addressLookupSettings, await countries);

            output.TagName = result.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            output.MergeAttributes(result);
            output.Content.SetHtmlContent(result.InnerHtml);
        }

        private TprAddressLookupContent GetContent()
        {
            var content = new TprAddressLookupContent();

            if (AddressLookupLegend is not null) content.AddressLookupLegend = AddressLookupLegend;
            if (AddressLookupHint is not null) content.AddressLookupHint = AddressLookupHint;
            if (AddressLine1Label is not null) content.AddressLine1Label = AddressLine1Label;
            if (AddressLine1RequiredErrorMessage is not null) content.AddressLine1RequiredErrorMessage = AddressLine1RequiredErrorMessage;
            if (AddressLine1MaxLengthErrorMessage is not null) content.AddressLine1MaxLengthErrorMessage = AddressLine1MaxLengthErrorMessage;
            if (AddressLine2Label is not null) content.AddressLine2Label = AddressLine2Label;
            if (AddressLine2MaxLengthErrorMessage is not null) content.AddressLine2MaxLengthErrorMessage = AddressLine2MaxLengthErrorMessage;
            if (AddressLine3Label is not null) content.AddressLine3Label = AddressLine3Label;
            if (AddressLine3MaxLengthErrorMessage is not null) content.AddressLine3MaxLengthErrorMessage = AddressLine3MaxLengthErrorMessage;
            if (PostTownLabel is not null) content.PostTownLabel = PostTownLabel;
            if (PostTownRequiredErrorMessage is not null) content.PostTownRequiredErrorMessage = PostTownRequiredErrorMessage;
            if (PostTownMaxLengthErrorMessage is not null) content.PostTownMaxLengthErrorMessage = PostTownMaxLengthErrorMessage;
            if (CountyLabel is not null) content.CountyLabel = CountyLabel;
            if (CountyMaxLengthErrorMessage is not null) content.CountyMaxLengthErrorMessage = CountyMaxLengthErrorMessage;
            if (CountyStateProvinceLabel is not null) content.CountyStateProvinceLabel = CountyStateProvinceLabel;
            if (CountyStateProvinceMaxLengthErrorMessage is not null) content.CountyStateProvinceMaxLengthErrorMessage = CountyStateProvinceMaxLengthErrorMessage;
            if (PostcodeLabel is not null) content.PostcodeLabel = PostcodeLabel;
            if (PostcodeRequiredErrorMessage is not null) content.PostcodeRequiredErrorMessage = PostcodeRequiredErrorMessage;
            if (PostcodeMaxLengthErrorMessage is not null) content.PostcodeMaxLengthErrorMessage = PostcodeMaxLengthErrorMessage;
            if (PostcodePatternErrorMessage is not null) content.PostcodePatternErrorMessage = PostcodePatternErrorMessage;
            if (CountryLabel is not null) content.CountryLabel = CountryLabel;
            if (CountryRequiredErrorMessage is not null) content.CountryRequiredErrorMessage = CountryRequiredErrorMessage;
            if (BuildingNameOrNumberLabel is not null) content.BuildingNameOrNumberLabel = BuildingNameOrNumberLabel;
            if (UkManualEntryLinkLabel is not null) content.UkManualEntryLinkLabel = UkManualEntryLinkLabel;
            if (UkManualEntryLegend is not null) content.UkManualEntryLegend = UkManualEntryLegend;
            if (InternationalManualEntryLabel is not null) content.InternationalManualEntryLabel = InternationalManualEntryLabel;
            if (InternationalManualEntryLegend is not null) content.InternationalManualEntryLegend = InternationalManualEntryLegend;
            if (FindAddressButton is not null) content.FindAddressButton = FindAddressButton;
            if (AddressNotFoundMessage is not null) content.AddressNotFoundMessage = AddressNotFoundMessage;
            if (ServiceUnavailableMessage is not null) content.ServiceUnavailableMessage = ServiceUnavailableMessage;
            if (SearchNotConfirmedErrorMessage is not null) content.SearchNotConfirmedErrorMessage = SearchNotConfirmedErrorMessage;
            if (AddressSelectLabel is not null) content.AddressSelectLabel = AddressSelectLabel;
            if (ConfirmAddressLabel is not null) content.ConfirmAddressLabel = ConfirmAddressLabel;
            if (BackToPostcodeSearchLabel is not null) content.BackToPostcodeSearchLabel = BackToPostcodeSearchLabel;
            if (ResultsNotConfirmedErrorMessage is not null) content.ResultsNotConfirmedErrorMessage = ResultsNotConfirmedErrorMessage;
            if (UkManualEntryNotConfirmedErrorMessage is not null) content.UkManualEntryNotConfirmedErrorMessage = UkManualEntryNotConfirmedErrorMessage;
            if (InternationalManualEntryNotConfirmedErrorMessage is not null) content.InternationalManualEntryNotConfirmedErrorMessage = InternationalManualEntryNotConfirmedErrorMessage;
            if (ChangeAddressLabel is not null) content.ChangeAddressLabel = ChangeAddressLabel;

            return content;
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