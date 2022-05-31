using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class WarningTextController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
