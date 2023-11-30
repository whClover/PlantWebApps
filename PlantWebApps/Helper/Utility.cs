using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using System.Drawing;

namespace PlantWebApps.Helper
{
    public class Utility
    {
        public static string Evar(object val, int valtype, int vallen = 255)
        {
            string eval;

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

        public static string eusername()
        {
            string username = Environment.UserName;

            return username;
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

        public static string GetStringOrNull(object value)
        {
            return value != DBNull.Value ? (string)value : null;
        }

        public static DataTable getDataTable(String qTxt)
        {
            var dataTable = SQLFunction.execQuery(qTxt);

            return dataTable;
        }

        public static ArrayList genDataTable(DataTable dt, Dictionary<String, String> sCol, String IDKey = "")
        {
            DataTable dt2 = new DataTable();
            ArrayList arrayList = new ArrayList();

            foreach (DataRow rows in dt.Rows)
            {
                Dictionary<string, string> dt3 = new Dictionary<string, string>();
                foreach (var col in sCol)
                {
                    if (col.Key == "#")
                    {
                        dt3.Add("#", col.Value.Replace("<id>", rows[IDKey].ToString()));
                    }
                    else
                    {
                        dt3.Add(col.Value, rows[col.Key].ToString());
                    }
                }
                arrayList.Add(dt3);
            }
            return arrayList;
        }
    }
}
