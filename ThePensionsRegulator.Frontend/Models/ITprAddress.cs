namespace ThePensionsRegulator.Frontend.Models
{
    public interface ITprAddress
    {
        public string? UPRNReference { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? PostTown { get; set; }
        public string? PostCounty { get; set; }
        public string? PostCode { get; set; }
        public int? CountryId { get; set; }
        void PopulateAddress(ITprAddress address);
    }
}
