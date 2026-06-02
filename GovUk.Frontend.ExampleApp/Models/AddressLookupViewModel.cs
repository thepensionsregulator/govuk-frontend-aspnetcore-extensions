using System.ComponentModel.DataAnnotations;
using ThePensionsRegulator.Frontend.Models;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class AddressLookupViewModel
    {
        [Required(ErrorMessage = "Enter a scheme name")]
        [MaxLength(200, ErrorMessage = "Scheme name must not exceed 200 characters")]
        public string? SchemeName { get; set; }

        public TprAddress? ShippingAddress { get; set; }

        public TprAddress? BillingAddress { get; set; }
    }
}
