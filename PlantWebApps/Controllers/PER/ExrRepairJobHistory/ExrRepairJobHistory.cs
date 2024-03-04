using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantWebApps.Helper;
using System;
using System.Diagnostics;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing.Printing;
using System.Globalization;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.AspNetCore.StaticFiles;
using System.Data.SqlClient;

namespace PlantWebApps.Controllers.PER.ExrRepairJobHistory
{
    public class ExrRepairJobHistory : Controller
    {
        private string _tempfilter;
        [TempData]
        public String Msg { get; set; }
        [TempData]
        public String Stat { get; set; }
        public IActionResult Index()
        {
            loadoption();

			var filter = TempData.Peek("jobRegisterFilter");
			var tempData = LoadInitialData(filter);

			return View("~/Views/PER/ExrRepairJobHistory/Index.cshtml", tempData);
        }
        public FileResult ViewAttachment(string filepath)
        {
            string fileName = Path.GetFileName(filepath);

            var contentTypeProvider = new FileExtensionContentTypeProvider();

            if (!contentTypeProvider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(filepath, contentType, fileName);
        }
        public IActionResult DeleteDispatch(string id)
        {
            var eID = (Utility.Evar(id, 1));
            var eRegisterDate = (Utility.Evar(DateTime.Now.ToString(), 1));
            var eByName = (Utility.Evar((Utility.GetCurrentUsername().Split('\\')[1]), 1)); ;

            var query = $"UPDATE tbl_DispatchJobDetail SET StatusID = 'del', DeletedDate = {eRegisterDate},DeletedBy = {eByName}  Where JobID = {eID}";
            Console.WriteLine(query);

            SQLFunction.execQuery(query);

            return new JsonResult("ok");
        }
        public IActionResult checkUpload(string id)
        {
            string filetypeid = "1";

            string query = @$"select * from tbl_Exrjobattachment Where fileid={Utility.Evar(filetypeid, 0)}
            and jobid = {Utility.Evar(id, 0)} and active = 1";

            var data = SQLFunction.execQuery(query);
            if (data.Rows.Count > 0)
            {
                return new JsonResult("exist");
            }
            else
            {
                return new JsonResult("new");
            }
        }
        public async Task<IActionResult> UploadFile(IFormFile fileData, string id, string intWo)
        {
            Console.WriteLine("int wo is " + intWo);
            string strname = "ANForRepair";
            string fileName = "";
            var filePath = "";
            string fileExtension = "";
            var targetDirectory = "";
            string newPath = "";
            string eFilePath = "";
            string fileName1 = "";
            string dirname = @$"{Utility.Evar(id, 0)}\";
            string[] allowedExtensions = { ".png", ".PNG", ".jpeg", ".JPEG", ".jpg", ".JPG", ".gif", ".GIF" };

            if (fileData != null && fileData.Length > 0)
            {
                fileExtension = Path.GetExtension(fileData.FileName); // nama file dengan ekstensi
                fileName = Path.GetFileNameWithoutExtension(fileData.FileName); // Mengambil nama file tanpa ekstensi
                fileName = fileName.Replace(" ", "");
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return Json(new { edirectToUrl = "/ExrRepairJobHistory/Edit/" + id, Massage = "cancel" });
                }

                fileName1 = $"{fileName}{fileExtension}";
                string enewname = $"{Utility.Evar(id, 0)}_1_{strname}{intWo}.{fileName1}";

                targetDirectory = $@"{dirname}";
                //targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), "image"); // local

                eFilePath = Path.Combine(targetDirectory, enewname);

                Console.WriteLine("upload file path" + eFilePath);

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                using (var stream = new FileStream(eFilePath, FileMode.Create))
                {
                    await fileData.CopyToAsync(stream);
                }

                string query = @$"exec dbo.EXRAttachmentUpdate {Utility.Evar(id, 0)}, 
			    1, {Utility.Evar(eFilePath, 1)}, {Utility.Evar(fileName1, 1)}, NULL, {Utility.ebyname()}";

                Console.WriteLine(query);

                SQLFunction.execQuery(query);

                string queryListAttachment = $@"select id,AttachmentType,FilePath,FullPath from v_ExrJobAttachment where JobID={id}";
                var listData = SQLFunction.execQuery(queryListAttachment);

                var rows = new List<object>();

                foreach (DataRow row in listData.Rows)
                {
                    var rowData = new
                    {
                        button = $@"<button type='button' class='btn btn-link btn-sm' onclick='viewAttachment(""{row["FullPath"]}"")'>
                                        <svg xmlns='http://www.w3.org/2000/svg' width='16' height='16' fill='currentColor' class='bi bi-file-earmark-arrow-down-fill' viewBox='0 0 16 16'>
                                            <path d='M9.293 0H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V4.707A1 1 0 0 0 13.707 4L10 .293A1 1 0 0 0 9.293 0M9.5 3.5v-2l3 3h-2a1 1 0 0 1-1-1m-1 4v3.793l1.146-1.147a.5.5 0 0 1 .708.708l-2 2a.5.5 0 0 1-.708 0l-2-2a.5.5 0 0 1 .708-.708L7.5 11.293V7.5a.5.5 0 0 1 1 0' />
                                        </svg>        
                                   </button>",
                        id = Utility.CheckNull(row["ID"]),
                        attachmenttype = Utility.CheckNull(row["AttachmentType"]),
                        filepath = Utility.CheckNull(row["FilePath"]),
                        fullpath = Utility.CheckNull(row["FullPath"]),
                    };
                    rows.Add(rowData);
                }
                return Json(new { Massage = "success", data = rows });
            }
            else
            {
                return Json(new { Massage = "not" });
            }
        }
        public IActionResult CfindWo(string OffSiteWO)
        {
            string query = $"SELECT ID,OffSiteWO as ParentWO,ChildWO as WOOffSite,StatusID from tbl_ExrJobDetail WHERE OffSiteWO= '{OffSiteWO}'";
            var data = SQLFunction.execQuery(query);
            if (data.Rows.Count > 1)
            {
                var rows = new List<object>();

                foreach (DataRow row in data.Rows)
                {
                    var rowData = new
                    {
                        select = "<a class='btn btn-primary btn-sm' href='/ExrRepairJobHistory/Edit/" + row["id"] + "'>Select</a>",
                        id = Utility.CheckNull(row["ID"]),
                        parentWo = Utility.CheckNull(row["ParentWO"]),
                        woOffsite = Utility.CheckNull(row["WOOffSite"]),
                        statusId = Utility.CheckNull(row["StatusID"])
                    };
                    rows.Add(rowData);
                }
                return new JsonResult(new { rows, Message = "DoubleWo" });
            }
            else
            {
                var id = data.Rows[0]["ID"].ToString();
                Console.WriteLine(id);
                return Json(new { redirectToUrl = "/ExrRepairJobHistory/Edit/" + id });
            }
        }
        public IActionResult CfindIntWO(string eoffsitewo, string eWOJobCost)
        {
            Console.WriteLine("eoffsitewo" + eoffsitewo);
            Console.WriteLine("eWOJobCost" + eWOJobCost);

            string queryParentWO = $"SELECT ParentWO , Section, WONO as IntWO, WoDesc, Qty as CompQty From v_WorkOrderJobDetail where ParentWO = '{eoffsitewo}' ORDER BY WONO";
            Console.WriteLine(queryParentWO);
            var isParentWo = SQLFunction.execQuery(queryParentWO);

            if (isParentWo.Rows.Count > 0)
            {
                var rows = new List<object>();

                foreach (DataRow row in isParentWo.Rows)
                {
                    var rowData = new
                    {
                        section = Utility.CheckNull(row["Section"]),
                        intWo = Utility.CheckNull(row["IntWO"]),
                        woDesc = Utility.CheckNull(row["WoDesc"]),
                        compQty = Utility.CheckNull(row["CompQty"])
                    };
                    rows.Add(rowData);
                }
                return new JsonResult(rows);
            }
            else
            {
                string queryJobCost = $"SELECT ParentWO , Section, WONO as IntWO, WoDesc, Qty as CompQty From v_WorkOrderJobDetail where ParentWO = '{eWOJobCost}' ORDER BY WONO";
                Console.WriteLine(queryJobCost);
                var isJobCost = SQLFunction.execQuery(queryJobCost);

                var rows = new List<object>();

                foreach (DataRow row in isJobCost.Rows)
                {
                    var rowData = new
                    {
                        section = Utility.CheckNull(row["Section"]),
                        intWo = Utility.CheckNull(row["IntWO"]),
                        woDesc = Utility.CheckNull(row["WoDesc"]),
                        compQty = Utility.CheckNull(row["CompQty"])
                    };
                    rows.Add(rowData);
                }
                return new JsonResult(rows);
            }
        }
        public IActionResult saveCompIDAddJob(string tciPartNo, string tciPartID, string equipClass, string stockItemNo,
        string partDesc1, string partDesc2, string remark, string serialNumber, string arrNumber)
        {
            Console.WriteLine(tciPartID);
            var eTciPartNo = Utility.Evar(tciPartNo, 1);
            var eTciPartID = Utility.Evar(tciPartID, 1);
            var eEquipClass = Utility.Evar(equipClass, 1);
            var eStockItemNo = Utility.Evar(stockItemNo, 1);
            var ePartDesc1 = Utility.Evar(partDesc1, 1);
            var ePartDesc2 = Utility.Evar(partDesc2, 1);
            var eRemark = Utility.Evar(remark, 1);
            var eSerialNumber = Utility.Evar(serialNumber, 1);
            var eArrNumber = Utility.Evar(arrNumber, 1);
            var eStatusID = 0;
            var etciid = Utility.Evar("", 1); ;
            var eMaintType = Utility.Evar("", 1); ;

            string queryCheck = $"SELECT * FROM tbl_ExrComponentID where TCIPartID={eTciPartID}";
            Console.WriteLine(queryCheck);
            var isExist = SQLFunction.execQuery(queryCheck);

            if (isExist.Rows.Count > 0)
            {
                if (tciPartNo == isExist.Rows[0]["TCIPartNo"].ToString())
                {
                    return new JsonResult("already");
                }
                else
                {
                    string query = @$"INSERT INTO tbl_ExrComponentID (TCIPartID,Remark,StatusID,SerialNumber
                    ,ArrNumber,TCIPartNo,StockItemNo,EquipClass,TCIID,MaintType) VALUES ({eTciPartID}, {eRemark},
                    {eStatusID}, {eSerialNumber}, {eArrNumber}, {eTciPartNo}, {eStockItemNo}, {eEquipClass},
                    {etciid}, {eMaintType})";

                    Console.WriteLine(query);
                    return new JsonResult("true");
                }
            }
            else
            {
                return new JsonResult("not");
            }
        }
        public IActionResult Cfind(string TTCIPartNo)
        {
            string query = $"Select * from v_PartNoDetail Where TCIPartno={Utility.Evar(TTCIPartNo, 1)}";
            var data = SQLFunction.execQuery(query);

            if (data.Rows.Count > 0)
            {
                var rows = new List<object>();

                foreach (DataRow row in data.Rows)
                {
                    var rowData = new
                    {
                        PartDesc1 = Utility.CheckNull(row["Description1"]),
                        PartDesc2 = Utility.CheckNull(row["Description2"]),
                        RoutableStock = Utility.CheckNull(row["RoutableStock"]),
                        MeterRun = Utility.CheckNull(row["MeterRun"]),
                        MeterToRun = Utility.CheckNull(row["MeterToRun"])
                    };
                    rows.Add(rowData);
                }
                return new JsonResult(rows);
            }
            else
            {
                return new JsonResult("not found");
            }
        }
        public IActionResult TSave(string eID, string eAddCost, string eChildWO, string eCompDesc, string eCompQty,
            string eCompTypeID, string eEquipClass, string eCostAfter, string eCostBefore, string eCostRepair, string eCurrID,
            string eDeliveryDate, string eIssuedBy, string eDestination, string eDisputeCompletedBy,
            string eDisputeCompletedBy2, string eDispCompDate, string eDComplDispDate, string eDnNumber,
            string eptYDocAvb, string eST, string eOt, string eDocDate, string eDocTypeID, string eEGEI,
            string eEIEK, string eEKEP, string eULL, string eetadate, string eFitToUnit, string eIntWO,
            string eIntWOPrevious, string eJobNo, string eSerialNumberOEM, string eTCICoreFitTo, string eUnitNumber,
            string eExtWO, string eLocationHold, string eLogANReceivedBy, string eLogANReceivedDate, string eLogANReceivedNo,
            string eLogANSentDate, string eMaintType, string eMeterRun, string eMeterToRun, string eOemCost,
            string eORRequestDate, string eORCompletedDAte, string eORRRNo, string eOPRRNo, string eOffSiteWO, string eOPDate,
            string eOPNo, string eOPPrevious, string eORNo, string eQuoteApprovedBy, string eQuoteDate, string eQuoteNo,
            string eQuotePrintBy, string eQuoteProcessBy, string eQuoteRevDate, string eQuoteRevNo, string eReasonType,
            string eReceivedBy, string eReceivedByName, string eReceivedDate, string eremark, string eRepairTypeID,
            string eReqStand, string eReqPart, string eRequestP1, string eRTSCost, string esavingcostCatID, string eWOFitToUnitID,
            string eSitetoLogAN, string eSitetoLogDate, string eNextStatus, string eStoreID, string eSTOtNo,
            string eSpvID, string eSuppForRepairAN, string eRepairByID, string eSuppReceiveANBy, string eSuppReceiveANDate,
            string eTaggingBy, string eDTaggingDate, string eTCICost, string eTCIPartID, string eTCIPartNo, string eUnitNo,
            string ebWarrantyResult, string eWCSDate, string eWOFitToUnit, string eWoJobCost, string eWOPrevious,
            string eDecisionDate, string eEmailDate, string eRepairAdvice, string eSuggestedStore, string eRemarkAdvice,
            string eSiteAlloc, string eWOAlloc, string eUnitAlloc, string eSchedStartAlloc, string eSOHAlloc, string eOutReqAlloc,
            string eDisputeCompletedDate, string eDisputeCompletedDate2, string estatusid, string esupplierid, string eDocAvb,
            string edata, string eHoldUntil, string ejobid, string eCurrentStatus, string eApp1, string eApp1By, string eApp1Date,
            string eApp2, string eApp2By, string eApp2Date, string eApp3, string eApp3By, string eApp3Date, string eAppSentDate,
            string eJobID, string eBuyerName, string tcApp1, string tcApp2, string tcApp3, string eAddOrder1, string eAddOrder2,
            string eAddOrder3, string eAddOrderORNo1, string eAddOrderORNo2, string eAddOrderORNo3, string eaddOrderDate1,
            string eaddOrderDate2, string eaddOrderDate3, string eaddOrderDNNo1, string eaddOrderDNNo2, string eaddOrderDNNo3)
        {
            var xID = Utility.Evar(eID, 0);
            var xOffSiteWO = Utility.Evar(eOffSiteWO, 1);
            var xCompQty = Utility.Evar(eCompQty, 0);
            var xDocTypeID = Utility.Evar(eDocTypeID, 0);
            var xDocDate = Utility.Evar(eDocDate, 2);
            var xTCIPartNo = Utility.Evar(eTCIPartNo, 1);
            var xEquipClass = Utility.Evar(eEquipClass, 1);
            var xUnitNumber = Utility.Evar(eUnitNumber, 1);
            var xLogANReceivedDate = Utility.Evar(eLogANReceivedDate, 2);
            var xLogANSentDate = Utility.Evar(eLogANSentDate, 2);
            var xLogANReceivedBy = Utility.Evar(eLogANReceivedBy, 1);
            var xLogANReceivedNo = Utility.Evar(eLogANReceivedNo, 1);
            var xSitetoLogAN = Utility.Evar(eSitetoLogAN, 1);
            var xSitetoLogDate = Utility.Evar(eSitetoLogDate, 2);
            var xCompTypeID = Utility.Evar(eCompTypeID, 0);
            var xCompDesc = Utility.Evar(eCompDesc, 1);
            var xTCIPartID = Utility.Evar(eTCIPartID, 1);
            var xOPNo = Utility.Evar(eOPNo, 1);
            var xOPDate = Utility.Evar(eOPDate, 2);
            var xORNo = Utility.Evar(eORNo, 1);
            var xMeterRun = Utility.Evar(eMeterRun, 0);
            var xMeterToRun = Utility.Evar(eMeterToRun, 0);
            var xDnNumber = Utility.Evar(eDnNumber, 1);
            var xDTaggingDate = Utility.Evar(eDTaggingDate, 2);
            var xTaggingBy = Utility.Evar(eTaggingBy, 1);
            var xStoreID = Utility.Evar(eStoreID, 1);
            var xSTOtNo = Utility.Evar(eSTOtNo, 1);
            var xFitToUnit = Utility.Evar(eFitToUnit, 1);
            var xWOFitToUnit = Utility.Evar(eWOFitToUnit, 1);
            var xDestination = Utility.Evar(eDestination, 1);
            var xDeliveryDate = Utility.Evar(eDeliveryDate, 2);
            var xIssuedBy = Utility.Evar(eIssuedBy, 1);
            var xDisputeCompletedDate = Utility.Evar(eDisputeCompletedDate, 2);
            var xDisputeCompletedBy = Utility.Evar(eDisputeCompletedBy, 1);
            var xDisputeCompletedDate2 = Utility.Evar(eDisputeCompletedDate2, 2);
            var xDisputeCompletedBy2 = Utility.Evar(eDisputeCompletedBy2, 1);
            var xSuppForRepairAN = Utility.Evar(eSuppForRepairAN, 1);
            var xSuppReceiveANDate = Utility.Evar(eSuppReceiveANDate, 2);
            var xSuppReceiveANBy = Utility.Evar(eSuppReceiveANBy, 1);
            var xRepairTypeID = Utility.Evar(eRepairTypeID, 0);
            var xReceivedDate = Utility.Evar(eReceivedDate, 1);
            var xReceivedBy = Utility.Evar(eReceivedBy, 1);
            var xReceivedByName = Utility.Evar(eReceivedByName, 1);
            var xQuoteRevDate = Utility.Evar(eQuoteRevDate, 1);
            var xQuoteRevNo = Utility.Evar(eQuoteRevNo, 1);
            var xReasonTypeID = Utility.Evar(eReasonType, 0);
            var xQuoteNo = Utility.Evar(eQuoteNo, 1);
            var xQuoteDate = Utility.Evar(eQuoteDate, 1);
            var xJobNo = Utility.Evar(eJobNo, 1);
            var xSerialNumberOEM = Utility.Evar(eSerialNumberOEM, 1);
            var xEGEI = Utility.Evar(eEGEI, 1);
            var xQuotePrintBy = Utility.Evar(eQuotePrintBy, 1);
            var xEIEK = Utility.Evar(eEIEK, 1);
            var xQuoteApprovedBy = Utility.Evar(eQuoteApprovedBy, 1);
            var xEKEP = Utility.Evar(eEKEP, 1);
            var xQuoteProcessBy = Utility.Evar(eQuoteProcessBy, 1);
            var xCostBefore = Utility.Evar(eCostBefore, 0);
            var xCostAfter = Utility.Evar(eCostAfter, 0);
            var xCurrID = Utility.Evar(eCurrID, 1);
            var xAddCost = Utility.Evar(eAddCost, 1);
            var xstatusid = Utility.Evar(estatusid, 1);
            var xRepairAdvice = Utility.Evar(eRepairAdvice, 1);
            var xremark = eremark;
            var xetadate = Utility.Evar(eetadate, 2);
            var xsupplierid = Utility.Evar(esupplierid, 0);
            var xsupervisorid = Utility.Evar(eSpvID, 0);
            var xChildWO = Utility.Evar(eChildWO, 1);
            var xmainttype = Utility.Evar(eMaintType, 1);
            var xIntWO = Utility.Evar(eIntWO, 1);
            var xReqStand = Utility.Evar(eReqStand, 0);
            var xReqPart = Utility.Evar(eReqPart, 0);
            var xWOJobCost = Utility.Evar(eWoJobCost, 1);
            var xWCSDate = Utility.Evar(eWCSDate, 2);
            var xWarrantyResult = Utility.Evar(ebWarrantyResult, 1);
            var xCostRepair = Utility.Evar(eCostRepair, 1);
            var xOemCost = Utility.Evar(eOemCost, 1);
            var xORRequestDate = Utility.Evar(eORRequestDate, 2);
            var xORCompletedDAte = Utility.Evar(eORCompletedDAte, 2);
            var xORRRNo = Utility.Evar(eORRRNo, 1);
            var xOPRRNo = Utility.Evar(eOPRRNo, 1);
            var xRequestP1 = Utility.Evar(eRequestP1, 1);
            var xLocationHold = Utility.Evar(eLocationHold, 1);
            var xTCICoreFitTo = Utility.Evar(eTCICoreFitTo, 0);
            var xExtWO = Utility.Evar(eExtWO, 0);
            var xDocAvb = Utility.Evar(eDocAvb, 0);
            var xST = Utility.Evar(eST, 0);
            var xOt = Utility.Evar(eOt, 0);
            var xsavingcostCatID = Utility.Evar(esavingcostCatID, 0);
            var xTCICost = Utility.Evar(eTCICost, 1);
            var xRTSCost = Utility.Evar(eRTSCost, 0);
            var xWOPrevious = Utility.Evar(eWOPrevious, 1);
            var xOPPrevious = Utility.Evar(eOPPrevious, 1);
            var xIntWOPrevious = Utility.Evar(eIntWOPrevious, 1);
            var xDecisionDate = Utility.Evar(eDecisionDate, 2);
            var xEmailDate = Utility.Evar(eEmailDate, 2);
            var xHoldUntil = Utility.Evar(eHoldUntil, 1);
            var xdata = Utility.Evar(edata, 1);
            var xjobid = Utility.Evar(ejobid, 0);
            var xSuggestedStore = Utility.Evar(eSuggestedStore, 1);
            var xRemarkAdvice = Utility.Evar(eRemarkAdvice, 1);
            var xSiteAlloc = Utility.Evar(eSiteAlloc, 1);
            var xWOAlloc = Utility.Evar(eWOAlloc, 1);
            var xSchedStartAlloc = Utility.Evar(eSchedStartAlloc, 1);
            var xSOHAlloc = Utility.Evar(eSOHAlloc, 1);
            var xUnitAlloc = Utility.Evar(eUnitAlloc, 1);
            var xOutReqAlloc = Utility.Evar(eOutReqAlloc, 1);
            var xCurrentStatus = Utility.Evar(eCurrentStatus, 1);
            var xApp1 = Utility.Evar(eApp1, 1);
            var xApp1By = Utility.Evar(eApp1By, 1);
            var xApp1Date = Utility.Evar(eApp1Date, 2);
            var xApp2 = Utility.Evar(eApp2, 1);
            var xApp2By = Utility.Evar(eApp2By, 1);
            var xApp2Date = Utility.Evar(eApp2Date, 2);
            var xApp3 = Utility.Evar(eApp3, 1);
            var xApp3By = Utility.Evar(eApp3By, 1);
            var xApp3Date = Utility.Evar(eApp3Date, 2);
            var xAppSentDate = Utility.Evar(eAppSentDate, 2);
            var xJobID = Utility.Evar(eJobID, 1);
            var xBuyerName = Utility.Evar(eBuyerName, 0);
            var xcApp1 = Utility.Evar(tcApp1, 1);
            var xcApp2 = Utility.Evar(tcApp2, 1);
            var xcApp3 = Utility.Evar(tcApp3, 1);
            var xAddOrder1 = Utility.Evar(eAddOrder1, 1);
            var xAddOrder2 = Utility.Evar(eAddOrder2, 1);
            var xAddOrder3 = Utility.Evar(eAddOrder3, 2);
            var xAddOrderORNo1 = Utility.Evar(eAddOrderORNo1, 1);
            var xAddOrderORNo2 = Utility.Evar(eAddOrderORNo2, 1);
            var xAddOrderORNo3 = Utility.Evar(eAddOrderORNo3, 1);
            var xaddOrderDate1 = Utility.Evar(eaddOrderDate1, 2);
            var xaddOrderDate2 = Utility.Evar(eaddOrderDate2, 2);
            var xaddOrderDate3 = Utility.Evar(eaddOrderDate3, 2);
            var xaddOrderDNNo1 = Utility.Evar(eaddOrderDNNo1, 1);
            var xaddOrderDNNo2 = Utility.Evar(eaddOrderDNNo2, 1);
            var xaddOrderDNNo3 = Utility.Evar(eaddOrderDNNo3, 1);
            var xRegisterDate = Utility.Evar(DateTime.Now.ToString("yyyy-MM-dd"), 2);

            string queryJobRegisterRev1 = @$"exec dbo.ExrJobRegisterRev1 {xID}, {xOffSiteWO}, {xCompQty}, {xDocTypeID}, {xDocDate}, {xTCIPartNo}
            , {xEquipClass}, {xUnitNumber}, {xLogANReceivedDate}, {xLogANSentDate}, {xLogANReceivedBy}, {xLogANReceivedNo}
            , {xSitetoLogAN}, {xSitetoLogDate}, {xCompTypeID}, {xCompDesc}, {xTCIPartID}, {xOPNo}, {xOPDate}, {xORNo}, {xMeterRun}
            , {xMeterToRun}, {xDnNumber}, {xDTaggingDate}, {xTaggingBy}, {xStoreID}, {xSTOtNo}, {xFitToUnit}
            , {xWOFitToUnit}, {xDestination}, {xDeliveryDate}, {xIssuedBy}, {xDisputeCompletedDate}, {xDisputeCompletedBy}
            , {xDisputeCompletedDate2}, {xDisputeCompletedBy2}, {xSuppForRepairAN}, {xSuppReceiveANDate}, {xSuppReceiveANBy}
            , {xRepairTypeID}, {xReceivedDate}, {xReceivedBy}, {xReceivedByName}, {xQuoteRevDate}, {xQuoteRevNo}, {xReasonTypeID}
            , {xQuoteNo}, {xQuoteDate}, {xJobNo}, {xSerialNumberOEM}, {xEGEI}, {xQuotePrintBy}, {xEIEK}, {xQuoteApprovedBy}
            , {xEKEP}, {xQuoteProcessBy}, {xCostBefore}, {xCostAfter}
            , {xCurrID}, {xAddCost}, {xstatusid}, {xRepairAdvice}, '{xremark}', {xetadate}, {xsupplierid} 
            , {xsupervisorid}, {xChildWO}, {xmainttype}, {xIntWO}, {xReqStand}, {xReqPart}, {xWOJobCost}, {xWCSDate}
            , {xWarrantyResult}, {xCostRepair}, {xOemCost}, {xORRequestDate}, {xORCompletedDAte},{xORRRNo}, {xOPRRNo}
            , {xRequestP1}, {xLocationHold}, {xTCICoreFitTo}, {xExtWO}, {xDocAvb}, {xST}, {xOt}, {xsavingcostCatID}
            , {xTCICost}, {xRTSCost}, {xWOPrevious}, {xOPPrevious}, {xIntWOPrevious}, {xDecisionDate}, {xEmailDate}, {xHoldUntil}
            , {xdata}, {Utility.ebyname(),1}";

            Console.WriteLine("save 1" + queryJobRegisterRev1);

            //SQLFunction.execQuery(queryJobRegisterRev1);

            string queryAdviceUpdate = $@"exec dbo.ExrRepairAdviceUpdate {xjobid}, {xSuggestedStore}, {xRemarkAdvice},
			{xSiteAlloc}, {xWOAlloc}, {xSchedStartAlloc}, {xSOHAlloc}, {xUnitAlloc}, {xOutReqAlloc}, {xRequestP1},
            {xRepairAdvice}, {Utility.ebyname(),1}";
            //SQLFunction.execQuery(queryAdviceUpdate);
            Console.WriteLine("save 2" + queryAdviceUpdate);

            if (eCurrentStatus == "OH" || eRepairAdvice == "OH")
            {
                string queryOnHold = $"exec dbo.EXRJobOnHold {xjobid}, 'OH', {xHoldUntil}";
                Console.WriteLine("save 1 if" + queryOnHold);
                //SQLFunction.execQuery(queryOnHold);
            }

            string queryJob2Add = @$"exec dbo.EXRJob2Add {xJobID}, {xApp1}, {xApp1By}, {xApp1Date}, {xApp2}, {xApp2By}, {xApp2Date},
            {xApp3}, {xApp3By}, {xApp3Date}, {xBuyerName}, {xAppSentDate}, {xAddOrder1}, {xAddOrder2}, {xAddOrder3},
            {xAddOrderORNo1}, {xAddOrderORNo2}, {xAddOrderORNo3}, {xaddOrderDate1}, {xAddOrder2}, {xaddOrderDate3},
            {xaddOrderDNNo1}, {xaddOrderDNNo2}, {xaddOrderDNNo3}, {xdata}, {xdata}, {xdata}, {Utility.ebyname()}";

            //SQLFunction.execQuery(queryJob2Add);
            Console.WriteLine("save 3" + queryJob2Add);

            if (!string.IsNullOrEmpty(eTCIPartID) && !string.IsNullOrEmpty(eID))
            {
                string queryUpdateCompId = $"UPDATE tbl_ExrComponentID SET JobID={xID} WHERE TCIPartID={xTCIPartID}";
                //SQLFunction.execQuery(queryUpdateCompId);
                Console.WriteLine("save 2 if" + queryUpdateCompId);
            }

            string queryPDFID = $"select PRFID from tbl_ExrJobDetail where ID={xID} AND PRFID is Not Null";
            Console.WriteLine("SAVE 4" + queryPDFID);
            var ePDFID = SQLFunction.execQuery(queryPDFID);

            if (ePDFID.Rows.Count > 0)
            {
                string pdfID = ePDFID.Rows[0]["PRFID"].ToString();
                string queryUpdatePartRequest = $"UPDATE tbl_PartRequest SET WOBin={xWOJobCost} WHERE PRFID={Utility.Evar(pdfID, 0)}";
                //SQLFunction.execQuery(queryUpdatePartRequest);
                Console.WriteLine("save 3 if" + queryUpdatePartRequest);
            }

            if (!string.IsNullOrEmpty(eWOFitToUnit) || !string.IsNullOrEmpty(eWOFitToUnitID))
            {
                string queryInsertJobDetail = @$"INSERT INTO tbl_ExrjobDetail(OffSiteWO,StatusID,UnitNumber,
                CompDesc,RegisterDate,RegisterBy) VALUES ({xWOFitToUnit}, 2, {xFitToUnit}, {xCompDesc}, {xRegisterDate},
                {Utility.ebyname()})";
                Console.WriteLine("save 4 if" + queryInsertJobDetail);
                //SQLFunction.execQuery(queryInsertJobDetail);
            }

