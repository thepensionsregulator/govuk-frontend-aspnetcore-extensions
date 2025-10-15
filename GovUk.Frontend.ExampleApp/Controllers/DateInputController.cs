using GovUk.Frontend.ExampleApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class DateInputController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Post(DateInputViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Response.GetTypedHeaders().Location = new Uri("/panel", UriKind.Relative);
                return new StatusCodeResult(303);
            }

            return View("Index", viewModel);
        }
    }
}
