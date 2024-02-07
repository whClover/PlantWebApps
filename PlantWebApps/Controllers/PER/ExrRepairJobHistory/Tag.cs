using Microsoft.AspNetCore.Mvc;
using PlantWebApps.Helper;
using System.Diagnostics;
using System.Drawing;

namespace PlantWebApps.Controllers.PER.ExrRepairJobHistory
{
    public class Tag : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/PER/ExrRepairJobHistory/Tag/Tag.cshtml");
        }
        public IActionResult StandardTag() 
        {
            return View("~/Views/PER/ExrRepairJobHistory/Tag/Report/Standard.cshtml");
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
                Arguments = $"--username minestar --password Mine1staR --orientation Landscape  https://{host}/Tag/StandardTag \"{namafile}\""
            };
            Process p = new Process { StartInfo = psi };
            p.Start();
            p.WaitForExit();

            namafile2 = Path.Combine(savePath, jobId + ".pdf");
            string ffname = "After Repair Tag " + jobId + ".pdf";

            return PhysicalFile(namafile2, "application/pdf", ffname);
        }
        public IActionResult GenerateQRCodeImage(string value)
        {
            Bitmap qrCodeImage = Utility.GenerateQRCode(value);

            return File(Utility.BitmapToByteArray(qrCodeImage), "image/png");
        }
    }
}