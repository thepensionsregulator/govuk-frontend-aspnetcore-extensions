using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class SectionCardsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
