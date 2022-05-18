using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class NotificationBannerController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
