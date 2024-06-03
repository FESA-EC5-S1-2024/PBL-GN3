using System;
using System.IO;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PBLprojectMVC.Utils;

namespace PBLprojectMVC.DAO
{
    public class ConnectionDAO
    {
        public static SqlConnection GetConnection()
        {
            string filePath = "config.json"; 
            string strConn = UtilsParams.Conn();

            SqlConnection conn;
            try
            {
                conn = new SqlConnection(strConn);
                conn.Open();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Could not open a connection to the database.", ex);
            }

            return conn;
        }
    }
}
