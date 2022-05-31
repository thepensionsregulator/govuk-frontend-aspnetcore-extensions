using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class DetailsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
