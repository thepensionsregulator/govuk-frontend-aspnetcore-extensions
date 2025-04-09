using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class YouTubeVideoController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
