using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class AddressLookupNoDataViewModel
    {
        public AddressLookupNoData? Page { get; set; }

        [Required(ErrorMessage = nameof(ShippingAddressLine1))]
        [MaxLength(500, ErrorMessage = nameof(ShippingAddressLine1))]
        public string? ShippingAddressLine1 { get; set; }

        [MaxLength(500, ErrorMessage = nameof(ShippingAddressLine2))]
        public string? ShippingAddressLine2 { get; set; }

        [Required(ErrorMessage = nameof(ShippingSomethingReallyRandom))]
        [MaxLength(500, ErrorMessage = nameof(ShippingSomethingReallyRandom))]
        public string? ShippingSomethingReallyRandom { get; set; }

        [MaxLength(500, ErrorMessage = nameof(ShippingCounty))]
        public string? ShippingCounty { get; set; }

        [Required(ErrorMessage = nameof(ShippingCountry))]
        public string? ShippingCountry { get; set; }

        [Required(ErrorMessage = nameof(ShippingPostcode))]
        [MaxLength(20, ErrorMessage = nameof(ShippingPostcode))]
        public string? ShippingPostcode { get; set; }

        public string? ShippingUPRN { get; set; }

        [Required(ErrorMessage = nameof(BillingAddressLine1))]
        [MaxLength(500, ErrorMessage = nameof(BillingAddressLine1))]
        public string? BillingAddressLine1 { get; set; }

        [MaxLength(500, ErrorMessage = nameof(BillingAddressLine2))]
        public string? BillingAddressLine2 { get; set; }

        [Required(ErrorMessage = nameof(BillingSomethingReallyRandom))]
        [MaxLength(500, ErrorMessage = nameof(BillingSomethingReallyRandom))]
        public string? BillingSomethingReallyRandom { get; set; }

        [MaxLength(500, ErrorMessage = nameof(BillingCounty))]
        public string? BillingCounty { get; set; }

        [Required(ErrorMessage = nameof(BillingCountry))]
        public string? BillingCountry { get; set; }

        [Required(ErrorMessage = nameof(BillingPostcode))]
        [MaxLength(20, ErrorMessage = nameof(BillingPostcode))]
        public string? BillingPostcode { get; set; }

        public string? BillingUPRN { get; set; }
    }
}
