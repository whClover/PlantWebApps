using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;

namespace PlantWebApps.Controllers.PER.ExrRepairJobHistory
{
    public class Upload : Controller
    {
        public async Task<ActionResult> WO(IFormFile fileData)
        {
            var fileName = Path.GetFileName(fileData.FileName);
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "temp"); // local
            var filePath = Path.Combine(directory, fileName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileData.CopyToAsync(stream);
            }

			FileInfo fileInfo = new FileInfo(filePath);
			if (fileInfo.Exists)
			{
				string upload = await UploadWo(filePath);


                if (upload == "invalid")
                {
                    return new JsonResult("invalid");
                }

                if (upload == "finish")
                {
                    return new JsonResult("finish");
                }

                if (upload == "error")
                {
                    return new JsonResult("error");
                }
                return new JsonResult("ok");
            }
			else
			{
				return new JsonResult("not found"); ;
			}
        }
        public async Task<ActionResult> OP(IFormFile fileData)
        {
            var fileName = Path.GetFileName(fileData.FileName);
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "temp"); // local
            var filePath = Path.Combine(directory, fileName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileData.CopyToAsync(stream);
            }

            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                string upload = await UploadOP(filePath);


                if (upload == "invalid")
                {
                    return new JsonResult("invalid");
                }

                if (upload == "finish")
                {
                    return new JsonResult("finish");
                }

