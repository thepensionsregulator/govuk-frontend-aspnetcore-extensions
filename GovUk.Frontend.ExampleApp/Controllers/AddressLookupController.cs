using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.ExampleApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class AddressLookupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NoDataProvided()
        {
            return View(new AddressLookupViewModel());
        }

        [HttpPost]
        [ModelType(typeof(AddressLookupViewModel))]
        public IActionResult PostNoDataProvided(AddressLookupViewModel viewModel)
        {
            return HandlePost(viewModel, nameof(NoDataProvided));
        }

        public IActionResult PrimaryDataProvided()
        {
            return View(new AddressLookupViewModel
            {
                ShippingAddressLine1 = "15 Maple Street",
                ShippingTownOrCity = "Edinburgh",
                ShippingCountry = "Scotland",
                ShippingPostcode = "AB12 3CD",
                ShippingUPRN = "10001234"
            });
        }

        [HttpPost]
        [ModelType(typeof(AddressLookupViewModel))]
        public IActionResult PostPrimaryDataProvided(AddressLookupViewModel viewModel)
        {
            return HandlePost(viewModel, nameof(PrimaryDataProvided));
        }

        public IActionResult PrimaryAndSecondaryDataProvided()
        {
            return View(new AddressLookupViewModel
            {
                ShippingAddressLine1 = "15 Maple Street",
                ShippingTownOrCity = "Edinburgh",
                ShippingCountry = "Scotland",
                ShippingPostcode = "AB12 3CD",
                ShippingUPRN = "10001234",
                BillingAddressLine1 = "15 Maple Street",
                BillingTownOrCity = "Edinburgh",
                BillingCountry = "Scotland",
                BillingPostcode = "AB12 3CD",
                BillingUPRN = "10001234"
            });
        }

        [HttpPost]
        [ModelType(typeof(AddressLookupViewModel))]
        public IActionResult PostPrimaryAndSecondaryDataProvided(AddressLookupViewModel viewModel)
        {
            return HandlePost(viewModel, nameof(PrimaryAndSecondaryDataProvided));
        }

        public IActionResult JavaScriptDisabled()
        {
            return View(new AddressLookupViewModel());
        }

        [HttpPost]
        [ModelType(typeof(AddressLookupViewModel))]
        public IActionResult PostJavaScriptDisabled(AddressLookupViewModel viewModel)
        {
            return HandlePost(viewModel, nameof(JavaScriptDisabled));
        }

        private IActionResult HandlePost(AddressLookupViewModel viewModel, string viewName)
        {
            if (ModelState.IsValid)
            {
                Response.StatusCode = 303;
                Response.GetTypedHeaders().Location = new Uri("/panel", UriKind.Relative);
            }

            return View(viewName, viewModel);
        }
    }
}