using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PBLprojectMVC.DAO
{
    public class HelperDAO
    {
        public static void ExecuteProc(string NameProc, SqlParameter[] parameters)
        {
            using (SqlConnection conn = ConnectionDAO.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(NameProc, conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
            }
        }
      
        public static DataTable ExecuteProcSelect(string NameProc, SqlParameter[] parameters)
        {
            using (SqlConnection conn = ConnectionDAO.GetConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(NameProc, conn))
                {
                if (parameters != null)
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable table = new DataTable();
                adapter.Fill(table);

                return table;
                }
            }
        }
    
        public static DataTable ExecuteSelect(string sql, SqlParameter[] parameters)
        {
            using(SqlConnection conn = ConnectionDAO.GetConnection())
            {
                using(SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    if (parameters != null)
                        adapter.SelectCommand.Parameters.AddRange(parameters);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }
      
        public static object NullAsDbNull(object value)
        {
            if (value == null)
                return DBNull.Value;
            else
                return value;
        }
    }
}

