using System.Text.Encodings.Web;
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

        private int _headingLevel = 2;
        /// <summary>
        /// The heading level.
        /// </summary>
        /// <remarks>
        /// Must be between <c>1</c> and <c>6</c> (inclusive). The default is <c>2</c>.
        /// </remarks>
        [HtmlAttributeName("heading-level")]
        public int HeadingLevel
        {
            get => _headingLevel;
            set
            {
                if (value < ComponentGenerator.AddressLookupMinHeadingLevel ||
                    value > ComponentGenerator.AddressLookupMaxHeadingLevel)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingLevel)} must be between {ComponentGenerator.AddressLookupMinHeadingLevel} and {ComponentGenerator.AddressLookupMaxHeadingLevel}.");
                }

                _headingLevel = value;
            }
        }

        /// <summary>
        /// The govuk heading class to apply. Default is <c>govuk-heading-l</c>
        /// </summary>
        /// <remarks>
        /// </remarks>
        private string _headingClass = "govuk-heading-l";
        [HtmlAttributeName("govuk-heading-class")]
        public string HeadingClass
        {
            get => _headingClass;
            set
            {
                if (!ComponentGenerator.AllHeadingClasses.Contains(value))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingClass)} must be one of govuk-heading-xl, govuk-heading-l, govuk-heading-m or govuk-heading-s");
                }

                _headingClass = value;
            }
        }

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

            var introduction = _htmlGenerator.GenerateTprAddressLookupIntroduction(HeadingLevel, HeadingClass, addressLookupContent);
            var fieldset = _htmlGenerator.GenerateTprAddressLookup(For, ViewContext.ModelState, addressLookupContent, await countries);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            PopulateDataAttribute(output.Attributes, addressLookupContent, addressLookupSettings);
            output.AddClass("tpr-address-lookup", HtmlEncoder.Default);
            output.Content.AppendHtml(introduction);
            output.Content.AppendHtml(fieldset);
        }

        private TprAddressLookupSettings GetSettings()

        {
            var settings = new TprAddressLookupSettings
            {
                SearchUrl = SearchUrl,
            };

            return settings;
        }

        private void PopulateDataAttribute(TagHelperAttributeList attributes, TprAddressLookupContent content, TprAddressLookupSettings settings)
        {
            //TODO: Add role attribute
            attributes.Add("data-address-lookup-search-url", settings.SearchUrl);
            attributes.Add("data-address-lookup-legend", content.AddressLookupIntroductionHeading);
            attributes.Add("data-address-lookup-hint", content.AddressLookupIntroductionText);
            attributes.Add("data-address-lookup-no-js-legend", content.NoJsLegend);
            attributes.Add("data-address-lookup-address-line-1-label", content.AddressLine1Label);
            attributes.Add("data-address-lookup-address-line-1-required-error-message", content.AddressLine1RequiredErrorMessage);
            attributes.Add("data-address-lookup-address-line-1-max-length-error-message", content.AddressLine1MaxLengthErrorMessage);
            attributes.Add("data-address-lookup-address-line-2-label", content.AddressLine2Label);
            attributes.Add("data-address-lookup-address-line-2-max-length-error-message", content.AddressLine2MaxLengthErrorMessage);
            attributes.Add("data-address-lookup-address-line-3-label", content.AddressLine3Label);
            attributes.Add("data-address-lookup-address-line-3-max-length-error-message", content.AddressLine3MaxLengthErrorMessage);
            attributes.Add("data-address-lookup-post-town-label", content.PostTownLabel);
            attributes.Add("data-address-lookup-post-town-required-error-message", content.PostTownRequiredErrorMessage);
            attributes.Add("data-address-lookup-post-town-max-length-error-message", content.PostTownMaxLengthErrorMessage);
            attributes.Add("data-address-lookup-county-label", content.CountyLabel);
            attributes.Add("data-address-lookup-county-max-length-error-message", content.CountyMaxLengthErrorMessage);
            attributes.Add("data-address-lookup-county-state-province-label", content.CountyStateProvinceLabel);
            attributes.Add("data-address-lookup-county-state-province-max-length-error-message", content.CountyStateProvinceMaxLengthErrorMessage);
            attributes.Add("data-address-lookup-postcode-label", content.PostcodeLabel);
            attributes.Add("data-address-lookup-postcode-required-error-message", content.PostcodeRequiredErrorMessage);
            attributes.Add("data-address-lookup-postcode-max-length-error-message", content.PostcodeMaxLengthErrorMessage);
            attributes.Add("data-address-lookup-postcode-pattern-error-message", content.PostcodePatternErrorMessage);
            attributes.Add("data-address-lookup-country-label", content.CountryLabel);
            attributes.Add("data-address-lookup-country-select-placeholder", content.CountrySelectPlaceholder);
            attributes.Add("data-address-lookup-country-required-error-message", content.CountryRequiredErrorMessage);
            attributes.Add("data-address-lookup-building-name-or-number-label", content.BuildingNameOrNumberLabel);

            // UK manual entry labels
            attributes.Add("data-address-lookup-uk-manual-entry-link-label", content.UkManualEntryLinkLabel);
            attributes.Add("data-address-lookup-uk-manual-entry-legend", content.UkManualEntryLegend);
            attributes.Add("data-address-lookup-uk-manual-entry-not-confirmed-error-message", content.UkManualEntryNotConfirmedErrorMessage);

            // international manual entry labels
            attributes.Add("data-address-lookup-international-manual-entry-label", content.InternationalManualEntryLabel);
            attributes.Add("data-address-lookup-international-manual-entry-legend", content.InternationalManualEntryLegend);
            attributes.Add("data-address-lookup-international-manual-entry-not-confirmed-error-message", content.InternationalManualEntryNotConfirmedErrorMessage);

            // search view
            attributes.Add("data-address-lookup-search-legend", content.SearchLegend);
            attributes.Add("data-address-lookup-find-address-button", content.FindAddressButton);
            attributes.Add("data-address-lookup-address-not-found-message", content.AddressNotFoundMessage);
            attributes.Add("data-address-lookup-service-unavailable-message", content.ServiceUnavailableMessage);
            attributes.Add("data-address-lookup-search-not-confirmed-error-message", content.SearchNotConfirmedErrorMessage);

            // results view
            attributes.Add("data-address-lookup-address-select-label", content.AddressSelectLabel);
            attributes.Add("data-address-lookup-address-select-error-message", content.AddressSelectErrorMessage);
            attributes.Add("data-address-lookup-confirm-address-label", content.ConfirmAddressLabel);
            attributes.Add("data-address-lookup-back-to-postcode-search-label", content.BackToPostcodeSearchLabel);
            attributes.Add("data-address-lookup-results-not-confirmed-error-message", content.ResultsNotConfirmedErrorMessage);

            // confirmed view
            attributes.Add("data-address-lookup-change-address-label", content.ChangeAddressLabel);
        }
    }
}