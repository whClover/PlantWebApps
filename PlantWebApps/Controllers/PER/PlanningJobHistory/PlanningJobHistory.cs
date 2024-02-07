using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;
using System.Drawing;

namespace PlantWebApps.Controllers.PER.PlanningJobHistory
{
    public class PlanningJobHistory : Controller
    {
        private string _tempfilter;
        public IActionResult Index()
        {
            LoadOption();
            return View("~/Views/PER/PlanningJobHistory/index.cshtml");
        }
        private string BuildTempFilter (string fDocType, string CWOType, string CCompIDType, string fstoretype, 
            string CbRepairAdvice, string tMaintType, string fstatusid,
            string fswo, string fsrepairby, string fsort, string fasc, string fdocno, string TPartID, string fstart, 
            string fend, string feqclass, string freason, string fstore,
            string CbTOCategory, string CbPriority, string fisnull)
        {
            string tempfilter = string.Empty;
            if (!string.IsNullOrEmpty(fDocType) && !string.IsNullOrEmpty(fdocno))
            {
                tempfilter = $"AND {fDocType} = {Utility.Evar(fdocno, 0)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(fstoretype) && !string.IsNullOrEmpty(fstore))
            {
                tempfilter = $"AND {fstoretype} = {Utility.Evar(fstore, 0)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(CCompIDType) && !string.IsNullOrEmpty(TPartID))
			{
				tempfilter = $"AND {CCompIDType} = {Utility.Evar(TPartID, 0)}" + tempfilter;
			}

			if (!string.IsNullOrEmpty(CWOType) && !string.IsNullOrEmpty(fswo))
			{
				tempfilter = $"AND {CWOType} = {Utility.Evar(fswo, 0)}" + tempfilter;
			}

			if (!string.IsNullOrEmpty(fisnull))
			{
				tempfilter = $"AND {fisnull} IS NULL" + tempfilter;
			}

            if (!string.IsNullOrEmpty(freason))
            {
                tempfilter = $"AND ReasonTypeID = {Utility.Evar(freason, 0)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(fstatusid))
            {
                tempfilter = $"AND StatusID ={Utility.Evar(fstatusid, 0)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(CbPriority))
            {
                tempfilter = $"AND RequestP1 ={Utility.Evar(CbPriority, 1)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(CbRepairAdvice))
            {
                tempfilter = " and RepairAdvice in (" + Utility.Evar(CbRepairAdvice, 1) + ")" + tempfilter;
                ViewBag.RepairAdvice = CbRepairAdvice;
            }

            if (!string.IsNullOrEmpty(tMaintType))
            {
                tempfilter = " and MaintType in (" + Utility.Evar(tMaintType, 1) + ")" + tempfilter;
                ViewBag.MaintType = tMaintType;
            }

            if (!string.IsNullOrEmpty(CbTOCategory))
            {
                tempfilter = $"AND TurnOverCat ={Utility.Evar(CbTOCategory, 1)}" + tempfilter;
            }

            if (!string.IsNullOrEmpty(feqclass))
            {
                tempfilter = $"AND UnitDescription = '{feqclass}'" + tempfilter;
            }

            return tempfilter;
        }
        public IActionResult LoadData(string fDocType, string CWOType, string CCompIDType, string fstoretype, string CbRepairAdvice, string tMaintType, string fstatusid,
            string fswo, string fsrepairby, string fsort, string fasc, string fdocno, string TPartID, string fstart, string fend, string feqclass, string freason, string fstore,
            string CbTOCategory, string CbPriority, string fisnull)
        {

            string filter = BuildTempFilter(fDocType, CWOType, CCompIDType, fstoretype, CbRepairAdvice, tMaintType, fstatusid, 
                                            fswo, fsrepairby, fsort, fdocno,
                                            fstart, fend, fstore, fasc, TPartID, 
                                            feqclass, freason, CbTOCategory, CbPriority, fisnull);

            string sortOrder = string.IsNullOrEmpty(fsort) ? "RegisterDate" : fsort;
            string ascdsc = string.IsNullOrEmpty(fasc) ? "desc" : fasc;

            _tempfilter = Utility.VarFilter(filter);

            string dataQuery = $"SELECT TOP 20 * from v_ExrJobDetail {_tempfilter} ORDER BY {sortOrder} {ascdsc}";
            var data = SQLFunction.execQuery(dataQuery);

            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                    requestp1 = Utility.CheckNull(row["RequestP1"]),
                    offsitewo = Utility.CheckNull(row["OffSiteWO"]),
                    unitnumber = Utility.CheckNull(row["UnitNumber"]),
                    unitdescription = Utility.CheckNull(row["UnitDescription"]),
                    status = Utility.CheckNull(row["Status"]),
                    compdesc = Utility.CheckNull(row["CompDesc"]),
                    mainttype = Utility.CheckNull(row["MaintType"]),
                    comptype = Utility.CheckNull(row["CompType"]),
                    repairadvice = Utility.CheckNull(row["RepairAdvice"]),
                    woalloc = Utility.CheckNull(row["WOAlloc"]),
                    siteallocname = Utility.CheckNull(row["SiteAllocName"]),
                    tcipartno = Utility.CheckNull(row["TCIPartNo"]),
                    supabbr = Utility.CheckNull(row["SupervisorAbbr"]),
                    supname = Utility.CheckNull(row["SupplierName"]),
                    lastchangedate = Utility.CheckNull(row["LastChangeDate"]),
                    lastchangeby = Utility.CheckNull(row["LastChangeBy"]),
                    remarkadvice = Utility.CheckNull(row["RemarkAdvice"]),
                    locationhold = Utility.CheckNull(row["LocationHold"])
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }
        private void LoadOption()
        {
            string queryStatusId = "SELECT ExrJobStatusID, ExrJobStatus, ExrJobStatusDesc FROM tbl_EXRJobStatus";
            ViewBag.StatusId = SQLFunction.execQuery(queryStatusId);

            string queryEqClass = "SELECT DISTINCT UnitDescription FROM v_ExrJobEquipment ORDER BY UnitDescription;";
            ViewBag.EqClass = SQLFunction.execQuery(queryEqClass);

            string queryReason = "SELECT DISTINCT ReasonTypeID, ReasonType, ReasonTypeDesc FROM tbl_EXRReasonType;";
            ViewBag.Reason = SQLFunction.execQuery(queryReason);

            string queryStore = "SELECT DISTINCT StoreID, StoreName FROM tbl_Store;";
            ViewBag.Store = SQLFunction.execQuery(queryStore);

            string queryCbToCategory = "SELECT TOCatID, TOCatDesc FROM tbl_TurnOverCat;";
            ViewBag.CbToCategory = SQLFunction.execQuery(queryCbToCategory);

            string queryMaintType = @"SELECT Distinct EXR.MaintType,MaintDesc as Description FROM tbl_ExrJobDetail as 
                                    EXR inner join tbl_MaintType as MT on EXR.MaintType=MT.MaintType where ID is not 
                                    null and RepairAdvice is not null AND EXR.MaintType <>'' and CompTypeID<>4 ORDER 
                                    BY MaintType";
            ViewBag.MaintType = SQLFunction.execQuery(queryMaintType);
        }
    }
}
