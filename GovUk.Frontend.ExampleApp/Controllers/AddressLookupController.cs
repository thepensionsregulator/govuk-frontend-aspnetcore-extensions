using GovUk.Frontend.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using ThePensionsRegulator.Frontend.Validation;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class AddressLookupController : BaseController
    {
        public IActionResult Index()
        {
            return View(new AddressLookupViewModel());
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
