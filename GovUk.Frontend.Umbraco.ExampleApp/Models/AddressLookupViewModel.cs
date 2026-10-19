using ThePensionsRegulator.Frontend.Umbraco.Models;
using Umbraco.Cms.Web.Common.PublishedModels;
namespace GovUk.Frontend.Umbraco.ExampleApp.Models;

public class AddressLookupViewModel
{
    public AddressLookup Page { get; set; }
    public UmbracoTprAddress ShippingAddress { get; set; }
    public UmbracoTprAddress BillingAddress { get; set; }
}
