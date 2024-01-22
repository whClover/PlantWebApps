using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;

namespace PlantWebApps.Controllers
{
	public class General : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CheckGranted(string formname, string accessType)
		{
			string uexists = "select * from tbl_user where username=" + Utility.ebyname();
			DataTable data = new DataTable();
			data = SQLFunction.execQuery(uexists);

			if (data.Rows.Count == 0)
			{
				return Json(false);
			}

			string userid = data.Rows[0]["userid"].ToString();
			string userpriv = data.Rows[0]["Previllege"].ToString();

			if (userpriv == "Administrator")
			{
				return Json(true);
			}
			string uform = "Select dbo.CheckGranted(" + userid + "," + Utility.Evar(formname, 1) + "," + accessType + ") as Granted";
			DataTable data2 = new DataTable();
			data2 = SQLFunction.execQuery(uform);
			if (data.Rows.Count == 0)
			{
				return Json(false);
			}
			string granted = data2.Rows[0]["Granted"].ToString();
			if (granted == "1")
			{
				return Json(true);
			}
			else
			{
				return Json(false);
			}
		}
	}
}
