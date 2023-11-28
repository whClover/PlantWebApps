using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Collections.Generic;

namespace PlantWebApps.Controllers
{
    public class SampleController : Controller
    {
        private string _tempfilter;
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            int totalRecords;
            int totalPages;

            string countQuery = "SELECT COUNT(*) FROM tbl_user";
            int.TryParse(SQLFunction.ExecuteScalar(countQuery).ToString(), out totalRecords);

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int offset = (page - 1) * pageSize;
            int limit = pageSize;

            string query = $"SELECT * FROM tbl_user order by userid desc OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
            ViewBag.data = SQLFunction.execQuery(query);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Filter = false;

            return View("~/Views/Sample/Index.cshtml");
        }

        public IActionResult Filter(int page = 1, int pageSize = 20)
        {
            int totalRecords;
            int totalPages;
            BuildTempFilter();

            string countQuery = "SELECT COUNT(*) FROM tbl_user" + _tempfilter;
            int.TryParse(SQLFunction.ExecuteScalar(countQuery).ToString(), out totalRecords);

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int offset = (page - 1) * pageSize;
            int limit = pageSize;

            string query = $"SELECT * FROM tbl_user {_tempfilter} order by userid desc OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
            ViewBag.data = SQLFunction.execQuery(query);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Filter = false;

            return View("~/Views/Sample/Index.cshtml");
        }

        private void BuildTempFilter()
        {
            string tempfilter = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["userid"]))
            {
                tempfilter = " and userid like" + Utility.Evar(Request.Form["userid"], 11) + tempfilter;
                string userid = Request.Form["userid"];
                ViewBag.userid = userid;
            }

            if (!string.IsNullOrEmpty(Request.Form["username"]))
            {
                tempfilter = " and username like" + Utility.Evar(Request.Form["username"], 11) + tempfilter;
                string username = Request.Form["username"];
                ViewBag.username = username;
            }

            if (!string.IsNullOrEmpty(Request.Form["fullname"]))
            {
                tempfilter = " and fullname like" + Utility.Evar(Request.Form["fullname"], 11) + tempfilter;
                string fullname = Request.Form["fullname"];
                ViewBag.fullname = fullname;
            }

            _tempfilter = Utility.VarFilter(tempfilter);
        }
    }
}
