using Microsoft.AspNetCore.Mvc;

namespace PlantWebApps.Controllers.Testing
{
    public class Testing : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Testing/JsPdf/JsPdfFooter.cshtml");
        }
    }
}
