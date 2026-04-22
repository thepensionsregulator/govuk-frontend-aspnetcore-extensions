using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
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
                ShippingAddressLine1 = "Telecom House",
                ShippingAddressLine2 = "125-135 Preston Road",
                ShippingSomethingReallyRandom = "Brighton",
                ShippingPostcode = "BN1 6AF",
                ShippingUPRN = "22275623",
                BillingAddressLine1 = "Telecom House",
                BillingAddressLine2 = "125-135 Preston Road",
                BillingSomethingReallyRandom = "Brighton",
                BillingPostcode = "BN1 6AF",
                BillingUPRN = "22275623"
            };

            ModelState.SetInitialValue(nameof(viewModel.ShippingAddressLine1), viewModel.ShippingAddressLine1.ToString());
            ModelState.SetInitialValue(nameof(viewModel.ShippingAddressLine2), viewModel.ShippingAddressLine2.ToString());
            ModelState.SetInitialValue(nameof(viewModel.ShippingSomethingReallyRandom), viewModel.ShippingSomethingReallyRandom.ToString());
            ModelState.SetInitialValue(nameof(viewModel.ShippingPostcode), viewModel.ShippingPostcode.ToString());
            ModelState.SetInitialValue(nameof(viewModel.ShippingUPRN), viewModel.ShippingUPRN.ToString());
            ModelState.SetInitialValue(nameof(viewModel.BillingAddressLine1), viewModel.BillingAddressLine1.ToString());
            ModelState.SetInitialValue(nameof(viewModel.BillingAddressLine2), viewModel.BillingAddressLine2.ToString());
            ModelState.SetInitialValue(nameof(viewModel.BillingSomethingReallyRandom), viewModel.BillingSomethingReallyRandom.ToString());
            ModelState.SetInitialValue(nameof(viewModel.BillingPostcode), viewModel.BillingPostcode.ToString());
            ModelState.SetInitialValue(nameof(viewModel.BillingUPRN), viewModel.BillingUPRN.ToString());

            return CurrentTemplate(viewModel);
        }
    }
}
