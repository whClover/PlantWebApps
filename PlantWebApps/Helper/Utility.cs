﻿using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Collections;
using System.Data;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using QRCoder;
using System.Drawing.Imaging;

namespace PlantWebApps.Helper
{
    public class Utility
    {
        public static string eusername()
        {
            string username = Environment.UserName;
            return username;
        }

        public static String ebyname() 
        {
            String username = Utility.Evar(GlobalString._username, 1);
            return username;
        }

        public static string getDate()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            return date;
        }
		public static string GetUserId()
		{
			string userId = string.Empty;
			string userName = ebyname();
			string query = $"SELECT UserID FROM vw_TCRPUserInformation WHERE UserName = {userName}";
			DataTable userIdTable = SQLFunction.execQuery(query);

			if (userIdTable.Rows.Count > 0)
			{
				userId = userIdTable.Rows[0]["UserID"].ToString();
			}

			return userId;
		}
		public static string GetCurrentUsername()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            string username = identity.Name;

            // Check if the user is in a Windows group
            bool isInWindowsGroup = principal.IsInRole(WindowsBuiltInRole.Administrator);

            // Additional checks or processing can be performed here

            return username;
        }

        public static bool checkgranted(string formname, string useraction)
        {
            //check user-exists
            string uexists = "select * from tbl_user where username=" + ebyname();
            DataTable data = new DataTable();
            data = SQLFunction.execQuery(uexists);

            if(data.Rows.Count == 0)
            {
                return false;
            }

            string userid = data.Rows[0]["userid"].ToString();
            string userpriv = data.Rows[0]["Previllege"].ToString();

            if(userpriv == "Administrator")
            {
                return true;
            }

            string uform = "Select dbo.CheckGranted(" + userid + "," + Utility.Evar(formname, 1) + "," + useraction + ") as Granted";
            DataTable data2 = new DataTable();
            data2 = SQLFunction.execQuery(uform);
            if (data.Rows.Count == 0)
            {
                return false;
            }

            string granted = data2.Rows[0]["Granted"].ToString();
            if(granted == "1")
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
        public static string Evar(object val, int valtype, int vallen = 255)
        {
            string eval;

			val = CheckNull(val);

			val = val.ToString().Replace("\"", "");
            val = val.ToString().Replace("#N/A", "");
            val = val.ToString().Trim();

            if (val == null || val.ToString() == "" || val.ToString() == " " || val.ToString() == string.Empty)
            {
                eval = "NULL";
            }
            else
            {
                val = val.ToString().Trim();
                switch (valtype)
                {
                    case 0:
                        if (double.TryParse(val.ToString(), out double numericVal))
                        {
                            eval = numericVal.ToString();
                        }
                        else
                        {
                            eval = "NULL";
                        }
                        break;
                    case 1:
                        if (val.ToString().Split(',').Any())
                        {
                            eval = "'" + string.Join("','", val.ToString().Split(',')) + "'";
                        }
                        else
                        {
                            eval = "NULL";
                        }
                        break;
                    case 2:
                        if (DateTime.TryParse(val.ToString(), out DateTime dateVal))
                        {
                            eval = "'" + dateVal.ToString("yyyy-M-d") + "'";
                        }
                        else
                        {
                            eval = "NULL";
                        }
                        break;
                    case 3:
                        if (DateTime.TryParse(val.ToString(), out DateTime dateTimeVal))
                        {
                            eval = "'" + dateTimeVal.ToString("yyyy-M-d H:m:s") + "'";
                        }
                        else
                        {
                            eval = "NULL";
                        }
                        break;
                    case 4:
                        eval = "'" + val.ToString().Substring(0, Math.Min(val.ToString().Length, 3750)) + "'";
                        break;
                    case 11:
                        eval = "'%" + val.ToString().Substring(0, Math.Min(val.ToString().Length, vallen)) + "%'";
                        break;
                    case 12:
                        eval = "'" + val.ToString().Replace(",", "','") + "'";
                        break;
                    case 13:
                        eval = "'%," + val.ToString().Substring(0, Math.Min(val.ToString().Length, vallen)) + ",%'";
                        break;
                    case 14:
                        eval = val.ToString();
                        break;
                    case 15:
                        if (DateTime.TryParse(val.ToString(), out DateTime dateVal2))
                        {
                            eval = dateVal2.ToString("d-MMM-yyyy");
                        }
                        else
                        {
                            eval = "NULL";
                        }
                        break;
                    case 16:
                        if (DateTime.TryParse(val.ToString(), out DateTime dateVal3))
                        {
                            eval = $"{dateVal3.ToString("yyyy")}.{GenTwiDig(dateVal3.ToString("MM"))}.{GenTwiDig(dateVal3.ToString("dd"))}";
                        }
                        else
                        {
                            eval = "NULL";
                        }
                        break;
                    case 17:
                        if (DateTime.TryParse(val.ToString(), out DateTime dateVal4))
                        {
                            eval = dateVal4.ToString("dd");
                        }
                        else
                        {
                            eval = "NULL";
                        }
                        break;
                    case 18:
                        eval = val.ToString().Replace("'", "");
                        break;
                    case 19:
                        eval = "(" + val.ToString().Substring(0, Math.Min(val.ToString().Length, vallen)) + ")";
                        break;
                    case 20:
                        if (DateTime.TryParse(val.ToString(), out DateTime dateTimehoursVal))
                        {
                            eval = "'" + dateTimehoursVal.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                        }
                        else
                        {
                            eval = "NULL";
                        }
                        break;
                    default:
                        eval = "'" + val.ToString().Substring(0, Math.Min(val.ToString().Length, vallen)) + "'";
                        break;
                }
            }

            return eval;
        }

        private static string GenTwiDig(string val)
        {
            return val.PadLeft(2, '0');
        }

        private static string ConvertToLetter(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = string.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        public static string VarFilter(string F)
        {
            if (F.Length > 0)
                return " WHERE " + F.Substring(4);
            else
                return string.Empty;
        }
		public static IActionResult ExportDataTableToExcel(DataTable dataTable, string fileName)
        {
            // Create a new Excel package
            using (ExcelPackage package = new ExcelPackage())
            {
                // Create a new worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Set the header row index
                int headerRowIndex = 1;

                // Populate the worksheet with data from the DataTable
                worksheet.Cells["A" + (headerRowIndex + 0)].LoadFromDataTable(dataTable, true);

                // Set the header row style
                using (ExcelRange headerRow = worksheet.Cells["A" + headerRowIndex + ":" + ConvertToLetter(dataTable.Columns.Count) + headerRowIndex])
                {
                    headerRow.Style.Font.Color.SetColor(Color.Black);
                    headerRow.Style.Font.Bold = true;
                    headerRow.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRow.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(209, 220, 235));
                }

                // Convert the Excel package to a byte array
                byte[] fileBytes = package.GetAsByteArray();

                // Return the Excel file as a byte array for download
                return new FileContentResult(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = fileName
                };
            }
        }
        public static void LaunchArrayToExcel(object[] arrerr)
        {
            int rowCount = arrerr.Length;
            if (rowCount <= 0)
                return;

            string[] firstRowData = arrerr[0].ToString().Split(',');
            int colCount = firstRowData.Length;

            if (colCount <= 0)
                return;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                for (int iRow = 0; iRow < rowCount; iRow++)
                {
                    string[] rowData = arrerr[iRow].ToString().Split(',');
                    for (int iCol = 0; iCol < colCount; iCol++)
                    {
                        worksheet.Cells[iRow + 1, iCol + 1].Value = rowData[iCol];
                    }
                }

                using (ExcelRange headerRange = worksheet.Cells[1, 1, 1, colCount])
                {
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Font.Color.SetColor(Color.White);
                    headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 104, 186));
                }

                worksheet.Cells.AutoFitColumns();

                string directory = Path.Combine(Directory.GetCurrentDirectory(), "temp");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string filePath = Path.Combine(directory, "output.xlsx");
                excelPackage.SaveAs(new FileInfo(filePath));
            }
        }
        public static string CheckNull(object value)
        {
            return value != null ? value.ToString() : "";
        }
        public static long FileCountB(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles();
            return files.Length;
        }
        public static Bitmap GenerateQRCode(string data, int size)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);

            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(size);

            return qrCodeImage;
        }
        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
