using Microsoft.AspNetCore.Mvc.Filters;

namespace PlantWebApps.Helper
{
    public class GetIdentity : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) 
        {
            if (context.HttpContext.User.Identity.Name == null) 
            {
                GlobalString._username = Environment.UserName;
            }
            else
            {
                GlobalString._username = context.HttpContext.User.Identity.Name.Replace("THIESS\\", "");
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
