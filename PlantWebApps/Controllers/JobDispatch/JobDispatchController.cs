using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlantWebApps.Helper;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using static System.Collections.Specialized.BitVector32;

namespace PlantWebApps.Controllers.JobDispatch
{
    public class JobDispatchController : Controller
    {
        private string _tempfilter;
        [TempData]
        public String Msg { get; set; }
        [TempData]
        public String Stat { get; set; }
        [TempData]
        public string EAnno { get; set; }

        public IActionResult Index()
        {
            loadOption();

            var filter = TempData.Peek("jobDispatchFilter");
            var tempData = LoadInitialData(filter);

            return View("~/Views/JobDispatch/Index.cshtml", tempData);
        }
        private string BuildTempFilter(string fwono, string tanno, string fsectionid,
            string fdocstart, string fdocend, string fdispatchtypeid)
        {
            string tempfilter = string.Empty;
            if (!string.IsNullOrEmpty(fwono))
            {
                tempfilter = "AND ID IN (Select DispatchID from tbl_DispatchJobDetail Where WONO = " + Utility.Evar(fwono, 1) + ")" + tempfilter;
            }

            if (!string.IsNullOrEmpty(tanno))
            {
                tempfilter = "AND ID like" + Utility.Evar(tanno, 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(fsectionid))
            {
                tempfilter = "AND SectionID = " + fsectionid + tempfilter;
            }

            if (!string.IsNullOrEmpty(fdocstart))
            {
                tempfilter = "AND DispatchDate >= " + Utility.Evar(fdocstart, 2) + tempfilter;
            }

            if (!string.IsNullOrEmpty(fdocend))
            {
                tempfilter = "AND DispatchDate <= " + Utility.Evar(fdocend, 2) + tempfilter;
            }

            if (!string.IsNullOrEmpty(fdispatchtypeid))
            {
                tempfilter = "AND DispatchType  IN (" + Utility.Evar(fdispatchtypeid, 1) + ")" + tempfilter;
            }

            var forder = " Order by ID DESC";
            var filter = $"AND StatusID != 'x' {tempfilter} {forder}";

            return filter;
        }
        public IActionResult loadData(string fwono, string tanno, string fsectionid,
            string fdocstart, string fdocend, string fdispatchtypeid)
        {
            string filter = BuildTempFilter(fwono, tanno, fsectionid, fdocstart, fdocend, fdispatchtypeid);
            _tempfilter = Utility.VarFilter(filter);

            TempData["jobDispatchFilter"] = _tempfilter;

            string dataQuery = $"SELECT TOP 50 * FROM v_DispatchJob {_tempfilter}";
            var data = SQLFunction.execQuery(dataQuery);

            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                    section = Utility.CheckNull(row["Section"]),
                    dispatchType = Utility.CheckNull(row["DispatchType"]),
                    attantionName = Utility.CheckNull(row["AttentionName"]),
                    attentionTo = Utility.CheckNull(row["AttentionTo"]),
                    project = Utility.CheckNull(row["Project"]),
                    dispatchDate = Utility.CheckNull(row["DispatchDate"]),
                    handledDate = Utility.CheckNull(row["HandledDate"]),
                    receivedDate = Utility.CheckNull(row["ReceivedDate"]),
                    statusId = Utility.CheckNull(row["StatusID"]),
                    edit = "<a class='btn btn-link btn-sm' href='/JobDispatch/Edit/" + row["ID"] + "'><i class='fa fa-edit'></i></a>",
                    delete = $@"<button type='button' class='btn btn-link btn-sm' onclick='confirmDelete(""{row["ID"]}"")'>
                                    <svg xmlns=""http://www.w3.org/2000/svg"" width=""16"" height=""16"" fill=""red"" class=""bi bi-trash3-fill"" viewBox=""0 0 16 16"">
                                      <path d=""M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5""/>
                                    </svg>       
                                </button>",
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }
        public List<object> LoadInitialData(object filter)
        {

            string query = $"SELECT TOP 50 * FROM v_DispatchJob {filter}";
            var data = SQLFunction.execQuery(query);

            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                    section = Utility.CheckNull(row["Section"]),
                    dispatchType = Utility.CheckNull(row["DispatchType"]),
                    attantionName = Utility.CheckNull(row["AttentionName"]),
                    attentionTo = Utility.CheckNull(row["AttentionTo"]),
                    project = Utility.CheckNull(row["Project"]),
                    dispatchDate = Utility.CheckNull(row["DispatchDate"]),
                    handledDate = Utility.CheckNull(row["HandledDate"]),
                    receivedDate = Utility.CheckNull(row["ReceivedDate"]),
                    statusId = Utility.CheckNull(row["StatusID"]),
                    edit = "<a class='btn btn-link btn-sm' href='/JobDispatch/Edit/" + row["ID"] + "'><i class='fa fa-edit'></i></a>",
                    delete = $@"<button type='button' class='btn btn-link btn-sm' onclick='confirmDelete(""{row["ID"]}"")'>
                                    <svg xmlns=""http://www.w3.org/2000/svg"" width=""16"" height=""16"" fill=""red"" class=""bi bi-trash3-fill"" viewBox=""0 0 16 16"">
                                      <path d=""M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5""/>
                                    </svg>       
                                </button>",
                };
                rows.Add(rowData);
            }
            return rows;
        }
        public IActionResult Add() 
        {
            loadOption();
            var eSpv = ViewBag.spvdesc;
            var eRepairBy = ViewBag.repairby;

            ViewBag.Project = "BSF";
            ViewBag.JobNo = "503002";
            ViewBag.ShipmentDate = DateTime.Now;
            ViewBag.AttentionTo = eRepairBy;
            return View("~/Views/JobDispatch/Form.cshtml");
        }
        public IActionResult Edit(string id)
        {
            loadOption();
            LoadAccess(id);

            ViewBag.id = id;
            Console.WriteLine("id is " + ViewBag.id);
            ViewBag.isEdit = true;

            string query = "select TOP 50 * from v_DispatchJob where ID =" + Utility.Evar(id, 1);
            ViewBag.data = SQLFunction.execQuery(query);

            string queryDispatchType = $"select DispatchType from v_DispatchJob where ID = {Utility.Evar(id, 1)} ";
            ViewBag.typeDispatch = SQLFunction.execQuery(queryDispatchType);

            return View("~/Views/JobDispatch/Form.cshtml");
        }
        public JsonResult SearchWonoDetail(string id, string wono)
        {
            loadOption();

            ViewBag.isEdit = true;

            string queryDispatchType = $"select DispatchType from v_DispatchJob where ID = {Utility.Evar(id, 1)} ";
            ViewBag.typeDispatch = SQLFunction.execQuery(queryDispatchType);

            string dataQuery = $@"SELECT * from tbl_DispatchJobDetail WHERE  WONO = {Utility.Evar(wono, 1)} AND StatusID != 'del'";
            Console.WriteLine(dataQuery);
            
            var result = SQLFunction.execQuery(dataQuery);

            if (result.Rows.Count > 1)
            {
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                List<string> columnNames = new List<string>();
                foreach (DataColumn column in result.Columns)
                {
                    columnNames.Add(column.ColumnName);
                }

                foreach (DataRow row in result.Rows)
                {
                    Dictionary<string, object> dataDict = new Dictionary<string, object>();
                    foreach (string columnName in columnNames)
                    {
                        dataDict[columnName] = row[columnName];
                    }
                    rows.Add(dataDict);
                }

                string json = JsonConvert.SerializeObject(rows);
                return Json(new { massage = "more", json });
            }
            else if (result.Rows.Count > 0)
            {
                List<string> columnNames = new List<string>();
                foreach (DataColumn column in result.Columns)
                {
                    columnNames.Add(column.ColumnName);
                }

                // Create a dictionary to store the column names and values
                Dictionary<string, object> dataDict = new Dictionary<string, object>();
                foreach (DataRow row in result.Rows)
                {
                    foreach (string columnName in columnNames)
                    {
                        dataDict[columnName] = row[columnName];
                    }
                }
                string json = JsonConvert.SerializeObject(dataDict);
                return Json(new { massage = "one", json });
            }
            else
            {
                string json = "123";
                return Json(new { massage = "empty", json });
            }

        }
        private void LoadAccess(string ID)
        {
            string dataQuery = @"SELECT * from tbl_DispatchJobDetail WHERE ID =" + Utility.Evar(ID, 1) + " AND StatusID != 'del'";
            Console.WriteLine(dataQuery);
            var result = SQLFunction.execQuery(dataQuery);
            if (result.Rows.Count > 0)
            {
                ViewBag.dataaccess = result;
            }
        }
        
