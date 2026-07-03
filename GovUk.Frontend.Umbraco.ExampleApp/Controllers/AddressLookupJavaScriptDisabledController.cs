using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using ThePensionsRegulator.Frontend.Umbraco.Models;
using ThePensionsRegulator.Frontend.Umbraco.Validation;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class AddressLookupJavaScriptDisabledController : RenderController
    {
        public AddressLookupJavaScriptDisabledController(ILogger<AddressLookupJavaScriptDisabledController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [ModelType(typeof(AddressLookupJavaScriptDisabledViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new AddressLookupJavaScriptDisabledViewModel
            {
                Page = new AddressLookupJavaScriptDisabled(CurrentPage, null),
                ShippingAddress = new UmbracoTprAddress
                {
                    AddressLine1 = "Shipping Address line 1",
                    AddressLine2 = "Shipping address line 2",
					PostTown = "Shipping town",
                    PostCounty = "Shipping county",
                    CountryId = 1,
                    PostCode = "CM12 0AG"
                }
            };

            ModelState.SetInitialAddressValues(viewModel.ShippingAddress, nameof(viewModel.ShippingAddress));

            return CurrentTemplate(viewModel);
        }
    }
}
