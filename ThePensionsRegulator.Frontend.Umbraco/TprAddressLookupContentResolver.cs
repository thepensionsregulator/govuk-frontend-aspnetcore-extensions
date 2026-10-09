using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Umbraco.Core;
using Umbraco.Cms.Core.Dictionary;

namespace ThePensionsRegulator.Frontend.Umbraco
{
    /// <summary>
    /// Resolves address lookup text from the block, then the Umbraco dictionary, then the library default.
    /// </summary>
    public static class TprAddressLookupContentResolver
    {
        internal record Field(string PropertyAlias, string DictionaryKey, Action<TprAddressLookupContent, string> Apply, bool FromSettings = false);

        internal static readonly IReadOnlyList<Field> Fields =
        [
            new(TprPropertyAliases.AddressLookupLegend, TprAddressLookupDictionaryKeys.AddressLookupLegend, (c, v) => c.AddressLookupIntroductionHeading = v),
            new(TprPropertyAliases.AddressLookupHint, TprAddressLookupDictionaryKeys.AddressLookupHint, (c, v) => c.AddressLookupIntroductionText = v),
            new(TprPropertyAliases.AddressLookupNoJsLegend, TprAddressLookupDictionaryKeys.AddressLookupNoJsLegend, (c, v) => c.NoJsLegend = v),
            new(TprPropertyAliases.AddressLookupAddressLine1Label, TprAddressLookupDictionaryKeys.AddressLine1Label, (c, v) => c.AddressLine1Label = v),
            new(TprPropertyAliases.AddressLookupAddressLine1RequiredErrorMessage, TprAddressLookupDictionaryKeys.AddressLine1RequiredErrorMessage, (c, v) => c.AddressLine1RequiredErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupAddressLine1MaxLengthErrorMessage, TprAddressLookupDictionaryKeys.AddressLine1MaxLengthErrorMessage, (c, v) => c.AddressLine1MaxLengthErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupAddressLine2Label, TprAddressLookupDictionaryKeys.AddressLine2Label, (c, v) => c.AddressLine2Label = v),
            new(TprPropertyAliases.AddressLookupAddressLine2MaxLengthErrorMessage, TprAddressLookupDictionaryKeys.AddressLine2MaxLengthErrorMessage, (c, v) => c.AddressLine2MaxLengthErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupAddressLine3Label, TprAddressLookupDictionaryKeys.AddressLine3Label, (c, v) => c.AddressLine3Label = v),
            new(TprPropertyAliases.AddressLookupAddressLine3MaxLengthErrorMessage, TprAddressLookupDictionaryKeys.AddressLine3MaxLengthErrorMessage, (c, v) => c.AddressLine3MaxLengthErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupPostTownLabel, TprAddressLookupDictionaryKeys.PostTownLabel, (c, v) => c.PostTownLabel = v),
            new(TprPropertyAliases.AddressLookupPostTownRequiredErrorMessage, TprAddressLookupDictionaryKeys.PostTownRequiredErrorMessage, (c, v) => c.PostTownRequiredErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupPostTownMaxLengthErrorMessage, TprAddressLookupDictionaryKeys.PostTownMaxLengthErrorMessage, (c, v) => c.PostTownMaxLengthErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupCountyLabel, TprAddressLookupDictionaryKeys.CountyLabel, (c, v) => c.CountyLabel = v),
            new(TprPropertyAliases.AddressLookupCountyMaxLengthErrorMessage, TprAddressLookupDictionaryKeys.CountyMaxLengthErrorMessage, (c, v) => c.CountyMaxLengthErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupCountyStateProvinceLabel, TprAddressLookupDictionaryKeys.CountyStateProvinceLabel, (c, v) => c.CountyStateProvinceLabel = v),
            new(TprPropertyAliases.AddressLookupCountyStateProvinceMaxLengthErrorMessage, TprAddressLookupDictionaryKeys.CountyStateProvinceMaxLengthErrorMessage, (c, v) => c.CountyStateProvinceMaxLengthErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupPostcodeLabel, TprAddressLookupDictionaryKeys.PostcodeLabel, (c, v) => c.PostcodeLabel = v),
            new(TprPropertyAliases.AddressLookupPostcodeRequiredErrorMessage, TprAddressLookupDictionaryKeys.PostcodeRequiredErrorMessage, (c, v) => c.PostcodeRequiredErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupPostcodeMaxLengthErrorMessage, TprAddressLookupDictionaryKeys.PostcodeMaxLengthErrorMessage, (c, v) => c.PostcodeMaxLengthErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupPostcodePatternErrorMessage, TprAddressLookupDictionaryKeys.PostcodePatternErrorMessage, (c, v) => c.PostcodePatternErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupCountryLabel, TprAddressLookupDictionaryKeys.CountryLabel, (c, v) => c.CountryLabel = v),
            new(TprPropertyAliases.AddressLookupCountrySelectPlaceholder, TprAddressLookupDictionaryKeys.CountrySelectPlaceholder, (c, v) => c.CountrySelectPlaceholder = v),
            new(TprPropertyAliases.AddressLookupCountryRequiredErrorMessage, TprAddressLookupDictionaryKeys.CountryRequiredErrorMessage, (c, v) => c.CountryRequiredErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupBuildingNameOrNumberLabel, TprAddressLookupDictionaryKeys.BuildingNameOrNumberLabel, (c, v) => c.BuildingNameOrNumberLabel = v),
            new(TprPropertyAliases.AddressLookupUkManualEntryLinkLabel, TprAddressLookupDictionaryKeys.UkManualEntryLinkLabel, (c, v) => c.UkManualEntryLinkLabel = v),
            new(TprPropertyAliases.AddressLookupUkManualEntryLegend, TprAddressLookupDictionaryKeys.UkManualEntryLegend, (c, v) => c.UkManualEntryLegend = v),
            new(TprPropertyAliases.AddressLookupInternationalManualEntryLabel, TprAddressLookupDictionaryKeys.InternationalManualEntryLabel, (c, v) => c.InternationalManualEntryLabel = v),
            new(TprPropertyAliases.AddressLookupInternationalManualEntryLegend, TprAddressLookupDictionaryKeys.InternationalManualEntryLegend, (c, v) => c.InternationalManualEntryLegend = v),
            new(TprPropertyAliases.AddressLookupSearchLegend, TprAddressLookupDictionaryKeys.AddressLookupSearchLegend, (c, v) => c.SearchLegend = v),
            new(TprPropertyAliases.AddressLookupUkManualEntryNotConfirmedErrorMessage, TprAddressLookupDictionaryKeys.UkManualEntryNotConfirmedErrorMessage, (c, v) => c.UkManualEntryNotConfirmedErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupInternationalManualEntryNotConfirmedErrorMessage, TprAddressLookupDictionaryKeys.InternationalManualEntryNotConfirmedErrorMessage, (c, v) => c.InternationalManualEntryNotConfirmedErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupFindAddressButton, TprAddressLookupDictionaryKeys.FindAddressButton, (c, v) => c.FindAddressButton = v),
            new(TprPropertyAliases.AddressLookupAddressNotFoundMessage, TprAddressLookupDictionaryKeys.AddressNotFoundMessage, (c, v) => c.AddressNotFoundMessage = v, true),
            new(TprPropertyAliases.AddressLookupServiceUnavailableMessage, TprAddressLookupDictionaryKeys.ServiceUnavailableMessage, (c, v) => c.ServiceUnavailableMessage = v, true),
            new(TprPropertyAliases.AddressLookupSearchNotConfirmedErrorMessage, TprAddressLookupDictionaryKeys.SearchNotConfirmedErrorMessage, (c, v) => c.SearchNotConfirmedErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupAddressSelectLabel, TprAddressLookupDictionaryKeys.AddressSelectLabel, (c, v) => c.AddressSelectLabel = v),
            new(TprPropertyAliases.AddressLookupAddressSelectErrorMessage, TprAddressLookupDictionaryKeys.AddressSelectErrorMessage, (c, v) => c.AddressSelectErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupConfirmAddressLabel, TprAddressLookupDictionaryKeys.ConfirmAddressLabel, (c, v) => c.ConfirmAddressLabel = v),
            new(TprPropertyAliases.AddressLookupBackToPostcodeSearchLabel, TprAddressLookupDictionaryKeys.BackToPostcodeSearchLabel, (c, v) => c.BackToPostcodeSearchLabel = v),
            new(TprPropertyAliases.AddressLookupResultsNotConfirmedErrorMessage, TprAddressLookupDictionaryKeys.ResultsNotConfirmedErrorMessage, (c, v) => c.ResultsNotConfirmedErrorMessage = v, true),
            new(TprPropertyAliases.AddressLookupChangeAddressLabel, TprAddressLookupDictionaryKeys.ChangeAddressLabel, (c, v) => c.ChangeAddressLabel = v),
        ];

        public static TprAddressLookupContent Resolve(IOverridablePublishedElement? blockContent, IOverridablePublishedElement? blockSettings, ICultureDictionary dictionary)
        {
            ArgumentNullException.ThrowIfNull(dictionary);

            var content = new TprAddressLookupContent();

            foreach (var field in Fields)
            {
                var source = field.FromSettings ? blockSettings : blockContent;
                var value = source?.Value<string>(field.PropertyAlias);
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = dictionary[field.DictionaryKey];
                }

                if (!string.IsNullOrWhiteSpace(value))
                {
                    field.Apply(content, value);
                }
            }

            return content;
        }
    }
}
