using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace PlantWebApps.Helper
{
	public class Checking
	{
        public static (bool IsGranted, bool ModEdit, bool ModAdd, bool ModDelete) CheckPrivilege(string frm_name, bool msg = false)
        {
            bool IsGranted = false;
            bool ModEdit = false;
            bool ModAdd = false;
            bool ModDelete = false;

            string query = "Select GrOpen, GrEdit, GrAdd, GrDelete, GrAdmin from tbl_UserGranted where grUserID='" + Utility.GetUserId() + "' and GrForm IN (" + Utility.Evar(frm_name, 10) + ")";
            Console.WriteLine(query);
            DataTable priv = SQLFunction.execQuery(query);

            if (priv.Rows.Count > 0)
            {
                IsGranted = Convert.ToInt32(priv.Rows[0]["GrOpen"]) == 1;
                ModEdit = Convert.ToInt32(priv.Rows[0]["GrEdit"]) == 1;
                ModAdd = Convert.ToInt32(priv.Rows[0]["GrAdd"]) == 1;
                ModDelete = Convert.ToInt32(priv.Rows[0]["GrDelete"]) == 1;
            }

            return (IsGranted, ModEdit, ModAdd, ModDelete);
        }
    }
}