                if (upload == "error")
                {
                    return new JsonResult("error");
                }
                return new JsonResult("ok");
            }
            else
            {
                return new JsonResult("not found"); ;
            }
        }
        private async Task<string> UploadWo(string filePath)
        {
            try
            {
                int estatusid = 1;
                int er = 0;
                int up = 0;
                int it = 0;


                int i = 0;
                int kWONo = -1;
                int kUnitNumber = -1;
                int kUnitDesc = -1;
                int kWoDesc = -1;
                int kWOStatus = -1;
                int kDocType = -1;
                int kPriority = -1;
                int kWoOrigin = -1;
                int kRepairType = -1;
                int kRequestedDate = -1;
                int kActualStartDate = -1;
                int kScheduledCompDate = -1;
                int kCompleteDate = -1;
                int kCompGroup = -1;
                int kMaintType = -1;
                int kMaintDesc = -1;
                int kParentWO = -1;
                int kWarantyClaimNo = -1;
                int kManager = -1;
                int kSupervisor = -1;
                int kSupervisorDesc = -1;
                int kDocDate = -1;
                int kStatusComment = -1;
                int kStoreID = -1;
                int kuserID = -1;
                int kWoUpdateDate = -1;
                int kAddressNo = -1;
                int kAddressNoDesc = -1;
                int kIssue = -1;
                int kRef = -1;
                int kSRCat = -1;
                int kAssignedTo = -1;
                int kAssignedDesc = -1;

                string eChildWO = "";
                string eUnitNumber = "";
                string eWodesc = "";
                string eunitdesc = "";
                string eWOStatus = "";
                string eDocType = "";
                string ePriority = "";
                string eWOOrigin = "";
                string eRepairType = "";
                string eRequestedDate = "";
                string eActualStartDate = "";
                string eScheduledCompDate = "";
                string eCompleteDate = "";
                string ecompgroup = "";
                string emainttype = "";
                string eMaintDesc = "";
                string eparentwo = "";
                string eWarantyClaimNo = "";
                string eManager = "";
                string esupervisorid = "";
                string eSupervisorDesc = "";
                string eDocDate = "";
                string eStatusComment = "";
                string eStoreID = "";
                string eUserID = "";
                string eWoUpdateDate = "";
                string eAddressNo = "";
                string eAddressNoDesc = "";
                string eIssue = "";
                string eRef = "";
                string eSRCat = "";
                string eAssignedTo = "";
                string eAssignedDesc = "";
                string eCompType = "";
                string eReasonTypeID = "";


                using (var reader = new StreamReader(new FileStream(filePath, FileMode.Open)))
                {
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            string eLineInput = reader.ReadLine();
                            string eLa = eLineInput;
                            if (i == 0)
                            {
                                string[] hea = eLa.Split('\t');
                                for (int h = 0; h < hea.Length; h++)
                                {
                                    if (hea[h] == "Work Order No.") kWONo = h;
                                    if (hea[h] == "Unit Number") kUnitNumber = h;
                                    if (hea[h] == "Asset Number Description") kUnitDesc = h;
                                    if (hea[h] == "Work Order Description") kWoDesc = h;
                                    if (hea[h] == "WO St") kWOStatus = h;
                                    if (hea[h] == "Doc Type") kDocType = h;
                                    if (hea[h] == "Priority") kPriority = h;
                                    if (hea[h] == "Work Order Origin") kWoOrigin = h;
                                    if (hea[h] == "Repair Type") kRepairType = h;
                                    if (hea[h] == "Request Date") kRequestedDate = h;
                                    if (hea[h] == "Start Date") kActualStartDate = h;
                                    if (hea[h] == "Planned Complete") kScheduledCompDate = h;
                                    if (hea[h] == "Complete Date") kCompleteDate = h;
                                    if (hea[h] == "Component Group") kCompGroup = h;
                                    if (hea[h] == "Service Type") kMaintType = h;
                                    if (hea[h] == "Service Type Description") kMaintDesc = h;
                                    if (hea[h] == "Parent W.O.") kParentWO = h;
                                    if (hea[h] == "Warranty Claim No.") kWarantyClaimNo = h;
                                    if (hea[h] == "Manager") kManager = h;
                                    if (hea[h] == "Supervisor") kSupervisor = h;
                                    if (hea[h] == "Supervisor Description") kSupervisorDesc = h;
                                    if (hea[h] == "Assigned To") kAssignedTo = h;
                                    if (hea[h] == "Assigned To Description") kAssignedDesc = h;
                                    if (hea[h] == "Doc Date") kDocDate = h;
                                    if (hea[h] == "Status Comment") kStatusComment = h;
                                    if (hea[h] == "Issue") kIssue = h;
                                    if (hea[h] == "Store") kStoreID = h;
                                    if (hea[h] == "User ID") kuserID = h;
                                    if (hea[h] == "Date Updated") kWoUpdateDate = h;
                                    if (hea[h] == "Address Number") kAddressNo = h;
                                    if (hea[h] == "Address Number Description") kAddressNoDesc = h;
                                    if (hea[h] == "Reference") kRef = h;
                                    if (hea[h] == "Special Reporting Cat") kSRCat = h;

                                }
                                if (kUnitNumber == -1 || kWONo == -1 || kParentWO == -1 || kSupervisor == -1 || kAddressNo == -1 || kStatusComment == -1 || kMaintType == -1 || kRequestedDate == -1)
                                {
                                    return "invalid";
                                }
                            }
                            else
                            {
                                string[] data = eLa.Split('\t');

                                string[] arrerr = new string[1];
                                arrerr[0] = "Parent WO,ChildWO,Source,Remark";
                                string errstring = "";
                                int xx = 0;

                                eChildWO = Utility.Evar(data[kWONo], 1);
                                eUnitNumber = Utility.Evar(data[kUnitNumber], 1);
                                eunitdesc = Utility.Evar(data[kWoDesc], 1);
                                eWOStatus = Utility.Evar(data[kWOStatus], 1);
                                eDocType = Utility.Evar(data[kDocType], 1);
                                ePriority = Utility.Evar(data[kPriority], 1);
                                eWOOrigin = Utility.Evar(data[kWoOrigin], 1);
                                eRepairType = Utility.Evar(data[kRepairType], 1);
                                eRequestedDate = Utility.Evar(data[kRequestedDate], 2);
                                eActualStartDate = Utility.Evar(data[kActualStartDate], 2);
                                eScheduledCompDate = Utility.Evar(data[kScheduledCompDate], 2);
                                eCompleteDate = Utility.Evar(data[kCompleteDate], 2);
                                ecompgroup = Utility.Evar(data[kCompGroup], 1);
                                emainttype = Utility.Evar(data[kMaintType], 1);
                                eMaintDesc = Utility.Evar(data[kMaintDesc], 1);
                                eparentwo = Utility.Evar(data[kParentWO], 1);
                                eWarantyClaimNo = Utility.Evar(data[kWarantyClaimNo], 1);
                                eManager = Utility.Evar(data[kManager], 1);
                                esupervisorid = Utility.Evar(data[kSupervisor], 1);
                                eSupervisorDesc = Utility.Evar(data[kSupervisorDesc], 1);
                                eDocDate = Utility.Evar(data[kDocDate], 2);
                                eStatusComment = Utility.Evar(data[kStatusComment], 1);
                                eStoreID = Utility.Evar(data[kStoreID], 1);
                                eUserID = Utility.Evar(data[kuserID], 1);
                                eWoUpdateDate = Utility.Evar(data[kWoUpdateDate], 2);
                                eAddressNo = Utility.Evar(data[kAddressNo], 1);
                                eAddressNoDesc = Utility.Evar(data[kAddressNoDesc], 1);
                                eIssue = Utility.Evar(data[kIssue], 1);
                                eRef = Utility.Evar(data[kRef], 1);
                                eSRCat = Utility.Evar(data[kSRCat], 1);
                                eAssignedTo = Utility.Evar(data[kAssignedTo], 1);
                                eAssignedDesc = Utility.Evar(data[kAssignedDesc], 1);

                                if (eIssue != "NULL")
                                {
                                    eWodesc = eIssue;
                                }
                                else
                                {
                                    eWodesc = Utility.Evar(data[kUnitDesc], 1);
                                }

                                switch (eRepairType)
                                {
                                    case "'RXT'":
                                        eCompType = "3";
                                        break;
                                    case "'RXX'":
                                        eCompType = "1";
                                        break;
                                    case "'RNP'":
                                        eCompType = "4";
                                        break;
                                    default:
                                        eCompType = "NULL";
                                        break;
                                }

                                switch (eSRCat)
                                {
                                    case "'RAD'":
                                        eReasonTypeID = "5";
                                        break;
                                    case "'RCC'":
                                        eReasonTypeID = "2";
                                        break;
                                    case "'RWX'":
                                        eReasonTypeID = "3";
                                        break;
                                    default:
                                        eReasonTypeID = "NULL";
                                        break;
                                }

                                string generalQuery = $"Select * from tbl_WODetail Where WONo= {eChildWO}";

                                var generalData = SQLFunction.execQuery(generalQuery);

                                if (generalData.Rows.Count > 0)
                                {
                                    string query = $@"UPDATE tbl_WODetail SET UnitNumber = {eUnitNumber}, UnitDesc = {eunitdesc},
                                                    WoDesc = {eWodesc}, WoStatus = {eWOStatus}, DocType = {eDocType}, Priority = {ePriority},
                                                    WOOrigin = {eWOOrigin}, RepairType = {eRepairType}, RequestedDate = {eRequestedDate},
                                                    ActualStartDate = {eActualStartDate}, ScheduledCompDate = {eScheduledCompDate},
                                                    CompleteDate = {eCompleteDate}, CompGroup = {ecompgroup}, MaintType = {emainttype},
                                                    ParentWo = {eparentwo}, WarantyClaimNo = {eWarantyClaimNo}, Manager = {eManager},
                                                    Supervisor = {esupervisorid}, AssignedTo = {eAssignedTo}, DocDate = {eDocDate},
                                                    StatusComment = {eStatusComment}, StoreID = {eStoreID}, UserID = {eUserID},
                                                    WoUpdateDate = {eWoUpdateDate}, AddressNo = {eAddressNo}, LastUpdate = {Utility.Evar(Utility.getDate(), 2)},
                                                    LastUpdateBy = {Utility.ebyname()} WHERE WONO= {eChildWO}";

                                    SQLFunction.execQuery(query);
                                }
                                else
                                {
                                    string query = $@"INSERT INTO tbl_WODetail (WONo,UnitNumber,UnitDesc,WoDesc,WOStatus,
                                                    DocType,Priority,WOOrigin,RepairType,RequestedDate,ActualStartDate,
                                                    ScheduledCompDate,CompleteDate,CompGroup,MaintType,ParentWO,WarantyClaimNo,
                                                    Manager,Supervisor,AssignedTo,DocDate,StatusComment,StoreID,UserID,
                                                    WoUpdateDate,AddressNo,LastUpdate,LastUpdateBy) VALUES ({eChildWO},
                                                    {eUnitNumber}, {eunitdesc}, {eWodesc}, {eWOStatus}, {eDocType}, {ePriority},
                                                    {eWOOrigin}, {eRepairType}, {eRequestedDate}, {eActualStartDate},{eScheduledCompDate},
                                                    {eCompleteDate}, {ecompgroup}, {emainttype}, {eparentwo}, {eWarantyClaimNo}
                                                    , {eManager}, {esupervisorid}, {eAssignedTo}, {eDocDate}, {eStatusComment}, 
                                                    {eStoreID}, {eUserID}, {eWoUpdateDate}, {eAddressNo}, 
                                                    {Utility.Evar(Utility.getDate(), 2)}, {Utility.ebyname()})";

                                    SQLFunction.execQuery(query);
                                }

                                string eCompDesc = eWodesc.Replace("Offsite Repair -", "");
                                string eAssigned = eAssignedTo.Replace("'", "");

                                string esupplierid = eAddressNo;
                                string eRemark = eStatusComment;

                                string erephours = "NULL";
                                string eCustomerDesc = "NULL";

                                // Inserting Supervisor Detail
                                string supervisorDetailQuery = $@"Select SUpervisorID from tbl_Supervisor Where SupervisorID= {esupervisorid}";
                                var supervisorData = SQLFunction.execQuery(supervisorDetailQuery);

                                if (supervisorData.Rows.Count < 0)
                                {
                                    string query = $@"INSERT INTO dbo_tbl_Supervisor (SupervisorID,SupervisorDesc) VALUES ({esupervisorid},
                                                    {eSupervisorDesc})";

                                    SQLFunction.execQuery(query);
                                }

                                // Inserting Customer Detail
                                if (eAssigned != "NULL")
                                {
                                    string dataQuery = $@"Select SupplierID from tbl_SupplierList Where SupplierID={eAssigned}";
                                    var supplierData = SQLFunction.execQuery(dataQuery);

                                    if (supplierData.Rows.Count < 0)
                                    {
                                        string query = $@"INSERT INTO tbl_SupplierList (SupplierID,SupplierName) VALUES ({eAssigned},
                                                    {eAssignedDesc})";

                                        SQLFunction.execQuery(query);
                                    }
                                }

                                // Inserting To Maint Type
                                if (emainttype != "NULL")
                                {
                                    string dataQuery = $@"Select MaintType from tbl_MaintType Where MaintType={emainttype}";
                                    var maintypeData = SQLFunction.execQuery(dataQuery);

                                    if (maintypeData.Rows.Count < 0)
                                    {
                                        string query = $@"INSERT INTO tbl_MaintType (MaintType,MaintDesc) VALUES ({emainttype},
                                                    {eMaintDesc})";

                                        SQLFunction.execQuery(query);
                                    }
                                }

                                // CHECKING on WO OUTSTANDING
                                string woOutstandingQuery = $@"SELECT * FROM tbl_ExrJobDetail WHERE OffSiteWO =  {eparentwo} AND StatusID = 2";
                                var woOutstandingData = SQLFunction.execQuery(woOutstandingQuery);

                                if (woOutstandingData.Rows.Count > 0)
                                {
                                    errstring = $@"{eparentwo}, {eChildWO}, At Row Number {i - 1}, Parent Wo Already Registered In Outstranding Status";
                                    goto gotoerrdisp;
                                }

                                // CHECKING on WO External
                                string woExternalQuery = $@"SELECT OffSIteWO FROM tbl_ExrJobDetail WHERE LCIWONumber = {eparentwo}";
                                var woExternalData = SQLFunction.execQuery(woExternalQuery);

                                if (woExternalData.Rows.Count > 0)
                                {
                                    errstring = $@"{eparentwo}, {eChildWO}, At Row Number {i - 1}, Parent WO Already Registered As External WO At Parent WO#({woExternalData.Rows[0]["OffSIteWO"]})";
                                    goto gotoerrdisp;
                                }

                                // Get StoreID
                                string storeIDQuery = $@"select * from tbl_StoreTranslator where ProjectStore= {eStoreID}";
                                var storeIDData = SQLFunction.execQuery(storeIDQuery);

                                if (storeIDData.Rows.Count < 0)
                                {
                                    eStoreID = "NULL";
                                }
                                else
                                {
                                    eStoreID = Utility.Evar(storeIDData.Rows[0]["StoreID"].ToString(), 1);
                                }

                                string jobDetailQuery = $@"SELECT ID FROM tbl_ExrJobDetail WHERE OffSiteWO = {eparentwo} AND ChildWO = {eChildWO}";
                                var jobDetailData = SQLFunction.execQuery(jobDetailQuery);

                                if (jobDetailData.Rows.Count < 0)
                                {
                                    string eRepairAdvice = "'NW'";
                                    string query = $@"INSERT INTO tbl_ExrJobDetail (ChildWO,UnitNumber,CompDesc,OffSiteWO,
                                    SupervisorID,SupplierID,MaintType,Rephours,Remark,StatusID,RepairAdvice,CompTypeID,StoreID,
                                    RepairByID,SitetoLogDate,SuppForRepairANNo,SuppReceiveANDate,ReasonTypeID,RegisterBy,
                                    RegisterDate,DocAvb,DocAvbDate,DocAvbBy) VALUES ({eChildWO}, {eUnitNumber}, {eCompDesc},
                                    {eparentwo}, {esupervisorid}, {eAssignedTo}, {emainttype}, {erephours}, {eRemark}, {estatusid},
                                    {eRepairAdvice}, {eCompType}, {eStoreID}, {eAssignedTo}, {eScheduledCompDate}, {eRef}, {eActualStartDate},
                                    {eReasonTypeID}, {Utility.ebyname()}, {Utility.Evar(Utility.getDate(), 2)}, 0, {Utility.Evar(Utility.getDate(), 2)},
                                    {Utility.ebyname()})";


                                    it += 1;
                                }
                                else
                                {
                                    errstring = $@"{eparentwo}, {eChildWO}, At Row Number {i - 1}, WO Already Exist"; 
                                    up += 1;
                                    goto gotoerrdisp;
                                }
                                gotoerrdisp:
                                    xx = xx + 1;
                                    Array.Resize(ref arrerr, xx + 1);
                                    arrerr[xx] = errstring.Replace("'", "");

                                if (arrerr.GetUpperBound(0) >= 1)
                                {
                                    Utility.LaunchArrayToExcel(arrerr);
                                }
                            }
                            i++;
                        }
                        catch (Exception ex)
                        {
                            return "error";
                        }
                    }
                }
                return "finish";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        private async Task<string> UploadOP(string filePath)
        {
            try
            {
                int i = 0;
                int kORNo = -1;
                int kOPNO = -1;
                int kDocDate = -1;

                string eORNo = "";
                string eOPNo = "";
                string eDocDate = "";

                using (var reader = new StreamReader(new FileStream(filePath, FileMode.Open)))
                {
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            string eLineInput = reader.ReadLine();
                            string eLa = eLineInput;
                            if (i == 0)
                            {
                                string[] hea = eLa.Split('\t');
                                for (int h = 0; h < hea.Length; h++)
                                {
                                    if (hea[h] == "Orig Doc No. [F4311]") kORNo = h;
                                    if (hea[h] == "Doc No. [F4311]") kOPNO = h;
                                    if (hea[h] == "Doc Date [F4311]") kDocDate = h;

                                }
                                if (kORNo == -1 || kOPNO == -1)
                                {
                                    return "invalid";
                                }
                            }
                            else
                            {
                                string[] data = eLa.Split('\t');

                                eORNo = Utility.Evar(data[kORNo], 1);
                                eOPNo = Utility.Evar(data[kOPNO], 1);
                                eDocDate = Utility.Evar(data[kDocDate], 1);

                                string query = @$"update tbl_ExrJobDetail set OPNo={eOPNo}, OPDate={eDocDate}, 
                                                where ORNo={eORNo} and OPNo is null";
                                Console.WriteLine(query);
                                //SQLFunction.execQuery(query);
                            }
                            i++;
                        }
                        catch (Exception ex)
                        {
                            return "error";
                        }
                    }
                }
                return "finish";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        public ActionResult DownloadExcelFile()
        {
            string tempPath = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            string filePath = Path.Combine(tempPath, "output.xlsx");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return PhysicalFile(filePath, contentType, "output.xlsx");
        }
    }
}