        public IActionResult LoadDetailAnData(string sectionid, string jobid, string wono, string compdesc)
        {
            string tempfilter = string.Empty;
            if (!string.IsNullOrEmpty(sectionid))
            {
                tempfilter = "AND SectionID = " + Utility.Evar(sectionid, 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(jobid))
            {
                tempfilter = "AND JobID = " + Utility.Evar(jobid, 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(wono))
            {
                tempfilter = "AND WONO Like " + Utility.Evar(wono, 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(compdesc))
            {
                tempfilter = "AND CompDesc Like " + Utility.Evar(compdesc, 1) + tempfilter;
            }

            _tempfilter = Utility.VarFilter(tempfilter);
            Console.WriteLine(tempfilter);
            string dataQuery = $"Select SectionID,Section,JobID,WONo,ChildWO,ExtWO,CompDesc,MaintType,CompQty from v_WOtoDispatch2 {_tempfilter}";
            Console.WriteLine(dataQuery);
            var data = SQLFunction.execQuery(dataQuery);

            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    sectionid = Utility.CheckNull(row["SectionID"]),
                    section = Utility.CheckNull(row["Section"]),
                    jobid = Utility.CheckNull(row["JobID"]),
                    wonodata = Utility.CheckNull(row["WONo"]),
                    childwo = Utility.CheckNull(row["ChildWO"]),
                    extwo = Utility.CheckNull(row["ExtWO"]),
                    compdesc = Utility.CheckNull(row["CompDesc"]),
                    mainttype = Utility.CheckNull(row["MaintType"]),
                    compqty = Utility.CheckNull(row["CompQty"]),
                    select = $"<a id='selectedDetailRow' class='btn btn-primary btn-sm' " +
                    $"value='{string.Join(" ", row.ItemArray)}' data-bs-target='#anDetailModal' " +
                    $"data-bs-toggle='modal'><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-eye-fill\" viewBox=\"0 0 16 16\">\r\n  <path d=\"M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0\"/>\r\n  <path d=\"M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7\"/>\r\n</svg></a>"
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }
        public IActionResult Save()
        {
            loadOption();

            var query = "";
            var querycheck = "";
            string tblName = "tbl_DispatchJob";
            var eid = Request.Form["tid"];
            string eDispatchType = (Utility.Evar(Request.Form["tdispatchtype"], 1));
            string eSectionId = (Utility.Evar(Request.Form["tSectionId"], 1));
            string eShipmentDate = (Utility.Evar(Request.Form["tShipmentDate"], 2));
            string eConsignedTo = (Utility.Evar(Request.Form["tConsignedTo"], 1));
            string eAttentionName = (Utility.Evar(Request.Form["tAttentionName"], 1));
            string eAttentionEmail = (Utility.Evar(Request.Form["tAttentionEmail"], 1));
            string eAttentionTo = (Utility.Evar(Request.Form["tAttentionTo"], 1));
            string eTransportMode = (Utility.Evar(Request.Form["tTransportMode"], 1));
            string eManifesNo = (Utility.Evar(Request.Form["tManifestNo"], 1));
            string eProject = (Utility.Evar(Request.Form["tProject"], 1));
            string eJobNo = (Utility.Evar(Request.Form["tJobNo"], 1));
            string eDispatchBy = (Utility.Evar(Request.Form["tDispatchBy"], 1));
            string eDispatchDate = (Utility.Evar(Request.Form["tDispatchDate"], 2));
            string eDispatchEmail = (Utility.Evar(Request.Form["tDispatchValue"], 1));
            string eHandledBy = (Utility.Evar(Request.Form["cHandledBy"], 1));
            string eHandledDate = (Utility.Evar(Request.Form["tHandledDate"], 1));
            string eRegisterBy = Utility.ebyname();
            string eRegisterDate = (Utility.Evar(Utility.getDate(), 2));

            querycheck = $"select ID from {tblName} where ID =" + eid;
            Console.WriteLine(querycheck);
            var result = SQLFunction.execQuery(querycheck);

            if (result != null && result.Rows.Count > 0)
            {
                query = $@"Update {tblName} set SectionID = {eSectionId},
                    DispatchType = {eDispatchType}, ShipmentDate = {eShipmentDate}, ConsignedTo = {eConsignedTo},
                    AttentionName = {eAttentionName}, AttentionEmail = {eAttentionEmail}, AttentionTo = {eAttentionTo},
                    TransportMode = {eTransportMode}, ManifestNo = {eManifesNo}, Project = {eProject}, JobNo = {eJobNo},
                    DispatchBy = {eDispatchBy}, DispatchDate = {eDispatchDate}, DispatchEmail = {eDispatchEmail}, 
                    HandledBy = {eHandledBy}, HandledDate = {eHandledDate}, ModBy = {eRegisterBy}, 
                    ModDate = {eRegisterDate} WHERE ID = {eid}";
                Console.WriteLine(query);
                SQLFunction.execQuery(query);
                Stat = "success";
                Msg = "Data has been saved";
                return Redirect("/JobDispatch/Edit/" + eid);
            }
            else
            {
                query = $@"INSERT INTO {tblName} (ID, SectionID, DispatchType, ShipmentDate, 
                ConsignedTo, AttentionName, AttentionEmail, AttentionTo, TransportMode, ManifestNo, 
                Project, JobNo, DispatchBy, DispatchDate,DispatchEmail, HandledBy, HandledDate, StatusID, 
                RegisterBy, RegisterDate) VALUES ({eid}, 
                {eSectionId}, {eDispatchType},{eShipmentDate}, {eConsignedTo}, {eAttentionName}, 
                {eAttentionEmail}, {eAttentionTo}, {eTransportMode}, {eManifesNo}, {eProject},
                {eJobNo}, {eDispatchBy}, {eDispatchDate}, {eDispatchEmail} , {eHandledBy}, {eHandledDate}, 'P',
                {eRegisterBy}, {eRegisterDate})";

                Console.WriteLine(query);
                SQLFunction.execQuery(query);

                Stat = "success";
                Msg = "Data has been saved";
                return Redirect("/JobDispatch/Edit/" + eid);
            }
        }
        public IActionResult SentEmail()
        {
            loadOption();
            var query = "";
            string namafile;
            string tempName;
            var tempAnno = Request.Form["emailAnno"];
            var eAnno = (Utility.Evar(tempAnno.ToString(), 1));
            var eRecipient = (Utility.Evar(Request.Form["emailAttentionTo"].ToString(), 1));
            var eSenderEmail = (Utility.Evar(Request.Form["emailSenderEmail"].ToString(), 1));
            var eRecipientEmail = (Utility.Evar(Request.Form["emailRecipientEmail"].ToString(), 1));
            var eCcEmail = (Utility.Evar(Request.Form["emailCcEmail"].ToString(), 1));
            var eSubject = (Utility.Evar(Request.Form["emailSubject"].ToString(), 1));
            var eBodyText = (Utility.Evar(Request.Form["emailBodyText"].ToString(), 1));
            var eBy = (Utility.Evar((Utility.GetCurrentUsername().Split('\\')[1]), 1));
            var eChkAttach = Request.Form["emailChkAttach"];
            var tempfolder = $@"C:\Users\trahayu\source\repos\PlantWebApps\PlantWebApps\src\export\{tempAnno}";
            tempName = Path.Combine(tempfolder, "rpt_Dispatch_Detail.pdf");

            Console.WriteLine("tempanno" + tempAnno);

            if (eChkAttach == "on")
            {
                string servername = "https://localhost:7056/";
                if (!Directory.Exists(tempfolder))
                {
                    Directory.CreateDirectory(tempfolder);
                }
                namafile = tempName ;
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    //FileName = "C:\\webroot\\TCRC Web\\Rotativa\\wkhtmltopdf.exe",
                    FileName = "C:\\htmltopdf\\wkhtmltopdf.exe",
                    Arguments = "https://localhost:7235/JobDispatch/Report/" + tempAnno + " --footer-html \"https://localhost:7235/JobDispatch/TestHeader" + "\" --footer-spacing 3 --header-html \"https://localhost:7235/JobDispatch/TestHeader" + "\" --header-spacing 3 " + " " + namafile
                };
                Process p = new Process { StartInfo = psi };
                p.Start();
                p.WaitForExit();
            }

            var eFileName = (Utility.Evar(tempName, 1));

             //sent email query
             query = $@"exec dbo.dispatchsupplieremail {eAnno},{eRecipientEmail},{eSenderEmail}
             ,{eCcEmail},{eSubject},{eBodyText},null,{eBy}";

             Console.WriteLine(query);
             SQLFunction.execQuery(query);

            Stat = "success";
            Msg = "Email Has Been Sent";

            return Redirect("/JobDispatch/Edit/" + tempAnno);
        }

        public IActionResult Report(string id)
        {
            Console.WriteLine(id);
            string namafile;
            string namafile2;
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            string host = $"{HttpContext.Request.Host}";

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            namafile = Path.Combine(savePath, id + ".pdf");
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "C:\\htmltopdf\\wkhtmltopdf.exe", //local
                //FileName = "C:\\webroot\\TCRC Web\\Rotativa\\wkhtmltopdf.exe",
                Arguments = $"--username minestar --password Mine1staR --margin-bottom 10mm " +
                            $"https://{host}/jobdispatch/PrintBody/{id} " +
                            $"--footer-html https://{host}/jobdispatch/PrintFooter/{id} " +
                            $"--header-html https://{host}/jobdispatch/PrintHeader/{id} " +
                            $"--header-spacing 3 \"{namafile}\""
            };

            Process p = new Process { StartInfo = psi };
            p.Start();
            p.WaitForExit();

            namafile2 = Path.Combine(savePath, id + ".pdf");
            string ffname = "Job Dispatch Report " + id + ".pdf";

            return PhysicalFile(namafile2, "application/pdf", ffname);
        }
        public IActionResult PrintHeader(string id)
        {
            Console.WriteLine("Print header id " + id);
            string Detailquery = $"SELECT * FROM tbl_DispatchJobDetail WHERE ID = {Utility.Evar(id, 0)}";

            ViewBag.detail = SQLFunction.execQuery(Detailquery);
            return View("~/Views/JobDispatch/Report/Header.cshtml");
        }

