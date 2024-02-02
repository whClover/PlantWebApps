using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;
using System.Data.SqlClient;
using static System.Collections.Specialized.BitVector32;
using System.Drawing;
using System.Security.Cryptography;

namespace PlantWebApps.Controllers.PER.OutstandingInternalWo
{
    public class OutstandingInternalWo : Controller
    {
        private string _tempfilter;
        public IActionResult Index()
        {
            LoadOption();
            return View("~/Views/PER/OutstandingInternalWo/Index.cshtml");
        }
        public IActionResult LoadData(string fParentWO, string fSection, string fwono, string fdocstart, string fdocend)
        {
            string filter = BuildTempFilter(fParentWO, fSection, fwono, fdocstart, fdocend);

            var forder = "Order by RegisterDate Asc";

            _tempfilter = Utility.VarFilter(filter);

            string dataQuery = $"SELECT * from v_ExrJobDetailIntWoOut {_tempfilter} {forder}";
            var data = SQLFunction.execQuery(dataQuery);

            Console.WriteLine(dataQuery);

            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    button = $@"<button type='button' class='btn btn-link btn-sm' onclick='updateIntWo(""{row["ID"]}"", ""{row["WONo"]}"")'>
                                    <svg xmlns=""http://www.w3.org/2000/svg"" width=""16"" height=""16"" fill=""currentColor"" class=""bi bi-pencil-square"" viewBox=""0 0 16 16"">
                                      <path d=""M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z""/>
                                      <path fill-rule=""evenodd"" d=""M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z""/>
                                    </svg>       
                                </button>",
                    id = Utility.CheckNull(row["ID"]),
                    section = Utility.CheckNull(row["Section"]),
                    registerdate = Utility.CheckNull(row["RegisterDate"]),
                    parentwo = Utility.CheckNull(row["ParentWO"]),
                    offsitewo = Utility.CheckNull(row["OffSiteWO"]),
                    supplierid = Utility.CheckNull(row["SupplierID"]),
                    compdesc = Utility.CheckNull(row["CompDesc"]),
                    compqty = Utility.CheckNull(row["CompQty"]),
                    intwo = Utility.CheckNull(row["IntWO"]),
                    wono = Utility.CheckNull(row["WONo"]),
                    jobdesc = Utility.CheckNull(row["Jobdesc"]),
                    qty = Utility.CheckNull(row["Qty"]),
                    jobstatusid = Utility.CheckNull(row["JobStatusID"]),
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }
        public IActionResult BulkConfirm(string fParentWO, string fSection, string fwono, string fdocstart, string fdocend)
        {
            string filter = BuildTempFilter(fParentWO, fSection, fwono, fdocstart, fdocend);
            _tempfilter = Utility.VarFilter(filter);

            string query = $"SELECT COUNT(*) from v_ExrJobDetailIntWoOut {_tempfilter} ";
            Console.WriteLine(query);

            var number = SQLFunction.ExecCountQuery(query);

            return new JsonResult(number);
        }
        public IActionResult BulkUpdateIntWo(string fParentWO, string fSection, string fwono, string fdocstart, string fdocend)
        {
            string filter = BuildTempFilter(fParentWO, fSection, fwono, fdocstart, fdocend);
            var forder = "Order by RegisterDate Asc";
            _tempfilter = Utility.VarFilter(filter);

            string[] arrerr = new string[1];
            arrerr[0] = "Parent WO,ChildWO,Source,Remark";
            string errstring = "";
            int xx = 0;

            string dataQuery = $"SELECT * from v_ExrJobDetailIntWoOut {_tempfilter} {forder}";
            Console.WriteLine("dataquery" + dataQuery);
            var data = SQLFunction.execQuery(dataQuery);

            if (data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    string id = Utility.CheckNull(row["ID"]);
                    string query = $"select ID,ParentWO,OffsiteWO,WONO from v_ExrJobDetailIntWOout where ID= {id}";
                    var recordData = SQLFunction.execQuery(query);

                    if (recordData.Rows.Count > 1)
                    {
                        foreach (DataRow rows in recordData.Rows)
                        {
                            string eid = Utility.CheckNull(rows["ID"]);
                            string eparentwo = Utility.CheckNull(rows["ParentWO"]);
                            string eoffsitewo = Utility.CheckNull(rows["OffsiteWO"]);
                            string ewono = Utility.CheckNull(rows["WONO"]);

                            errstring = eid + "," + eparentwo + "," + eoffsitewo + "," + ewono;
                            Console.WriteLine("errstring" + errstring);

                            xx = xx + 1;
                            Array.Resize(ref arrerr, xx + 1);
                            arrerr[xx] = errstring;
                        }
                    }
                    if (recordData.Rows.Count == 1)
                    {
                        foreach (DataRow rows in recordData.Rows)
                        {
                            string eid = Utility.CheckNull(rows["ID"]);
                            string ewono = Utility.CheckNull(rows["ParentWO"]);
                            string str2 = "Update tbl_EXRJobDetail set IntWO=" + ewono + " where ID=" + eid;
                            Console.WriteLine("query" + str2);
                            SQLFunction.execQuery(str2);
                        }
                    }
                }
            }

            if (arrerr.GetUpperBound(0) >= 1)
            {
                Utility.LaunchArrayToExcel(arrerr);
            }

            return new JsonResult("ok");
        }
        public ActionResult DownloadExcelFile()
        {
            string tempPath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            string filePath = Path.Combine(tempPath, "output.xlsx");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return PhysicalFile(filePath, contentType, "output.xlsx");
        }
        private string BuildTempFilter(string fParentWO, string fSection, string fwono, string fdocstart, string fdocend)
        {
            string tempfilter = string.Empty;

            if (!string.IsNullOrEmpty(fParentWO))
            {
                tempfilter = $"AND ParentWO = {Utility.Evar(fParentWO, 0)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(fSection))
            {
                tempfilter = $"AND Section = {Utility.Evar(fSection, 1)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(fwono))
            {
                tempfilter = $"AND OffsiteWO = {Utility.Evar(fwono, 0)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(fdocstart))
            {
                tempfilter = $"AND RegisterDate >= {Utility.Evar(fdocstart, 2)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(fdocend))
            {
                tempfilter = $"AND RegisterDate <={Utility.Evar(fdocend, 2)}" + tempfilter;
            }

            return tempfilter;
        }
        public IActionResult UpdateIntWo(string id, string wono)
        {
            string query = $"exec dbo.ExrIntJobUpdate {Utility.Evar(id, 0)}, {Utility.Evar(wono, 1)}, NULL, {Utility.ebyname()}";
            Console.WriteLine(query);
            SQLFunction.execQuery(query);

            return new JsonResult("ok");
        }
        private void LoadOption()
        {
            string querySection = "select JobSource,JobSourceID from tbl_JobSource where JobSourceID in(2,3,13)";
            ViewBag.section = SQLFunction.execQuery(querySection);
        }
    }
}
