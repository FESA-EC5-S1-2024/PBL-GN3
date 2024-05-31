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

  		public LoginDAO()
        {
            Table = "Users";
        }

        protected override SqlParameter[] CreateParameters(UserViewModel model)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@Id", model.Id);
            parameters[1] = new SqlParameter("@Nome", model.Nome);
            parameters[2] = new SqlParameter("@Email", model.Email);
            parameters[3] = new SqlParameter("@Senha", model.Senha);
            parameters[4] = new SqlParameter("@IsAdmin", model.IsAdmin);
            return parameters;
        }

        protected override UserViewModel MountModel(DataRow row)
        {
            UserViewModel user = new UserViewModel();

            user.Id = (int)row["Id"];
            user.Nome = row["Nome"].ToString();
            user.Email = row["Email"].ToString();
            user.Senha = "";
            user.IsAdmin = (bool)row["IsAdmin"];
            
            return user;
        }
        
        public bool LoginExists(string email, string password){

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Email", email);
            parameters[1] = new SqlParameter("@Senha", password);

            string sql = "SELECT * FROM " + Table + " WHERE Email = @Email AND Senha = @Senha";

            return HelperDAO.ExecuteSelect(sql, parameters).Rows.Count >= 1;
        }

        public int LoginExists(string email, string password, bool return_ID){

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Email", email);
            parameters[1] = new SqlParameter("@Senha", password);

            string sql = "SELECT * FROM " + Table + " WHERE Email = @Email AND Senha = @Senha";

            return (int)HelperDAO.ExecuteSelect(sql, parameters).Rows[0]["Id"];
        }

        public bool IsAdmin(string email, string password){

            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Email", email);
            parameters[1] = new SqlParameter("@Senha", password);

            string sql = "SELECT * FROM " + Table + "  WHERE IsAdmin = 1 AND Email = @Email AND Senha = @Senha";

            return HelperDAO.ExecuteSelect(sql, parameters).Rows.Count >= 1;

        }
    }
}
