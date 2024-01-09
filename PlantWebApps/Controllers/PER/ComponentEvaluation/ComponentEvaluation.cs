using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PlantWebApps.Helper;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq.Expressions;
using System.Security.Cryptography;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace PlantWebApps.Controllers.PER.ComponentEvaluation
{
    public class ComponentEvaluation : Controller
    {
		private string _tempfilter;
		[TempData]
		public String Msg { get; set; }
		[TempData]
		public String Stat { get; set; }
		public IActionResult Index()
        {
            LoadOption();
            return View("~/Views/PER/ComponentEvaluation/Index.cshtml");
        }
		private void BuildTempFilter()
		{
			string tempfilter = string.Empty;

			Dictionary<string, string> formFields = new Dictionary<string, string>
			{
				{ "EvalByID", Request.Form["fSpvDesc"] },
				{ "MaintType", Request.Form["fMaintBase"] },
				{ "RootCauseCode", Request.Form["fRootCause"] },
				{ "EvalCode", Request.Form["fEvalCode"] },
				{ "SysFailCode", Request.Form["fFailedCode"] },
				{ "RecCode", Request.Form["fRecCode"] },
				{ "ComptypeID", Request.Form["tCompTypeID"] },
				{ "ReasonTypeID", Request.Form["tReasonType"] },
				{ "WarrantyResult", Request.Form["cbWarrantyResult"] }
			};

			foreach (var field in formFields)
			{
				if (!string.IsNullOrEmpty(field.Value))
				{
					var viewBagDict = ViewBag as IDictionary<string, object>;

					if (viewBagDict != null)
					{
						viewBagDict[field.Key] = field.Value;
					}

					tempfilter = $" and {field.Key} = {Utility.Evar(field.Value, 1)}" + tempfilter;
				}
			}

			string CwoTypeCategory = string.Empty;
			switch (Request.Form["CWOType"])
			{
				case "1":
					CwoTypeCategory = "WOno";
					break;
				case "2":
					CwoTypeCategory = "JobID";
					break;
				case "3":
					CwoTypeCategory = "ID";
					break;
			}

			if (!string.IsNullOrEmpty(Request.Form["CWOType"]))
			{
				tempfilter = $" and {CwoTypeCategory} = " + Utility.Evar(Request.Form["fSwo"], 0) + tempfilter;
			}

			if (!string.IsNullOrEmpty(Request.Form["fUnitDesc"]))
			{
				tempfilter = " AND UnitDescription in (" + Utility.Evar(Request.Form["fUnitDesc"], 1) + ")" + tempfilter;
			}

			if (!string.IsNullOrEmpty(Request.Form["dStart"]))
			{
				tempfilter = " and " + "EvalDate" + " >= " + Utility.Evar(Request.Form["dStart"], 2) + tempfilter;
			}

			if (!string.IsNullOrEmpty(Request.Form["dEnd"]))
			{
				tempfilter = " and " + "EvalDate" + " <= " + Utility.Evar(Request.Form["dEnd"], 2) + tempfilter;
			}

			string temporder = $"ORDER BY {Request.Form["fSort"]} {Request.Form["fAsc"]}";
			_tempfilter = Utility.VarFilter(tempfilter);
		}
		public IActionResult LoadData(string fSpvDesc, string fSwo, string 
			fUnitDesc, string fRootCause, string fEvalCode, string fMaintBase, 
			string fFailedCode, string fRecCode, string tCompTypeID, string tReasonType, 
			string cbWarrantyResult, string tRecord, string fSort, string fAsc, string 
			dStart, string dEnd, string CWOType)
			{
			LoadOption();
			string tempfilter = string.Empty;

			Dictionary<string, string> formFields = new Dictionary<string, string>
			{
				{ "EvalByID", fSpvDesc },
				{ "MaintType", fMaintBase },
				{ "RootCauseCode", fRootCause },
				{ "EvalCode", fEvalCode },
				{ "SysFailCode", fFailedCode },
				{ "RecCode", fRecCode },
				{ "ComptypeID", tCompTypeID },
				{ "ReasonTypeID", tReasonType },
				{ "WarrantyResult", cbWarrantyResult },
			};

			foreach (var field in formFields)
			{
				if (!string.IsNullOrEmpty(field.Value))
				{
					var viewBagDict = ViewBag as IDictionary<string, object>;

					if (viewBagDict != null)
					{
						viewBagDict[field.Key] = field.Value;
					}

					tempfilter = $" and {field.Key} = {Utility.Evar(field.Value, 1)}" + tempfilter;
				}
			}

			string CwoTypeCategory = string.Empty;
			switch (CWOType)
			{ 
				case "1":
					CwoTypeCategory = "WOno";
					break;
				case "2":
					CwoTypeCategory = "JobID";
					break;
				case "3":
					CwoTypeCategory = "ID";
					break;
			}

			if (!string.IsNullOrEmpty(CWOType))
			{
				tempfilter = $" and {CwoTypeCategory} = " + Utility.Evar(fSwo, 0) + tempfilter;
			}

			if (!string.IsNullOrEmpty(fUnitDesc))
			{
				tempfilter = " AND UnitDescription in (" + Utility.Evar(fUnitDesc, 1) + ")" + tempfilter;
			}

			if (!string.IsNullOrEmpty(dStart))
			{
				tempfilter = " and " + "EvalDate" + " >= " + Utility.Evar(dStart, 2) + tempfilter;
			}

			if (!string.IsNullOrEmpty(dEnd))
			{
				tempfilter = " and " + "EvalDate" + " <= " + Utility.Evar(dEnd, 2) + tempfilter;
			}

			string temporder = $"ORDER BY {fSort} {fAsc}";
			_tempfilter = Utility.VarFilter(tempfilter);
			Console.WriteLine(_tempfilter);

			string query = $"SELECT * from v_ExrCEDetail {_tempfilter} {temporder}";
			Console.WriteLine(query);

			var data = SQLFunction.execQuery(query);

			var rows = new List<object>();

			foreach (DataRow row in data.Rows)
			{
				var rowData = new
				{
					id = Utility.CheckNull(row["ID"]),
					wono = Utility.CheckNull(row["Wono"]),
					unitNumber = Utility.CheckNull(row["UnitNumber"]),
					unitDescription = Utility.CheckNull(row["UnitDescription"]),
					maintType = Utility.CheckNull(row["MaintType"]),
					evalByAbr = Utility.CheckNull(row["EvalByAbr"]),
					evalCode = Utility.CheckNull(row["EvalCode"]),
					rootCause = Utility.CheckNull(row["RootCauseCode"]),
					sysFailCode = Utility.CheckNull(row["SysFailCode"]),
					recCode = Utility.CheckNull(row["RecCode"]),
					smu = Utility.CheckNull(row["SMU"]),
					bdgtHours = Utility.CheckNull(row["BdgtHours"]),
					jobID = Utility.CheckNull(row["JobID"]),
					registerDate = Utility.CheckNull(row["RegisterDate"]),
					evalDate = Utility.CheckNull(row["EvalDate"]),
					pcamRequired = Utility.CheckNull(row["PCAMRequired"]),
					pcamStatus = Utility.CheckNull(row["PCAMStatusID"]),
					pcamID = Utility.CheckNull(row["PCAMID"]),
					psDate = Utility.CheckNull(row["PSDate"]),
					edit = "<button class='btn btn-link btn-sm' formaction='componentevaluation/edit/" + row["ID"] + "'><i class='fa fa-edit'></i></button>",
					delete = $@"<button type='button' class='btn btn-link btn-sm' id='btnDeleteDetail' onclick='confirmDelete({row["ID"]})'><i class='fa fa-trash text-danger'></i></button>"
				};
				rows.Add(rowData);
			}
			return new JsonResult(rows);
		}
		public IActionResult Edit(string id, string tWono)
		{
			LoadOption();

			// general data query
			string query = $"SELECT * from v_ExrCEDetail WHERE ID = '{id}'";
			ViewBag.data = SQLFunction.execQuery(query);

			// PCAM Required query
			string queryPcamRequired = @ViewBag.data.Rows[0]["PCAMRequired"].ToString();
			ViewBag.PCAMRequired = queryPcamRequired;

			return View("~/Views/PER/ComponentEvaluation/Form.cshtml");
		}
		public IActionResult Delete(string id)
		{
			LoadOption();

			string query = $"DELETE FROM tbl_EXRCEDetail WHERE ID = {Utility.Evar(id, 0)}";
			Console.WriteLine(query);

			SQLFunction.execQuery(query);

			Stat = "success";
			Msg = "Data Has Been Deleted";

			return Json(new { success = true });
		}
		public IActionResult CreateCESummary(string ID, string WONO)
		{
			LoadOption();
			string query = $"select dbo.CreateCESummary({Utility.Evar(WONO, 0)}) as OUTPUT";
			Console.WriteLine(query);

			var data = SQLFunction.execQuery(query);

			var rows = new List<object>();

			foreach (DataRow row in data.Rows)
			{
				var rowData = new
				{
					output = Utility.CheckNull(row["OUTPUT"]),
				};
				rows.Add(rowData);
			}
			return new JsonResult(rows);

		}
		public IActionResult RootCauseDetail(string rootCauseCode)
		{
			string query = $"SELECT RootCauseDetail FROM tbl_EXRCERootCauseDetail Where RootCauseCode = {rootCauseCode}";
			Console.WriteLine(query);

			var data = SQLFunction.execQuery(query);
			ViewBag.tRemark = data;
			var rows = new List<object>();

			foreach (DataRow row in data.Rows)
			{
				var rowData = new
				{
					tRemark = Utility.CheckNull(row["RootCauseDetail"]),
				};
				rows.Add(rowData);
			}

			return new JsonResult(rows);
		}
		public IActionResult AssignWo(string tWono)
		{
			string querytWono = $"SELECT DISTINCT Wono FROM v_ExrCEDetail WHERE WONO = '{tWono}'";
			var data = SQLFunction.execQuery(querytWono);

			var rows = new List<object>();

			foreach (DataRow row in data.Rows)
			{
				var rowData = new
				{
					wono = Utility.CheckNull(row["Wono"]),
				};
				rows.Add(rowData);
			}

			return new JsonResult(rows);
		}
		public IActionResult ListInvest(string tWono)
		{
			// table list invest query
			string queryListInvest = $"SELECT ID,CEID,WONO,InvesDate as [When],InvesDesc as [What],InvesDetail as [Detail],InvesWhen FROM tbl_EXRCEInvestigation where Wono = '{tWono}'";
			Console.WriteLine(queryListInvest);
			var data = SQLFunction.execQuery(queryListInvest);

			var rows = new List<object>();

			foreach (DataRow row in data.Rows)
			{
				var rowData = new
				{
					id = Utility.CheckNull(row["ID"]),
					when = Utility.CheckNull(row["When"]),
					what = Utility.CheckNull(row["What"]),
					detail = Utility.CheckNull(row["Detail"]),
					edit = $"<a id='selectedDetailRow' class='btn btn-primary btn-sm' " +
					$"value='{string.Join(" ", row.ItemArray)}' " +
					$">Edit</a>"
				};
				rows.Add(rowData);
			}
			return new JsonResult(rows);
		}
		public IActionResult CCESAVE(string id, string wono, string investDate, string investDesc, string investDetail)
		{
			LoadOption();

			var eid = Utility.Evar(Utility.CheckNull(id), 1);
			var eCEID = Utility.Evar(Utility.CheckNull(""), 1);
			var eWono = Utility.Evar(Utility.CheckNull(wono), 1);
			var eInvesDate = Utility.Evar(Utility.CheckNull(investDate), 2);
			var eInvesDesc = Utility.Evar(Utility.CheckNull(investDesc), 1);
			var eInvesDetail = Utility.Evar(Utility.CheckNull(investDetail), 1);
			var eInvesWhen = Utility.Evar(Utility.CheckNull(""), 1);

			var query = @$"exec dbo.EXrCEAdd {eid}, {eCEID}, {eWono}, {eInvesDate}, {eInvesDesc}, {eInvesDetail}, {eInvesWhen}";

			Console.WriteLine(query);
			SQLFunction.execQuery(query);

			Stat = "success";
			Msg = "Data Has Been Inserted";

			return Redirect("/ComponentEvaluation/Edit/" + id);
		}
		public IActionResult Save(string ID, string WONO, string UnitNumber, string PCAMID, 
			string PCAMRequired, string PCAMStatusID, string PCAMStartDate, string PCAMCompletedDate, 
			string MaintType, string BdgtHours, string SMU, string Complaint, string FlatRate, 
			string OROP, string ExcPart, string PartCost, string LabourCost, string OtherCost, 
			string TCISupply, string SavingCost, string PriceType, string Price, string CurrID, 
			string StripDown, string SysTypeID, string RootCauseCode, string SysFailCode, string RecCode, 
			string M1, string M2, string M3, string M4, string M5, string M6, string M7, string M8, 
			string M9, string M10, string M11, string M12, string M13, string M14, string M15, 
			string M16, string M17, string M18, string M19, string M20, string M21, string ActionTaken, 
			string EvalByID, string EvalDate, string EvalCode, string PSName, string PSDate, string POSName 
			, string POSDate, string PMName, string PMDate, string Conclusion, string ModBy, string ModDate,
			string JobID, string ConsCost, string SupplyDate, string InstallDate, string RemoveDate, string Remark)
		{
			LoadOption();

			string eID =  (Utility.Evar(ID, 1));
			string eWONO = (Utility.Evar(WONO, 1));
			string eUnitNumber = (Utility.Evar(UnitNumber, 1));
			string ePCAMID = (Utility.Evar(PCAMID, 1));
			string ePCAMRequired = (Utility.Evar(PCAMRequired, 1));
			string ePCAMStatusID = (Utility.Evar(PCAMStatusID, 1));
			string ePCAMStartDate = (Utility.Evar(PCAMStartDate, 1));
			string ePCAMCompletedDate = (Utility.Evar(PCAMCompletedDate, 1));
			string eMaintType = (Utility.Evar(MaintType, 1));
			string eBdgtHours = (Utility.Evar(BdgtHours, 1));
			string eSMU = (Utility.Evar(SMU, 1));
			string eComplaint = (Utility.Evar(Complaint, 1));
			string eFlatRate = (Utility.Evar(FlatRate, 1));
			string eOROP = (Utility.Evar(OROP, 1));
			string eExcPart = (Utility.Evar(ExcPart, 1));
			string ePartCost = (Utility.Evar(PartCost, 1));
			string eLabourCost = (Utility.Evar(LabourCost, 1));
			string eOtherCost = (Utility.Evar(OtherCost, 1));
			string eTCISupply = (Utility.Evar(TCISupply, 1));
			string eSavingCost = (Utility.Evar(SavingCost, 1));
			string ePriceType = (Utility.Evar(PriceType, 1));
			string ePrice = (Utility.Evar(Price, 1));
			string eCurrID = (Utility.Evar(CurrID, 1));
			string eStripDown = (Utility.Evar(StripDown, 1));
			string eSysTypeID = (Utility.Evar(SysTypeID, 1));
			string eRootCauseCode = (Utility.Evar(RootCauseCode, 1));
			string eSysFailCode = (Utility.Evar(SysFailCode, 1));
			string eRecCode = (Utility.Evar(RecCode, 1));
			string eM1 = (Utility.Evar(M1, 1));
			string eM2 = (Utility.Evar(M2, 1));
			string eM3 = (Utility.Evar(M3, 1));
			string eM4 = (Utility.Evar(M4, 1));
			string eM5 = (Utility.Evar(M5, 1));
			string eM6 = (Utility.Evar(M6, 1));
			string eM7 = (Utility.Evar(M7, 1));
			string eM8 = (Utility.Evar(M8, 1));
			string eM9 = (Utility.Evar(M9, 1));
			string eM10 = (Utility.Evar(M10, 1));
			string eM11 = (Utility.Evar(M11, 1));
			string eM12 = (Utility.Evar(M12, 1));
			string eM13 = (Utility.Evar(M13, 1));
			string eM14 = (Utility.Evar(M14, 1));
			string eM15 = (Utility.Evar(M15, 1));
			string eM16 = (Utility.Evar(M16, 1));
			string eM17 = (Utility.Evar(M17, 1));
			string eM18 = (Utility.Evar(M18, 1));
			string eM19 = (Utility.Evar(M19, 1));
			string eM20 = (Utility.Evar(M20, 1));
			string eM21 = (Utility.Evar(M21, 1));
			string eActionTaken = (Utility.Evar(ActionTaken, 1));
			string eEvalByID = (Utility.Evar(EvalByID, 1));
			string eEvalDate = (Utility.Evar(EvalDate, 1));
			string eEvalCode = (Utility.Evar(EvalCode, 1));
			string ePSName = (Utility.Evar(PSName, 1));
			string ePSDate = (Utility.Evar(PSDate, 1));
			string ePOSName = (Utility.Evar(POSName, 1));
			string ePOSDate = (Utility.Evar(POSDate, 1));
			string ePMName = (Utility.Evar(PMName, 1));
			string ePMDate = (Utility.Evar(PMDate, 1));
			string eConclusion = (Utility.Evar(Conclusion, 1));
			string eModBy = (Utility.Evar(ModBy, 1));
			string eModDate = (Utility.Evar(ModDate, 2));
			string eJobID = (Utility.Evar(JobID, 1));
			string eConsCost = (Utility.Evar(ConsCost, 1));
			string eSupplyDate = (Utility.Evar(SupplyDate, 2));
			string eInstallDate = (Utility.Evar(InstallDate, 2));
			string eRemoveDate = (Utility.Evar(RemoveDate, 2));
			string eRemark = (Utility.Evar(Remark, 1));

			string query = @$"UPDATE tbl_EXRCEDetail SET UnitNumber = {eUnitNumber}, 
			PCAMID = {ePCAMID}, MaintType = {eMaintType}, BdgtHours = {eBdgtHours}, SMU = {eSMU},
			Complaint = {eComplaint}, FlatRate = {eFlatRate}, OROP = {eOROP}, ExcPart = {eExcPart}, 
			PartCost = {ePartCost}, LabourCost = {eLabourCost},OtherCost = {eOtherCost},
			PriceType = {ePriceType},Price = {ePrice},CurrID = {eCurrID},StripDown = {eStripDown}
			,SysTypeID = {eSysTypeID},RootCauseCode = {eRootCauseCode},SysFailCode = {eSysFailCode},
			RecCode = {eRecCode},M1 = {eM1}, M2 = {eM2},M3 = {eM3},M4 = {eM4},
			M5 = {eM5},M6 = {eM6},M7 = {eM7},M8 = {eM8},M9 = {eM9},M10 = {eM10},M11 = {eM11},M12 = {eM12},
			M13 = {eM13}, M14 = {eM14},M15 = {eM15},M16 = {eM16},
			M17 = {eM17},M18 = {eM18},M19 = {eM19},M20 = {eM20},M21 = {eM21},ActionTaken = {eActionTaken},
			EvalByID = {eEvalByID},EvalDate = {eEvalDate},
			EvalCode = {eEvalCode},PSName = {ePSName},PSDate = {ePSDate},POSName = {ePOSName},
			POSDate = {ePOSDate},PMName = {ePMName},PMDate = {ePMDate},
			Conclusion = {eConclusion},ModBy = {eModBy},ModDate = {eModDate},JobID = {eJobID},ConsCost = {eConsCost}
			,SavingCost = {eSavingCost},TCISupply = {eTCISupply}
			,SupplyDate = {eSupplyDate},InstallDate = {eInstallDate},RemoveDate = {eRemoveDate},Remark = {eRemark} WHERE ID = {eID}";

			Console.Write(query);

			SQLFunction.execQuery(query);

			Stat = "success";
			Msg = "Data Has Been Inserted";

			return Redirect("/ComponentEvaluation/Edit/" + ID);
		}
		public IActionResult ReportBody(string ID)
		{
			LoadOption();

			Console.WriteLine("id is " + ID);
			
			string query = $"SELECT * from v_ExrCEDetail WHERE ID = '{ID}'";
			var dataTable = SQLFunction.execQuery(query);
			ViewBag.data = SQLFunction.execQuery(query);
			ViewBag.id = ID;

			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				var unitNumber = dataTable.Rows[0]["unitnumber"].ToString();
				var evalcode = dataTable.Rows[0]["EvalCode"].ToString();
				var wono = dataTable.Rows[0]["Wono"].ToString();

				string queryUnitNumber = $@"SELECT UnitNumber, UnitDescription, Location, LocationName FROM v_UnitNumber WHERE UnitNumber = '{unitNumber}'";
				ViewBag.tUnitNumber = SQLFunction.execQuery(queryUnitNumber);
				Console.WriteLine(queryUnitNumber);

                string querytEvalCode = $@"SELECT EvalCode, EvalDesc, Route, Costing FROM tbl_EXRCEEvalCode WHERE EvalCode = '{evalcode}'";
                ViewBag.tEvalCode = SQLFunction.execQuery(querytEvalCode);

                string queryListInvest = $"SELECT ID,CEID,WONO,InvesDate as [When],InvesDesc as [What],InvesDetail as [Detail],InvesWhen FROM tbl_EXRCEInvestigation where Wono = '{wono}'";
				ViewBag.tListInvest = SQLFunction.execQuery(queryListInvest);
            }

			return View("~/Views/PER/ComponentEvaluation/Reports/Report.cshtml");
		}
		public IActionResult ReportHeading(string ID)
		{
			Console.WriteLine("evaluation wo" + ID);
			return View("~/Views/PER/ComponentEvaluation/Reports/ReportHeader.cshtml");
		}
		public IActionResult ReportFooter()
		{
			return View("~/Views/PER/ComponentEvaluation/Reports/ReportFooter.cshtml");
		}
		public IActionResult Report(string ID)
		{
            string tempAnno = ID;
            string servername = "https://localhost:5001/";
			string namafile;
			string namafile2;
			string savePath = Path.Combine(Directory.GetCurrentDirectory(), "temp");

			if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}
			namafile = Path.Combine(savePath, tempAnno + "_dc.pdf");
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = "C:\\htmltopdf\\wkhtmltopdf.exe",
				Arguments = "--username minestar --password Mine1staR --margin-bottom 10mm --orientation Landscape " +
				   "\"https://localhost:7235/ComponentEvaluation/ReportBody/" + ID +
                   "\" --footer-html  --footer-right \"\"Page [page] of [topage]\"\" --footer-font-size 6 --footer-spacing -3\" \"https://localhost:7235/ComponentEvaluation/ReportFooter" +
				   "\" --footer-spacing 3 --header-html \"https://localhost:7235/ComponentEvaluation/ReportHeading" +
                   "\" --header-spacing 3 " +
                   "\"" + namafile + "\""
            
			};
			Process p = new Process { StartInfo = psi };
			p.Start();
			p.WaitForExit();

			namafile2 = Path.Combine(savePath, tempAnno + "_dc.pdf");
			string ffname = "AN." + tempAnno + ".pdf";

			return PhysicalFile(namafile2, "application/pdf", ffname);
		}
		public IActionResult Export()
		{
			BuildTempFilter();
			string temporder = $"ORDER BY {Request.Form["fSort"]} {Request.Form["fAsc"]}";

			string dataQuery = $@"SELECT Wono,LocationName,UnitDescription,UnitNumber,MaintType,MaintDesc,
			CompTypeDesc as CompType,ReasonTypeDesc as ReasonType,WarrantyResult as Warranty,ExrRepairBy
			,SMU,BdgtHours,Complaint,CurrID,FlatRate,ExcPart as [AddPartCost],PartCost,LabourCost,ConsCost
			,OtherCost,TotalCostBefore,SavingCost,TotalQuoteRev,TCISupply,OROP,PriceType,Price
			,PercentNew as [% New Price],StripDown,SysTypeID,RootCauseCode,RootCauseDesc,SysFailCode
			,SysFail,RecCode,RecDesc,ActionTaken,SupplyDate,InstallDate,RemoveDate,Remark,Conclusion,JobID,
			PCAMID,EvalCode,EvalDesc,EvalDate,EvalByDesc from v_ExrCEDetail {_tempfilter} {temporder}";

			Console.WriteLine(dataQuery);

			DataTable data = SQLFunction.execQuery(dataQuery);
			string fileName = "Component Evaluation.xlsx";

			return Utility.ExportDataTableToExcel(data, fileName);
		}
		public IActionResult ExportInvestigation()
		{
			BuildTempFilter();

			string dataQuery = $@"select wono,InvesDate,InvesDesc,InvesDetail from tbl_EXRCEInvestigation 
			WHERE WOno IN (Select WONO from v_ExrCEDetail {_tempfilter}) order by wono";

			Console.WriteLine(dataQuery);

			DataTable data = SQLFunction.execQuery(dataQuery);
			string fileName = "Component Evaluation Investigation.xlsx";

			return Utility.ExportDataTableToExcel(data, fileName);
		}
		private void LoadOption()
        {
            // load fSpvDesc
			string queryfSpvDesc = "SELECT UserID, UserAbr, UserDesc FROM tbl_EXRUserDetail WHERE (((UserID) Not In (16,17,24,25,28))); ";
			ViewBag.fSpvDesc = SQLFunction.execQuery(queryfSpvDesc);

			// load fUnitDesc
			string queryfUnitDesc = "Select UnitDescription from v_ExrCEDetail where unitDescription is not null Group by UnitDescription Order By UnitDescription ASC";
			ViewBag.fUnitDesc = SQLFunction.execQuery(queryfUnitDesc);

			// load fRootCause
			string queryfRootCause = "SELECT RootCauseCode, RootCauseDesc FROM tbl_EXRCERootCause";
			ViewBag.fRootCause = SQLFunction.execQuery(queryfRootCause);

			// load fEvalCode
			string queryfEvalCode = "SELECT EvalCode, EvalDesc FROM tbl_EXRCEEvalCode";
			ViewBag.fEvalCode = SQLFunction.execQuery(queryfEvalCode);

			// load fmaintbase
			string queryfmaintbase = "SELECT DISTINCT MaintID, MaintIDDesc FROM v_MaintType INNER JOIN tbl_EXRCEDetail ON v_MaintType.MaintType=tbl_EXRCEDetail.MaintType";
			ViewBag.fmaintbase = SQLFunction.execQuery(queryfmaintbase);

			// load fFailedCode
			string queryfFailedCode = "SELECT SysFailCode, SysFail FROM tbl_EXRCESysFail;";
			ViewBag.fFailedCode = SQLFunction.execQuery(queryfFailedCode);

			// load tCompTypeID
			string querytCompTypeID = "SELECT CompTypeID, CompType, CompTypeDesc FROM tbl_EXRCompType WHERE CompTypeID<> 2";
			ViewBag.tCompTypeID = SQLFunction.execQuery(querytCompTypeID);

			// load fRecCode
			string queryfRecCode = "SELECT RecCode, RecDesc FROM tbl_EXRCERec;";
			ViewBag.fRecCode = SQLFunction.execQuery(queryfRecCode);

			// load tReasonType
			string querytReasonType = "SELECT ReasonTypeID, ReasonType, ReasonTypeDesc FROM tbl_EXRReasonType;";
			ViewBag.tReasonType = SQLFunction.execQuery(querytReasonType);

			// load tUnitNumber For Form
			string querytUnitNumber = "SELECT UnitNumber, UnitDescription, Location, LocationName FROM v_UnitNumber;";
			ViewBag.tUnitNumber = SQLFunction.execQuery(querytUnitNumber);

			// load tMaintType For Form
			string querytMaintType = "SELECT MaintType, MaintDesc FROM v_MaintType;";
			ViewBag.tMaintType = SQLFunction.execQuery(querytMaintType);

			// load tCurrID For Form
			string querytCurrID = "Select CurrID from tbl_Currency";
			ViewBag.tCurrID = SQLFunction.execQuery(querytCurrID);

			// load tRootCauseCode For Form
			string querytRootCauseCode = "SELECT RootCauseCode, RootCauseDesc FROM tbl_EXRCERootCause WHERE (((SysTypeID)=3));";
			ViewBag.tRootCauseCode = SQLFunction.execQuery(querytRootCauseCode);

			// load tRecCode For Form
			string querytRecCode = "SELECT RecCode, RecDesc FROM tbl_EXRCERec WHERE SysTypeID=0;";
			ViewBag.tRecCode = SQLFunction.execQuery(querytRecCode);

			// load tSysFailCode For Form
			string querytSysFailCode = "SELECT SysFailCode, SysFail FROM tbl_EXRCESysFail;";
			ViewBag.tSysFailCode = SQLFunction.execQuery(querytSysFailCode);

			// load tEvalByID For Form
			string querytEvalByID = "SELECT UserID, UserDesc, Active FROM tbl_EXRUserDetail WHERE (((Active)=1) AND ((UserID) Not In (16,17,24,25,28)));";
			ViewBag.tEvalByID = SQLFunction.execQuery(querytEvalByID);

			// load tEvalCode For Form
			string querytEvalCode = "SELECT EvalCode, EvalDesc, Route, Costing FROM tbl_EXRCEEvalCode;";
			ViewBag.tEvalCode = SQLFunction.execQuery(querytEvalCode);
		}
    }
}
