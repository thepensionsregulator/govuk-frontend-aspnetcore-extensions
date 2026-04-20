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

        [HttpPost]
        [ModelType(typeof(AddressLookupViewModel))]
        public IActionResult Post(AddressLookupViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Response.StatusCode = 303;
                Response.GetTypedHeaders().Location = new Uri("/panel", UriKind.Relative);
            }

            return View("Index", viewModel);
        }
    }
}