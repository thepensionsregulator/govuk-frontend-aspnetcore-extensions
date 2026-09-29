using GovUk.Frontend.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
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
                ShippingAddress = new TprAddress
                {
                    AddressLine1 = "Address line 1 value",
                    AddressLine2 = "Address line 2 value",
                    AddressLine3 = "Address line 3 value",
                    PostTown = "Post town value",
                    PostCounty = "Post county value",
                    Postcode = "BN1 4DW",
                    UPRNReference = "UPRN123"
                }
            };

            ModelState.SetModelValues(viewModel.ShippingAddress, nameof(viewModel.ShippingAddress));

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(AddressLookupViewModel viewModel)
        {
            return View("Index", viewModel);
        }
    }
}
