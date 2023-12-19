using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System;
using System.Data;

namespace PlantWebApps.Controllers.PER.ComponentEvaluation
{
    public class ComponentEvaluation : Controller
    {
		private string _tempfilter;
		public IActionResult Index()
        {
            LoadOption();
            return View("~/Views/PER/ComponentEvaluation/Index.cshtml");
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
				tempfilter = $" and {CwoTypeCategory} = " + Utility.Evar(CWOType, 1) + tempfilter;
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

			string query = $"SELECT TOP 50 * from v_ExrCEDetail {_tempfilter} {temporder}";
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
					edit = "<a class='btn btn-link btn-sm' href='/ComponentEvaluation/Edit/" + row["ID"] + "'><i class='fa fa-edit'></i></a>",
				};
				rows.Add(rowData);
			}
			return new JsonResult(rows);
		}
		public IActionResult Edit()
		{
			return View("~/Views/PER/ComponentEvaluation/Form.cshtml");
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
		}
    }
}
