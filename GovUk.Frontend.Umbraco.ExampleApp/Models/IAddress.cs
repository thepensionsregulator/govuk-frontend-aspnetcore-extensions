namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public interface IAddress
    {
		public int? AddressReference { get; set; }

		public string AddressLine1 { get; set; }

		public string AddressLine2 { get; set; }

		public string AddressLine3 { get; set; }

		public string AddressLine4 { get; set; }

		public string AddressLine5 { get; set; }

		public string PostCode { get; set; }

		public int? CountryId { get; set; }

		public string CountryCode { get; set; }

		public string Country { get; set; }
	}
}
