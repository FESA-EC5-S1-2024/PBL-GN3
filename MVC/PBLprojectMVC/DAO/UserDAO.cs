using PBLprojectMVC.Models;
using System.Data.SqlClient;
using System.Data;
using PBLprojectMVC.Controllers;

namespace PBLprojectMVC.DAO
{
    public class UserDAO : StandardDAO<UserViewModel>
    {
        protected override SqlParameter[] CreateParameters(UserViewModel user)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@Id", user.Id));
            parameters.Add(new SqlParameter("@Name", user.Name));
            parameters.Add(new SqlParameter("@Email", user.Email));
            parameters.Add(new SqlParameter("@Password", user.Password));
            parameters.Add(new SqlParameter("@IsAdmin", user.IsAdmin));

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
            Table = "Users";
        }

        public UserViewModel UserExists(string email)
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

        public List<UserViewModel> GetSearchUsers(string query){

            List<UserViewModel> users = new List<UserViewModel>();

            string sql = "SELECT * FROM " + Table + " WHERE Name LIKE @Query";

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@Query", SqlDbType.NVarChar) { Value = '%' + query + '%' };
            
            try{
                foreach(DataRow row in HelperDAO.ExecuteSelect(sql, parameters).Rows){
                    users.Add(CreateModel(row));
                }
            }
            catch{
                users.Clear();
            }

            return users;

        }

        public override void Update(UserViewModel model)
        {
            model.Password = HashHelper.ComputeSha256Hash(model.Password);
            base.Update(model);
        }

        public override void Insert(UserViewModel model)
        {
            model.Password = HashHelper.ComputeSha256Hash(model.Password);
            base.Insert(model);
        }
    }
}
