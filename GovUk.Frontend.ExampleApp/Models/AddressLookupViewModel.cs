using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class AddressLookupViewModel
    {
        [Required(ErrorMessage = "Enter address line 1")]
        [MaxLength(500, ErrorMessage = "Address line 1 must not exceed 500 characters")]
        public string? ShippingAddressLine1 { get; set; }

        [MaxLength(500, ErrorMessage = "Address line 2 must not exceed 500 characters")]
        public string? ShippingAddressLine2 { get; set; }

        [Required(ErrorMessage = "Enter a town or city")]
        [MaxLength(500, ErrorMessage = "Town or city must not exceed 500 characters")]
        public string? ShippingTownOrCity { get; set; }

        [MaxLength(500, ErrorMessage = "County must not exceed 500 characters")]
        public string? ShippingCounty { get; set; }

        [Required(ErrorMessage = "Enter a country")]
        public string? ShippingCountry { get; set; }

        [Required(ErrorMessage = "Enter a postcode")]
        [MaxLength(20, ErrorMessage = "Postcode must not exceed 20 characters")]
        public string? ShippingPostcode { get; set; }

        public string? ShippingUPRN { get; set; }

        [Required(ErrorMessage = "Enter address line 1")]
        [MaxLength(500, ErrorMessage = "Address line 1 must not exceed 500 characters")]
        public string? BillingAddressLine1 { get; set; }

        [MaxLength(500, ErrorMessage = "Address line 2 must not exceed 500 characters")]
        public string? BillingAddressLine2 { get; set; }

        [Required(ErrorMessage = "Enter a town or city")]
        [MaxLength(500, ErrorMessage = "Town or city must not exceed 500 characters")]
        public string? BillingTownOrCity { get; set; }

        [MaxLength(500, ErrorMessage = "County must not exceed 500 characters")]
        public string? BillingCounty { get; set; }

        [Required(ErrorMessage = "Enter a country")]
        public string? BillingCountry { get; set; }

        [Required(ErrorMessage = "Enter a postcode")]
        [MaxLength(20, ErrorMessage = "Postcode must not exceed 20 characters")]
        public string? BillingPostcode { get; set; }

        public string? BillingUPRN { get; set; }
    }
}
