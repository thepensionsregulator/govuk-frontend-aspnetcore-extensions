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
        internal record Field(string PropertyAlias, string DictionaryKey, Action<TprAddressLookupContent, string> Apply);

        internal static readonly IReadOnlyList<Field> Fields =
        [
            new(TprPropertyAliases.AddressLookupLegend, TprAddressLookupDictionaryKeys.AddressLookupLegend, (c, v) => c.AddressLookupLegend = v),
            new(TprPropertyAliases.AddressLookupHint, TprAddressLookupDictionaryKeys.AddressLookupHint, (c, v) => c.AddressLookupHint = v),
            new(TprPropertyAliases.AddressLookupAddressLine1Label, TprAddressLookupDictionaryKeys.AddressLine1Label, (c, v) => c.AddressLine1Label = v),
            new(TprPropertyAliases.AddressLookupAddressLine1RequiredErrorMessage, TprAddressLookupDictionaryKeys.AddressLine1RequiredErrorMessage, (c, v) => c.AddressLine1RequiredErrorMessage = v),
            new(TprPropertyAliases.AddressLookupAddressLine1MaxLengthErrorMessage, TprAddressLookupDictionaryKeys.AddressLine1MaxLengthErrorMessage, (c, v) => c.AddressLine1MaxLengthErrorMessage = v),
            new(TprPropertyAliases.AddressLookupAddressLine2Label, TprAddressLookupDictionaryKeys.AddressLine2Label, (c, v) => c.AddressLine2Label = v),
            new(TprPropertyAliases.AddressLookupAddressLine2MaxLengthErrorMessage, TprAddressLookupDictionaryKeys.AddressLine2MaxLengthErrorMessage, (c, v) => c.AddressLine2MaxLengthErrorMessage = v),
            new(TprPropertyAliases.AddressLookupAddressLine3Label, TprAddressLookupDictionaryKeys.AddressLine3Label, (c, v) => c.AddressLine3Label = v),
            new(TprPropertyAliases.AddressLookupAddressLine3MaxLengthErrorMessage, TprAddressLookupDictionaryKeys.AddressLine3MaxLengthErrorMessage, (c, v) => c.AddressLine3MaxLengthErrorMessage = v),
            new(TprPropertyAliases.AddressLookupPostTownLabel, TprAddressLookupDictionaryKeys.PostTownLabel, (c, v) => c.PostTownLabel = v),
            new(TprPropertyAliases.AddressLookupPostTownRequiredErrorMessage, TprAddressLookupDictionaryKeys.PostTownRequiredErrorMessage, (c, v) => c.PostTownRequiredErrorMessage = v),
            new(TprPropertyAliases.AddressLookupPostTownMaxLengthErrorMessage, TprAddressLookupDictionaryKeys.PostTownMaxLengthErrorMessage, (c, v) => c.PostTownMaxLengthErrorMessage = v),
            new(TprPropertyAliases.AddressLookupCountyLabel, TprAddressLookupDictionaryKeys.CountyLabel, (c, v) => c.CountyLabel = v),
            new(TprPropertyAliases.AddressLookupCountyMaxLengthErrorMessage, TprAddressLookupDictionaryKeys.CountyMaxLengthErrorMessage, (c, v) => c.CountyMaxLengthErrorMessage = v),
            new(TprPropertyAliases.AddressLookupCountyStateProvinceLabel, TprAddressLookupDictionaryKeys.CountyStateProvinceLabel, (c, v) => c.CountyStateProvinceLabel = v),
            new(TprPropertyAliases.AddressLookupCountyStateProvinceMaxLengthErrorMessage, TprAddressLookupDictionaryKeys.CountyStateProvinceMaxLengthErrorMessage, (c, v) => c.CountyStateProvinceMaxLengthErrorMessage = v),
            new(TprPropertyAliases.AddressLookupPostcodeLabel, TprAddressLookupDictionaryKeys.PostcodeLabel, (c, v) => c.PostcodeLabel = v),
            new(TprPropertyAliases.AddressLookupPostcodeRequiredErrorMessage, TprAddressLookupDictionaryKeys.PostcodeRequiredErrorMessage, (c, v) => c.PostcodeRequiredErrorMessage = v),
            new(TprPropertyAliases.AddressLookupPostcodeMaxLengthErrorMessage, TprAddressLookupDictionaryKeys.PostcodeMaxLengthErrorMessage, (c, v) => c.PostcodeMaxLengthErrorMessage = v),
            new(TprPropertyAliases.AddressLookupPostcodePatternErrorMessage, TprAddressLookupDictionaryKeys.PostcodePatternErrorMessage, (c, v) => c.PostcodePatternErrorMessage = v),
            new(TprPropertyAliases.AddressLookupCountryLabel, TprAddressLookupDictionaryKeys.CountryLabel, (c, v) => c.CountryLabel = v),
            new(TprPropertyAliases.AddressLookupCountrySelectPlaceholder, TprAddressLookupDictionaryKeys.CountrySelectPlaceholder, (c, v) => c.CountrySelectPlaceholder = v),
            new(TprPropertyAliases.AddressLookupCountryRequiredErrorMessage, TprAddressLookupDictionaryKeys.CountryRequiredErrorMessage, (c, v) => c.CountryRequiredErrorMessage = v),
            new(TprPropertyAliases.AddressLookupBuildingNameOrNumberLabel, TprAddressLookupDictionaryKeys.BuildingNameOrNumberLabel, (c, v) => c.BuildingNameOrNumberLabel = v),
            new(TprPropertyAliases.AddressLookupUkManualEntryLinkLabel, TprAddressLookupDictionaryKeys.UkManualEntryLinkLabel, (c, v) => c.UkManualEntryLinkLabel = v),
            new(TprPropertyAliases.AddressLookupUkManualEntryLegend, TprAddressLookupDictionaryKeys.UkManualEntryLegend, (c, v) => c.UkManualEntryLegend = v),
            new(TprPropertyAliases.AddressLookupInternationalManualEntryLabel, TprAddressLookupDictionaryKeys.InternationalManualEntryLabel, (c, v) => c.InternationalManualEntryLabel = v),
            new(TprPropertyAliases.AddressLookupInternationalManualEntryLegend, TprAddressLookupDictionaryKeys.InternationalManualEntryLegend, (c, v) => c.InternationalManualEntryLegend = v),
            new(TprPropertyAliases.AddressLookupUkManualEntryNotConfirmedErrorMessage, TprAddressLookupDictionaryKeys.UkManualEntryNotConfirmedErrorMessage, (c, v) => c.UkManualEntryNotConfirmedErrorMessage = v),
            new(TprPropertyAliases.AddressLookupInternationalManualEntryNotConfirmedErrorMessage, TprAddressLookupDictionaryKeys.InternationalManualEntryNotConfirmedErrorMessage, (c, v) => c.InternationalManualEntryNotConfirmedErrorMessage = v),
            new(TprPropertyAliases.AddressLookupFindAddressButton, TprAddressLookupDictionaryKeys.FindAddressButton, (c, v) => c.FindAddressButton = v),
            new(TprPropertyAliases.AddressLookupAddressNotFoundMessage, TprAddressLookupDictionaryKeys.AddressNotFoundMessage, (c, v) => c.AddressNotFoundMessage = v),
            new(TprPropertyAliases.AddressLookupServiceUnavailableMessage, TprAddressLookupDictionaryKeys.ServiceUnavailableMessage, (c, v) => c.ServiceUnavailableMessage = v),
            new(TprPropertyAliases.AddressLookupSearchNotConfirmedErrorMessage, TprAddressLookupDictionaryKeys.SearchNotConfirmedErrorMessage, (c, v) => c.SearchNotConfirmedErrorMessage = v),
            new(TprPropertyAliases.AddressLookupAddressSelectLabel, TprAddressLookupDictionaryKeys.AddressSelectLabel, (c, v) => c.AddressSelectLabel = v),
            new(TprPropertyAliases.AddressLookupAddressSelectErrorMessage, TprAddressLookupDictionaryKeys.AddressSelectErrorMessage, (c, v) => c.AddressSelectErrorMessage = v),
            new(TprPropertyAliases.AddressLookupConfirmAddressLabel, TprAddressLookupDictionaryKeys.ConfirmAddressLabel, (c, v) => c.ConfirmAddressLabel = v),
            new(TprPropertyAliases.AddressLookupBackToPostcodeSearchLabel, TprAddressLookupDictionaryKeys.BackToPostcodeSearchLabel, (c, v) => c.BackToPostcodeSearchLabel = v),
            new(TprPropertyAliases.AddressLookupResultsNotConfirmedErrorMessage, TprAddressLookupDictionaryKeys.ResultsNotConfirmedErrorMessage, (c, v) => c.ResultsNotConfirmedErrorMessage = v),
            new(TprPropertyAliases.AddressLookupChangeAddressLabel, TprAddressLookupDictionaryKeys.ChangeAddressLabel, (c, v) => c.ChangeAddressLabel = v),
        ];

        public static TprAddressLookupContent Resolve(IOverridablePublishedElement? blockContent, ICultureDictionary dictionary)
        {
            ArgumentNullException.ThrowIfNull(dictionary);

            var content = new TprAddressLookupContent();

            foreach (var field in Fields)
            {
                var value = blockContent?.Value<string>(field.PropertyAlias);
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