        public IActionResult PrintBody(string id)
        {
            Console.WriteLine("Print body id " + id);

            string query = $"SELECT * FROM tbl_DispatchJob WHERE ID = {Utility.Evar(id, 0)}";
            ViewBag.report = SQLFunction.execQuery(query);
            Console.WriteLine("query is" + query);

            string Detailquery = $"SELECT * FROM tbl_DispatchJobDetail WHERE ID = {Utility.Evar(id, 0)}";
            ViewBag.detail = SQLFunction.execQuery(Detailquery);

            return View("~/Views/JobDispatch/Report/Body.cshtml");
        }
        public IActionResult PrintFooter(string id)
        {
            Console.WriteLine("Print footer id " + id);
            string query = $"SELECT * FROM tbl_DispatchJob WHERE ID = {Utility.Evar(id, 0)}";

            ViewBag.report = SQLFunction.execQuery(query);
            return View("~/Views/JobDispatch/Report/Footer.cshtml");
        }
        public IActionResult AddDetail()
        {
            var query = "";
            loadOption() ;

            var eStatusID = (Utility.Evar(Request.Form["TStatusID"].ToString(), 1));
            var eType = (Utility.Evar(Request.Form["TType"].ToString(), 1));
            var id = Request.Form["TID"];
            var eDispatchID = (Utility.Evar(Request.Form["TDispatchID"].ToString(), 1));
            var eSectionID = (Utility.Evar(Request.Form["TSectionID"].ToString(), 1));
            var eWONo = (Utility.Evar(Request.Form["TWONo"].ToString(), 1));
            var eID = (Utility.Evar(Request.Form["TID"].ToString(), 1));
            var eJobID = (Utility.Evar(Request.Form["TJobID"].ToString(), 1));
            var eChildWO = (Utility.Evar(Request.Form["TChildWO"].ToString(), 1));
            var eItem = (Utility.Evar(Request.Form["TItem"].ToString(), 1));
            var eQty = (Utility.Evar(Request.Form["TQty"].ToString(), 1));
            var eItemDesc = (Utility.Evar(Request.Form["TItemDesc"].ToString(), 1));
            var ePONo = (Utility.Evar(Request.Form["TPONo"].ToString(), 1));
            var ePRNo = (Utility.Evar(Request.Form["TPRNo"].ToString(), 1));
            var eRemarks = (Utility.Evar(Request.Form["TRemarks"].ToString(), 1));
            var eRegisterBy = (Utility.Evar(Request.Form["TRegisterBy"].ToString(), 1));
            var eRegisterDate = (Utility.Evar(Request.Form["TRegisterDate"].ToString(), 1));
            var eByName = (Utility.Evar((Utility.GetCurrentUsername().Split('\\')[1]), 1)); ;
            var eModDate = (Utility.Evar(Request.Form["TModDate"].ToString(), 1));
            var eDeletedBy = (Utility.Evar(Request.Form["TDeletedBy"].ToString(), 1));
            var eDeletedDate = (Utility.Evar(Request.Form["TDeletedDate"].ToString(), 1));
            var eUOM = (Utility.Evar(Request.Form["TTUOM"].ToString(), 1));

            Console.WriteLine($"eWONo: {eWONo}, eDispatchID: {eDispatchID}");
            if (eWONo != null && eDispatchID != null)
            {
                query = $@"dbo.DispatchJobDetailUpdate {eID}, {eDispatchID},{eItem},{eQty},{eUOM},
                {eItemDesc},{ePRNo},{ePONo},{eRemarks},{eSectionID},{eJobID},{eWONo},{eChildWO}
                ,{eStatusID},null,null,null,{eByName}";

                Console.WriteLine(query);

                SQLFunction.execQuery(query);

                return Redirect("/JobDispatch/Edit/" + id);
            }
            else 
            { 
                query = $@"exec dbo.DispatchJobDetailAdd {eDispatchID},{eItem},{eQty},{eUOM},
                {eItemDesc},{ePRNo},{ePONo},{eRemarks},{eSectionID},{eJobID},{eWONo},{eChildWO}
                ,{eStatusID},null,null,null,{eByName}";

                Console.WriteLine(query);

                SQLFunction.execQuery(query);

                return Redirect("/JobDispatch/Edit/" + id);
            }
            
        }
        public IActionResult Delete(string id)
        {
            var query = "";
            var queryDetail = "";
            var eID = (Utility.Evar(id, 1));
            var eRegisterDate = (Utility.Evar(DateTime.Now.ToString(), 1));
            var eByName = (Utility.Evar((Utility.GetCurrentUsername().Split('\\')[1]), 1)); ;
            
            query = $"UPDATE tbl_DispatchJobDetail SET StatusID = 'del', DeletedDate = {eRegisterDate},DeletedBy = {eByName}  Where ID = {eID}";
            queryDetail = $"UPDATE tbl_DispatchJob SET StatusID = 'x'  Where ID = {eID}";
            
            SQLFunction.execQuery(query);
            SQLFunction.execQuery(queryDetail);
            Console.WriteLine(query);

            return new JsonResult("ok");
        }
        public IActionResult DeleteDetail(string id)
        {
            var query = "";
            var eID = (Utility.Evar(id, 1));
            var eRegisterDate = (Utility.Evar(DateTime.Now.ToString(), 1));
            var eByName = (Utility.Evar((Utility.GetCurrentUsername().Split('\\')[1]), 1)); ;
            query = $"UPDATE tbl_DispatchJobDetail SET StatusID = 'DEL', DeletedDate = {eRegisterDate},DeletedBy = {eByName}  Where ID = {eID}";

            SQLFunction.execQuery(query);
            Console.WriteLine(query);
            Stat = "success";
            Msg = "Data Has Been Deleted";
            return Redirect("/jobdispatch/edit/" + id);
        }
        private void loadOption()
        {
            //section
            string querySection = "SELECT JobSourceID, JobSource FROM tbl_JobSource WHERE (((JobSourceID) In (1,2,3,13)));";
            ViewBag.section = SQLFunction.execQuery(querySection);

            // tAttentionTo for form
            string queryTattentionTo = "SELECT isnull(SupplierName,SupplierID) as SupplierName, SupplierID FROM tbl_SupplierList ORDER BY SupplierName";
            ViewBag.tAatentionTo = SQLFunction.execQuery(queryTattentionTo);

            // tAttentionName for form
            string queryTaatentionName = "SELECT DisplayName, Department, EmailAddress FROM v_AddressBook";
            ViewBag.tAttentionName = SQLFunction.execQuery(queryTaatentionName);

            // tSectionId for form
            string queryTsectionId = "SELECT JobSourceID, JobSource FROM tbl_JobSource";
            ViewBag.tSectionId = SQLFunction.execQuery(queryTsectionId);

            // tDispatchBy for form
            string queryTdispatchType = "SELECT DispatchTypeID, DispatchType FROM tbl_DispatchType";
            ViewBag.tDispatchType = SQLFunction.execQuery(queryTdispatchType);

            // tDispatchBy for form
            string queryTdispatchBy = "SELECT DisplayName,Account, Department, EmailAddress FROM v_AddressBook";
            ViewBag.tDispatchBy = SQLFunction.execQuery(queryTdispatchBy);

            // tDispatchBy for form
            string queryChandledBy = "SELECT DisplayName,Account, Department, EmailAddress FROM v_AddressBook";
            ViewBag.cHandledBy = SQLFunction.execQuery(queryChandledBy);

            // cRecipientCategory for email form
            string queryCrecipientCategory = "select distinct EmailCategory from tbl_supplierListEmail  order by emailCategory asc";
            ViewBag.cRecipientCategory = SQLFunction.execQuery(queryCrecipientCategory);

            // cccEmailCategory for email form
            string queryCccEmailCategory = "select distinct EmailCategory from tbl_supplierListEmail where CCemail=1 order by emailCategory asc";
            ViewBag.cccEmailCategory = SQLFunction.execQuery(queryCccEmailCategory);
        }
        public IActionResult getRecipientEmailFromDropdown(string cRecipientCategory)
        {
            loadOption();
            var query = "SELECT SuppRecipientEmail FROM tbl_SupplierListEmail WHERE EmailCategory = " + Utility.Evar(cRecipientCategory, 1);
            var result = SQLFunction.execQuery(query);

            var emails = new List<string>();

            foreach (DataRow row in result.Rows)
            {
                var email = Utility.CheckNull(row["SuppRecipientEmail"]);
                emails.Add(email);
            }

            var concatenatedEmails = string.Join(",", emails);

            return new JsonResult(new { emails = concatenatedEmails });
        }
        public IActionResult PrintJsPdf(string id)
        {
            string query = $"SELECT * FROM tbl_DispatchJob WHERE ID = {Utility.Evar(id, 0)}";
            ViewBag.report = SQLFunction.execQuery(query);
            Console.WriteLine("query is" + query);

            string Detailquery = $"SELECT * FROM tbl_DispatchJobDetail WHERE ID = {Utility.Evar(id, 0)}";
            ViewBag.detail = SQLFunction.execQuery(Detailquery);

            return View("~/Views/JobDispatch/Report/JsPdf.cshtml");
        }
    }
}
