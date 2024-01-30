using Microsoft.AspNetCore.Mvc;

namespace PlantWebApps.Controllers.JobDispatch
{
    public class JournalDetail : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/JobDispatch/JournalDetail/JournalDetail.cshtml");
        }
    }
}
