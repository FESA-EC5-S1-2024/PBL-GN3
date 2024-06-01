using PBLprojectMVC.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBLprojectMVC.DAO
{
    public class UserDAO : StandardDAO<UserViewModel>
    {
        protected override SqlParameter[] CreateParameters(UserViewModel user)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("Id", user.Id));
            parameters.Add(new SqlParameter("Name", user.Name));
            parameters.Add(new SqlParameter("Email", user.Email));
            parameters.Add(new SqlParameter("Password", user.Password));
            parameters.Add(new SqlParameter("IsAdmin", user.IsAdmin));

            return parameters.ToArray();
        }

        protected override UserViewModel CreateModel(DataRow record)
        {
            UserViewModel user = new UserViewModel();
            user.Id = Convert.ToInt32(record["Id"]);
            user.Name = record["Name"].ToString();
            user.Email = record["Email"].ToString();
            user.Password = record["Password"].ToString();
            user.IsAdmin = Convert.ToBoolean(record["IsAdmin"]);
            return user;
        }

        protected override void SetTable()
        {
            Table = "User";
        }

        public UserViewModel SelectUser(string email)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("Email", email)
            };
            var Table = HelperDAO.ExecuteProcSelect("spSelect_User", p);
            if (Table.Rows.Count == 0)
                return null;
            else
                return CreateModel(Table.Rows[0]);
        }
    }
}
