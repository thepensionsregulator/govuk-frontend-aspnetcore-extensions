using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class FeaturedImageController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
