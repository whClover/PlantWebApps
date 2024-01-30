using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace PlantWebApps.Controllers.TCRC.Attachment
{
    public class TCRC_Attachment : Controller
    {
        private string _tempfilter;
        [TempData]
        public string Msg { get; set; }
        [TempData]
        public string Stat { get; set; }
        public IActionResult DetailAttachment(string wono)
        {
            LoadOption();
            ViewBag.wono = wono;

            return View("~/Views/TCRC/Attachment/Index.cshtml");
        }
        public IActionResult LoadData(string wono, string jobNumber, string statusInput)
        {
            string tempfilter = string.Empty;

            if (!string.IsNullOrEmpty(statusInput))
            {
                tempfilter = "AND JobStatusID in ( " + Utility.Evar(statusInput, 1) + ")" + tempfilter;
            }

            if (!string.IsNullOrEmpty(jobNumber))
            {
                tempfilter = "AND JobNumber =" + Utility.Evar(jobNumber, 1) + tempfilter;
            }

            var forder = "Order By WONO DESC,StatusOrder ASC,Name ASC";

            string dataQuery = "SELECT *,dbo.lastActivity(WONO) as lastactivity from v_IntJobPackageAttachmentRev where wono = " + Utility.Evar(wono, 1) + " " + tempfilter + " " + forder;
            var data = SQLFunction.execQuery(dataQuery);

            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                    section = Utility.CheckNull(row["Section"]),
                    wono = Utility.CheckNull(row["WONo"]),
                    woDesc = Utility.CheckNull(row["WoDesc"]),
                    woStatus = Utility.CheckNull(row["WOStatus"]),
                    jobStatusDesc = Utility.CheckNull(row["JobStatusDesc"]),
                    lastActivity = Utility.CheckNull(row["lastactivity"]),
                    idJobStatus = Utility.CheckNull(row["IDJobstatus"]),
                    name = Utility.CheckNull(row["Name"]),
                    createdDate = Utility.CheckNull(row["CreatedDate"]),
                    createdBy = Utility.CheckNull(row["CreatedBy"]),
                    viewAttachment = $@"<button type='button' onclick='viewAttachment({row["ID"]})' class='btn btn-primary btn-sm d-flex 
                                                align-items-center justify-content-center mx-2' id='btnViewAttachment'>
                                            <svg xmlns=""http://www.w3.org/2000/svg"" width=""16"" height=""16"" fill=""currentColor"" class=""bi bi-eye-fill"" viewBox=""0 0 16 16"">
                                              <path d=""M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0""/>
                                              <path d=""M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7""/>
                                            </svg>
                                        </button>"
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }
        public IActionResult ViewAttachment(string id)
        {
            Console.WriteLine(id);

            string query = $@"Select * from v_IntJobPackageAttachmentRev where ID= {Utility.Evar(id, 0)}";
            var data = SQLFunction.execQuery(query);

            if (data.Rows.Count > 0)
            {
                var ePath = Utility.CheckNull(data.Rows[0]["FilePath"]);
                var ewono = Utility.CheckNull(data.Rows[0]["WONO"]);
                var efilename = Utility.CheckNull(data.Rows[0]["Name"]);
                var eCategory = Utility.CheckNull(data.Rows[0]["Category"]);

                string[] FileData = ePath.Split('\\');
                string lfile = FileData[FileData.Length - 1];
                string etemp = Environment.GetEnvironmentVariable("temp");
                string xtemp = $@"{etemp}\";

                //string tempfile = $@"{xtemp}{Utility.FileCountB(xtemp)}{lfile}"; //server
                string localTest = $@"C:\Users\trahayu\AppData\Local\Temp\855915046_1_InternalWOSheet.pdf"; //local

                return new JsonResult(new
                {
                    message = "exist",
                    tempfile = localTest,
                    wono = ewono,
                    category = eCategory,
                    name = efilename,
                });
            }
            else
            {
                return new JsonResult("!exist");
            }
        }
        public IActionResult OpenAttachment(string tempfile)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = tempfile,
                UseShellExecute = true
            });
            return new JsonResult("ok");
        }
        private void LoadOption()
        {
            string queryWorkType = "SELECT JobNumber, WorkTypeName FROM tbl_WorkType;";
            ViewBag.queryWorkType = SQLFunction.execQuery(queryWorkType);

            string qeuryJstat = "SELECT JobStatusID, JobStatus FROM tbl_IntJobStatus ORDER BY StatusOrder";
            ViewBag.qeuryJstat = SQLFunction.execQuery(qeuryJstat);
        }
    }
}
