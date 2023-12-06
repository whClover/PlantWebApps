using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;
using System.Web;

namespace PlantWebApps.Controllers.PER.ExrRepairJobHistory
{ 
    public class ExrRepairJobHistory : Controller
    {
        private string _tempfilter;
        [TempData]
        public String Msg { get; set; }
        [TempData]
        public String Stat { get; set; }
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            LoadData(page, pageSize);
            return View("~/Views/PER/ExrRepairJobHistory/Index.cshtml");
        }
        public IActionResult Filter(int page = 1, int pageSize = 20)
        {
            int totalRecords;
            int totalPages;
            loadoption();
            BuildTempFilter();

            string countQuery = "SELECT COUNT(*) FROM v_ExrJobDetailRev1" + _tempfilter;
            int.TryParse(SQLFunction.ExecuteScalar(countQuery).ToString(), out totalRecords);

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int offset = (page - 1) * pageSize;
            int limit = pageSize;
            var sort = Request.Form["ascdsc"];
            string sortOrder = string.IsNullOrEmpty(sort) ? "desc" : sort;

            string query = $"SELECT * FROM v_ExrJobDetailRev1 {_tempfilter} order by id {sortOrder} OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
            Console.WriteLine(query);
            ViewBag.data = SQLFunction.execQuery(query);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Filter = false;

            return View("~/Views/PER/ExrRepairJobHistory/Index.cshtml");
        }

        public IActionResult Export()
        {
            var cReportTypeQuery = "";

            loadoption();
            BuildTempFilter();

            if (Request.Form["creportype"] == "3")
            {
                Stat = "error";
                Msg = "Not Available";
                return RedirectToAction(nameof(Index));
            }
            else if (Request.Form["creportype"] == "2" &&
                     (Request.Form["fdoctype"] != "3" && string.IsNullOrEmpty(Request.Form["fdoctypevalue"])) ||
                     string.IsNullOrEmpty(Request.Form["fdoctypevalue"]))
            {
                Stat = "error";
                Msg = Request.Form["fdoctype"] == "3" ? "Please Select Doc Type to Dn Number" : "Please Select Dn Number";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var cReportTypeValue = FilterHelper.SelectCreportType(Request.Form["creportype"]);
                cReportTypeQuery = ApplyCreportTypeQuery(cReportTypeValue, Request.Form["fdoctypevalue"]);

                if (Request.Form["creportype"] == "2")
                {
                    cReportTypeQuery += $"{_tempfilter}";
                }

                string dataQuery = $"{cReportTypeQuery}";
                Console.WriteLine(dataQuery);

                DataTable data = SQLFunction.execQuery(dataQuery);
                string fileName = "External Repair Job History.xlsx";

                return Utility.ExportDataTableToExcel(data, fileName);
            }
        }
        private void LoadData(int page, int pageSize)
        {
            loadoption();
            int totalRecords;
            int totalPages;

            string countQuery = "SELECT COUNT(*) FROM v_ExrJobDetailRev1";
            int.TryParse(SQLFunction.ExecuteScalar(countQuery).ToString(), out totalRecords);

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            int offset = (page - 1) * pageSize;
            int limit = pageSize;

            string query = $"SELECT * FROM v_ExrJobDetailRev1 order by id desc OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
            ViewBag.data = SQLFunction.execQuery(query);

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Filter = false;
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
        private void BuildTempFilter()
        {
            string tempfilter = string.Empty;

            // for option using case in exrRepairHistoryFilterHelper
            var sortByValue = FilterHelper.SelectSortBy(Request.Form["sortby"]);
            tempfilter = ApplySortCategory(sortByValue, tempfilter);

            var (cwoCategory, cwoTypeValue) = FilterHelper.SelectCwoTypeFilter(Request.Form["cwotype"], Request.Form["cwotypevalue"]);
            tempfilter = ApplyFilterCategory(cwoCategory, cwoTypeValue, tempfilter);

            var (fdocTypeCategory, fdocTypeValue) = FilterHelper.SelectFdocTypeFilter(Request.Form["fdoctype"], Request.Form["fdoctypevalue"]);
            tempfilter = ApplyFilterCategory(fdocTypeCategory, fdocTypeValue, tempfilter);

            var (ccompIdTypeCategory, ccompIdTypeValue) = FilterHelper.SelectCCompIdTypeFilter(Request.Form["ccompidtype"], Request.Form["ccompidvalue"]);
            tempfilter = ApplyFilterCategory(ccompIdTypeCategory, ccompIdTypeValue, tempfilter);

            var(lmodByCategory, lmodByValue) = FilterHelper.SelectImodByFilter(Request.Form["lmodby"], Request.Form["lmodbyvalue"]);
            tempfilter = ApplyFilterCategory(lmodByCategory, lmodByValue, tempfilter);

            var (fReasonCategory, fReasonValue) = FilterHelper.SelectFreasonFilter(Request.Form["freasontype"], Request.Form["freasonvalue"]);
            tempfilter = ApplyFilterCategory(fReasonCategory, fReasonValue, tempfilter);

            var (cbDelayCategory, cbDelayValue) = FilterHelper.SelectCbDelay(Request.Form["cbdelay"], Request.Form["cbdelayvalue"]);
            tempfilter = ApplyCbDelayCategory(cbDelayCategory, cbDelayValue, tempfilter);

            var fisNullValue = FilterHelper.SelectFisNull(Request.Form["fissnull"]);
            tempfilter = ApplyFisNullCategory(fisNullValue, tempfilter);

            // for regular filter
            Dictionary<string, string> formFields = new Dictionary<string, string>
            {
                { "repairtypeid", "ViewBag.repairtypeid" },
                { "comptype", "ViewBag.comptype" },
                { "supervisorid", "ViewBag.supervisorid" },
                { "supplierid", "ViewBag.supplierid" },
                { "unitnumber", "ViewBag.unitnumber" },
                { "reasontypeid", "ViewBag.reasontypeid" },
                { "repairadvice", "ViewBag.repairadvice" },
                { "tocatdesc", "ViewBag.tocatdesc" },
                { "RequestP1", "ViewBag.RequestP1" }
            };

            if (!string.IsNullOrEmpty(Request.Form["pcam"]))
            {
                tempfilter = " and PCAMStatusID = " + Utility.Evar(Request.Form["pcam"], 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(Request.Form["statusValue"]))
            {
                tempfilter = " and status in (" + Utility.Evar(Request.Form["statusValue"], 1) + ")" + tempfilter;
                string statusValue = Request.Form["statusValue"];
                ViewBag.statusValue = statusValue;
            }

            // for datepicker
            string strdate;
            switch (Request.Form["sdate"])
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

            if (!string.IsNullOrEmpty(Request.Form["startdate"]))
            {
                tempfilter = " and " + strdate + " >= " + Utility.Evar(Request.Form["startdate"], 2) + tempfilter;
                string startdate = Request.Form["startdate"];
                ViewBag.startdate = startdate;
            }

            if (!string.IsNullOrEmpty(Request.Form["endate"]))
            {
                tempfilter = " and " + strdate + " <= " + Utility.Evar(Request.Form["endate"], 2) + tempfilter;
                string endate = Request.Form["endate"];
                ViewBag.endate = endate;
            }

            // for dictionary
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
    }
}
