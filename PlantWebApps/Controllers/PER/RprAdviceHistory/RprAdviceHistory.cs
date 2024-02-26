using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using OfficeOpenXml;
using System.Data;
using System.Collections.Generic;

namespace PlantWebApps.Controllers.PER.RprAdviceHistory
{
    public class RprAdviceHistory : Controller
    {
        private string _tempfilter;
        public IActionResult Index()
        {
            Console.WriteLine("RprAdviceHistory Index");

            var filter = TempData.Peek("rprFilter");

            Console.WriteLine("filter" + filter);

            var data = LoadInitialData(filter);

            return View("~/Views/PER/RprAdviceHistory/Index.cshtml", data);
        }
        public IActionResult LoadData(string jobid, string parentwo, string childwo, string itemchange,
                            string descchange, string jobstatus, string modby)
        {
            string filter = BuildTempFilter(jobid, parentwo, childwo, itemchange, jobstatus, descchange, modby);
            Console.WriteLine("filter" + filter);
            _tempfilter = Utility.VarFilter(filter);

			TempData["rprFilter"] = _tempfilter;

			string query = $"SELECT TOP 50 * FROM v_ExrJobChangeHistory {_tempfilter} order by id desc";
            Console.WriteLine(query);
            var data = SQLFunction.execQuery(query);

            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                    jobid = Utility.CheckNull(row["JobID"]),
                    parentwo = Utility.CheckNull(row["ParentWO"]),
                    childwo = Utility.CheckNull(row["ChildWO"]),
                    itemchange = Utility.CheckNull(row["ItemChange"]),
                    descchange = Utility.CheckNull(row["DescChange"]),
                    jobstatus = Utility.CheckNull(row["JobStatus"]),
                    moddate = Utility.CheckNull(row["ModDate"]),
                    modby = Utility.CheckNull(row["ModBy"]),
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }
        public List<object> LoadInitialData(object filter)
        {

            string query = $"SELECT TOP 50 * FROM v_ExrJobChangeHistory {filter} order by id desc";
            Console.WriteLine(query);
            var data = SQLFunction.execQuery(query);

            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                    jobid = Utility.CheckNull(row["JobID"]),
                    parentwo = Utility.CheckNull(row["ParentWO"]),
                    childwo = Utility.CheckNull(row["ChildWO"]),
                    itemchange = Utility.CheckNull(row["ItemChange"]),
                    descchange = Utility.CheckNull(row["DescChange"]),
                    jobstatus = Utility.CheckNull(row["JobStatus"]),
                    moddate = Utility.CheckNull(row["ModDate"]),
                    modby = Utility.CheckNull(row["ModBy"]),
                };
                rows.Add(rowData);
            }
            return rows;
        }
        private string BuildTempFilter(string jobid, string parentwo, string childwo, string itemchange,
                            string descchange, string jobstatus, string modby)
        {
            string tempfilter = string.Empty;

            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "JobID", jobid },
                { "ParentWO", parentwo },
                { "ChildWO", childwo },
                { "ItemChange", itemchange },
                { "JobStatus",  descchange},
                { "DescChange",  jobstatus},
                { "ModBy", modby }
            };

            foreach (var field in formFields)
            {
                if (!string.IsNullOrEmpty(field.Value))
                {
                    var viewBagDict = ViewBag as IDictionary<string, object>;

                    if (viewBagDict != null)
                    {
                        viewBagDict[field.Key] = field.Value;
                    }

                    tempfilter = $" and {field.Key} like {Utility.Evar(field.Value, 11)}" + tempfilter;
                }
            }

            return tempfilter;
        }

    }
}
