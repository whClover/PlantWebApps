using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantWebApps.Helper;
using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing.Printing;
using System.Globalization;
using System.Xml.Linq;

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
			var xCurrID = Utility.Evar(eCurrID, 0);
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
			var xRTSCost = Utility.Evar(eRTSCost, 1);
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
            , {xdata}, {Utility.ebyname(), 1}";

            //Console.WriteLine(queryJobRegisterRev1);
            //SQLFunction.execQuery(queryJobRegisterRev1);

            string queryAdviceUpdate = $@"exec dbo.ExrRepairAdviceUpdate {xjobid}, {xSuggestedStore}, {xRemarkAdvice},
			{xSiteAlloc}, {xWOAlloc}, {xSchedStartAlloc}, {xSOHAlloc}, {xUnitAlloc}, {xOutReqAlloc}, {xRequestP1},
            {xRepairAdvice}, {Utility.ebyname(), 1}";

            //Console.WriteLine(queryAdviceUpdate);
            //SQLFunction.execQuery(queryAdviceUpdate);

            if (eCurrentStatus == "OH" || eRepairAdvice == "OH") 
            {
                string queryOnHold = $"exec dbo.EXRJobOnHold {xjobid}, 'OH', {xHoldUntil}";
                //Console.WriteLine(queryOnHold);
                //SQLFunction.execQuery(queryOnHold);
            }

            string queryJob2Add = @$"exec dbo.EXRJob2Add {xJobID}, {xApp1}, {xApp1By}, {xApp1Date}, {xApp2}, {xApp2By}, {xApp2Date},
            {xApp3}, {xApp3By}, {xApp3Date}, '{xBuyerName}', {xAppSentDate}, {xAddOrder1}, {xAddOrder2}, {xAddOrder3},
            {xAddOrderORNo1}, {xAddOrderORNo2}, {xAddOrderORNo3}, {xaddOrderDate1}, {xAddOrder2}, {xaddOrderDate3},
            {xaddOrderDNNo1}, {xaddOrderDNNo2}, {xaddOrderDNNo3}, {xdata}, {xdata}, {xdata}, {Utility.ebyname()}";

            //Console.WriteLine(queryJob2Add);
            //SQLFunction.execQuery(queryJob2Add);

            Console.WriteLine(xTCIPartID);

            if (eTCIPartID != "" && eID != "")
            {
                string queryUpdateCompId = $"UPDATE tbl_ExrComponentID SET JobID={xID} WHERE TCIPartID={xTCIPartID}";
                //Console.WriteLine(queryUpdateCompId);
                //SQLFunction.execQuery(queryJob2Add);
            }

            string queryPDFID = $"select PRFID from tbl_ExrJobDetail where ID={xID} AND PRFID is Not Null";
            Console.WriteLine(queryPDFID);

            var ePDFID = SQLFunction.execQuery(queryPDFID);
            if (ePDFID.Rows.Count > 0)
            {
                string pdfID = ePDFID.Rows[0]["PRFID"].ToString();
                string queryUpdatePartRequest = $"UPDATE tbl_PartRequest SET WOBin={xWOJobCost} WHERE PRFID={Utility.Evar(pdfID, 1)}";
                //Console.WriteLine(queryUpdatePartRequest);
                //SQLFunction.execQuery(queryPDF);
            }

            Console.WriteLine(eWOFitToUnit);
            Console.WriteLine(eWOFitToUnitID);

            if (eWOFitToUnit != "" || eWOFitToUnitID != "")
            {
                string queryInsertJobDetail = @$"INSERT INTO tbl_ExrjobDetail(OffSiteWO,StatusID,UnitNumber,
                CompDesc,RegisterDate,RegisterBy) VALUES ({xWOFitToUnit}, 2, {xFitToUnit}, {xCompDesc}, {xRegisterDate},
                {Utility.ebyname()})";

                Console.WriteLine(queryInsertJobDetail);
                //SQLFunction.execQuery(queryInsertJobDetail);
            }

			Stat = "success";
			Msg = "Data Has Been Inserted";

			return Json(new { redirectToUrl = "/ExrRepairJobHistory/Edit/" + eID });
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
