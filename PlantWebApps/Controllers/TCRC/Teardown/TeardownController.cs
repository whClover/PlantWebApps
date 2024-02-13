using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;

namespace PlantWebApps.Controllers.TCRC.Teardown
{
    public class TeardownController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/TCRC/Teardown/Index.cshtml");
        }

        [HttpGet]  
        public IActionResult LoadWO(String wo)
        {
            string query = "select *,convert(varchar,registerdate,103) as formregdate,convert(varchar,moddate,103) as formmoddate " +
                "from v_TeardownDetails where wono=" + Utility.Evar(wo, 1);
            var data = SQLFunction.executeQuery(query);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    wono = Utility.CheckNull(row["wono"]),
                    wodesc = Utility.CheckNull(row["WoDesc"]),
                    unitdesc = Utility.CheckNull(row["UnitDescription"]),
                    comp = Utility.CheckNull(row["Component"]),
                    workshop = Utility.CheckNull(row["workshop"]),
                    readsowcfi = Utility.CheckNull(row["ReadSOWCFI"]),
                    readswp = Utility.CheckNull(row["ReadSWP"]),
                    comptype = Utility.CheckNull(row["CompType"]),
                    compid = Utility.CheckNull(row["CompID"]),
                    jobid = Utility.CheckNull(row["JobID"]),
                    idtd = Utility.CheckNull(row["IDTD"]),
                    remark = Utility.CheckNull(row["Remarks"]),
                    regby = Utility.CheckNull(row["registerby"]),
                    regdate = Utility.CheckNull(row["formregdate"]),
                    modby = Utility.CheckNull(row["ModBy"]),
                    moddate = Utility.CheckNull(row["formmoddate"]),
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }

        [HttpGet]
        public IActionResult LoadMechTD(String wono)
        {
            string query = "select Distinct(FullName) from v_TCRPDailyTimeSheet where wono=" + Utility.Evar(wono, 1) + " and Description='Tear down' Order By FullName";
            var data = SQLFunction.executeQuery(query);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    fullname = Utility.CheckNull(row["FullName"]),
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }

        [HttpGet]
        public IActionResult checkcompid(String compid)
        {
            string query = "select wono from tbl_IntJobDetailx where wono=" + Utility.Evar(compid, 1) + " and JobStatusID='C'";
            var data = SQLFunction.executeQuery(query);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    wono = Utility.CheckNull(row["wono"]),
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }

        [HttpGet]
        public IActionResult loadpicture(String jobid, String type)
        {
            string query;
            if (type == "1")
            {
                query = "select replace(dbo.RemapPicCSharp_ver2(PicturePath),'\\','/') as PictPath,dbo.FileFileName(PicturePath) as FileName,IDPict from tbv_PictGenTeardown() " +
                "where PictureSection='BeforeDissassembly' and JobID=" + Utility.Evar(jobid, 0) + " and PictureStatus=1";
            } else
            {
                query = "select replace(dbo.RemapPicCSharp_ver2(PicturePath),'\\','/') as PictPath,dbo.FileFileName(PicturePath) as FileName,IDPict from tbv_PictGenTeardown() " +
                "where PictureSection='AfterDissassembly' and JobID=" + Utility.Evar(jobid, 0) + " and PictureStatus=1";
            }

            var data = SQLFunction.executeQuery(query);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    pictpath = Utility.CheckNull(row["PictPath"]),
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }

        [HttpGet]
        public IActionResult checkJP(String wono)
        {
            string query = "select ID from tbl_IntJobAttachment where wono=" + Utility.Evar(wono, 1) + " and IDFile=29";
            var data = SQLFunction.executeQuery(query);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }

        [HttpPost]
        public IActionResult savedata(String wono, String comptype, String compid, String remark)
        {
            string qry;
            if (comptype == "1")
            {
                qry = "update tbl_IntJobDetailx set CompID='OEM' where wono=" + Utility.Evar(wono, 1);
                
            }
            else if (comptype == "2")
            {
                qry = "update tbl_IntJobDetailx set CompID='BSF" + compid + "' where wono=" + Utility.Evar(wono, 1);

            } 
            else
            {
                qry = "update tbl_IntJobDetailx set CompID=" + Utility.Evar(compid, 1) + " where wono=" + Utility.Evar(wono, 1);

            }

            Console.WriteLine(qry);
            string query = "exec dbo.TeardownSubmit " + Utility.Evar(wono, 1) + "," + Utility.Evar(comptype, 1) + "," + Utility.Evar(remark, 1) + ",2," + Utility.ebyname();
            Console.WriteLine(query);

            return Ok();
        }
    }
}
