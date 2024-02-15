using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;

namespace PlantWebApps.Controllers.PER.ExrRepairJobHistory
{
    public class ExrRepairJobHistoryInspection : Controller
    {
        private string _tempfilter;
        [TempData]
        public String Msg { get; set; }
        [TempData]
        public String Stat { get; set; }
        public IActionResult OldCore(string id)
        {
            string tNoReg = Utility.Evar(id, 0);
            string eUserName = Utility.Evar(Utility.eusername(), 1);

            string query = $@"select * from tv_ExrOldCoreInspection({tNoReg}, {eUserName})";
            Console.WriteLine(query);

            ViewBag.Data = SQLFunction.execQuery(query);

            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/OldCoreInvestigation.cshtml");
        }
        public IActionResult FinalInspection(string id)
        {
            string tNoReg = Utility.Evar(id, 0);
            string eUserName = Utility.Evar(Utility.eusername(), 1);

            string query = $@"select * from tv_ExrFinalInspectionResult({tNoReg}, {eUserName})";

            ViewBag.Data = SQLFunction.execQuery(query);

            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/FinalInspection.cshtml");
        }
        
        public IActionResult Save(string eID, string eJobID, string eWono, string eNoRegister,
            string eCheck1, string eCheck2, string eCheck3, string eCheck4a, string eCheck4b,
            string eCheck4c, string eCheck4d, string eCheck5a, string eCheck5b, string eCheck5c,
            string eCheck6, string eCheck7, string eCheckAccept, string eCheckReject, string eRemark,
            string eIssuedSign, string eIssuedDate, string eAcknowledgedSign, string eAcknowledgedDate)
        {
            string xID = (Utility.Evar(eID, 0));
            string xJobID = (Utility.Evar(eJobID, 0));
            string xWono = (Utility.Evar(eWono, 1));
            string xNoRegister = (Utility.Evar(eNoRegister, 0));
            string xCheck1 = (Utility.Evar(eCheck1, 0));
            string xCheck2 = (Utility.Evar(eCheck2, 0));
            string xCheck3 = (Utility.Evar(eCheck3, 0));
            string xCheck4a = (Utility.Evar(eCheck4a, 0));
            string xCheck4b = (Utility.Evar(eCheck4b, 0));
            string xCheck4c = (Utility.Evar(eCheck4c, 0));
            string xCheck4d = (Utility.Evar(eCheck4d, 0));
            string xCheck5a = (Utility.Evar(eCheck5a, 0));
            string xCheck5b = (Utility.Evar(eCheck5b, 0));
            string xCheck5c = (Utility.Evar(eCheck5c, 0));
            string xCheck6 = (Utility.Evar(eCheck6, 0));
            string xCheck7 = (Utility.Evar(eCheck7, 0));
            string xCheckAccept = (Utility.Evar(eCheckAccept, 0));
            string xCheckReject = (Utility.Evar(eCheckReject, 0));
            string xRemark = (Utility.Evar(eRemark, 1));
            string xIssuedSign = (Utility.Evar(eIssuedSign, 1));
            string xIssuedDate = (Utility.Evar(eIssuedDate, 2));
            string xAcknowledgedSign = eAcknowledgedSign;
            string xAcknowledgedDate = (Utility.Evar(eAcknowledgedDate, 2));

            string query = $@"exec dbo.ExrFinalInspect {xID}, {xJobID}, {xWono}, {xNoRegister}, 
			{xCheck1}, {xCheck2}, {xCheck3}, {xCheck4a}, {xCheck4b}, {xCheck4c}, {xCheck4d}, {xCheck5a}, 
			{xCheck5b}, {xCheck5c}, {xCheck6}, {xCheck7}, {xRemark}, {xCheckAccept}, {xCheckReject},  {xIssuedSign}, 
			{xIssuedDate}, '{xAcknowledgedSign}', {xAcknowledgedDate}, {Utility.Evar(Utility.eusername(), 1)}";

            Console.WriteLine(query);
            SQLFunction.execQuery(query);
            return Redirect("/ExrRepairJobHistoryInspection/FinalInspection/" + eJobID);
        }
        public IActionResult Delete(string id)
        {

            string query = $"exec dbo.ExrOldCoreInspectPicDelete {Utility.Evar(id, 0)}, {Utility.Evar(Utility.eusername(), 1)}";
            Console.WriteLine(query);

            SQLFunction.execQuery(query);

            Stat = "success";
            Msg = "Data Has Been Deleted";

            return Json(new { success = true });
        }
        public IActionResult FinalReportHeader()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/ReportHeader.cshtml");
        }
        public IActionResult FinalReportBody(string ID)
        {
            FinalInspection(ID);
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/ReportBody.cshtml");
        }
        public IActionResult FinalReportFooter()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/ReportFooter.cshtml");
        }

        public IActionResult DataFinalReportPictBody(string ID)
        {
            Console.WriteLine("wono is" + ID);
            string placeholder = "https://placekitten.com/200/300";
            string query = @$"Select ID,JobID,WoNO,IDReg,PictureCaption,picturepath,inspectType 
			from tblv_ExrOldCoreInspAttachPic where InspectType='FinalInspect' AND WONO={Utility.Evar(ID, 1)}";
            Console.WriteLine(query);
            ViewBag.data = SQLFunction.execQuery(query);
            ViewBag.placeholder = placeholder;
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/PictBody.cshtml");
        }
        public IActionResult FinalReportPictHeader(string ID)
        {
            DataFinalReportPictBody(ID);
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/PictHeader.cshtml");
        }
        public IActionResult FinalReportPictBody(string ID)
        {
            DataFinalReportPictBody(ID);
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/PictBody.cshtml");
        }
        public IActionResult FinalReportPictFooter()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/PictFooter.cshtml");
        }

        
        public IActionResult SaveOld(string eID, string eJobID, string eWono,
            string eCheck1a, string eCheck1b, string eCheck1c, string eCheck1d, string eCheck2a,
            string eCheck2b, string eCheck2c, string eCheck3, string eInspectResult, string eRemark,
            string eIssuedSign, string eIssuedDate, string eAcknowledgedSign,
            string eAcknowledgedDate)
        {
            string xID = (Utility.Evar(eID, 0));
            string xJobID = (Utility.Evar(eJobID, 0));
            string xWono = (Utility.Evar(eWono, 1));
            string xCheck1a = (Utility.Evar(eCheck1a, 0));
            string xCheck1b = (Utility.Evar(eCheck1b, 0));
            string xCheck1c = (Utility.Evar(eCheck1c, 0));
            string xCheck1d = (Utility.Evar(eCheck1d, 0));
            string xCheck2a = (Utility.Evar(eCheck2a, 0));
            string xCheck2b = (Utility.Evar(eCheck2b, 0));
            string xCheck2c = (Utility.Evar(eCheck2c, 0));
            string xCheck3 = (Utility.Evar(eCheck3, 0));
            string xInspectResult = (Utility.Evar(eInspectResult, 0));
            string xRemark = (Utility.Evar(eRemark, 1));
            string xIssuedSign = (Utility.Evar(eIssuedSign, 1));
            string xIssuedDate = (Utility.Evar(eIssuedDate, 2));
            string xAcknowledgedSign = eAcknowledgedSign;
            string xAcknowledgedDate = (Utility.Evar(eAcknowledgedDate, 2));

            string query = $@"exec dbo.ExrOldCoreInspect {xID}, {xJobID}, {xWono},
			{xCheck1a}, {xCheck1b}, {xCheck1c}, {xCheck1d}, {xCheck2a}, {xCheck2b}, {xCheck2c}, {xCheck3}, 
			 {xRemark}, {xInspectResult},  {xIssuedSign}, 
			{xIssuedDate}, '{xAcknowledgedSign}', {xAcknowledgedDate}, {Utility.Evar(Utility.eusername(), 1)}";

            Console.WriteLine(query);
            SQLFunction.execQuery(query);
            return Redirect("/ExrRepairJobHistoryInspection/FinalInspection/" + eJobID);
        }
        public IActionResult OldReportHeader()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Old/ReportHeader.cshtml");
        }
        public IActionResult OldReportBody(string ID)
        {
            OldCore(ID);
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Old/ReportBody.cshtml");
        }
        public IActionResult OldReportFooter()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Old/ReportFooter.cshtml");
        }
        public IActionResult OldReport(string ID)
        {
            string jobId = ID;
            string servername = "https://localhost:5001/";
            string namafile;
            string namafile2;
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            string host = $"{HttpContext.Request.Host}";

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            namafile = Path.Combine(savePath, jobId + ".pdf");
            ProcessStartInfo psi = new ProcessStartInfo
            {
                //FileName = "C:\\htmltopdf\\wkhtmltopdf.exe",
                FileName = "C:\\webroot\\TCRC Web\\Rotativa\\wkhtmltopdf.exe",
                Arguments = $"--username minestar --password Mine1staR --margin-bottom 10mm " +
                            $"http://{host}/ExrRepairJobHistoryInspection/OldReportBody/{jobId} " +
                            $"--footer-html http://{host}/ExrRepairJobHistoryInspection/OldReportFooter " +
                            $"--footer-spacing 3 --header-html http://{host}/ExrRepairJobHistoryInspection/OldReportHeader " +
                            $"--header-spacing 3 \"{namafile}\""

            };
            Process p = new Process { StartInfo = psi };
            p.Start();
            p.WaitForExit();

            namafile2 = Path.Combine(savePath, jobId + ".pdf");
            string ffname = "Old Core Inspection Report " + jobId + ".pdf";

            return PhysicalFile(namafile2, "application/pdf", ffname);
        }
		public IActionResult LstPict(string tWono)
		{
			string queryLstPict = $@"Select ID,Picturecaption,PicturePath from tbl_ExrOldCoreInspAttachPic WHERE InspectType='FinalInspect' AND WONO={Utility.Evar(tWono, 1)} and Active != 0";

			Console.WriteLine(queryLstPict);

			var data = SQLFunction.execQuery(queryLstPict);

			var rows = new List<object>();

			foreach (DataRow row in data.Rows)
			{
				string basePath = @"..\..\..\wwwroot\img";

				string dataPath = $@"SELECT dbo.RemapPicW(PicturePath) from tbl_ExrOldCoreInspAttachPic where id = {Utility.CheckNull(row["ID"])}";
                string dataPath2 = SQLFunction.ExecuteScalar(dataPath).ToString();
				string dataPath3 = dataPath2.Substring(21);
				string dataPath4 = basePath + dataPath3;

				string pictPath = dataPath4.Replace(@"\", "/");

				var rowData = new
				{
					id = Utility.CheckNull(row["ID"]),
					pictureCaption = Utility.CheckNull(row["PictureCaption"]),
					picturePath = $"{pictPath}",
					open = $"<a id='btnPictureOpen' class='btn btn-primary btn-sm' data-bs-toggle='modal' " +
					$"data-bs-target='#openFinalInvestigationPicture'><svg xmlns=\"http://www.w3.org/2000/svg\" " +
					$"width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-eye-fill\" viewBox=\"0 0 16 16\">\r\n  " +
					$"<path d=\"M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0\"/>\r\n  <path d=\"M0 8s3-5.5 8-5.5S16 8 16 8s-3 " +
					$"5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7\"/>\r\n</svg></a> ",
					delete = $@"<button type='button' class='btn btn-link btn-sm' id='btnDeleteDetail' onclick='confirmDelete({row["ID"]})'><i class='fa fa-trash text-danger'></i></button>"
				};
				rows.Add(rowData);
			}
			return new JsonResult(rows);
		}
		public IActionResult OldLstPict(string tWono)
        {
            string queryLstPict = $@"Select ID,Picturecaption,PicturePath from tbl_ExrOldCoreInspAttachPic WHERE InspectType='OldCoreInspect' AND WONO={Utility.Evar(tWono, 1)} and Active != 0";
            Console.WriteLine(queryLstPict);

            var data = SQLFunction.execQuery(queryLstPict);

			var rows = new List<object>();
            foreach (DataRow row in data.Rows)
            {

                string basePath = @"..\..\..\wwwroot\img";

				string dataPath = $@"SELECT dbo.RemapPicW(PicturePath) from tbl_ExrOldCoreInspAttachPic where id = {Utility.CheckNull(row["ID"])}";
                string dataPath2 = SQLFunction.ExecuteScalar(dataPath).ToString();
				string dataPath3 = dataPath2.Substring(21);
                string dataPath4 = basePath + dataPath3;

                string pictPath = dataPath4.Replace(@"\", "/");

				var rowData = new
                {
                    id = Utility.CheckNull(row["ID"]),
                    pictureCaption = Utility.CheckNull(row["PictureCaption"]),
                    //picturePath = @"http:\\bpnaps07:3355\wwwroot\img\PictInspection\ExrJobInspection\OldCoreInspect\6887356\WO6887356(2)_20230621175845.JPG",
                    //picturePath = "..\\img\\PictInspection\\ExrJobInspection\\OldCoreInspect\\6887356\\WO6887356(2)_20230621175845.JPG",
                    picturePath = $"{pictPath}",
                    open = $"<a id='btnPictureOpen' class='btn btn-primary btn-sm' data-bs-toggle='modal' " +
                    $"data-bs-target='#openFinalInvestigationPicture'><svg xmlns=\"http://www.w3.org/2000/svg\" " +
                    $"width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-eye-fill\" viewBox=\"0 0 16 16\">\r\n  " +
                    $"<path d=\"M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0\"/>\r\n  <path d=\"M0 8s3-5.5 8-5.5S16 8 16 8s-3 " +
                    $"5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7\"/>\r\n</svg></a> ",
                    delete = $@"<button type='button' class='btn btn-link btn-sm' id='btnDeleteDetail' onclick='confirmDelete({row["ID"]})'><i class='fa fa-trash text-danger'></i></button>"
                };
                rows.Add(rowData);
            }
            return new JsonResult(rows);
        }
		public async Task<IActionResult> UploadFile(IFormFile fileupload)
		{
			string ID = Request.Form["ejobid"];
			string TWONO = Request.Form["formWono"];
			Console.WriteLine(TWONO);
			string einspectionType = "FinalInspect";

			string fileName = "";
			var filePath = "";
			string fileExtension = "";
			var targetDirectory = "";
			string newPath = "";
			string newFile = "";
			string ePictureCaption = "";
			string PathExrJobInspection = @$"\\BPNAPS07.THIESS.AUS\database\Plant_Component\PictInspection\ExrJobInspection";
			string[] allowedExtensions = { ".png", ".PNG", ".jpeg", ".JPEG", ".jpg", ".JPG", ".gif", ".GIF" };
			if (fileupload != null && fileupload.Length > 0)
			{
				fileExtension = Path.GetExtension(fileupload.FileName); // nama file dengan ekstensi
				fileName = Path.GetFileNameWithoutExtension(fileupload.FileName); // Mengambil nama file tanpa ekstensi
				fileName = fileName.Replace(" ", "");

				if (!allowedExtensions.Contains(fileExtension))
				{
					Stat = "warning";
					Msg = "File Must Be An Image";
					return Redirect("/ExrRepairJobHistoryInspection/FinalInspection/" + ID);
				}
				// Menambahkan format jam (yyyyMMddHHmmss) di ujung nama file
				string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");

				string fileName1 = $"{fileName}_{currentTime}{fileExtension}";
				ePictureCaption = fileName1;
				targetDirectory = $"{PathExrJobInspection}";
				//targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), "img"); // local

				newPath = $@"{PathExrJobInspection}\{einspectionType}\{TWONO}";
				newFile = $@"{newPath}\{fileName1}";

                Console.WriteLine("new file is -" + newFile);

				//filePath = Path.Combine(targetDirectory, ePictureCaption);

				if (!Directory.Exists(newPath))
				{
					Directory.CreateDirectory(newPath);
				}

				using (var stream = new FileStream(newFile, FileMode.Create))
				{
					await fileupload.CopyToAsync(stream);
				}
			}

			string query = @$"exec dbo.ExrOldCoreInspectPic {Utility.Evar(TWONO, 1)}, 
			{Utility.Evar(ID, 1)}, {Utility.Evar(newFile, 1)}, {Utility.Evar(ePictureCaption, 1)}, 
			{Utility.Evar(einspectionType, 1)}, {Utility.Evar(Utility.eusername(), 1)}";

			Console.WriteLine(query);
			SQLFunction.execQuery(query);

			Stat = "success";
			Msg = "Image has been uploaded successfully";
			return Redirect("/ExrRepairJobHistoryInspection/FinalInspection/" + ID);
		}
		public async Task<IActionResult> OldUploadFile(IFormFile fileupload)
        {
            string ID = Request.Form["ejobid"];
            string TWONO = Request.Form["formWono"];
            Console.WriteLine(TWONO);
            string einspectionType = "OldCoreInspect";

            string fileName = "";
            var filePath = "";
            string fileExtension = "";
            var targetDirectory = "";
            string newPath = "";
            string newFile = "";
            string ePictureCaption = "";
            string PathExrJobInspection = @$"\\BPNAPS07.THIESS.AUS\database\Plant_Component\PictInspection\ExrJobInspection";
            string[] allowedExtensions = { ".png", ".PNG", ".jpeg", ".JPEG", ".jpg", ".JPG", ".gif", ".GIF" };

            if (fileupload != null && fileupload.Length > 0)
            {
                fileExtension = Path.GetExtension(fileupload.FileName); // nama file dengan ekstensi
                fileName = Path.GetFileNameWithoutExtension(fileupload.FileName); // Mengambil nama file tanpa ekstensi
                fileName = fileName.Replace(" ", "");

                if (!allowedExtensions.Contains(fileExtension))
                {
                    Stat = "warning";
                    Msg = "File Must Be An Image";
                    return Redirect("/ExrRepairJobHistoryInspection/OldCore/" + ID);
                }

                // Menambahkan format jam (yyyyMMddHHmmss) di ujung nama file
                string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                string fileName1 = $"{fileName}_{currentTime}{fileExtension}";
                ePictureCaption = fileName1;

                targetDirectory = $"{PathExrJobInspection}";
                //targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), "img"); // local

                newPath = $@"{PathExrJobInspection}\{einspectionType}\{TWONO}";
                newFile = $@"{newPath}\{fileName1}";

                //filePath = Path.Combine(targetDirectory, ePictureCaption);
                Console.WriteLine("file path is -" + newFile);

				if (!Directory.Exists(newPath))
				{
					Directory.CreateDirectory(newPath);
				}

				using (var stream = new FileStream(newFile, FileMode.Create))
                {
                    await fileupload.CopyToAsync(stream);
                }
            }

            string query = @$"exec dbo.ExrOldCoreInspectPic {Utility.Evar(TWONO, 1)}, 
			{Utility.Evar(ID, 1)}, {Utility.Evar(newFile, 1)}, {Utility.Evar(ePictureCaption, 1)}, 
			{Utility.Evar(einspectionType, 1)}, {Utility.Evar(Utility.eusername(), 1)}";

            Console.WriteLine(query);
            SQLFunction.execQuery(query);

            Stat = "success";
            Msg = "Image has been uploaded successfully";
            return Redirect("/ExrRepairJobHistoryInspection/OldCore/" + ID);
        }
        public IActionResult DataOldReportPictBody(string ID)
        {
            Console.WriteLine("wono is" + ID);
            string placeholder = "https://placekitten.com/200/300";
            string query = @$"Select ID,JobID,WoNO,IDReg,PictureCaption,picturepath,inspectType 
			from tblv_ExrOldCoreInspAttachPic where InspectType='OldCoreInspect' AND WONO={Utility.Evar(ID, 1)}";
            Console.WriteLine(query);
            ViewBag.data = SQLFunction.execQuery(query);
            ViewBag.placeholder = placeholder;
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Old/PictBody.cshtml");
        }
        public IActionResult OldReportPictHeader(string ID)
        {
            DataOldReportPictBody(ID);
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Old/PictHeader.cshtml");
        }
        public IActionResult OldReportPictBody(string ID)
        {
            DataOldReportPictBody(ID);
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Old/PictBody.cshtml");
        }
        public IActionResult OldReportPictFooter()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Old/PictFooter.cshtml");
        }
        public IActionResult PrintPict(string ID)
        {
            Console.WriteLine(ID);
            string servername = "https://localhost:5001/";
            string namafile;
            string namafile2;
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            string host = $"{HttpContext.Request.Host}";

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            namafile = Path.Combine(savePath, ID + ".pdf");
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "C:\\webroot\\TCRC Web\\Rotativa\\wkhtmltopdf.exe",
                Arguments = $"--username minestar --password Mine1staR --margin-bottom 10mm " +
                            $"http://{host}/ExrRepairJobHistoryInspection/FinalReportPictBody/{ID} " +
                            $"--footer-html http://{host}/ExrRepairJobHistoryInspection/FinalReportPictFooter " +
                            $"--footer-center \"Page [page] of [topage]\" --footer-font-size 11 --footer-spacing 3 --header-html http://{host}/ExrRepairJobHistoryInspection/FinalReportPictHeader/{ID} " +
                            $"--header-spacing 3 \"{namafile}\""
            };
            Process p = new Process { StartInfo = psi };
            p.Start();
            p.WaitForExit();

            namafile2 = Path.Combine(savePath, ID + ".pdf");
            string ffname = "Final Inspection Image " + ID + ".pdf";

            return PhysicalFile(namafile2, "application/pdf", ffname);
        }
        public IActionResult OldPrintPict(string ID)
        {
            Console.WriteLine(ID);
            string servername = "https://localhost:5001/";
            string namafile;
            string namafile2;
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            string host = $"{HttpContext.Request.Host}";

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            namafile = Path.Combine(savePath, ID + ".pdf");
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "C:\\webroot\\TCRC Web\\Rotativa\\wkhtmltopdf.exe",
                Arguments = $"--username minestar --password Mine1staR --margin-bottom 10mm " +
                            $"http://{host}/ExrRepairJobHistoryInspection/OldReportPictBody/{ID} " +
                            $"--footer-html http://{host}/ExrRepairJobHistoryInspection/OldReportPictFooter " +
                            $"--footer-center \"Page [page] of [topage]\" --footer-font-size 11 --footer-spacing 3 --header-html http://{host}/ExrRepairJobHistoryInspection/OldReportPictHeader/{ID} " +
                            $"--header-spacing 3 \"{namafile}\""
            };
            Process p = new Process { StartInfo = psi };
            p.Start();
            p.WaitForExit();

            namafile2 = Path.Combine(savePath, ID + ".pdf");
            string ffname = "Old Core Inspection Image " + ID + ".pdf";

            return PhysicalFile(namafile2, "application/pdf", ffname);
        }
        public IActionResult Report(string ID)
        {
            Console.WriteLine(ID);
            string jobId = ID;
            string servername = "https://localhost:5001/";
            string namafile;
            string namafile2;
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            string host = $"{HttpContext.Request.Host}";

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            namafile = Path.Combine(savePath, jobId + ".pdf");
            ProcessStartInfo psi = new ProcessStartInfo
            {
                //FileName = "C:\\htmltopdf\\wkhtmltopdf.exe", //local
                FileName = "C:\\webroot\\TCRC Web\\Rotativa\\wkhtmltopdf.exe",
                Arguments = $"--username minestar --password Mine1staR --margin-bottom 10mm " +
                            $"http://{host}/ExrRepairJobHistoryInspection/FinalReportBody/{jobId} " +
                            $"--footer-html http://{host}/ExrRepairJobHistoryInspection/FinalReportFooter " +
                            $"--footer-spacing 3 --header-html http://{host}/ExrRepairJobHistoryInspection/FinalReportHeader " +
                            $"--header-spacing 3 \"{namafile}\""
            };

            Process p = new Process { StartInfo = psi };
            p.Start();
            p.WaitForExit();

            namafile2 = Path.Combine(savePath, jobId + ".pdf");
            string ffname = "Final Inspection Report " + jobId + ".pdf";

            return PhysicalFile(namafile2, "application/pdf", ffname);
        }
        public async Task<IActionResult> UploadIndexImage(IFormFile formData)
        {
            string inspectType = "test";
            string fileName = "";
            var filePath = "";
            string fileExtension = "";
            var targetDirectory = "";
            string newPath = "";
            string newFile = "";
            string ePictureCaption = "";
            string eInspectType = "";
            string jobId = "";
            string PathExrJobInspection = @$"\\BPNAPS07.THIESS.AUS\database\Plant_Component\PictInspection\ExrJobInspection\";
            string[] allowedExtensions = { ".png", ".PNG", ".jpeg", ".JPEG", ".jpg", ".JPG", ".gif", ".GIF" };

            fileExtension = Path.GetExtension(formData.FileName); // nama file dengan ekstensi
            fileName = Path.GetFileNameWithoutExtension(formData.FileName); // Mengambil nama file tanpa ekstensi
            fileName = fileName.Replace(" ", "");

            if (!allowedExtensions.Contains(fileExtension))
            {
                return new JsonResult("invalid format");
            }

            // Menambahkan format jam (yyyyMMddHHmmss) di ujung nama file
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            string fileName1 = $"{fileName}_{currentTime}{fileExtension}";

            string wonum = fileName1.Substring(0, 7);
            string eWonum = Utility.Evar(wonum, 1);

            string checkQuery = $"Select ID from tbl_ExrjobDetail Where OffSIteWO={eWonum}";
            var checkData = SQLFunction.execQuery(checkQuery);

            if (checkData.Rows.Count > 0)
            {
                targetDirectory = $"{PathExrJobInspection}";
                //targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), "img"); // local

                newPath = $@"{targetDirectory}\{inspectType}\{wonum}";
                newFile = $@"{newPath}\{fileName1}";
                string eNewFile = Utility.Evar(newFile, 1);

                ePictureCaption = Utility.Evar(fileName, 1);
                eInspectType = Utility.Evar(inspectType, 1);
                string ejobid = Utility.CheckNull(checkData.Rows[0]["ID"]);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await formData.CopyToAsync(stream);
                }

                string query = @$"exec dbo.ExrOldCoreInspectPic {eWonum}, {ejobid}, {eNewFile}, {ePictureCaption}, 
					{eInspectType}, {Utility.ebyname()}";

                Console.WriteLine(query);
                SQLFunction.execQuery(query);

                return new JsonResult("ok");
            }
            else
            {
                return new JsonResult("not exist");
            }
        }
        public IActionResult OldJsPdf(string ID)
        {
            OldCore(ID);
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Old/JsPdf.cshtml");
        }
        public IActionResult FinalJsPdf(string ID)
        {
			FinalInspection(ID);
			return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/JsPdf.cshtml");
        }
		public IActionResult PictFinalJsPdf(string ID)
		{
			DataFinalReportPictBody(ID);
			return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/PictJsPdf.cshtml");
		}
        public IActionResult PictOldJsPdf(string ID)
        {
            DataOldReportPictBody(ID);
            return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Old/PictJsPdf.cshtml");
        }
    }
}
