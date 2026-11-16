using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class AddressLookupController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
