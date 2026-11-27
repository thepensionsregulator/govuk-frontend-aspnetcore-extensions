using GovUk.Frontend.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Validation;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class AddressLookupController : BaseController
    {
        public IActionResult Index()
        {
            var viewModel = new AddressLookupViewModel
            {
                BillingAddress = new TprAddress
                {
                    AddressLine1 = "Example line 1",
                    AddressLine2 = "Example line 2",
                    AddressLine3 = "Example line 3",
                    PostTown = "Example town",
                    PostCounty = "Example county",
                    CountryId = 1,
                    Postcode = "AA1 1AA",
                    Uprn = "123456"
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(AddressLookupViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                ModelState.SetModelValues(viewModel.ShippingAddress, nameof(viewModel.ShippingAddress));
            };

            return View("Index", viewModel);
        }
    }
}
