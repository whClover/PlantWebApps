using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using PlantWebApps.Helper;
using System.Data;
using System.Security.Cryptography;

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
                    idpict = Utility.CheckNull(row["IDPict"]),
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

            SQLFunction.executeQuery(qry);
            string query = "exec dbo.TeardownSubmit " + Utility.Evar(wono, 1) + "," + Utility.Evar(comptype, 1) + "," + Utility.Evar(remark, 1) + ",2," + Utility.ebyname();
            SQLFunction.executeQuery(query);

            var response = new
            {
                message = "Data has been saved"
            };

            return Json(response);
        }

        [HttpGet]
        public IActionResult DownloadSOWCFI(String type, String wono)
        {
            string qry;
            if (type == "1")
            {
                qry = "SELECT Replace(Replace(dbo.RemapJP(FilePath),'/JobPackage',''),'\','/') as filepath " +
                    "from tbl_IntJobAttachment Where IDFile=3 AND WONO=" + Utility.Evar(wono, 1);
            } 
            else
            {
                qry = "SELECT Replace(Replace(dbo.RemapJP(FilePath),'/JobPackage',''),'\','/') as filepath " +
                    "from tbl_IntJobAttachment Where IDFile=4 AND WONO=" + Utility.Evar(wono, 1);
            }

            var data = SQLFunction.executeQuery(qry);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    filepath = Utility.CheckNull(row["filepath"]),
                };
                rows.Add(rowData);
            }

            string query = "update tbl_TCRCTearDownDetails set ReadSOWCFI=1,ReadBySOWCFI=" + Utility.ebyname() + " " +
                    "where JobID=(select ID from tbl_intjobdetailx where wono=" + Utility.Evar(wono, 1) + ")";
            SQLFunction.executeQuery(query);

            return new JsonResult(rows);
        }

        [HttpGet]
        public IActionResult DownloadSWP(String wono)
        {
            string query = "select Replace(dbo.RemapPicCSharp_ver2(dbo.SharepointLinkByWO(" + Utility.Evar(wono, 1) + ",1)), '\','/') as LinkFile";
            var data = SQLFunction.executeQuery(query);
            var rows = new List<object>();

            foreach (DataRow row in data.Rows)
            {
                var rowData = new
                {
                    filepath = Utility.CheckNull(row["LinkFile"]),
                };
                rows.Add(rowData);
            }

            string qry = "update tbl_TCRCTearDownDetails set ReadSWP=1,ReadBySWP=" + Utility.ebyname() + " " +
                    "where JobID=(select ID from tbl_intjobdetailx where wono=" + Utility.Evar(wono, 1) + ")";
            SQLFunction.executeQuery(qry);

            return new JsonResult(rows);
        }

        [HttpPost]
        [Route("/Teardown/UploadBefore")]
        public async Task<IActionResult> UploadBefore(List<IFormFile> uploadbefore, string ejobid, string eidtd)
        {
            var targetDirectory = "";
            string fileExtension = "";
            string fileName = "";
            foreach (var file in uploadbefore)
            {
                fileExtension = Path.GetExtension(file.FileName);
                fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string currentTime = DateTime.Now.ToString("HHmmss");
                fileName = $"{fileName}_{currentTime}{fileExtension}";
                targetDirectory = $"{GlobalString.path_dbattach}/{ejobid}/Teardown/BeforeDissassembly/{fileName}";

                using (var stream = new FileStream(targetDirectory, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string query = "insert into tbl_GeneralPicture(IDReff,PictureSection,PicturePath,PictureStatus,ModuleID,JobID) values " +
                    "(" + eidtd + ",'BeforeDissassembly'," + Utility.Evar(targetDirectory, 1) + ",1,1," + ejobid + ")";
                SQLFunction.executeQuery(query);
            }

            var response = new
            {
                message = "Data has been saved"
            };

            return Json(response);
        }

        [HttpPost]
        [Route("/Teardown/UploadAfter")]
        public async Task<IActionResult> UploadAfter(List<IFormFile> uploadafter, string ejobid, string eidtd)
        {
            var targetDirectory = "";
            string fileExtension = "";
            string fileName = "";
            foreach (var file in uploadafter)
            {
                fileExtension = Path.GetExtension(file.FileName);
                fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string currentTime = DateTime.Now.ToString("HHmmss");
                fileName = $"{fileName}_{currentTime}{fileExtension}";
                targetDirectory = $"{GlobalString.path_dbattach}/{ejobid}/Teardown/AfterDissassembly/{fileName}";

                using (var stream = new FileStream(targetDirectory, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string query = "insert into tbl_GeneralPicture(IDReff,PictureSection,PicturePath,PictureStatus,ModuleID,JobID) values " +
                    "(" + eidtd + ",'AfterDissassembly'," + Utility.Evar(targetDirectory, 1) + ",1,1," + ejobid + ")";
                SQLFunction.executeQuery(query);
            }

            var response = new
            {
                message = "Data has been saved"
            };

            return Json(response);
        }

        [HttpPost]
        public IActionResult DeletePicture(String eidpict) 
        {
            string query = "update tbl_GeneralPicture set PictureStatus=0,DeactiveBy=" + Utility.ebyname() + ",DeactiveDate=GetDate() where IDPict=" + eidpict;
            //Console.WriteLine(query);
            SQLFunction.executeQuery(query);

            var response = new
            {
                message = "Data has been saved"
            };

            return Json(response);
        }

        public IActionResult PrintReport(String id)
        {
            string query = "select *," +
                "CASE WHEN CompType = 1 THEN 'OEM' WHEN CompType = 2 THEN 'TCRC' ELSE 'Other' END AS CompSource," +
                "replace(dbo.RemapPicCSharp_ver2(PicturePath),'\\','/') as PictPath " +
                "from fr_TeardownReport(" + id + ") order by PictureSection";
            Console.WriteLine(query);
            ViewBag.Data = SQLFunction.executeQuery(query);

            return View("~/Views/TCRC/Teardown/TeardownReport.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> UploadJP(IFormFile file, string wono)
        {
            string fileExtension = "";
            string fileName = "";
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was submitted.");
            }

            fileExtension = Path.GetExtension(file.FileName);
            fileName = Path.GetFileNameWithoutExtension(file.FileName);
            fileName = "29_TeardownReport" + fileExtension;
            string targetDirectory = GlobalString.path_JobPackage + wono + "/29/" + fileName;
            using (var stream = new FileStream(targetDirectory, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string query = "exec dbo.InsertAttachmentJP " + Utility.Evar(wono, 1) + ",'29'," + Utility.Evar(targetDirectory, 1) + ",'29_TeardownReport.pdf','1'," + Utility.ebyname();
            SQLFunction.executeQuery(query);

            var response = new
            {
                message = "Data has been saved"
            };

            return Json(response);
        }
    }
}
