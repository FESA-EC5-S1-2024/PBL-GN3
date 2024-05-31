using System.Data.SqlClient;

namespace PBLprojectMVC.DAO
{
    public class ConnectionDB
    {
        public static SqlConnection GetConnection()
        {
            string strCon = "Data Source=LOCALHOST;Initial Catalog=PBL;user id=sa; password=123456";
            SqlConnection connection = new SqlConnection(strCon);
            connection.Open();
            return connection;
        }
    }
}
