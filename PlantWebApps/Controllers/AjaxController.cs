using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using PlantWebApps.Models;
using System.Data;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text;
using System.Data.SqlClient;

namespace PlantWebApps.Controllers
{
    public class AjaxController : Controller
    {
        private string _tempfilter;
        public IActionResult Index()
        {
            return View("~/Views/Sample/IndexAjax.cshtml");
        }

        [HttpGet]
        public IActionResult LoadData(string userID, string username, string fullname)
        {
            string tempfilter = string.Empty;
            if (!string.IsNullOrEmpty(userID))
            {
                tempfilter = " and userid like" + Utility.Evar(userID, 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(username))
            {
                tempfilter = " and username like" + Utility.Evar(username, 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(fullname))
            {
                tempfilter = " and fullname like" + Utility.Evar(fullname, 11) + tempfilter;
            }

            tempfilter = Utility.VarFilter(tempfilter);

            string dataQuery = "SELECT * FROM tbl_user" + tempfilter;

            var data = SQLFunction.execQuery(dataQuery);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    userID = row["UserID"],
                    username = row["Username"],
                    fullname = row["Fullname"],
                    email = row["email"] != null ? row["email"].ToString() : "",
                    jobcost = row["JobCost"] != null ? row["JobCost"].ToString() : "",
                    edit = "<a class='btn btn-link btn-sm' href='#'><i class='fa fa-edit'></i></a>"
                };

                rows.Add(rowData);
            }

            return new JsonResult(rows);
        }
    }
}
