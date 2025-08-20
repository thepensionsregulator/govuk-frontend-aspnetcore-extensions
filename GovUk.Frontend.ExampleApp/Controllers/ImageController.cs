using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class ImageController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
