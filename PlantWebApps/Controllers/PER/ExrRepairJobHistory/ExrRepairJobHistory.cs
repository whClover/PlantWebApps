using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantWebApps.Helper;
using System;
using System.Data;
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
        public IActionResult Index()
        {
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

            string dataQuery = $"SELECT TOP 50 * FROM v_ExrJobDetailRev1 {_tempfilter} order by id {sortOrder}";
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
                    receivedDate = Utility.CheckNull(row["ReceivedDate"])
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }
        public IActionResult Export()
        {
            var cReportTypeQuery = "";

            loadoption();

            var creportType = Request.Form["creportype"];
            var fdocType = Request.Form["fdoctype"];
            var fdocTypeValue = Request.Form["fdoctypevalue"];

            switch (creportType)
            {
                case "3":
                    Stat = "error";
                    Msg = "Not Available";
                    return RedirectToAction(nameof(Index));

                case "2" when (fdocType != "3" && string.IsNullOrEmpty(fdocTypeValue)) || string.IsNullOrEmpty(fdocTypeValue):
                    Stat = "error";
                    Msg = fdocType == "3" ? "Please select a document type for DN number." : "Please select a DN number.";
                    return RedirectToAction(nameof(Index));

                default:
                    var cReportTypeValue = FilterHelper.SelectCreportType(Request.Form["creportype"]);
                    cReportTypeQuery = ApplyCreportTypeQuery(cReportTypeValue, Request.Form["fdoctypevalue"]);

                    string dataQuery = $"{cReportTypeQuery}";
                    Console.WriteLine(dataQuery);

                    DataTable data = SQLFunction.execQuery(dataQuery);
                    string fileName = "External Repair Job History.xlsx";

                    return Utility.ExportDataTableToExcel(data, fileName);
            }
        }

        public IActionResult Add()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Add.cshtml");
        }

        // load option for dropdown
        private void loadoption()
        {            
            //repair type
            string queryrepair = "Select ExrRepairTypeID,ExrRepairTypeAbr,ExrRepairType from tbl_ExrRepairType";
            ViewBag.repairType = SQLFunction.execQuery(queryrepair);

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
        private string ApplyCreportTypeQuery(string value, string fdocTypeValue)
        {
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(fdocTypeValue))
            {
                return $"{value} {Utility.Evar(fdocTypeValue, 19)}";
            }
            return value;
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
