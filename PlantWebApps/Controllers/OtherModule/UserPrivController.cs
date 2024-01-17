using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;
using PlantWebApps.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using static System.Collections.Specialized.BitVector32;
using System;
using OfficeOpenXml.Drawing;
using System.Reflection;

namespace PlantWebApps.Controllers.OtherModule
{
    public class UserPrivController : Controller
    {
        [TempData]
        public String Msg { get; set; }
        [TempData]
        public String Stat { get; set; }

        private string _tempfilter;
        public IActionResult Index()
        {
            loadoption();
            return View("~/Views/OtherModule/userpriv/Index.cshtml");
        }
        public IActionResult Add() {
            loadoption();
            ViewBag.Stat = "Add";
            return View("~/Views/OtherModule/userpriv/Form.cshtml"); 
        }

        public IActionResult Edit(string id)
        {
            bool priv = Utility.checkgranted("frm_UserPrevillage", "2");
            if (priv == false)
            {
                Stat = "warning";
                Msg = "You dont have access to this function";
                return Redirect("/UserPriv");
            }

            loadoption();
            loadAccess(id);
            string query = "select * from tbl_User where UserID=" + Utility.Evar(id,1);
            ViewBag.data = SQLFunction.execQuery(query);

            return View("~/Views/OtherModule/userpriv/Form.cshtml");
        }
        public IActionResult Export()
        {
            BuildTempFilter();
            string dataQuery = "select DISTINCT JobCost,GroupName,UserID,UserName,FullName, SpvFullName from vw_TCRPUserInformation" + _tempfilter;
            DataTable data = SQLFunction.execQuery(dataQuery);
            string fileName = "User Priv.xlsx";
            return Utility.ExportDataTableToExcel(data, fileName);
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
                Stat = "success";
                Msg = "Data has been saved";
                return Redirect("/UserPriv/Edit/" + Request.Form["userid"]);
            } 
            else
            {
                query = "Insert into tbl_User (UserID,UserName,FullName,ShortName,SupervisorID,JobCost,GroupName,JobTitle," +
                    "Email,Previllege,CreateDate,CreateBy,ActiveStatus,LogInStatus,SectionID,NeverExpired,DatePassChanged," +
                    "CompAccess,TimeSheetAccess,ProductiveGroup) values (" + eUserID + "," + eusername + "," + eFullName + "," + eShortName +
                    "," + esupervisor + "," + eJobCost + "," + eGroupName + "," + eJobTitle + "," + eEmail + "," + ePrivilege + "," +
                    "getdate()," + Utility.Evar(Utility.eusername(), 1) + "," + status + ",NULL,NULL,NULL,GetDate()," + eCompAccess + "," +
                    eTimeSheetAccess + "," + eProductivityGroup + "); SELECT SCOPE_IDENTITY();";
                ViewBag.newID = SQLFunction.execQuery(query);

                Stat = "success";
                Msg = "Data has been saved";
                return Redirect("/UserPriv/Edit/" + ViewBag.newID);
            }
        }

        public IActionResult SaveAccess()
        {
            string huserid = Request.Form["huserid"].ToString();
            string smodule = Request.Form["smodule"].ToString();

            string q_select = "select GrName from tbl_UserGrantedModule where GRForm=" + Utility.Evar(smodule,1);
            var data = SQLFunction.execQuery(q_select);
            string grName = data.Rows[0]["GrName"].ToString();

            string sopen = Request.Form["sopen"].ToString();
            string sadd = Request.Form["sadd"].ToString();
            string sedit = Request.Form["sedit"].ToString();
            string sdelete = Request.Form["sdelete"].ToString();

            //Console.WriteLine(userId); OK
            string query = "exec save_UserGranted " + Utility.Evar(huserid, 1) + "," + Utility.Evar(smodule, 1) + "," + Utility.Evar(grName, 1) +
                "," + Utility.Evar(sopen, 1) + "," + Utility.Evar(sedit, 1) + "," + Utility.Evar(sadd, 1) + "," + Utility.Evar(sdelete, 1) + ",'Component Database'";
            SQLFunction.execQuery(query);

            return Redirect("/UserPriv/Edit/" + huserid);
        }

