﻿using System.Data;
using System.Data.SqlClient;

namespace PlantWebApps.Helper
{
    public class SQLFunction
    {
        public static DataTable execQuery(String commandText)
        {
            SqlConnection conn = new SqlConnection(GlobalString.conString());
            DataTable dataTable = new DataTable();
            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (reader.HasRows)
                {
                    dataTable.Load(reader);
                }

                conn.Close();

                return dataTable;
            }
        }

        public static object ExecuteScalar(string query)
        {
            using (SqlConnection connection = new SqlConnection(GlobalString.conString()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return command.ExecuteScalar();
                }
            }
        }
    }
}
