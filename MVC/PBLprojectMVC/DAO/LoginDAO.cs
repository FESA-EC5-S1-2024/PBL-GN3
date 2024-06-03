using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBLprojectMVC.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace PBLprojectMVC.DAO
{
    public class LoginDAO : StandardDAO<UserViewModel>
    {
        protected override void SetTable()
        {
            Table = "Users";
        }

        protected override SqlParameter[] CreateParameters(UserViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@Id", model.Id);
            parameters[1] = new SqlParameter("@Name", model.Name);
            parameters[2] = new SqlParameter("@Email", model.Email);
            parameters[3] = new SqlParameter("@Password", model.Password);
            parameters[4] = new SqlParameter("@IsAdmin", model.IsAdmin);
            return parameters;
        }

        protected override UserViewModel CreateModel(DataRow row)
        {
            UserViewModel user = new UserViewModel();

            user.Id = (int)row["Id"];
            user.Name = row["Name"].ToString();
            user.Email = row["Email"].ToString();
            user.Password = "";
            user.IsAdmin = (bool)row["IsAdmin"];
            
            return user;
        }

        public bool LoginExists(string email){

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Email", email);

            string sql = "SELECT * FROM " + Table + " WHERE Email = @Email";

            return HelperDAO.ExecuteSelect(sql, parameters).Rows.Count >= 1;
        }
        
        public bool LoginExists(string email, string password){

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Email", email);
            parameters[1] = new SqlParameter("@Password", password);

            string sql = "SELECT * FROM " + Table + " WHERE Email = @Email AND Password = @Password";

            return HelperDAO.ExecuteSelect(sql, parameters).Rows.Count >= 1;
        }

        public int LoginExists(string email, string password, bool return_ID){

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Email", email);
            parameters[1] = new SqlParameter("@Password", password);

            string sql = "SELECT * FROM " + Table + " WHERE Email = @Email AND Password = @Password";

            return (int)HelperDAO.ExecuteSelect(sql, parameters).Rows[0]["Id"];
        }

        public bool IsAdmin(string email, string password){

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Email", email);
            parameters[1] = new SqlParameter("@Password", password);

            string sql = "SELECT * FROM " + Table + "  WHERE IsAdmin = 1 AND Email = @Email AND Password = @Password";

            return HelperDAO.ExecuteSelect(sql, parameters).Rows.Count >= 1;

        }
    }
}
