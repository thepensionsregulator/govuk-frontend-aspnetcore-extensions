using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class AddressLookupViewModel
    {
        [Required(ErrorMessage = "Shipping address line 1 is required")]
        [MaxLength(500, ErrorMessage = "Shipping address line 1 must not exceed 500 characters")]
        public string? ShippingAddressLine1 { get; set; }

        [MaxLength(500, ErrorMessage = "Shipping address line 2 must not exceed 500 characters")]
        public string? ShippingAddressLine2 { get; set; }

        [Required(ErrorMessage = "Shipping town or city is required")]
        [MaxLength(500, ErrorMessage = "Shipping town or city must not exceed 500 characters")]
        public string? ShippingTownOrCity { get; set; }

        [MaxLength(500, ErrorMessage = "Shipping county must not exceed 500 characters")]
        public string? ShippingCounty { get; set; }

        [Required(ErrorMessage = "Shipping country is required")]
        public string? ShippingCountry { get; set; }

        [Required(ErrorMessage = "Shipping postcode is required")]
        [MaxLength(20, ErrorMessage = "Shipping postcode must not exceed 20 characters")]
        public string? ShippingPostcode { get; set; }

        [Required(ErrorMessage = "Billing address line 1 is required")]
        [MaxLength(500, ErrorMessage = "Billing address line 1 must not exceed 500 characters")]
        public string? BillingAddressLine1 { get; set; }

        [MaxLength(500, ErrorMessage = "Billing address line 2 must not exceed 500 characters")]
        public string? BillingAddressLine2 { get; set; }

        [Required(ErrorMessage = "Billing town or city is required")]
        [MaxLength(500, ErrorMessage = "Billing town or city must not exceed 500 characters")]
        public string? BillingTownOrCity { get; set; }

        [MaxLength(500, ErrorMessage = "Billing county must not exceed 500 characters")]
        public string? BillingCounty { get; set; }

        [Required(ErrorMessage = "Billing country is required")]
        public string? BillingCountry { get; set; }

        [Required(ErrorMessage = "Billing postcode is required")]
        [MaxLength(20, ErrorMessage = "Billing postcode must not exceed 20 characters")]
        public string? BillingPostcode { get; set; }
    }
}
