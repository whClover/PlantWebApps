using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;

namespace PlantWebApps.Controllers.PER.ExrRepairJobHistory
{
    public class FilterHelper : Controller
    {
        [TempData]
        public static String Msg { get; set; }
        [TempData]
        public static String Stat { get; set; }
        public static string SelectSortBy(string sortByOption)
        {
            int parsedFilterOption = int.TryParse(sortByOption, out int result) ? result : 0;
            string sortByValue = "";
            switch (parsedFilterOption)
            {
                case 1:
                    sortByValue = "ID";
                    break;
                case 2:
                    sortByValue = "RegisterDate";
                    break;
                case 3:
                    sortByValue = "DeliveryDate";
                    break;
                case 4:
                    sortByValue = "ReceivedDate";
                    break;
                case 5:
                    sortByValue = "DisputeCompletedDate";
                    break;
            }
            return (sortByValue);
        }
        public static string SelectCreportType(string cReportTypeOption)
        {
            int parsedFilterOption = int.TryParse(cReportTypeOption, out int result) ? result : 0;
            string cReportTypeValue = "";
            switch (parsedFilterOption)
            {
                case 1:
                    cReportTypeValue = "SELECT * from dbo.fR_PERJobDetail()  WHERE ID IN (SELECT ID FROM v_ExrJobDetailRev1)";
                    break;
                case 2:
                    cReportTypeValue = "SELECT * from dbo.fR_EXRORRequestTCI";
                    break;
                case 4:
                    cReportTypeValue = "SELECT * from dbo.fr_EXRInspectionDetail()  WHERE JobID IN (SELECT ID FROM v_ExrJobDetailRev1)";
                    break;
                case 5:
                    cReportTypeValue = "SELECT * from dbo.fr_EXRInspectionDetail()  WHERE JobID IN (SELECT ID FROM v_ExrJobDetailRev1)";
                    break;
                case 6:
                    cReportTypeValue = "SELECT * from dbo.fr_EXRCompSummary()  WHERE JobID IN (SELECT ID FROM v_ExrJobDetailRev1)";
                    break;
                case 7:
                    cReportTypeValue = "SELECT * from dbo.fr_EXRJobUnitSummary()  WHERE JobID IN (SELECT ID FROM v_ExrJobDetailRev1)";
                    break;
                case 8:
                    cReportTypeValue = "SELECT * from dbo.fr_EXRJobUnitSummary()  WHERE JobID IN (SELECT ID FROM v_ExrJobDetailRev1)";
                    break;
                case 9:
                    cReportTypeValue = "SELECT * from dbo.fr_EXRWarranty()  WHERE JobID IN (SELECT ID FROM v_ExrJobDetailRev1)";
                    break;
                case 10:
                    cReportTypeValue = "SELECT * from dbo.fr_EXRWIPReport()  WHERE JobID IN (SELECT ID FROM v_ExrJobDetailRev1)";
                    break;
                case 11:
                    cReportTypeValue = "SELECT * from dbo.fr_ExrVarianceAnalysis  WHERE JobID IN (SELECT ID FROM v_ExrJobDetailRev1)";
                    break;
            }
            return (cReportTypeValue);
        }
        public static string SelectCwoTypeFilter(string cwoTypeOption)
        {
            int parsedFilterOption = int.TryParse(cwoTypeOption, out int result) ? result : 0;
            string cwoCategory = "";

            switch (parsedFilterOption)
            {
                case 1:
                    cwoCategory = "OffSiteWO";
                    break;
                case 2:
                    cwoCategory = "WOFitToUnit";
                    break;
                case 3:
                    cwoCategory = "ChildWO";
                    break;
                case 4:
                    cwoCategory = "IntWO";
                    break;
                case 5:
                    cwoCategory = "WOJobCost";
                    break;
                case 6:
                    cwoCategory = "LCIWONumber";
                    break;
                case 7:
                    cwoCategory = "WOPrevious";
                    break;
                case 8:
                    cwoCategory = "OPPrevious";
                    break;
            }
            return (cwoCategory);
        }
        public static string SelectFdocTypeFilter(string fdocTypeOption)
        {
            int parsedFilterOption = int.TryParse(fdocTypeOption, out int result) ? result : 0;
            string fdocTypeCategory = "";
            switch (parsedFilterOption)
            {
                case 1:
                    fdocTypeCategory = "OPNo";
                    break;
                case 2:
                    fdocTypeCategory = "ORNo";
                    break;
                case 3:
                    fdocTypeCategory = "DNNumber";
                    break;
                case 4:
                    fdocTypeCategory = "STOTNo";
                    break;
                case 5:
                    fdocTypeCategory = "JObNo";
                    break;
                case 6:
                    fdocTypeCategory = "JObNo2nd";
                    break;
                case 7:
                    fdocTypeCategory = "QuoteNo";
                    break;
                case 8:
                    fdocTypeCategory = "ORRRNo";
                    break;
                case 9:
                    fdocTypeCategory = "OPRRNo";
                    break;
                case 10:
                    fdocTypeCategory = "SuppForRepairANNo";
                    break;
                case 11:
                    fdocTypeCategory = "WOJobCost";
                    break;
                case 12:
                    fdocTypeCategory = "LogANReceivedNo";
                    break;
                case 13:
                    fdocTypeCategory = "WOAlloc";
                    break;
            }
            return (fdocTypeCategory);
        }
        public static string SelectCCompIdTypeFilter(string ccompIdTypeOption)
        {
            int parsedFilterOption = int.TryParse(ccompIdTypeOption, out int result) ? result : 0;
            string ccompIdTypeCategory = "";
            switch (parsedFilterOption)
            {
                case 1:
                    ccompIdTypeCategory = "TCIPartNo";
                    break;
                case 2:
                    ccompIdTypeCategory = "TCIPARTID";
                break;
                case 3:
                    ccompIdTypeCategory = "ID = ";
                    break;
                case 4:
                    ccompIdTypeCategory = "UnitNumber";
                    break;
                case 5:
                    ccompIdTypeCategory = "Remark";
                    break;
                case 6:
                    ccompIdTypeCategory = "LCIUnitNumber";
                    break;
                case 7:
                    ccompIdTypeCategory = "SerialNumberOEM";
                    break;
                case 8:
                    ccompIdTypeCategory = "CompDesc";
                    break;
            }
            return (ccompIdTypeCategory);
        }
        public static string SelectImodByFilter(string lmodByOption)
        {
            int parsedFilterOption = int.TryParse(lmodByOption, out int result) ? result : 0;
            string lmodByCategory = "";
            switch (parsedFilterOption)
            {
                case 1:
                    lmodByCategory = "ClosedBy";
                    break;
                case 2:
                    lmodByCategory = "DisputeCompletedBy2";
                    break;
                case 3:
                    lmodByCategory = "DisputeCompletedBy";
                    break;
                case 4:
                    lmodByCategory = "IssuedBy";
                    break;
                case 5:
                    lmodByCategory = "LastChangeBy";
                    break;
                case 6:
                    lmodByCategory = "ModBy";
                    break;
                case 7:
                    lmodByCategory = "LogANReceivedBy";
                    break;
                case 8:
                    lmodByCategory = "QuotePrintBy";
                    break;
                case 9:
                    lmodByCategory = "QuoteProcessBy";
                    break;
                case 10:
                    lmodByCategory = "QuoteApprovedBy";
                    break;
                case 11:
                    lmodByCategory = "ReceivedByName";
                    break;
                case 12:
                    lmodByCategory = "RegisterBy";
                    break;
                default:
                    lmodByCategory = "TaggingBy";
                    break;
            }
            return (lmodByCategory);
        }
        public static string SelectFreasonFilter(string fReasonOption)
        {
            int parsedFilterOption = int.TryParse(fReasonOption, out int result) ? result : 0;
            string fReasonCategory = "";
            switch (parsedFilterOption)
            {
                case 1:
                    fReasonCategory = "Destination";
                    break;
                case 2:
                    fReasonCategory = "Allocation";
                    break;
                case 3:
                    fReasonCategory = "StoreID";
                    break;
            }
            return (fReasonCategory);
        }
        public static string SelectCbDelay(string cbDelayOption)
        {
            int parsedFilterOption = int.TryParse(cbDelayOption, out int result) ? result : 0;
            string cbDelayCategory = "";
            switch (parsedFilterOption)
            {
                case 1:
                    cbDelayCategory = "DiffANReceivedDate";
                    break;
                case 2:
                    cbDelayCategory = "DiffClosed";
                    break;
            }
            return (cbDelayCategory);
        }
        public static string SelectFisNull (string fissNullOption)
        {
            int parsedFilterOption = int.TryParse(fissNullOption, out int result) ? result : 0;
            string fisNullValue = "";
            switch (parsedFilterOption)
            {
                case 1:
                    fisNullValue = "DeliveryDate";
                    break;
                case 2:
                    fisNullValue = "DNNumber";
                    break;
                case 3:
                    fisNullValue = "EGEI";
                    break;
                case 4:
                    fisNullValue = "EIEK";
                    break;
                case 5:
                    fisNullValue = "EKEP";
                    break;
                case 6:
                    fisNullValue = "STOTNo";
                    break;
                case 7:
                    fisNullValue = "JobNo";
                    break;
                case 8:
                    fisNullValue = "RegisterDate";
                    break;
                case 9:
                    fisNullValue = "ReceivedDate";
                    break;
                case 10:
                    fisNullValue = "DocDate";
                    break;
                case 11:
                    fisNullValue = "QuoteRevDate";
                    break;
                case 12:
                    fisNullValue = "QuoteDate";
                    break;
                case 13:
                    fisNullValue = "OPDate";
                    break;
                case 14:
                    fisNullValue = "ORNo";
                    break;
                case 15:
                    fisNullValue = "DisputeCompletedDate";
                    break;
                case 16:
                    fisNullValue = "PRFID";
                    break;
                case 17:
                    fisNullValue = "SuppForRepairANNo";
                    break;
                case 18:
                    fisNullValue = "LogANReceivedDate";
                    break;
                case 19:
                    fisNullValue = "SitetoLogDate";
                    break;
                case 20:
                    fisNullValue = "CEID";
                    break;
                case 21:
                    fisNullValue = "SuppReceiveANDate";
                    break;
                case 22:
                    fisNullValue = "IntWO";
                    break;
            }
            return (fisNullValue);
        }
    }
}