            return Json(new { redirectToUrl = "/ExrRepairJobHistory/Edit/" + eID });
        }
        private string BuildTempFilter(string repairType, string compType, string statusInput, string supervisorId,
            string supplierId, string cwoType, string cwoTypeValue, string fdocType,
            string fdocTypeValue, string ccompIdType, string ccompIdValue, string sDate, string startDate,
            string endDate, string lmodBy, string lmodByValue, string reasonTypeId,
            string freasonType, string freasonValue, string cbDelay, string cbDelayValue,
            string repairAdvice, string toCatDesc, string requestP1, string fissNull,
            string pCam, string sortBy, string ascDesc, string unitnumber)
        {
            string tempfilter = string.Empty;

            var sortByValue = FilterHelper.SelectSortBy(sortBy);
            tempfilter = ApplySortCategory(sortByValue, tempfilter);

            var cwoCategory = FilterHelper.SelectCwoTypeFilter(cwoType);
            tempfilter = ApplyFilterCategory(cwoCategory, cwoTypeValue, tempfilter);

            var fdocTypeCategory = FilterHelper.SelectFdocTypeFilter(fdocType);
            tempfilter = ApplyFilterCategory(fdocTypeCategory, fdocTypeValue, tempfilter);

            var ccompIdTypeCategory = FilterHelper.SelectCCompIdTypeFilter(ccompIdType);
            tempfilter = ApplyFilterCategory(ccompIdTypeCategory, ccompIdValue, tempfilter);

            var lmodByCategory = FilterHelper.SelectImodByFilter(lmodBy);
            tempfilter = ApplyFilterCategory(lmodByCategory, lmodByValue, tempfilter);

            var fReasonCategory = FilterHelper.SelectFreasonFilter(freasonType);
            tempfilter = ApplyFilterCategory(fReasonCategory, freasonValue, tempfilter);

            var cbDelayCategory = FilterHelper.SelectCbDelay(cbDelay);
            tempfilter = ApplyCbDelayCategory(cbDelayCategory, cbDelayValue, tempfilter);

            var fisNullValue = FilterHelper.SelectFisNull(fissNull);
            tempfilter = ApplyFisNullCategory(fisNullValue, tempfilter);

            Dictionary<string, string> formFields = new Dictionary<string, string>
                {
                    { "repairtypeid", repairType },
                    { "comptype", compType },
                    { "supervisorid", supervisorId },
                    { "supplierid", supplierId },
                    { "unitnumber", unitnumber },
                    { "reasontypeid", reasonTypeId },
                    { "repairadvice", repairAdvice },
                    { "tocatdesc", toCatDesc },
                    { "RequestP1", requestP1 }
                };

            if (!string.IsNullOrEmpty(pCam))
            {
                tempfilter = " and PCAMStatusID = " + Utility.Evar(pCam, 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(statusInput))
            {
                tempfilter = " and status in (" + Utility.Evar(statusInput, 1) + ")" + tempfilter;
                ViewBag.statusValue = statusInput;
            }

            string strdate;
            switch (sDate)
            {
                case "1":
                    strdate = "ModDate";
                    break;
                case "2":
                    strdate = "CompletedDate";
                    break;
                default:
                    strdate = "ModDate";
                    break;
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                tempfilter = " and " + strdate + " >= " + Utility.Evar(startDate, 2) + tempfilter;
                ViewBag.startdate = startDate;
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                tempfilter = " and " + strdate + " <= " + Utility.Evar(endDate, 2) + tempfilter;
                ViewBag.endate = endDate;
            }

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
        public IActionResult LoadData(
            string repairType, string compType, string statusInput, string supervisorId,
            string supplierId, string cwoType, string cwoTypeValue, string fdocType,
            string fdocTypeValue, string ccompIdType, string ccompIdValue, string sDate, string startDate,
            string endDate, string lmodBy, string lmodByValue, string reasonTypeId,
            string freasonType, string freasonValue, string cbDelay, string cbDelayValue,
            string repairAdvice, string toCatDesc, string requestP1, string fissNull,
            string pCam, string sortBy, string ascDesc, string unitnumber)
        {
            loadoption();

            string filter = BuildTempFilter(repairType, compType, statusInput, supervisorId, supplierId, cwoType, cwoTypeValue, fdocType,
                     fdocTypeValue, ccompIdType, ccompIdValue, sDate, startDate, endDate, lmodBy, lmodByValue, reasonTypeId,
                     freasonType, freasonValue, cbDelay, cbDelayValue, repairAdvice, toCatDesc, requestP1, fissNull, pCam,
                     sortBy, ascDesc, unitnumber);

            var sort = ascDesc;
            string sortOrder = string.IsNullOrEmpty(sort) ? "desc" : sort;
            string status = "AND StatusID != '9'";

            string efilter = $"{filter} {status}";

            _tempfilter = Utility.VarFilter(efilter);
            TempData["jobRegisterFilter"] = _tempfilter;

			string dataQuery = $"SELECT TOP 50 * FROM v_ExrJobDetailRev1 {_tempfilter} {status}  order by id {sortOrder}";
            Console.WriteLine("Load Data " + dataQuery);
            var data = SQLFunction.execQuery(dataQuery);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                    totalAgeWO = Utility.CheckNull(row["TotalAgeWO"]),
                    unitNumber = Utility.CheckNull(row["AgeWaitingQuote"]),
                    offSiteWO = Utility.CheckNull(row["OffSiteWO"]),
                    woAlloc = Utility.CheckNull(row["WoAlloc"]),
                    siteAllocName = Utility.CheckNull(row["SiteAllocName"]),
                    woJobCost = Utility.CheckNull(row["WOJobCost"]),
                    logANReceivedDate = Utility.CheckNull(row["LogANReceivedDate"]),
                    logANSentDate = Utility.CheckNull(row["LogANSentDate"]),
                    status = Utility.CheckNull(row["Status"]),
                    compDesc = Utility.CheckNull(row["CompDesc"]),
                    compQty = Utility.CheckNull(row["CompQty"]),
                    compType = Utility.CheckNull(row["CompType"]),
                    repairAdvice = Utility.CheckNull(row["RepairAdvice"]),
                    pcamStatusID = Utility.CheckNull(row["PCAMStatusID"]),
                    tciPartNo = Utility.CheckNull(row["TCIPartNo"]),
                    supervisorAbbr = Utility.CheckNull(row["SupervisorAbbr"]),
                    supplierName = Utility.CheckNull(row["SupplierName"]),
                    intWO = Utility.CheckNull(row["IntWO"]),
                    suppForRepairANNo = Utility.CheckNull(row["SuppForRepairANNo"]),
                    quoteNo = Utility.CheckNull(row["QuoteNo"]),
                    jobNo = Utility.CheckNull(row["JobNo"]),
                    quoteDate = Utility.CheckNull(row["QuoteDate"]),
                    orNo = Utility.CheckNull(row["ORNo"]),
                    opDate = Utility.CheckNull(row["OPDate"]),
                    receivedDate = Utility.CheckNull(row["ReceivedDate"]),
                    edit = "<a class='btn btn-link btn-sm' href='/ExrRepairJobHistory/Edit/" + row["id"] + "' @isEdit><i class='fa fa-edit'></i></a>",
                    delete = $@"<button type='button' class='btn btn-link btn-sm' id='btnDeleteDetail' onclick='confirmDelete({row["id"]})' @isEdit><i class='fa fa-trash text-danger'></i></button>"
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }
		public List<object> LoadInitialData(object filter)
		{

			string query = $"SELECT TOP 50 * FROM v_ExrJobDetailRev1 {filter} order by id desc";
			Console.WriteLine(query);
			var data = SQLFunction.execQuery(query);

			var rows = new List<object>();

			foreach (DataRow row in data.Rows)
			{
				var rowData = new
				{
					id = Utility.CheckNull(row["ID"]),
					totalAgeWO = Utility.CheckNull(row["TotalAgeWO"]),
					unitNumber = Utility.CheckNull(row["AgeWaitingQuote"]),
					offSiteWO = Utility.CheckNull(row["OffSiteWO"]),
					woAlloc = Utility.CheckNull(row["WoAlloc"]),
					siteAllocName = Utility.CheckNull(row["SiteAllocName"]),
					woJobCost = Utility.CheckNull(row["WOJobCost"]),
					logANReceivedDate = Utility.CheckNull(row["LogANReceivedDate"]),
					logANSentDate = Utility.CheckNull(row["LogANSentDate"]),
					status = Utility.CheckNull(row["Status"]),
					compDesc = Utility.CheckNull(row["CompDesc"]),
					compQty = Utility.CheckNull(row["CompQty"]),
					compType = Utility.CheckNull(row["CompType"]),
					repairAdvice = Utility.CheckNull(row["RepairAdvice"]),
					pcamStatusID = Utility.CheckNull(row["PCAMStatusID"]),
					tciPartNo = Utility.CheckNull(row["TCIPartNo"]),
					supervisorAbbr = Utility.CheckNull(row["SupervisorAbbr"]),
					supplierName = Utility.CheckNull(row["SupplierName"]),
					intWO = Utility.CheckNull(row["IntWO"]),
					suppForRepairANNo = Utility.CheckNull(row["SuppForRepairANNo"]),
					quoteNo = Utility.CheckNull(row["QuoteNo"]),
					jobNo = Utility.CheckNull(row["JobNo"]),
					quoteDate = Utility.CheckNull(row["QuoteDate"]),
					orNo = Utility.CheckNull(row["ORNo"]),
					opDate = Utility.CheckNull(row["OPDate"]),
					receivedDate = Utility.CheckNull(row["ReceivedDate"]),
					edit = "<a class='btn btn-link btn-sm' href='/ExrRepairJobHistory/Edit/" + row["id"] + "' @isEdit><i class='fa fa-edit'></i></a>",
					delete = $@"<button type='button' class='btn btn-link btn-sm' id='btnDeleteDetail' onclick='confirmDelete({row["id"]})' @isEdit><i class='fa fa-trash text-danger'></i></button>"
				};
				rows.Add(rowData);
			}
			return rows;
		}
		public IActionResult Export()
        {
            var cReportTypeQuery = "";
            string tempfilter = string.Empty;

            loadoption();

            var creportType = Request.Form["creportype"];
            var repairType = Request.Form["repairtypeid"];
            var compType = Request.Form["comptype"];
            var statusInput = Request.Form["statusInput"];
            var supervisorId = Request.Form["supervisorid"];
            var supplierId = Request.Form["supplierid"];
            var cwoType = Request.Form["cwotype"];
            var cwoTypeValue = Request.Form["cwotypevalue"];
            var fdocType = Request.Form["fdoctype"];
            var fdocTypeValue = Request.Form["fdoctypevalue"];
            var ccompIdType = Request.Form["ccompidtype"];
            var ccompIdValue = Request.Form["ccompidvalue"];
            var sDate = Request.Form["sdate"];
            var startDate = Request.Form["startdate"];
            var endDate = Request.Form["endate"];
            var lmodBy = Request.Form["lmodby"];
            var lmodByValue = Request.Form["lmodbyvalue"];
            var reasonTypeId = Request.Form["reasontypeid"];
            var freasonType = Request.Form["freasontype"];
            var freasonValue = Request.Form["freasonvalue"];
            var cbDelay = Request.Form["cbdelay"];
            var cbDelayValue = Request.Form["cbdelayvalue"];
            var repairAdvice = Request.Form["repairadvice"];
            var toCatDesc = Request.Form["tocatdesc"];
            var requestP1 = Request.Form["RequestP1"];
            var fissNull = Request.Form["fissnull"];
            var pCam = Request.Form["pcam"];
            var sortBy = Request.Form["sortby"];

            if (creportType == "")
            {
                Stat = "error";
                Msg = "Please Select A Report Type";
                return RedirectToAction(nameof(Index));
            }
            if (creportType == "3")
            {
                Stat = "error";
                Msg = "Not Available";
                return RedirectToAction(nameof(Index));
            }
            if (creportType == "2" && fdocType != "3" && fdocTypeValue == "")
            {
                Stat = "error";
                Msg = "Please Select DN Number And Its Value";
                return RedirectToAction(nameof(Index));
            }

            var sortByValue = FilterHelper.SelectSortBy(sortBy);
            tempfilter = ApplySortCategory(sortByValue, tempfilter);

            var cwoCategory = FilterHelper.SelectCwoTypeFilter(cwoType);
            tempfilter = ApplyFilterCategory(cwoCategory, cwoTypeValue, tempfilter);

            var fdocTypeCategory = FilterHelper.SelectFdocTypeFilter(fdocType);
            tempfilter = ApplyFilterCategory(fdocType, ccompIdValue, tempfilter);

            var ccompIdTypeCategory = FilterHelper.SelectCCompIdTypeFilter(ccompIdType);
            tempfilter = ApplyFilterCategory(ccompIdTypeCategory, ccompIdValue, tempfilter);

            var lmodByCategory = FilterHelper.SelectImodByFilter(lmodBy);
            tempfilter = ApplyFilterCategory(lmodByCategory, lmodByValue, tempfilter);

            var fReasonCategory = FilterHelper.SelectFreasonFilter(freasonType);
            tempfilter = ApplyFilterCategory(fReasonCategory, freasonValue, tempfilter);

            var cbDelayCategory = FilterHelper.SelectCbDelay(cbDelay);
            tempfilter = ApplyCbDelayCategory(cbDelayCategory, cbDelayValue, tempfilter);

            var fisNullValue = FilterHelper.SelectFisNull(fissNull);
            tempfilter = ApplyFisNullCategory(fisNullValue, tempfilter);

            Dictionary<string, string> formFields = new Dictionary<string, string>
                {
                    { "repairtypeid", repairType },
                    { "comptype", compType },
                    { "supervisorid", supervisorId },
                    { "supplierid", supplierId },
                    { "unitnumber", toCatDesc },
                    { "reasontypeid", reasonTypeId },
                    { "repairadvice", repairAdvice },
                    { "tocatdesc", toCatDesc },
                    { "RequestP1", requestP1 }
                };

            if (!string.IsNullOrEmpty(pCam))
            {
                tempfilter = " and PCAMStatusID = " + Utility.Evar(pCam, 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(statusInput))
            {
                tempfilter = " and status in (" + Utility.Evar(statusInput, 1) + ")" + tempfilter;
                ViewBag.statusValue = statusInput;
            }

            string strdate;
            switch (sDate)
            {
                case "1":
                    strdate = "ModDate";
                    break;
                case "2":
                    strdate = "CompletedDate";
                    break;
                default:
                    strdate = "ModDate";
                    break;
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                tempfilter = " and " + strdate + " >= " + Utility.Evar(startDate, 2) + tempfilter;
                ViewBag.startdate = startDate;
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                tempfilter = " and " + strdate + " <= " + Utility.Evar(endDate, 2) + tempfilter;
                ViewBag.endate = endDate;
            }

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

            var cReportTypeValue = FilterHelper.SelectCreportType(creportType);
            _tempfilter = Utility.VarFilter(tempfilter);
            cReportTypeQuery = ApplyCreportTypeQuery(cReportTypeValue, _tempfilter);
            var plusFilter = $"{cReportTypeQuery}{tempfilter})";

            string dataQuery = $"{plusFilter}";
            Console.WriteLine(dataQuery);
            DataTable data = SQLFunction.execQuery(dataQuery);
            string fileName = "External Repair Job History.xlsx";

            if (data != null && data.Rows.Count > 0)
            {
                return Utility.ExportDataTableToExcel(data, fileName);
            }
            else
            {
                Stat = "error";
                Msg = "No Data To Export";
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Delete(string id)
        {
            var deletedDate = Utility.Evar(Utility.getDate(), 2);
            var deletedBy = Utility.Evar(Utility.eusername(), 1);
            var ID = Utility.Evar(id, 1);

            Console.WriteLine(deletedDate);
            Console.WriteLine(deletedBy);
            Console.WriteLine(ID);

            string queryUpdate = @$"Update tbl_ExrJobDetail set StatusID='9', DeletedDate = {deletedDate}, 
            DeletedBy = {deletedBy} WHERE ID = {ID}";

            Console.WriteLine(queryUpdate);

            string queryNotified = $@"exec dbo.ExrJobStatusUpdate {ID}, '9', {deletedDate}, {deletedBy}";

            SQLFunction.execQuery(queryUpdate);
            SQLFunction.execQuery(queryNotified);

            Stat = "success";
            Msg = "Data Has Been Deleted";

            return Json(new { success = true });
        }
        public IActionResult Add()
        {
            loadoption();
            return View("~/Views/PER/ExrRepairJobHistory/Form.cshtml");
        }
        public IActionResult Edit(string id)
        {
            loadoption();

            // general data query
            string query = $"SELECT * from v_ExrJobDetail WHERE ID = '{id}'";
            Console.WriteLine(query);

            // v_ExrJobDetailRev1 data query
            //string queryDetail = @$"SELECT *,dbo.GetExrInspectResult(ID,'OLD') as resultOldCoreInspect,
            //dbo.GetExrInspectResult(ID,'FIN') AS resultFinalInspect,dbo.exrtotalrepair(TCIPartno) as TotalRepair
            //from v_ExrJobDetailRev1 WHERE ID={id}";

            string queryDetail = @$"SELECT *,dbo.GetExrInspectResult(ID,'OLD') as resultOldCoreInspect,
            dbo.GetExrInspectResult(ID,'FIN') AS resultFinalInspect
            from v_ExrJobDetailRev1 WHERE ID={id}";

            string queryAddOrderEtc = $"Select * from tbl_ExrJobDetailTwo WHERE ID={id}";
            string queryListStatus = $"SELECT id,itemchange,descchange,moddate,modby,src from v_ExrJobChangeHistory  Where JobID = {id}";

            // THE PROBLEM
            string queryCheckDispatch = $@"SELECT * from tbl_DispatchJobDetail WHERE JobID = {id}";
            Console.WriteLine(queryCheckDispatch);
            var result = SQLFunction.execQuery(queryCheckDispatch);
            ViewBag.dataaccess = SQLFunction.execQuery(queryCheckDispatch);

            if (result.Rows.Count > 0)
            {
                string statusId = result.Rows[0]["StatusID"].ToString();

                if (statusId != "del")
                {
                    string queryListDispatch = $@"Select DetailID,ID as ANNO,DispatchType as Type, Attention ,StatusID as 
                Status ,DispatchTypeID,RegisterBy,Registerdate from v_DispatchJobDetail WHERE SectionIDDetail=1 and JobID= {id} AND StatusID != 'del'";

                    ViewBag.ListDispatch = SQLFunction.execQuery(queryListDispatch);
                }
            }

            string queryListAttachment = $@"select id,AttachmentType,FilePath,FullPath from v_ExrJobAttachment where JobID={id}";
            string queryHoldUntil = $"select top 1 TargetDate from tbl_ExrJobChangeHistory where JobID={id} and JobStatus='OH' order by ModDate desc";

            ViewBag.data = SQLFunction.execQuery(query);
            ViewBag.detail = SQLFunction.execQuery(queryDetail);
            ViewBag.AddOrderEtc = SQLFunction.execQuery(queryAddOrderEtc);
            ViewBag.ListStatus = SQLFunction.execQuery(queryListStatus);
            ViewBag.ListAttachment = SQLFunction.execQuery(queryListAttachment);
            ViewBag.HoldUntil = SQLFunction.execQuery(queryHoldUntil);
            ViewBag.by = Utility.eusername();
            ViewBag.date = Utility.getDate();

            //string queryDispatchDetail = $@"SELECT * from tbl_DispatchJobDetail WHERE ID = {Utility.Evar(id, 1)} AND StatusID != 'del'";
            //Console.WriteLine(queryDispatchDetail);
            //var result = SQLFunction.execQuery(queryDispatchDetail);
            //if (result.Rows.Count > 0)
            //{
            //    Console.WriteLine("ada data");
            //    ViewBag.dataaccess = result;
            //}

            //string queryDispatchType = $"select DispatchType from v_DispatchJob where ID = {Utility.Evar(id, 1)} ";

            //ViewBag.typeDispatch = SQLFunction.execQuery(queryDispatchType);
            //ViewBag.id = id;

            return View("~/Views/PER/ExrRepairJobHistory/Form.cshtml");
        }
        public IActionResult DispatchDetail(string anno)
        {
            string queryDispatchDetail = $@"SELECT * from v_DispatchJobDetail WHERE DetailID = {Utility.Evar(anno, 1)} AND StatusID != 'del'";
            Console.WriteLine(queryDispatchDetail);
            var result = SQLFunction.execQuery(queryDispatchDetail);
            ViewBag.dispatchdata = SQLFunction.execQuery(queryDispatchDetail);

            if (result.Rows.Count > 0)
            {
                var rows = new List<object>();

                foreach (DataRow row in result.Rows)
                {
                    var rowData = new
                    {
                        id = anno,
                        StatusID = Utility.CheckNull(row["StatusID"]),
                        DispatchID = Utility.CheckNull(row["DispatchID"]),
                        dispatchType = Utility.CheckNull(row["DispatchType"]),
                        SectionID = Utility.CheckNull(row["SectionID"]),
                        JobID = Utility.CheckNull(row["JobID"]),
                        WONO = Utility.CheckNull(row["WONO"]),
                        childwo = Utility.CheckNull(row["ChildWO"]),
                        item = Utility.CheckNull(row["Item"]),
                        Qty = Utility.CheckNull(row["Qty"]),
                        UOM = Utility.CheckNull(row["UOM"]),
                        ItemDesc = Utility.CheckNull(row["ItemDesc"]),
                        PRNo = Utility.CheckNull(row["PRNo"]),
                        PONo = Utility.CheckNull(row["PONo"]),
                        Remarks = Utility.CheckNull(row["Remarks"]),
                        RegisterBy = Utility.CheckNull(row["RegisterBy"]),
                        registerdate = Utility.CheckNull(row["RegisterDate"]),
                        ModBy = Utility.CheckNull(row["ModBy"]),
                        ModDate = Utility.CheckNull(row["ModDate"]),
                        DeletedBy = Utility.CheckNull(row["DeletedBy"]),
                        DeletedDate = Utility.CheckNull(row["DeletedDate"]),
                    };
                    rows.Add(rowData);
                }
                return new JsonResult(rows);
            }

            //string queryDispatchType = $"select DispatchType from v_DispatchJob where ID = {Utility.Evar(anno, 1)} ";

            //ViewBag.typeDispatch = SQLFunction.execQuery(queryDispatchType);

            return new JsonResult("ok");
        }
        public IActionResult CreateAN()
        {
            return Redirect("/JobDispatch/Add");
        }
        public IActionResult CSave()
        {
            loadoption();

            string tblName = "tbl_DispatchJob";

            var attentionTo = Request.Form["tAttentionTo"];
            var shipmentDate = Request.Form["tShipmentDate"];
            var sectionId = Request.Form["tSectionId"];
            var eid = Request.Form["tid"];
            var dispatchType = Request.Form["tdispatchtype"];
            var consignedTo = Request.Form["tConsignedTo"];
            var attentionName = Request.Form["tAttentionName"];
            var attentionEmail = Request.Form["tAttentionEmail"];
            var transportMode = Request.Form["tTransportMode"];
            var manifestNo = Request.Form["tManifestNo"];
            var project = Request.Form["tProject"];
            var jobNo = Request.Form["tJobNo"];
            var dispatchBy = Request.Form["tDispatchBy"];
            var dispatchDate = Request.Form["tDispatchDate"];
            //var dispatchEmail = Request.Form[""];
            var handledBy = Request.Form["cHandledBy"];
            var handledDate = Request.Form["tHandledDate"];
            //var receivedBy = Request.Form["tReceivedBy"];
            //var receivedDate = Request.Form["tReceivedDate"];
            //var statusId = Request.Form["tStatusID"];

            if (string.IsNullOrEmpty(attentionTo) || string.IsNullOrEmpty(shipmentDate) || string.IsNullOrEmpty(sectionId))
            {
                Stat = "error";
                Msg = "Please Fill Mandatory (*) Field";
            }
            else
            {
                string eidValue = !string.IsNullOrEmpty(eid) ? eid : "null";
                string eDispatchType = !string.IsNullOrEmpty(dispatchType) ? dispatchType : HandleError("Please Select Dispatch Type");
                string eSectionId = !string.IsNullOrEmpty(sectionId) ? sectionId : HandleError("Please Select Dispatch From");
                string eShipmentDate = !string.IsNullOrEmpty(shipmentDate) ? Utility.Evar(shipmentDate, 2) : "null";
                string eConsignedTo = !string.IsNullOrEmpty(consignedTo) ? consignedTo : "null";
                string eAttentionName = !string.IsNullOrEmpty(attentionName) ? Utility.Evar(attentionName, 18) : "null";
                string eAttentionEmail = !string.IsNullOrEmpty(attentionEmail) ? attentionEmail : "null";
                string eAttentionTo = !string.IsNullOrEmpty(attentionTo) ? Utility.Evar(attentionTo, 0) : HandleError("Please Select tAttentionTo");
                string eTransportMode = !string.IsNullOrEmpty(transportMode) ? transportMode : "null";
                string eManifesNo = !string.IsNullOrEmpty(manifestNo) ? manifestNo : "null";
                string eProject = !string.IsNullOrEmpty(project) ? project : "null";
                string eJobNo = !string.IsNullOrEmpty(jobNo) ? jobNo : "null";
                string eDispatchBy = !string.IsNullOrEmpty(dispatchBy) ? dispatchBy : "null";
                string eDispatchDate = !string.IsNullOrEmpty(dispatchDate) ? Utility.Evar(dispatchDate, 2) : "null";
                //string eDispatchEmail = !string.IsNullOrEmpty(dispatchDate) ? Utility.Evar(dispatchDate, 16) : null;
                string eHandledBy = !string.IsNullOrEmpty(handledBy) ? handledBy : "null";
                string eHandledDate = !string.IsNullOrEmpty(handledDate) ? Utility.Evar(handledDate, 2) : "null";
                //string eReceivedBy = !string.IsNullOrEmpty(receivedBy) ? $"'{receivedBy}'" : null;
                //string eReceivedDate = !string.IsNullOrEmpty(receivedDate) ? Utility.Evar(receivedDate, 16) : null;
                //string eStatusId = !string.IsNullOrEmpty(statusId) ? $"'{statusId}'" : null;

                //string queryDispatchNumber = "Select dbo.GetANNumber()";
                //var rsrDispatchNumber = SQLFunction.execQuery(queryDispatchNumber);

                var dataQuery = $@"INSERT INTO {tblName} (ID, SectionID, DispatchType, ShipmentDate, ConsignedTo, AttentionName, AttentionEmail, AttentionTo, TransportMode, ManifestNo, Project, JobNo, DispatchBy, DispatchDate, HandledBy, HandledDate) VALUES ('{eidValue}', '{eSectionId}', '{eDispatchType}',{eShipmentDate}, '{eConsignedTo}', '{eAttentionName}', '{eAttentionEmail}', '{eAttentionTo}', '{eTransportMode}', '{eManifesNo}', '{eProject}', '{eJobNo}', '{eDispatchBy}', {eDispatchDate}, '{eHandledBy}', {eHandledDate})";

                Console.WriteLine(dataQuery);
                var data = SQLFunction.execQuery(dataQuery);

            }

            return RedirectToAction(nameof(Index));
        }

        private string HandleError(string message)
        {
            Stat = "error";
            Msg = message;
            return null;
        }
        public IActionResult OldCoreInspection()
        {
            return Redirect("/ExrRepairJobHistoryInspection/Index");
        }
        public IActionResult FinalInspection()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/FinalInvestigation.cshtml");
        }
        public IActionResult IndexPrintOldCore(string repairType, string compType, string statusInput, string supervisorId,
            string supplierId, string cwoType, string cwoTypeValue, string fdocType,
            string fdocTypeValue, string ccompIdType, string ccompIdValue, string sDate, string startDate,
            string endDate, string lmodBy, string lmodByValue, string reasonTypeId,
            string freasonType, string freasonValue, string cbDelay, string cbDelayValue,
            string repairAdvice, string toCatDesc, string requestP1, string fissNull,
            string pCam, string sortBy, string ascDesc, string unitnumber)
        {
            Console.WriteLine("repair type" + repairType);
            string filter = BuildTempFilter(repairType, compType, statusInput, supervisorId, supplierId, cwoType, cwoTypeValue, fdocType,
                     fdocTypeValue, ccompIdType, ccompIdValue, sDate, startDate, endDate, lmodBy, lmodByValue, reasonTypeId,
                     freasonType, freasonValue, cbDelay, cbDelayValue, repairAdvice, toCatDesc, requestP1, fissNull, pCam,
                     sortBy, ascDesc, unitnumber);

            Console.WriteLine("Filter " + filter);

            _tempfilter = Utility.VarFilter(filter);
            string query = $"SELECT TOP 10 * FROM v_ExrJobDetailRev1 {_tempfilter} AND ResultEOI is not null";
            var data = SQLFunction.execQuery(query);

            var rows = new List<object>();

            if (data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    var ID = Utility.CheckNull(row["ID"]);
                    rows.Add("/ExrRepairJobHistoryInspection/OldJsPdf/" + ID);
                }
            }
            else
            {
                return new JsonResult("not found");
            }
            return new JsonResult(rows);
        }
        public IActionResult IndexPrintFinalInspection(string repairType, string compType, string statusInput, string supervisorId,
            string supplierId, string cwoType, string cwoTypeValue, string fdocType,
            string fdocTypeValue, string ccompIdType, string ccompIdValue, string sDate, string startDate,
            string endDate, string lmodBy, string lmodByValue, string reasonTypeId,
            string freasonType, string freasonValue, string cbDelay, string cbDelayValue,
            string repairAdvice, string toCatDesc, string requestP1, string fissNull,
            string pCam, string sortBy, string ascDesc, string unitnumber)
        {
            string filter = BuildTempFilter(repairType, compType, statusInput, supervisorId, supplierId, cwoType, cwoTypeValue, fdocType,
                     fdocTypeValue, ccompIdType, ccompIdValue, sDate, startDate, endDate, lmodBy, lmodByValue, reasonTypeId,
                     freasonType, freasonValue, cbDelay, cbDelayValue, repairAdvice, toCatDesc, requestP1, fissNull, pCam,
                     sortBy, ascDesc, unitnumber);

            _tempfilter = Utility.VarFilter(filter);
            string query = $"SELECT * FROM v_ExrJobDetailRev1 {_tempfilter} AND ResultSFI is not null";
            Console.WriteLine(query);
            var data = SQLFunction.execQuery(query);

            var rows = new List<object>();

            if (data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    var ID = Utility.CheckNull(row["ID"]);
                    rows.Add("/ExrRepairJobHistoryInspection/FinalJsPdf/" + ID);
                }
            }
            else
            {
                return new JsonResult("not found");
            }
            return new JsonResult(rows);
        }
        public IActionResult SingleTag(string repairType, string compType, string statusInput, string supervisorId,
            string supplierId, string cwoType, string cwoTypeValue, string fdocType,
            string fdocTypeValue, string ccompIdType, string ccompIdValue, string sDate, string startDate,
            string endDate, string lmodBy, string lmodByValue, string reasonTypeId,
            string freasonType, string freasonValue, string cbDelay, string cbDelayValue,
            string repairAdvice, string toCatDesc, string requestP1, string fissNull,
            string pCam, string sortBy, string ascDesc, string unitnumber)
        {
            string filter = BuildTempFilter(repairType, compType, statusInput, supervisorId, supplierId, cwoType, cwoTypeValue, fdocType,
                     fdocTypeValue, ccompIdType, ccompIdValue, sDate, startDate, endDate, lmodBy, lmodByValue, reasonTypeId,
                     freasonType, freasonValue, cbDelay, cbDelayValue, repairAdvice, toCatDesc, requestP1, fissNull, pCam,
                     sortBy, ascDesc, unitnumber);

            _tempfilter = Utility.VarFilter(filter);
            string query = $"SELECT * FROM v_ExrJobDetailRev1 {_tempfilter}";
            var data = SQLFunction.execQuery(query);

            var rows = new List<object>();

            if (data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    var ID = Utility.CheckNull(row["ID"]);
                    rows.Add(ID);
                }
                return new JsonResult(rows);
            }
            else
            {
                return new JsonResult("not found");
            }
        }
        public IActionResult ChangeWO(string ParentWONew, string ID)
        {
            string query = $"Update tbl_ExrJobDetail Set OffSiteWO= {Utility.Evar(ParentWONew, 0)} WHERE ID = {Utility.Evar(ID, 0)}";
            Console.WriteLine(query);
            SQLFunction.execQuery(query);

            return Json(new { redirectToUrl = "/ExrRepairJobHistory/Edit/" + ID });
        }
        // load option for dropdown
        private void loadoption()
        {
            //repair type
            string queryrepair = "Select ExrRepairTypeID,ExrRepairTypeAbr,ExrRepairType from tbl_ExrRepairType";
            ViewBag.repairType = SQLFunction.execQuery(queryrepair);

            // repair type 2
            string queryrepair2 = "SELECT ExrRepairtypeID, ExrRepairtypeAbr, ExrRepairtype FROM tbl_ExrRepairType WHERE ExrRepairtypeID not in (1,4,5)";
            ViewBag.repairType2 = SQLFunction.execQuery(queryrepair2);

            //comp type
            string querycomp = "SELECT CompTypeID, CompType, CompTypeDesc FROM tbl_EXRCompType WHERE (((CompTypeID)<>2)); ";
            ViewBag.compType = SQLFunction.execQuery(querycomp);

            //spv desc
            string queryspvdesc = "SELECT SupervisorID, SupervisorAbbr, SupervisorDesc, Email FROM tbl_Supervisor WHERE (((SupervisorDesc) Is Not Null)); ";
            ViewBag.spvdesc = SQLFunction.execQuery(queryspvdesc);

            //repair by
            string queryrepairby = "SELECT SupplierID, SupplierName2, SupplierName FROM tblv_SupplierList  ORDER BY SupplierName;";
            ViewBag.repairby = SQLFunction.execQuery(queryrepairby);

            //Eq.Class
            string queryeqclass = "SELECT DISTINCT UnitDescription, UnitNumber FROM v_ExrJobEquipment ORDER BY UnitDescription; ";
            ViewBag.feqclass = SQLFunction.execQuery(queryeqclass);

            //FReason
            string queryfreason = "SELECT DISTINCT ReasonTypeID, ReasonType, ReasonTypeDesc FROM tbl_EXRReasonType;  ";
            ViewBag.freason = SQLFunction.execQuery(queryfreason);

            //FStore
            string queryfstore = "SELECT DISTINCT StoreID, StoreName FROM tbl_Store; ";
            ViewBag.fstore = SQLFunction.execQuery(queryfstore);

            //CBToCategory
            string querycbtocategory = "SELECT TOCatID, TOCatDesc FROM tbl_TurnOverCat; ";
            ViewBag.cbtocategory = SQLFunction.execQuery(querycbtocategory);

            //Currency
            string querycurrency = "SELECT CurrID FROM tbl_Currency; ";
            ViewBag.currency = SQLFunction.execQuery(querycurrency);

            // tsaving cost category
            string querytsavingcost = "select * from tbl_SavingCostCategory order by savingcostCatID";
            ViewBag.tsavingcost = SQLFunction.execQuery(querytsavingcost);

            // tdestination
            string querytdestionation = "SELECT StoreID, StoreName FROM tblv_Store";
            ViewBag.tdestination = SQLFunction.execQuery(querytdestionation);

            // tsitealloc
            string querytsitealloc = "SELECT Location, LocationName FROM tblv_LocationTS";
            ViewBag.tsitealloc = SQLFunction.execQuery(querytsitealloc);

            // tnextstatus
            string querytnextstatus = "SELECT * FROM tbl_EXRJobStatus Where ExrJobStatusID Not IN (0,5,6,11,12)";
            ViewBag.tnextstatus = SQLFunction.execQuery(querytnextstatus);

            // tnextstatus
            string queryrepairadvice = "Select JobStatusID,JobStatus from dbo.JobStatusEXR('ALL') order by StatusOrder";
            ViewBag.repairadvice = SQLFunction.execQuery(queryrepairadvice);

            // tEquipClass
            string queryEquipClass = "select * from tbl_Equipclass";
            ViewBag.EquipClass = SQLFunction.execQuery(queryEquipClass);

            // tAttentionTo for CreateAN
            string queryTattentionTo = "SELECT isnull(SupplierName,SupplierID) as SupplierName, SupplierID FROM tbl_SupplierList ORDER BY SupplierName";
            ViewBag.tAatentionTo = SQLFunction.execQuery(queryTattentionTo);

            // tAttentionName for CreateAN
            string queryTaatentionName = "SELECT DisplayName, Department, EmailAddress FROM v_AddressBook";
            ViewBag.tAttentionName = SQLFunction.execQuery(queryTaatentionName);

            // tSectionId for CreateAN
            string queryTsectionId = "SELECT JobSourceID, JobSource FROM tbl_JobSource";
            ViewBag.tSectionId = SQLFunction.execQuery(queryTsectionId);

            // tDispatchBy for CreateAN
            string queryTdispatchType = "SELECT DispatchTypeID, DispatchType FROM tbl_DispatchType";
            ViewBag.tDispatchType = SQLFunction.execQuery(queryTdispatchType);

            // tDispatchBy for CreateAN
            string queryTdispatchBy = "SELECT DisplayName,Account, Department, EmailAddress FROM v_AddressBook";
            ViewBag.tDispatchBy = SQLFunction.execQuery(queryTdispatchBy);

            // tDispatchBy for CreateAN
            string queryChandledBy = "SELECT DisplayName,Account, Department, EmailAddress FROM v_AddressBook";
            ViewBag.cHandledBy = SQLFunction.execQuery(queryChandledBy);
        }
        private string ApplyFilterCategory(string category, string value, string currentFilter)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return $" and {category} like {Utility.Evar(value, 11)}" + currentFilter;
            }
            return currentFilter;
        }
        private string ApplySortCategory(string value, string currentFilter)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return $" and {value} like {value}" + currentFilter;
            }
            return currentFilter;
        }
        private string ApplyCreportTypeQuery(string value, string filter)
        {
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(filter))
            {
                return $"{value}{filter}";
            }
            return value + ")";
        }
        private string ApplyCbDelayCategory(string category, string value, string currentFilter)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return $" and {category} {value} 0" + currentFilter;
            }
            return currentFilter;
        }
        private string ApplyFisNullCategory(string value, string currentFilter)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return $" and {value} IS Null" + currentFilter;
            }
            return currentFilter;
        }
    }
}
