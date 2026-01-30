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
    public class AddressLookupController : RenderController
    {
        public AddressLookupController(ILogger<AddressLookupController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [ModelType(typeof(AddressLookupViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new AddressLookupViewModel
            {
                Page = new AddressLookup(CurrentPage, null),
                ShippingAddressLine1 = "Shipping Address line 1"
            };

            ModelState.SetInitialValue(nameof(viewModel.ShippingAddressLine1), viewModel.ShippingAddressLine1.ToString());

            return CurrentTemplate(viewModel);
        }
    }
}
