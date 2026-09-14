namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprAddressLookupContent
    {
        public string Legend { get; set; } = "Legend";
        public string? Hint { get; set; }
        public string AddressLine1Label { get; set; } = "Address line 1";
        public string AddressLine2Label { get; set; } = "Address line 2";
        public string AddressLine3Label { get; set; } = "Address line 3";
        public string PostTownLabel { get; set; } = "Town or city";
        public string PostCountyLabel { get; set; } = "County";
        public string PostcodeLabel { get; set; } = "Postcode";
        public string CountryLabel { get; set; } = "Country";
        public string SearchButtonText { get; set; } = "Find address";
    }
}
