using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantWebApps.Helper;
using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing.Printing;
using System.Globalization;

namespace PlantWebApps.Controllers.PER.ExrRepairJobHistory
{ 
    public class ExrRepairJobHistory : Controller
    {
        private string _tempfilter;
        [TempData]
        public String Msg { get; set; }
        [TempData]
        public String Stat { get; set; }
		public IActionResult CheckHub(string formName, string accessType)
		{
			bool isGranted = Utility.checkgranted(formName, accessType);
			return Json(new { isGranted });
		}
		public IActionResult Index()
        {
            loadoption();
            return View("~/Views/PER/ExrRepairJobHistory/Index.cshtml");
        }
		public IActionResult TSave(string eID, string eAddCost, string eChildWO, string eCompDesc, string eCompQty, 
            string eCompTypeID, string eCostAfter, string eCostBefore, string eCostRepair, string eCurrID, 
            string eDeliveryDate, string eIssuedBy, string eDestination, string eDisputeCompletedBy, 
            string eDisputeCompletedBy2, string eDispCompDate, string eDComplDispDate, string eDnNumber, 
            string eptYDocAvb, string eST, string eOt, string eDocDate, string eDocTypeID, string eEGEI, 
            string eEIEK, string eEKEP, string eULL, string eetadate, string eFitToUnit, string eIntWO, 
            string eIntWOPrevious, string eJobNo, string eSerialNumberOEM, string eTCICoreFitTo, 
            string eExtWO, string eLocationHold, string eLogANReceivedBy, string eDlogAnDate, string eDlogAnNo, 
            string eDPerAnDate, string eMaintType, string eMeterRun, string eMeterToRun, string eOemCost, 
            string eORRequestDate, string eORCompletedDAte, string eORRRNo, string eOPRRNo, string eOffSiteWO, string eOPDate, 
            string eOPNo, string eOPPrevious, string eORNo, string eQuoteApprovedBy, string eQuoteDate, string eQuoteNo, 
            string eQuotePrintBy, string eQuoteProcessBy, string eQuoteRevDate, string eQuoteRevNo, string eReasonType, 
            string eReceivedBy, string eReceivedByName, string eReceivedDate, string eremark, string eRepairTypeID, 
            string eReqStand, string eReqPart, string eRequestP1, string eRTSCost, string esavingcostCatID, 
            string eSitetoLogAN, string eSitetoLogDate, string eNextStatus, string eStoreID, string eSTOtNo, 
            string eSpvID, string eSuppForRepairAN, string eRepairByID, string eSuppReceiveANBy, string eSuppReceiveANDate, 
            string eTaggingBy, string eDTaggingDate, string eTCICost, string eTCIPartID, string eTCIPartNo, string eUnitNo, 
            string ebWarrantyResult, string eWCSDate, string eWOFitToUnit, string eWoJobCost, string eWOPrevious, 
            string eDecisionDate, string eEmailDate, string eRepairAdvice, string eSuggestedStore, string eRemarkAdvice, 
            string eSiteAlloc, string eWOAlloc, string eUnitAlloc, string eSchedStartAlloc, string eSOHAlloc, string eOutReqAlloc)
		{
			Console.WriteLine("1 " + eID);
			Console.WriteLine("2 " + eAddCost);
			Console.WriteLine("3 " + eChildWO);
			Console.WriteLine("4 " + eCompDesc);
			Console.WriteLine("5 " + eCompQty);
			Console.WriteLine("6 " + eCompTypeID);
			Console.WriteLine("7 " + eCostAfter);
			Console.WriteLine("8 " + eCostBefore);
			Console.WriteLine("9 " + eCostRepair);
			Console.WriteLine("10 " + eCurrID);
			Console.WriteLine("11 " + eDeliveryDate);
			Console.WriteLine("12 " + eIssuedBy);
			Console.WriteLine("13 " + eDestination);
			Console.WriteLine("14 " + eDisputeCompletedBy);
			Console.WriteLine("15 " + eDisputeCompletedBy2);
			Console.WriteLine("16 " + eDispCompDate);
			Console.WriteLine("17 " + eDComplDispDate);
			Console.WriteLine("18 " + eDnNumber);
			Console.WriteLine("19 " + eptYDocAvb);
			Console.WriteLine("20 " + eST);
			Console.WriteLine("21 " + eOt);
			Console.WriteLine("22 " + eDocDate);
			Console.WriteLine("23 " + eDocTypeID);
			Console.WriteLine("24 " + eEGEI);
			Console.WriteLine("25 " + eEIEK);
			Console.WriteLine("26 " + eEKEP);
			Console.WriteLine("27 " + eULL);
			Console.WriteLine("28 " + eetadate);
			Console.WriteLine("29 " + eFitToUnit);
			Console.WriteLine("30 " + eIntWO);
			Console.WriteLine("31 " + eIntWOPrevious);
			Console.WriteLine("32 " + eJobNo);
			Console.WriteLine("33 " + eSerialNumberOEM);
			Console.WriteLine("34 " + eTCICoreFitTo);
			Console.WriteLine("35 " + eExtWO);
			Console.WriteLine("36 " + eLocationHold);
			Console.WriteLine("37 " + eLogANReceivedBy);
			Console.WriteLine("38 " + eDlogAnDate);
			Console.WriteLine("39 " + eDlogAnNo);
			Console.WriteLine("40 " + eDPerAnDate);
			Console.WriteLine("41 " + eMaintType);
			Console.WriteLine("42 " + eMeterRun);
			Console.WriteLine("43 " + eMeterToRun);
			Console.WriteLine("44 " + eOemCost);
			Console.WriteLine("45 " + eORRequestDate);
			Console.WriteLine("46 " + eORCompletedDAte);
			Console.WriteLine("47 " + eORRRNo);
			Console.WriteLine("48 " + eOPRRNo);
			Console.WriteLine("49 " + eOffSiteWO);
			Console.WriteLine("50 " + eOPDate);
			Console.WriteLine("51 " + eOPNo);
			Console.WriteLine("52 " + eOPPrevious);
			Console.WriteLine("53 " + eORNo);
			Console.WriteLine("54 " + eOt);
			Console.WriteLine("55 " + eQuoteApprovedBy);
			Console.WriteLine("56 " + eQuoteDate);
			Console.WriteLine("57 " + eQuoteNo);
			Console.WriteLine("58 " + eQuotePrintBy);
			Console.WriteLine("59 " + eQuoteProcessBy);
			Console.WriteLine("60 " + eQuoteRevDate);
			Console.WriteLine("61 " + eQuoteRevNo);
			Console.WriteLine("62 " + eReasonType);
			Console.WriteLine("63 " + eReceivedBy);
			Console.WriteLine("64 " + eReceivedByName);
			Console.WriteLine("65 " + eReceivedDate);
			Console.WriteLine("66 " + eremark);
			Console.WriteLine("67 " + eRepairTypeID);
			Console.WriteLine("68 " + eReqStand);
			Console.WriteLine("69 " + eReqPart);
			Console.WriteLine("70 " + eRequestP1);
			Console.WriteLine("71 " + eRTSCost);
			Console.WriteLine("72 " + esavingcostCatID);
			Console.WriteLine("73 " + eSitetoLogAN);
			Console.WriteLine("74 " + eSitetoLogDate);
			Console.WriteLine("75 " + eNextStatus);
			Console.WriteLine("76 " + eStoreID);
			Console.WriteLine("77 " + eSTOtNo);
			Console.WriteLine("78 " + eSpvID);
			Console.WriteLine("79 " + eSuppForRepairAN);
			Console.WriteLine("80 " + eRepairByID);
			Console.WriteLine("81 " + eSuppReceiveANBy);
			Console.WriteLine("82 " + eSuppReceiveANDate);
			Console.WriteLine("83 " + eTaggingBy);
			Console.WriteLine("84 " + eDTaggingDate);
			Console.WriteLine("85 " + eTCICost);
			Console.WriteLine("86 " + eTCIPartID);
			Console.WriteLine("87 " + eTCIPartNo);
			Console.WriteLine("88 " + eUnitNo);
			Console.WriteLine("89 " + ebWarrantyResult);
			Console.WriteLine("90 " + eWCSDate);
			Console.WriteLine("91 " + eWOFitToUnit);
			Console.WriteLine("92 " + eWoJobCost);
			Console.WriteLine("93 " + eWOPrevious);
			Console.WriteLine("94 " + eDecisionDate);
			Console.WriteLine("95 " + eEmailDate);
			Console.WriteLine("96 " + eRepairAdvice);
			Console.WriteLine("97 " + eRequestP1);
			Console.WriteLine("98 " + eNextStatus);
			Console.WriteLine("99 " + eRepairAdvice);
			Console.WriteLine("100 " + eSuggestedStore);
			Console.WriteLine("101 " + eRemarkAdvice);
			Console.WriteLine("102 " + eSiteAlloc);
			Console.WriteLine("103 " + eWOAlloc);
			Console.WriteLine("104 " + eUnitAlloc);
			Console.WriteLine("105 " + eSchedStartAlloc);
			Console.WriteLine("106 " + eSOHAlloc);
			Console.WriteLine("107 " + eOutReqAlloc);
			Console.WriteLine("108 " + eRequestP1);

			loadoption();
			return View("~/Views/PER/ExrRepairJobHistory/Index.cshtml");
		}
		public IActionResult LoadData(
            string repairType, string compType, string statusInput, string supervisorId,
            string supplierId, string cwoType, string cwoTypeValue, string fdocType,
            string fdocTypeValue, string ccompIdType, string ccompIdValue, string sDate, string startDate,
            string endDate, string lmodBy, string lmodByValue, string reasonTypeId,
            string freasonType, string freasonValue, string cbDelay, string cbDelayValue,
            string repairAdvice, string toCatDesc, string requestP1, string fissNull,
            string pCam, string sortBy, string ascDesc)
            {
                loadoption();
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

            var sort = ascDesc;
            string sortOrder = string.IsNullOrEmpty(sort) ? "desc" : sort;

            _tempfilter = Utility.VarFilter(tempfilter);
            string dataQuery = $"SELECT TOP 50 * FROM v_ExrJobDetailRev1 {_tempfilter} AND StatusID != '9' order by id {sortOrder}";
            Console.WriteLine(dataQuery);
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
            if (fdocType != "3" && fdocTypeValue == "")
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
            cReportTypeQuery = ApplyCreportTypeQuery(cReportTypeValue, fdocTypeCategory, Utility.Evar(fdocTypeValue, 1));
            var plusFilter = $"{cReportTypeQuery}{tempfilter})";

            string dataQuery = $"{plusFilter}";
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

            string queryListDispatch = $@"Select DetailID,ID as ANNO,DispatchType as Type, Attention ,StatusID as 
            Status ,DispatchTypeID,RegisterBy,Registerdate  from v_DispatchJobDetail WHERE SectionIDDetail=1 and JobID= {id}";

            string queryListAttachment = $@"select id,AttachmentType,FilePath,FullPath from v_ExrJobAttachment where id={id}";

            string queryHoldUntil = $"select top 1 TargetDate from tbl_ExrJobChangeHistory where JobID={id} and JobStatus='OH' order by ModDate desc";

			ViewBag.data = SQLFunction.execQuery(query);
            ViewBag.detail = SQLFunction.execQuery(queryDetail);
            ViewBag.AddOrderEtc = SQLFunction.execQuery(queryAddOrderEtc);
            ViewBag.ListStatus = SQLFunction.execQuery(queryListStatus);
            ViewBag.ListDispatch = SQLFunction.execQuery(queryListDispatch);
            ViewBag.ListAttachment = SQLFunction.execQuery(queryListAttachment);
            ViewBag.HoldUntil = SQLFunction.execQuery(queryHoldUntil);
            ViewBag.by = Utility.eusername();
            ViewBag.date = Utility.getDate();

            return View("~/Views/PER/ExrRepairJobHistory/Form.cshtml");
		}
		public IActionResult CreateAN()
        {
            loadoption();
            var eSpv = ViewBag.spvdesc;
            var eRepairBy = ViewBag.repairby;

            ViewBag.Project = "BSF";
            ViewBag.JobNo = "503002";
            ViewBag.ShipmentDate = DateTime.Now;
            ViewBag.AttentionTo = eRepairBy;

            //string tempstring = $"select displayName from v_AddressBook ";
            //var rst2 = SQLFunction.execQuery(tempstring);

            //foreach (DataRow row in rst2.Rows)
            //{
            //    string eName = row["DisplayName"].ToString();
            //    ViewBag.cHandledBy = eName;
            //    ViewBag.DetailVisible = false;
            //}

            return View("~/Views/PER/ExrRepairJobHistory/CreateAN/CreateAN.cshtml");
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
                string eAttentionTo = !string.IsNullOrEmpty(attentionTo) ?  Utility.Evar(attentionTo, 0) : HandleError("Please Select tAttentionTo");
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
        private string ApplyCreportTypeQuery(string value, string fdoc, string fdocTypeValue)
        {
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(fdoc) && !string.IsNullOrEmpty(fdocTypeValue))
            {
                return $"{value} WHERE {fdoc} = {fdocTypeValue}";
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
