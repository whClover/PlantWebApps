using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;

namespace PlantWebApps.Controllers.OtherModule
{
    public class UserPrivController : Controller
    {
        [TempData]
        public String Msg { get; set; }
        [TempData]
        public String Stat { get; set; }

        public IActionResult Index()
        {
            return View("~/Views/OtherModule/userpriv/Index.cshtml");
        }

        public IActionResult Add() {
            loadoption();
            return View("~/Views/OtherModule/userpriv/Form.cshtml"); 
        }

        public IActionResult Edit(string id)
        {
            loadoption();
            loadAccess(id);
            string query = "select * from tbl_User where UserID=" + Utility.Evar(id,1);
            ViewBag.data = SQLFunction.execQuery(query);

            return View("~/Views/OtherModule/userpriv/Form.cshtml");
        }

        public IActionResult Save()
        {
            var query = "";
            var querycheck = "";
            var eUserID = Utility.Evar(Request.Form["userid"], 1);
            var eusername = Utility.Evar(Request.Form["username"], 1);
            var eFullName = Utility.Evar(Request.Form["fullname"], 1);
            var eShortName = Utility.Evar(Request.Form["shortname"], 1);
            var esupervisor = Utility.Evar(Request.Form["supervisorid"], 1);
            var eGroupName = Utility.Evar(Request.Form["groupname"], 1);
            var eJobCost = Utility.Evar(Request.Form["jobcost"], 1);
            var eJobTitle = Utility.Evar(Request.Form["jobtitle"], 1);
            var eEmail = Utility.Evar(Request.Form["email"], 1);
            var status = Utility.Evar(Request.Form["status"], 1);
            var ePrivilege = Utility.Evar(Request.Form["priv"], 1);
            var eCompAccess = Utility.Evar(Request.Form["compaccess"], 1);
            var eTimeSheetAccess = Utility.Evar(Request.Form["timesheetaccess"], 1);
            var eProductivityGroup = Utility.Evar(Request.Form["prodgp"], 1);

            querycheck = "select userid from tbl_user where userid=" + eUserID;
            var result = SQLFunction.execQuery(querycheck);

            if (result != null && result.Rows.Count > 0)
            {
                query = "Update tbl_User set UserName=" + eusername + ",FullName=" + eFullName + "," +
                    "ShortName=" + eShortName + ",SupervisorID=" + esupervisor + ",JobCost=" + eJobCost + "," +
                    "GroupName=" + eGroupName + ",JobTitle=" + eJobTitle + ",Email=" + eEmail + ",Previllege=" + ePrivilege + "," +
                    "ModDate=getDate(),ModBy=" + Utility.Evar(Utility.eusername(), 1) + ",ActiveStatus=" + status + "," +
                    "LogInStatus=NULL,SectionID=NULL,NeverExpired=NULL,CompAccess=" + eCompAccess + "," +
                    "TimeSheetAccess=" + eTimeSheetAccess + ",ProductiveGroup=" + eProductivityGroup
                    + " where UserID=" + eUserID + "";
                SQLFunction.execQuery(query);
            } 
            else
            {
                query = "Insert into tbl_User (UserID,UserName,FullName,ShortName,SupervisorID,JobCost,GroupName,JobTitle," +
                    "Email,Previllege,CreateDate,CreateBy,ActiveStatus,LogInStatus,SectionID,NeverExpired,DatePassChanged," +
                    "CompAccess,TimeSheetAccess,ProductiveGroup) values (" + eUserID + "," + eusername + "," + eFullName + "," + eShortName +
                    "," + esupervisor + "," + eJobCost + "," + eGroupName + "," + eJobTitle + "," + eEmail + "," + ePrivilege + "," +
                    "getdate()," + Utility.Evar(Utility.eusername(), 1) + "," + status + ",NULL,NULL,GetDate()," + eCompAccess + "," +
                    eTimeSheetAccess + "," + eProductivityGroup + ")";
                SQLFunction.execQuery(query);
            }

            Stat = "success";
            Msg = "Data has been saved";

            return RedirectToAction(nameof(Index));
        }

        public void SaveAccess()
        {
            string userId = Request.Form["userid"].ToString();
            //Edit(userId);
            Console.WriteLine(userId);
        }

        private void loadAccess(string userID)
        {
            string dataQuery = "SELECT GrForm, GrDesc, GrOpen, grAdd, GrEdit,GrDelete FROM tbl_UserGranted where GrUSerID=" + Utility.Evar(userID, 1) + "";
            ViewBag.dataaccess = SQLFunction.execQuery(dataQuery);
        } 

        [HttpGet]
        public IActionResult LoadData(string userID, string username, string fullname)
        {
            string tempfilter = string.Empty;
            if (!string.IsNullOrEmpty(userID))
            {
                tempfilter = " and userid like" + Utility.Evar(userID, 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(username))
            {
                tempfilter = " and username like" + Utility.Evar(username, 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(fullname))
            {
                tempfilter = " and fullname like" + Utility.Evar(fullname, 11) + tempfilter;
            }

            tempfilter = Utility.VarFilter(tempfilter);

            string dataQuery = "SELECT * FROM vw_TCRPUserInformation" + tempfilter;

            var data = SQLFunction.execQuery(dataQuery);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {

                var status = Utility.CheckNull(row["ActiveStatus"]);
                if (status == "0")
                {
                    status = "<span class='badge rounded-pill bg-danger'>In-Active</span>";
                }
                else
                {
                    status = "<span class='badge rounded-pill bg-success'>Active</span>";
                }

                var productivegp = Utility.CheckNull(row["ProductiveGroup"]);
                if (productivegp == "True") {
                    productivegp = "<span class='badge rounded-pill bg-success'>Productive</span>";
                }
                else
                {
                    productivegp = "<span class='badge rounded-pill bg-primary'>Non-Productive</span>";
                }

                var rowData = new
                {
                    section = Utility.CheckNull(row["Section"]),
                    jobcost = Utility.CheckNull(row["JobCost"]),
                    groupname = Utility.CheckNull(row["GroupName"]),
                    userID = Utility.CheckNull(row["UserID"]),
                    username = Utility.CheckNull(row["Username"]),
                    fullname = Utility.CheckNull(row["Fullname"]),
                    productivegp = productivegp,
                    status = status,
                    jobtitle = Utility.CheckNull(row["JobTitle"]),
                    edit = "<a class='btn btn-link btn-sm' href='/UserPriv/Edit/" + row["UserID"] + "'><i class='fa fa-edit'></i></a>"
                };

                rows.Add(rowData);
            }

            return new JsonResult(rows);
        }

        private void loadoption()
        {
            //Job Cost
            string queryjobcost = "Select Location,LocationName from tbl_Location where Labcost='1'";
            ViewBag.jobcost = SQLFunction.execQuery(queryjobcost);

            //Supervisor
            string querysupv = "SELECT UserID, FullName FROM tbl_User ORDER BY FullName";
            ViewBag.supv = SQLFunction.execQuery(querysupv);

            //Module
            string querymodule = "SELECT GrForm,GrName from tbl_UserGrantedModule Where Active IN (1,-1)";
            ViewBag.module = SQLFunction.execQuery(querymodule);
        }
    }
}
