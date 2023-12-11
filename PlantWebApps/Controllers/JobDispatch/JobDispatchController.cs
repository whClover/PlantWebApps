using Microsoft.AspNetCore.Mvc;

namespace PlantWebApps.Controllers.JobDispatch
{
    public class JobDispatchController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/JobDispatch/Index.cshtml");
        }
        public IActionResult loadData(string fwono, string tanno, string fsectionid, 
            string fdocstart, string fdocend, string fdispatchtypeid)
        {
            string tempfilter = string.Empty;



            return null;
        }
    }
}
