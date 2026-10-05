namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprAddressLookupContent
    {
        // manual entry labels and error messages
        public string AddressLookupLegend { get; set; } = "Address lookup default";
        public string AddressLookupHint { get; set; } = "Enter a building name or number and postcode to find your address default";
        public string AddressLine1Label { get; set; } = "Address line 1 default";
        public string AddressLine1RequiredErrorMessage { get; set; } = "Enter address line 1 default";
        public string AddressLine1MaxLengthErrorMessage { get; set; } = "Address line 1 must be 100 characters or less default";
        public string AddressLine2Label { get; set; } = "Address line 2 default";
        public string AddressLine2MaxLengthErrorMessage { get; set; } = "Address line 2 must be 100 characters or less default";
        public string AddressLine3Label { get; set; } = "Address line 3 default";
        public string AddressLine3MaxLengthErrorMessage { get; set; } = "Address line 3 must be 100 characters or less default";
        public string PostTownLabel { get; set; } = "Town or city default";
        public string PostTownRequiredErrorMessage { get; set; } = "Enter town or city default";
        public string PostTownMaxLengthErrorMessage { get; set; } = "Town or city must be 100 characters or less default";
        public string CountyLabel { get; set; } = "County default";
        public string CountyMaxLengthErrorMessage { get; set; } = "County must be 100 characters or less default";
        public string CountyStateProvinceLabel { get; set; } = "County, state or province default";
        public string CountyStateProvinceMaxLengthErrorMessage { get; set; } = "County, state or province must be 100 characters or less default";
        public string PostcodeLabel { get; set; } = "Postcode default";
        public string PostcodeRequiredErrorMessage { get; set; } = "Enter postcode default";
        public string PostcodeMaxLengthErrorMessage { get; set; } = "Postcode must be 100 characters or less default";
        public string PostcodePatternErrorMessage { get; set; } = "Enter a valid postcode default";
        public string CountryLabel { get; set; } = "Country default";
        public string CountrySelectPlaceholder { get; set; } = "Select a country";
        public string CountryRequiredErrorMessage { get; set; } = "Select a country default";
        public string BuildingNameOrNumberLabel { get; set; } = "Building name or number default";

        // UK manual entry labels
        public string UkManualEntryLinkLabel { get; set; } = "Enter address manually default";
        public string UkManualEntryLegend { get; set; } = "Enter your address default";

        // international manual entry labels
        public string InternationalManualEntryLabel { get; set; } = "Enter an international address default";
        public string InternationalManualEntryLegend { get; set; } = "Enter your international address default";

        // search view
        public string FindAddressButton { get; set; } = "Find address default";
        public string AddressNotFoundMessage { get; set; } = "We could not find any addresses for this postcode default";
        public string ServiceUnavailableMessage { get; set; } = "The address lookup service is currently unavailable default";
        public string SearchNotConfirmedErrorMessage { get; set; } = "Find and confirm an address before continuing default";

        // results view
        public string AddressSelectLabel { get; set; } = "Select an address default";
        public string ConfirmAddressLabel { get; set; } = "Confirm address default";
        public string BackToPostcodeSearchLabel { get; set; } = "Back to postcode search default";
        public string AddressSelectErrorMessage { get; set; } = "Select an address before continuing default";
        public string ResultsNotConfirmedErrorMessage { get; set; } = "Select and confirm an address before continuing default";

        // UK manual entry view
        public string UkManualEntryNotConfirmedErrorMessage { get; set; } = "Enter and confirm an address before continuing default";

        // international manual entry view
        public string InternationalManualEntryNotConfirmedErrorMessage { get; set; } = "Enter and confirm an international address before continuing default";

        // confirmed view
        public string ChangeAddressLabel { get; set; } = "Change address default";
    }
}
