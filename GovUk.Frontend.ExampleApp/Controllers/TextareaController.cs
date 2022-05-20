using GovUk.Frontend.ExampleApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class TextareaController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Post(TextareaViewModel viewModel)
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
