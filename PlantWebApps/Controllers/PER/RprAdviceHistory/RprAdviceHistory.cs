using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using OfficeOpenXml;
using System.Data;

namespace PlantWebApps.Controllers.PER.RprAdviceHistory
{
    public class Export
    {
        public static void ToCsv(DataTable dataTable, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromDataTable(dataTable, true);
                package.SaveAs(new FileInfo(filePath));
            }
        }

        public static void ToExcel(DataTable dataTable, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.LoadFromDataTable(dataTable, true);
                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
    public class RprAdviceHistory : Controller
    {
        private string _tempfilter;
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            LoadData(page, pageSize);
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
        public IActionResult Export()
        {
            BuildTempFilter();
            string dataQuery = $"SELECT *,convert(varchar,ModDate,103) as FormModDate " +
                  $"FROM v_ExrJobChangeHistory {_tempfilter}";

            DataTable data = SQLFunction.execQuery(dataQuery);
            string fileName = "Job Change History.xlsx";

            return Utility.ExportDataTableToExcel(data, fileName);
        }

        private void LoadData(int page, int pageSize)
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
        }

        private void BuildTempFilter()
        {
            string tempfilter = string.Empty;

            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "id", "ViewBag.id" },
                { "jobid", "ViewBag.jobid" },
                { "parentwo", "ViewBag.parentwo" },
                { "childwo", "ViewBag.childwo" },
                { "itemchange", "ViewBag.itemchange" },
                { "descchange", "ViewBag.descchange" },
                { "jobstatus", "ViewBag.jobstatus" },
                { "moddate", "ViewBag.moddate" },
                { "modby", "ViewBag.modby" }
            };

            foreach (var field in formFields)
            {
                if (!string.IsNullOrEmpty(Request.Form[field.Key]))
                {
                    var viewBagDict = ViewBag as IDictionary<string, object>;

                    if (viewBagDict != null)
                    {
                        viewBagDict[field.Value] = Request.Form[field.Key];
                    }

                    tempfilter = $" and {field.Key} like {Utility.Evar(Request.Form[field.Key], 11)}" + tempfilter;
                }
            }

            _tempfilter = Utility.VarFilter(tempfilter);
        }

    }
}
