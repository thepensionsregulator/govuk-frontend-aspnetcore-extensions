namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprAddressLookupContent
    {
        // manual entry labels and error messages
        public string AddressLookupLegend { get; set; } = "Address lookup";
        public string AddressLookupHint { get; set; } = "Enter a building name or number and postcode to find your address";
        public string AddressLine1Label { get; set; } = "Address line 1";
        public string AddressLine1RequiredErrorMessage { get; set; } = "Enter address line 1";
        public string AddressLine1MaxLengthErrorMessage { get; set; } = "Address line 1 must be 100 characters or less";
        public string AddressLine2Label { get; set; } = "Address line 2";
        public string AddressLine2MaxLengthErrorMessage { get; set; } = "Address line 2 must be 100 characters or less";
        public string AddressLine3Label { get; set; } = "Address line 3";
        public string AddressLine3MaxLengthErrorMessage { get; set; } = "Address line 3 must be 100 characters or less";
        public string PostTownLabel { get; set; } = "Town or city";
        public string PostTownRequiredErrorMessage { get; set; } = "Enter town or city";
        public string PostTownMaxLengthErrorMessage { get; set; } = "Town or city must be 100 characters or less";
        public string CountyLabel { get; set; } = "County";
        public string CountyMaxLengthErrorMessage { get; set; } = "County must be 100 characters or less";
        public string CountyStateProvinceLabel { get; set; } = "County, state or province";
        public string CountyStateProvinceMaxLengthErrorMessage { get; set; } = "County, state or province must be 100 characters or less";
        public string PostcodeLabel { get; set; } = "Postcode";
        public string PostcodeRequiredErrorMessage { get; set; } = "Enter postcode";
        public string PostcodeMaxLengthErrorMessage { get; set; } = "Postcode must be 100 characters or less";
        public string PostcodePatternErrorMessage { get; set; } = "Enter a valid postcode";
        public string CountryLabel { get; set; } = "Country";
        public string CountryRequiredErrorMessage { get; set; } = "Select a country";
        public string BuildingNameOrNumberLabel { get; set; } = "Building name or number";

        // UK manual entry labels
        public string UkManualEntryLinkLabel { get; set; } = "Enter address manually";
        public string UkManualEntryLegend { get; set; } = "Enter your address";

        // international manual entry labels
        public string InternationalManualEntryLabel { get; set; } = "Enter an international address";
        public string InternationalManualEntryLegend { get; set; } = "Enter your international address";

        // search view
        public string FindAddressButton { get; set; } = "Find address";
        public string AddressNotFoundMessage { get; set; } = "We could not find any addresses for this postcode";
        public string ServiceUnavailableMessage { get; set; } = "The address lookup service is currently unavailable";
        public string SearchNotConfirmedErrorMessage { get; set; } = "Find and confirm an address before continuing";

        // results view
        public string AddressSelectLabel { get; set; } = "Select an address";
        public string ConfirmAddressLabel { get; set; } = "Confirm address";
        public string BackToPostcodeSearchLabel { get; set; } = "Back to postcode search";
        public string ResultsNotConfirmedErrorMessage { get; set; } = "Select and confirm an address before continuing";

        // UK manual entry view
        public string UkManualEntryNotConfirmedErrorMessage { get; set; } = "Enter and confirm an address before continuing";

        // international manual entry view
        public string InternationalManualEntryNotConfirmedErrorMessage { get; set; } = "Enter and confirm an international address before continuing";

        // confirmed view
        public string ChangeAddressLabel { get; set; } = "Change address";
    }
}
