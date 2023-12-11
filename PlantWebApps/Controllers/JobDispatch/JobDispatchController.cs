using Microsoft.AspNetCore.Mvc;

namespace PlantWebApps.Controllers.JobDispatch
{
    public class JobDispatchController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/JobDispatch/Index.cshtml");
        }
        public IActionResult loadData()
        {
            return null;
        }
    }
}
