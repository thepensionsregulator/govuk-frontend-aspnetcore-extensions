using GovUk.Frontend.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class AddressLookupController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(AddressLookupViewModel viewModel)
        {
            return View("Index", viewModel);
        }
    }
}
