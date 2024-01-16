using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;

namespace PlantWebApps.Controllers.TCRC.JobDetails
{
    public class JobDetailsController : Controller
    {
        [TempData]
        public String Msg { get; set; }
        [TempData]
        public String Stat { get; set; }

        public IActionResult Index()
        {
            loadoption();
            return View("~/Views/TCRC/JobDetails/Index.cshtml");
        }

        public IActionResult Edit()
        {
            return View("~/Views/TCRC/JobDetails/Form.cshtml");
        }

        [HttpGet]
        public IActionResult loadDataByID(String id)
        {
            string query = "select * from tbl_intJobDetailx where wono=" + Utility.Evar(id, 1);
            var data = SQLFunction.execQuery(query);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                    storeid = Utility.CheckNull(row["StoreID"]),
                    jobstatusid = Utility.CheckNull(row["JobStatusID"]),
                    parentwo = Utility.CheckNull(row["ParentWO"]),
                    currentlocation = Utility.CheckNull(row["CurrentLocation"]),
                    unitnumber = Utility.CheckNull(row["UnitNumber"]),
                    unitdesc = Utility.CheckNull(row["UnitDescription"]),
                    location = Utility.CheckNull(row["Location"]),
                    exunit = Utility.CheckNull(row["ExUnit"]),
                    worktypeid = Utility.CheckNull(row["WorkTypeID"]),
                    jobpriority = Utility.CheckNull(row["JobPriority"]),
                    tcipartno = Utility.CheckNull(row["TCIPartNo"]),
                    equipclass = Utility.CheckNull(row["EquipClass"]),
                    stand = Utility.CheckNull(row["Stand"]),
                    tagremark = Utility.CheckNull(row["TagRemark"]),
                    jobdesc = Utility.CheckNull(row["JobDesc"]),
                    repairtype = Utility.CheckNull(row["RepairType"]),
                    lifetype = Utility.CheckNull(row["Lifetype"]),
                    lifecost = Utility.CheckNull(row["lifeCost"]),
                    currentwdv = Utility.CheckNull(row["currentwdv"]),
                    datesendan = Utility.CheckNull(row["DateSendAN"]),
                    arrivalan = Utility.CheckNull(row["ArrivalAN"]),
                    jpdatetime = Utility.CheckNull(row["JPDateTime"]),
                    accepteddate = Utility.CheckNull(row["AcceptedDate"]),
                    quoteno = Utility.CheckNull(row["QuoteNo"]),
                    quoteno = Utility.CheckNull(row["QuoteNo"]),
                };
                rows.Add(rowData);
            }

