using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Umbraco.Models;
using ThePensionsRegulator.Frontend.Umbraco.Validation;
using ThePensionsRegulator.Frontend.Validation;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class AddressLookupPrimaryAndSecondaryDataProvidedController : RenderController
    {
        public AddressLookupPrimaryAndSecondaryDataProvidedController(ILogger<AddressLookupPrimaryAndSecondaryDataProvidedController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [ModelType(typeof(AddressLookupPrimaryAndSecondaryDataProvidedViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new AddressLookupPrimaryAndSecondaryDataProvidedViewModel
            {
                Page = new AddressLookupPrimaryAndSecondaryDataProvided(CurrentPage, null),
                ShippingAddress = new UmbracoTprAddress
                {
                    AddressLine1 = "Telecom House",
                    AddressLine2 = "125-135 Preston Road",
                    AddressLine3 = "Brighton",
                    PostTown = "Brighton",
                    PostCode = "BN1 6AF",
                    UPRNReference = "22275623",
                    CountryId = 1,
                },
                BillingAddress = new UmbracoTprAddress
                {
                    AddressLine1 = "Telecom House",
                    AddressLine2 = "125-135 Preston Road",
                    AddressLine3 = "Brighton",
                    PostTown = "Brighton",
                    PostCode = "BN1 6AF",
                    UPRNReference = "22275623",
                    CountryId = 1
                }
            };

            ModelState.SetInitialAddressValues(viewModel.ShippingAddress, nameof(viewModel.ShippingAddress));
            ModelState.SetInitialAddressValues(viewModel.BillingAddress, nameof(viewModel.BillingAddress));

            return CurrentTemplate(viewModel);
        }
    }
}
