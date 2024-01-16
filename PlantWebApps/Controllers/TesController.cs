using Microsoft.AspNetCore.Mvc;

namespace PlantWebApps.Controllers
{
    public class TesController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Sample/tes.cshtml");
        }
    }
}
