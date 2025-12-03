using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class FooterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