            return new JsonResult(rows);
        }

        private void loadoption()
        {
            string querymaintid = "Select distinct LEFT(MaintType,5) as CompType,isnull(MaintIDDesc,LEFT(MaintType,5)) as MaintIDDesc from v_WorkOrderJObDetail as  W left join tbl_MaintBase as M on LEFT(W.MaintType,5) =M.MaintID where MaintType is NOt Null order by LEFT(MaintType,5) ";
            ViewBag.maintid = SQLFunction.execQuery(querymaintid);

            string queryunitdesc = "SELECT DISTINCT UnitDescription FROM v_WorkOrderJObDetail Where UnitDescription is Not NULL ORDER BY UnitDescription";
            ViewBag.unitdesc = SQLFunction.execQuery(queryunitdesc);

            string queryprio = "Select * from tbl_IntRepairPriority";
            ViewBag.prio = SQLFunction.execQuery(queryprio);

            string querycurloc = "SELECT Location, LocationName FROM tbl_Location";
            ViewBag.curloc = SQLFunction.execQuery(querycurloc);

            string querypic = "SELECT EmailTypeID, Description, JP FROM tbl_EmailType WHERE (((JP)=1)); ";
            ViewBag.pic = SQLFunction.execQuery(querypic);

            string queryworktype = "SELECT WorkTypeID, WorkTypeAbr, WorkTypeName FROM tbl_WorkType";
            ViewBag.worktype = SQLFunction.execQuery(queryworktype);

            string queryjobstatus = "SELECT JobStatusID, JobStatus FROM tbl_IntJobStatus ORDER BY StatusOrder";
            ViewBag.jobstatus = SQLFunction.execQuery(queryjobstatus);

            string queryactivity = "Select Description from tbl_WOrkStatusTCRP Where WorkStatusType in ('IDLE','WORK I','DONE')  AND WorkStatusID Not IN (18) ORDER by WorkStatusType ASC , Description ASC ";
            ViewBag.activity = SQLFunction.execQuery(queryactivity);

            string querywostat = "select Stat,StatDesc from tbl_WOStatus Where left(Stat,1)='M'";
            ViewBag.wostat = SQLFunction.execQuery(querywostat);
        }

        [HttpGet]
        public IActionResult LoadData(String swono, String twono, String twodesc, String sdate, String tdatestart, String tdateend, String[] twostat, String tunitdesc, String tmaintid,
            String tpriority, String tcurrentloc, String tpic, String twotype, String[] tjobstat, String[] tactivity, String tsortby, String tsorttype)
        {
            string tempfilter = string.Empty;
            string temporderby = string.Empty;
            char[] chartsToTrim = { ',', '.', ' ' };

            if (!string.IsNullOrEmpty(twono)) tempfilter = " and " + swono + "=" + Utility.Evar(twono, 1) + tempfilter;
            if (!string.IsNullOrEmpty(twodesc)) tempfilter = " and WODesc like" + Utility.Evar(twodesc, 11) + tempfilter;
            if (!string.IsNullOrEmpty(tdatestart)) tempfilter = " and " + sdate + ">=" + Utility.Evar(tdatestart, 2) + tempfilter;
            if (!string.IsNullOrEmpty(tdateend)) tempfilter = " and " + sdate + "<=" + Utility.Evar(tdateend, 2) + tempfilter;
            if (twostat.Length > 0)
            {
                String swostat = "";
                foreach (String s in twostat)
                {
                    swostat += "'" + s + "',";
                }
                tempfilter = " and WOStatus in(" + swostat.TrimEnd(chartsToTrim) + ")" + tempfilter;
            }
            if (!string.IsNullOrEmpty(tunitdesc)) tempfilter = " and UnitDescription=" + Utility.Evar(tunitdesc, 1) + tempfilter;
            if (!string.IsNullOrEmpty(tmaintid)) tempfilter = " and MaintID=" + Utility.Evar(tmaintid, 1) + tempfilter;
            if (!string.IsNullOrEmpty(tpriority)) tempfilter = " and JobPriority=" + Utility.Evar(tpriority, 1) + tempfilter;
            if (!string.IsNullOrEmpty(tcurrentloc)) tempfilter = " and CurrentLocation=" + Utility.Evar(tcurrentloc, 1) + tempfilter;
            if (!string.IsNullOrEmpty(tpic)) tempfilter = " and JobStatusPICID=" + Utility.Evar(tpic, 1) + tempfilter;
            if (!string.IsNullOrEmpty(twotype)) tempfilter = " and WorkTypeID=" + Utility.Evar(twotype, 1) + tempfilter;
            if (tjobstat.Length > 0)
            {
                String sjobstat = "";
                foreach (String s in tjobstat)
                {
                    sjobstat += "'" + s + "',";
                }
                tempfilter = " and JobStatusID in(" + sjobstat.TrimEnd(chartsToTrim) + ")" + tempfilter;
            }
            if (tactivity.Length > 0)
            {
                String sactivity = "";
                foreach (String s in tactivity)
                {
                    sactivity += "'" + s + "',";
                }
                tempfilter = " and LastActivity in(" + sactivity.TrimEnd(chartsToTrim) + ")" + tempfilter;
            }
            temporderby = " Order by " + tsortby + " " + tsorttype;
            tempfilter = Utility.VarFilter(tempfilter);
            string query = "select *,convert(varchar,createddate,103) as formcreateddate from v_IntJobDetailRev3" + tempfilter + temporderby;
            var data = SQLFunction.execQuery(query);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    editcmd = "<a class='btn btn-soft-light btn-sm' onclick=\"editData()\"><i class='fa fa-edit'></i></a>",
                    id = Utility.CheckNull(row["ID"]),
                    worktypeabr = Utility.CheckNull(row["WorkTypeAbr"]),
                    wono = Utility.CheckNull(row["WONo"]),
                    parentwo = Utility.CheckNull(row["ParentWO"]),
                    wodesc = Utility.CheckNull(row["WoDesc"]),
                    mainttype = Utility.CheckNull(row["MaintType"]),
                    stand = Utility.CheckNull(row["Stand"]),
                    wostatus = Utility.CheckNull(row["WOStatus"]),
                    woage = Utility.CheckNull(row["WOAge"]),
                    jobstatusdesc = Utility.CheckNull(row["JobStatusDesc"]),
                    lastactivity = Utility.CheckNull(row["LastActivity"]),
                    subdescription = Utility.CheckNull(row["SubDescription"]),
                    jobpriority = Utility.CheckNull(row["JobPriority"]),
                    jobstatuspic = Utility.CheckNull(row["JobStatusPIC"]),
                    remark = Utility.CheckNull(row["Remark"]),
                    createddate = Utility.CheckNull(row["formcreateddate"]),
                };
                rows.Add(rowData);
            }

            return new JsonResult(rows);
        }
    }
}
