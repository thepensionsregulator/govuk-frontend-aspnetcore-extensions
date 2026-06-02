using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Umbraco.Models;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class AddressLookupBaseViewModel
    {
        public AddressLookupBaseViewModel()
        {
            ShippingAddress = new();
            BillingAddress = new();
        }

        public UmbracoTprAddress ShippingAddress { get; set; }

        public UmbracoTprAddress BillingAddress { get; set; }
    }
}
