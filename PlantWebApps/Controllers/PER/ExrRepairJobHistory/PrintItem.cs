using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;
using System.Diagnostics;
using System.Dynamic;

namespace PlantWebApps.Controllers.PER.ExrRepairJobHistory
{
    public class PrintItem : Controller
    {
        public IActionResult Index(string data)
        {
            Console.WriteLine("list Id is " + data);

            string query = "Select * from v_EXRStickerHold Where ID IN (" + data + ")";
            Console.WriteLine(query);

            var dataList = SQLFunction.execQuery(query);

            return View("~/Views/PER/ExrRepairJobHistory/PrintItem/HoldItem.cshtml");
        }
        public IActionResult DataView()
        {
            string filePath = "Views/PER/ExrRepairJobHistory/PrintItem/data.html";
            string hardcodedCode = System.IO.File.ReadAllText(filePath);

            return Content(hardcodedCode, "text/html");
        }
        public IActionResult SendIndex(string value)
        {
            string query = "Select *, CONVERT(varchar, PERANDate, 103) AS FormatDate from v_EXRStickerSend Where ID IN (" + value + ")";
            Console.WriteLine(query);

            var dataTable = SQLFunction.execQuery(query);

            List<dynamic> dataList = new List<dynamic>();

            foreach (DataRow row in dataTable.Rows)
            {
                dynamic dataItem = new ExpandoObject();
                var rowData = dataItem as IDictionary<string, object>;

                foreach (DataColumn column in dataTable.Columns)
                {
                    rowData[column.ColumnName] = row[column];
                }

                dataList.Add(dataItem);
            }
            ViewBag.data = dataList;

            return View("~/Views/PER/ExrRepairJobHistory/PrintItem/SendItem.cshtml");
        }
        public IActionResult HoldIndex(string value)
        {
            string query = "Select *, CONVERT(varchar, OHDATE, 103) AS FormatDate from v_EXRStickerHold Where ID IN (" + value + ")";
            Console.WriteLine(query);

            var dataTable = SQLFunction.execQuery(query);

            List<dynamic> dataList = new List<dynamic>();

            foreach (DataRow row in dataTable.Rows)
            {
                dynamic dataItem = new ExpandoObject();
                var rowData = dataItem as IDictionary<string, object>;

                foreach (DataColumn column in dataTable.Columns)
                {
                    rowData[column.ColumnName] = row[column];
                }

                dataList.Add(dataItem);
            }
            ViewBag.data = dataList;

            return View("~/Views/PER/ExrRepairJobHistory/PrintItem/HoldItem.cshtml");
        }
        public IActionResult AfterRepairStandard(string value)
        {
            string query = "SELECT *, CONVERT(varchar, ReceivedDate, 103) AS FormatDate FROM v_ExrRepairTagx WHERE ID IN (" + value + ")";
            Console.WriteLine(query);

            var dataTable = SQLFunction.execQuery(query);

            List<dynamic> dataList = new List<dynamic>();

            foreach (DataRow row in dataTable.Rows)
            {
                dynamic dataItem = new ExpandoObject();
                var rowData = dataItem as IDictionary<string, object>;

                foreach (DataColumn column in dataTable.Columns)
                {
                    rowData[column.ColumnName] = row[column];

                    var eid = rowData["ID"];
                    Console.WriteLine("console" + eid);

                    if (eid != "")
                    {
                        string dataQuery = $"select concat('Parent WO ',OffsiteWO,', ','Offsite WO ',ChildWO,',','Internal WO ',IntWO,', ','PN ','(',TCIPartNo,')',', ','Description1',', ','OP ','<',OPNONew,'>',', ','Unit No ','[',UnitNumber,'] ', 'Unit Desc ','@',UnitDescription,'#') as Combine from v_ExrRepairTagx where ID= {Utility.Evar(eid, 0)}";

                        var dataResult = SQLFunction.execQuery(dataQuery);
                        Console.WriteLine("data result is " + dataQuery);
                        var combineValues = new List<string>();

                        foreach (DataRow dataRow in dataResult.Rows)
                        {
                            combineValues.Add(dataRow["Combine"].ToString());
                        }

                        rowData["DataValue"] = combineValues;

                        Console.WriteLine("is id");
                    }
                    else
                    {
                        string dataQuery = @"select concat('Parent WO ',OffsiteWO,', ','Offsite WO ',ChildWO,', ','Internal WO ',IntWO,', ','PN ','(',TCIPartNo,')',', ','Description1',', ','OP ','<',OPNONew,'>',', ','Unit No ','[',UnitNumber,'] ', 'Unit Desc ','@',UnitDescription,'#') as Combine from v_ExrRepairTagx where ID=325";

                        var dataResult = SQLFunction.execQuery(dataQuery);
                        var dataRow = dataResult.Rows[0];
                        rowData["DataValue"] = dataRow["Combine"];

                        Console.WriteLine("is not id");
                    }
                }

                dataList.Add(dataItem);
            }
            ViewBag.data = dataList;

            return View("~/Views/PER/ExrRepairJobHistory/PrintItem/AfterRepairStandard.cshtml");
        }
        public IActionResult AfterRepairDirect(string value)
        {
            string query = "SELECT *, CONVERT(varchar, ReceivedDate, 103) AS FormatDate FROM v_ExrRepairTagx WHERE ID IN (" + value + ")";
            Console.WriteLine(query);

            var dataTable = SQLFunction.execQuery(query);

            List<dynamic> dataList = new List<dynamic>();

            foreach (DataRow row in dataTable.Rows)
            {
                dynamic dataItem = new ExpandoObject();
                var rowData = dataItem as IDictionary<string, object>;

                foreach (DataColumn column in dataTable.Columns)
                {
                    rowData[column.ColumnName] = row[column];

                    var eid = rowData["ID"];
                    Console.WriteLine("console" + eid);

                    if (eid != "")
                    {
                        string dataQuery = $"select concat('Parent WO ',OffsiteWO,', ','Offsite WO ',ChildWO,',','Internal WO ',IntWO,', ','PN ','(',TCIPartNo,')',', ','Description1',', ','OP ','<',OPNONew,'>',', ','Unit No ','[',UnitNumber,'] ', 'Unit Desc ','@',UnitDescription,'#') as Combine from v_ExrRepairTagx where ID= {Utility.Evar(eid, 0)}";

                        var dataResult = SQLFunction.execQuery(dataQuery);
                        Console.WriteLine("data result is " + dataQuery);
                        var combineValues = new List<string>();

                        foreach (DataRow dataRow in dataResult.Rows)
                        {
                            combineValues.Add(dataRow["Combine"].ToString());
                        }

                        rowData["DataValue"] = combineValues;

                        Console.WriteLine("is id");
                    }
                    else
                    {
                        string dataQuery = @"select concat('Parent WO ',OffsiteWO,', ','Offsite WO ',ChildWO,', ','Internal WO ',IntWO,', ','PN ','(',TCIPartNo,')',', ','Description1',', ','OP ','<',OPNONew,'>',', ','Unit No ','[',UnitNumber,'] ', 'Unit Desc ','@',UnitDescription,'#') as Combine from v_ExrRepairTagx where ID=325";

                        var dataResult = SQLFunction.execQuery(dataQuery);
                        var dataRow = dataResult.Rows[0];
                        rowData["DataValue"] = dataRow["Combine"];

                        Console.WriteLine("is not id");
                    }
                }

                dataList.Add(dataItem);
            }
            ViewBag.data = dataList;

            return View("~/Views/PER/ExrRepairJobHistory/PrintItem/AfterRepairDirect.cshtml");
        }
        public IActionResult PrintHoldItem()
        {
            return new JsonResult("ok");
        }
        public IActionResult TestJsPdfAutoTable()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Testing/Index.cshtml");
        }
    }
}
