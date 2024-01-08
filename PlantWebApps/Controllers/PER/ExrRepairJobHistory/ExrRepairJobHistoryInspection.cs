﻿using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;
using System.Diagnostics;
using System.Drawing;

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
        public IActionResult LstPict(string tWono)
        {
			string queryLstPict = $@"Select ID,Picturecaption,PicturePath from tbl_ExrOldCoreInspAttachPic WHERE InspectType='FinalInspect' AND WONO={Utility.Evar(tWono, 1)} and Active != 0";

			Console.WriteLine(queryLstPict);

			var data = SQLFunction.execQuery(queryLstPict);

			var rows = new List<object>();

			foreach (DataRow row in data.Rows)
			{
				var rowData = new
				{
					id = Utility.CheckNull(row["ID"]),
					pictureCaption = Utility.CheckNull(row["PictureCaption"]),
					picturePath = "https://placekitten.com/200/300",
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
        public IActionResult Report(string ID)
        {
			Console.WriteLine(ID);
            string jobId = ID;
            string servername = "https://localhost:5001/";
            string namafile;
            string namafile2;
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            namafile = Path.Combine(savePath, jobId + ".pdf");
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "C:\\htmltopdf\\wkhtmltopdf.exe",
                Arguments = "--username minestar --password Mine1staR --margin-bottom 10mm " +
                   "\"https://localhost:7235/ExrRepairJobHistoryInspection/FinalReportBody/" + ID +
                   "\" --footer-html \"https://localhost:7235/ExrRepairJobHistoryInspection/FinalReportFooter" +
                   "\" --footer-spacing 3  --header-html \"https://localhost:7235/ExrRepairJobHistoryInspection/FinalReportHeader" +
                   "\" --header-spacing 3 " +
                   "\"" + namafile + "\""

            };
            Process p = new Process { StartInfo = psi };
            p.Start();
            p.WaitForExit();

            namafile2 = Path.Combine(savePath, jobId + ".pdf");
            string ffname = "Final Inspection Report " + jobId + ".pdf";

            return PhysicalFile(namafile2, "application/pdf", ffname);
        }
		public IActionResult DataFinalReportPictBody(string wo)
		{
			Console.WriteLine("wono is" + wo);
			string placeholder = "https://placekitten.com/200/300";
			string query = @$"Select ID,JobID,WoNO,IDReg,PictureCaption,picturepath,inspectType 
			from tblv_ExrOldCoreInspAttachPic where InspectType='FinalInspect' AND WONO={Utility.Evar(wo, 1)}";
			Console.WriteLine(query);
			ViewBag.data = SQLFunction.execQuery(query);
			ViewBag.placeholder = placeholder;
			return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/PictBody.cshtml");
		}
		public IActionResult FinalReportPictHeader(string wo)
		{
			DataFinalReportPictBody(wo);
			return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/PictHeader.cshtml");
		}
		public IActionResult FinalReportPictBody(string wo)
		{
			DataFinalReportPictBody(wo);
			return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/PictBody.cshtml");
		}
		public IActionResult FinalReportPictFooter()
		{
			return View("~/Views/PER/ExrRepairJobHistory/Form/Investigation/Reports/Final/PictFooter.cshtml");
		}
		public IActionResult TestWono (string wono)
		{
			Console.WriteLine(wono);
			return Json(new { success = true });
		}
		public IActionResult PrintPict(string wo)
		{
			Console.WriteLine(wo);
			string servername = "https://localhost:5001/";
            string namafile;
            string namafile2;
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
			namafile = Path.Combine(savePath, wo + ".pdf");
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = "C:\\htmltopdf\\wkhtmltopdf.exe",
				Arguments = "--username minestar --password Mine1staR --margin-bottom 10mm " +
				   "\"https://localhost:7235/ExrRepairJobHistoryInspection/FinalReportPictBody/" + wo +
				   "\" --footer-html \"https://localhost:7235/ExrRepairJobHistoryInspection/FinalReportPictFooter" +
				   "\" --footer-spacing 3  --header-html \"https://localhost:7235/ExrRepairJobHistoryInspection/FinalReportPictHeader/" + wo +
				   "\" --header-spacing 3 " +
				   "\"" + namafile + "\""

			};
			Process p = new Process { StartInfo = psi };
			p.Start();
			p.WaitForExit();

			namafile2 = Path.Combine(savePath, wo + ".pdf");
            string ffname = "Final Inspection Image " + wo + ".pdf";

            return PhysicalFile(namafile2, "application/pdf", ffname);
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
			string PathExrJobInspection  = @$"\\BPNAPS07.THIESS.AUS\database\Plant_Component\PictInspection\ExrJobInspection\";
			string[] allowedExtensions = { ".png", ".PNG", ".jpeg", ".JPEG", ".jpg", ".JPG", ".gif", ".GIF" };
			if (fileupload != null && fileupload.Length > 0)
			{
				fileExtension = Path.GetExtension(fileupload.FileName); // nama file dengan ekstensi
				Console.WriteLine("File Extension is" + fileExtension);
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
				//targetDirectory = $"{PathExrJobInspection}"; server
				targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp"); // local

				newPath = $@"{PathExrJobInspection}\{einspectionType}\{TWONO}";
				newFile = $@"{newPath}\{fileName1}";

				filePath = Path.Combine(targetDirectory, ePictureCaption);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await fileupload.CopyToAsync(stream);
				}
			}

			string query = @$"exec dbo.ExrOldCoreInspectPic {Utility.Evar(TWONO, 1)}, 
			{Utility.Evar(ID, 1)}, {Utility.Evar(newFile, 1)}, {Utility.Evar(ePictureCaption, 1)}, 
			{Utility.Evar(einspectionType,1 )}, {Utility.Evar(Utility.eusername(), 1)}";

			Console.WriteLine(query);

			Stat = "success";
			Msg = "Image has been uploaded successfully";
			return Redirect("/ExrRepairJobHistoryInspection/FinalInspection/" + ID);
		}
		public IActionResult SaveOld (string eID, string eJobID, string eWono,
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
			//SQLFunction.execQuery(query);
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
			Console.WriteLine(ID);
			string jobId = ID;
			string servername = "https://localhost:5001/";
			string namafile;
			string namafile2;
			string savePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");

			if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}
			namafile = Path.Combine(savePath, jobId + ".pdf");
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = "C:\\htmltopdf\\wkhtmltopdf.exe",
				Arguments = "--username minestar --password Mine1staR --margin-bottom 10mm " +
				   "\"https://localhost:7235/ExrRepairJobHistoryInspection/OldReportBody/" + ID +
				   "\" --footer-html \"https://localhost:7235/ExrRepairJobHistoryInspection/OldReportFooter" +
				   "\" --footer-spacing 3  --header-html \"https://localhost:7235/ExrRepairJobHistoryInspection/OldReportHeader" +
				   "\" --header-spacing 3 " +
				   "\"" + namafile + "\""

			};
			Process p = new Process { StartInfo = psi };
			p.Start();
			p.WaitForExit();

			namafile2 = Path.Combine(savePath, jobId + ".pdf");
			string ffname = "Old Core Inspection Report " + jobId + ".pdf";

			return PhysicalFile(namafile2, "application/pdf", ffname);
		}
		public IActionResult OldLstPict(string tWono)
		{
			string queryLstPict = $@"Select ID,Picturecaption,PicturePath from tbl_ExrOldCoreInspAttachPic WHERE InspectType='OldCoreInspect' AND WONO={Utility.Evar(tWono, 1)} and Active != 0";

			Console.WriteLine(queryLstPict);

			var data = SQLFunction.execQuery(queryLstPict);

			var rows = new List<object>();

			foreach (DataRow row in data.Rows)
			{
				var rowData = new
				{
					id = Utility.CheckNull(row["ID"]),
					pictureCaption = Utility.CheckNull(row["PictureCaption"]),
					picturePath = "https://placekitten.com/200/300",
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
			string PathExrJobInspection = @$"\\BPNAPS07.THIESS.AUS\database\Plant_Component\PictInspection\ExrJobInspection\";
			string[] allowedExtensions = { ".png", ".PNG", ".jpeg", ".JPEG", ".jpg", ".JPG", ".gif", ".GIF" };
			if (fileupload != null && fileupload.Length > 0)
			{
				fileExtension = Path.GetExtension(fileupload.FileName); // nama file dengan ekstensi
				Console.WriteLine("File Extension is" + fileExtension);
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
				//targetDirectory = $"{PathExrJobInspection}"; server
				targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp"); // local

				newPath = $@"{PathExrJobInspection}\{einspectionType}\{TWONO}";
				newFile = $@"{newPath}\{fileName1}";

				filePath = Path.Combine(targetDirectory, ePictureCaption);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await fileupload.CopyToAsync(stream);
				}
			}

			string query = @$"exec dbo.ExrOldCoreInspectPic {Utility.Evar(TWONO, 1)}, 
			{Utility.Evar(ID, 1)}, {Utility.Evar(newFile, 1)}, {Utility.Evar(ePictureCaption, 1)}, 
			{Utility.Evar(einspectionType, 1)}, {Utility.Evar(Utility.eusername(), 1)}";

			Console.WriteLine(query);

			Stat = "success";
			Msg = "Image has been uploaded successfully";
			return Redirect("/ExrRepairJobHistoryInspection/OldCore/" + ID);
		}
	}
}