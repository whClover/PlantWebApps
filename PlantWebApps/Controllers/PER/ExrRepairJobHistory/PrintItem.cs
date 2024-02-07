using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Data;
using System.Diagnostics;

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
        public IActionResult TestPrintHoldItem()
        {
            return View("~/Views/PER/ExrRepairJobHistory/PrintItem/HoldItem.cshtml");
        }
        public IActionResult TestPrint()
        {
            string jobId = "123";
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
                FileName = "C:\\htmltopdf\\wkhtmltopdf.exe",
                //FileName = "C:\\webroot\\TCRC Web\\Rotativa\\wkhtmltopdf.exe",
                Arguments = $"--username minestar --password Mine1staR --orientation Landscape  https://{host}/PrintItem/TestPrintHoldItem \"{namafile}\""
            };
            Process p = new Process { StartInfo = psi };
            p.Start();
            p.WaitForExit();

            namafile2 = Path.Combine(savePath, jobId + ".pdf");
            string ffname = "Hold item " + jobId + ".pdf";

            return PhysicalFile(namafile2, "application/pdf", ffname);
        }
        public IActionResult PrintHoldItem()
        {
            return new JsonResult("ok");
        }
    }
}
