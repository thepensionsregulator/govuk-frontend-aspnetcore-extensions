using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.ExampleApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Validation;

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
            var viewModel = new AddressLookupViewModel
            {
                ShippingAddress = new TprAddress
                {
                    AddressLine1 = "15 Maple Street",
                    PostTown = "Edinburgh",
                    CountryId = 1,
                    PostCode = "AB12 3CD",
                    UPRNReference = "10001234"
                }
            };

            ModelState.SetModelValues(viewModel.ShippingAddress, nameof(viewModel.ShippingAddress));

            return View(viewModel);
        }

        [HttpPost]
        [ModelType(typeof(AddressLookupViewModel))]
        public IActionResult PostPrimaryDataProvided(AddressLookupViewModel viewModel)
        {
            return HandlePost(viewModel, nameof(PrimaryDataProvided));
        }

        public IActionResult PrimaryAndSecondaryDataProvided()
        {
            var viewModel = new AddressLookupViewModel
            {
                ShippingAddress = new TprAddress
                {
                    AddressLine1 = "15 Maple Street",
                    PostTown = "Edinburgh",
                    CountryId = 1,
                    PostCode = "AB12 3CD",
                    UPRNReference = "10001234"
                },
                BillingAddress = new TprAddress
                {
                    AddressLine1 = "15 Maple Street",
                    PostTown = "Edinburgh",
                    CountryId = 1,
                    PostCode = "AB12 3CD",
                    UPRNReference = "10001234"
                }
            };

            ModelState.SetModelValues(viewModel.ShippingAddress, nameof(viewModel.ShippingAddress));
            ModelState.SetModelValues(viewModel.BillingAddress, nameof(viewModel.BillingAddress));

            return View(viewModel);
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