        public IActionResult EditAccess(int id)
        {
            string query = "select * from tbl_UserGranted where GrID=" + id;
            var data = SQLFunction.execQuery(query);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    grform = Utility.CheckNull(row["GrForm"]),
                    gropen = Utility.CheckNull(row["GrOpen"]).ToString() == "True" ? "1" : "0",
                    gradd = Utility.CheckNull(row["GrAdd"]).ToString() == "True" ? "1" : "0",
                    gredit = Utility.CheckNull(row["GrEdit"]).ToString() == "True" ? "1" : "0",
                    grdelete = Utility.CheckNull(row["GrDelete"]).ToString() == "True" ? "1" : "0"
                };
                rows.Add(rowData);
            }

            return new JsonResult(rows);
        }

        public ActionResult DeleteAccess(int id)
        {
            string query = "delete from tbl_UserGranted where GrID=" + id;
            SQLFunction.execQuery(query);

            return Json(new { success = true });
        }

        public IActionResult AddProfile()
        {
            string huserid = Request.Form["huserid2"].ToString();
            string profileid = Request.Form["profileid"].ToString();
            string query = "exec dbo.AddUserProfile " + Utility.Evar(huserid,1) + "," + Utility.Evar(profileid,1) + "," + Utility.Evar(Utility.eusername(),1);
            SQLFunction.execQuery(query);

            return Redirect("/UserPriv/Edit/" + huserid);
        }

        public ActionResult ResetPassword(string newPassword, string userid)
        {
            string query = "Update tbl_User set Pass=dbo.EncryptPass(" + Utility.Evar(newPassword, 1) + ") where UserID=" + Utility.Evar(userid, 1);
            SQLFunction.execQuery(query);

            return Redirect("/UserPriv/Edit/" + userid);
        }

        private void loadAccess(string userID)
        {
            string dataQuery = "SELECT GrForm, GrDesc, GrOpen, grAdd, GrEdit,GrDelete, GrID FROM tbl_UserGranted where GrUSerID=" + Utility.Evar(userID, 1) + "";
            ViewBag.dataaccess = SQLFunction.execQuery(dataQuery);
        } 

        [HttpGet]
        public IActionResult LoadData(string sectionid, string jobcost, string group, string userID, string username, string fullname, string jobtitle, string prodgp, string active)
        {
            string tempfilter = string.Empty;
            if (!string.IsNullOrEmpty(sectionid))
            {
                tempfilter = " and sectionid=" + Utility.Evar(sectionid, 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(jobcost))
            {
                tempfilter = " and JobCost like " + Utility.Evar(jobcost, 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(group))
            {
                tempfilter = " and GroupName=" + Utility.Evar(group, 1) + tempfilter;
            }

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

            if (!string.IsNullOrEmpty(jobtitle))
            {
                tempfilter = " and jobtitle like" + Utility.Evar(jobtitle, 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(prodgp))
            {
                tempfilter = " and ProductiveGroup=" + Utility.Evar(prodgp, 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(active))
            {
                tempfilter = " and ActiveStatus=" + Utility.Evar(active, 1) + tempfilter;
            }

            tempfilter = Utility.VarFilter(tempfilter);
            Console.WriteLine(tempfilter);
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

        private void BuildTempFilter()
        {
            string tempfilter = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["sectionid"]))
            {
                tempfilter = " and sectionid=" + Utility.Evar(Request.Form["sectionid"], 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(Request.Form["jobcost"]))
            {
                tempfilter = " and JobCost like " + Utility.Evar(Request.Form["jobcost"], 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(Request.Form["group"]))
            {
                tempfilter = " and GroupName=" + Utility.Evar(Request.Form["group"], 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(Request.Form["userID"]))
            {
                tempfilter = " and userid like" + Utility.Evar(Request.Form["userID"], 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(Request.Form["username"]))
            {
                tempfilter = " and username like" + Utility.Evar(Request.Form["username"], 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(Request.Form["fullname"]))
            {
                tempfilter = " and fullname like" + Utility.Evar(Request.Form["fullname"], 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(Request.Form["jobtitle"]))
            {
                tempfilter = " and jobtitle like" + Utility.Evar(Request.Form["jobtitle"], 11) + tempfilter;
            }

            if (!string.IsNullOrEmpty(Request.Form["prodgp"]))
            {
                tempfilter = " and ProductiveGroup=" + Utility.Evar(Request.Form["prodgp"], 1) + tempfilter;
            }

            if (!string.IsNullOrEmpty(Request.Form["active"]))
            {
                tempfilter = " and ActiveStatus=" + Utility.Evar(Request.Form["active"], 1) + tempfilter;
            }

            _tempfilter = Utility.VarFilter(tempfilter);
        }

        private void loadoption()
        {
            //Section ID
            string querysection = "SELECT JobSourceID, JobSource FROM tbl_JobSource";
            ViewBag.sectionid = SQLFunction.execQuery(querysection);

            //Job Cost
            string queryjobcost = "Select Location,LocationName from tbl_Location where Labcost='1'";
            ViewBag.jobcost = SQLFunction.execQuery(queryjobcost);

            //Supervisor
            string querysupv = "SELECT UserID, FullName FROM tbl_User ORDER BY FullName";
            ViewBag.supv = SQLFunction.execQuery(querysupv);

            //Module
            string querymodule = "SELECT GrForm,GrName from tbl_UserGrantedModule Where Active IN (1,-1)";
            ViewBag.module = SQLFunction.execQuery(querymodule);

            //Profile
            string queryprofile = "Select distinct P.profileCode,C.ProfileName from tbl_UserGrantedProfile as P inner join tbl_UserGrantedProfileCode as C on C.ProfileCode=P.ProfileCode";
            ViewBag.profile = SQLFunction.execQuery(queryprofile);
        }
    }
}
