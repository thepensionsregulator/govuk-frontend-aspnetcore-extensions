using System.ComponentModel.DataAnnotations;
using ThePensionsRegulator.Frontend.Models;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class AddressLookupNoDataViewModel
    {
        public AddressLookupNoDataViewModel()
        {
            ShippingAddress = new();
            BillingAddress = new();
        }

        public AddressLookupNoData? Page { get; set; }

        public TprAddress ShippingAddress { get; set; }

        public TprAddress BillingAddress { get; set; }

    }
}
