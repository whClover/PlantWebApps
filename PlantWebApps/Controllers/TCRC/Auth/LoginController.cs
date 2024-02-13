using Microsoft.AspNetCore.Mvc;

namespace PlantWebApps.Controllers.TCRC.Auth
{
    public class LoginController : Controller
    {
        public IActionResult Index(String id)
        {
            if(id == "1")
            {
                return View("~/Views/Auth/LoginTCRC.cshtml");
            }
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
