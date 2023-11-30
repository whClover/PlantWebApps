using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;

namespace PlantWebApps.Controllers.PER.RprAdviceHistory
{
    public class RprAdviceHistory : Controller
    {
        private string _tempfilter;
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            int totalRecords;
            int totalPages;

            string countQuery = "SELECT COUNT(*) FROM v_ExrJobChangeHistory";
            int.TryParse(SQLFunction.ExecuteScalar(countQuery).ToString(), out totalRecords);

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int offset = (page - 1) * pageSize;
            int limit = pageSize;

            string query = $"SELECT * FROM v_ExrJobChangeHistory order by id desc OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
            ViewBag.data = SQLFunction.execQuery(query);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Filter = false;

            return View("~/Views/PER/RprAdviceHistory/Index.cshtml");
        }
        public IActionResult Filter(int page = 1, int pageSize = 20)
        {
            int totalRecords;
            int totalPages;
            BuildTempFilter();

            string countQuery = "SELECT COUNT(*) FROM v_ExrJobChangeHistory" + _tempfilter;
            int.TryParse(SQLFunction.ExecuteScalar(countQuery).ToString(), out totalRecords);

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int offset = (page - 1) * pageSize;
            int limit = pageSize;

            string query = $"SELECT * FROM v_ExrJobChangeHistory {_tempfilter} order by id desc OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
            ViewBag.data = SQLFunction.execQuery(query);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Filter = false;

            return View("~/Views/PER/RprAdviceHistory/Index.cshtml");
        }
        private void BuildTempFilter()
        {
            string tempfilter = string.Empty;

            if (!string.IsNullOrEmpty(Request.Form["id"]))
            {
                tempfilter = " and id like" + Utility.Evar(Request.Form["id"], 11) + tempfilter;
                string id = Request.Form["id"];
                ViewBag.id = id;
            }

            if (!string.IsNullOrEmpty(Request.Form["jobid"]))
            {
                tempfilter = " and jobid like" + Utility.Evar(Request.Form["jobid"], 11) + tempfilter;
                string jobid = Request.Form["jobid"];
                ViewBag.jobid = jobid;
            }

            if (!string.IsNullOrEmpty(Request.Form["parentwo"]))
            {
                tempfilter = " and parentwo like" + Utility.Evar(Request.Form["parentwo"], 11) + tempfilter;
                string parentwo = Request.Form["parentwo"];
                ViewBag.parentwo = parentwo;
            }

            if (!string.IsNullOrEmpty(Request.Form["childwo"]))
            {
                tempfilter = " and childwo like" + Utility.Evar(Request.Form["childwo"], 11) + tempfilter;
                string childwo = Request.Form["childwo"];
                ViewBag.childwo = childwo;
            }

            if (!string.IsNullOrEmpty(Request.Form["itemchange"]))
            {
                tempfilter = " and itemchange like" + Utility.Evar(Request.Form["itemchange"], 11) + tempfilter;
                string itemchange = Request.Form["itemchange"];
                ViewBag.itemchange = itemchange;
            }

            if (!string.IsNullOrEmpty(Request.Form["descchange"]))
            {
                tempfilter = " and descchange like" + Utility.Evar(Request.Form["descchange"], 11) + tempfilter;
                string descchange = Request.Form["descchange"];
                ViewBag.descchange = descchange;
            }

            if (!string.IsNullOrEmpty(Request.Form["jobstatus"]))
            {
                tempfilter = " and jobstatus like" + Utility.Evar(Request.Form["jobstatus"], 11) + tempfilter;
                string jobstatus = Request.Form["jobstatus"];
                ViewBag.jobstatus = jobstatus;
            }

            if (!string.IsNullOrEmpty(Request.Form["moddate"]))
            {
                tempfilter = " and moddate like" + Utility.Evar(Request.Form["moddate"], 11) + tempfilter;
                string moddate = Request.Form["moddate"];
                ViewBag.moddate = moddate;
            }

            if (!string.IsNullOrEmpty(Request.Form["modby"]))
            {
                tempfilter = " and modby like" + Utility.Evar(Request.Form["modby"], 11) + tempfilter;
                string modby = Request.Form["modby"];
                ViewBag.moddate = modby;
            }

            _tempfilter = Utility.VarFilter(tempfilter);
        }
    }
}
