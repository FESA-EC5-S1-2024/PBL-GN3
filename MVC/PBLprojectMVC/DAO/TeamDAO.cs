using PBLprojectMVC.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBLprojectMVC.DAO
{
    public class TeamDAO : StandardDAO<TeamViewModel>
    {
        protected override SqlParameter[] CreateParameters(TeamViewModel team)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("Id", team.Id));
            parameters.Add(new SqlParameter("Name", team.Name));
            parameters.Add(new SqlParameter("Description", team.Description));

            return parameters.ToArray();
        }

        protected override TeamViewModel CreateModel(DataRow record)
        {
            TeamViewModel team = new TeamViewModel();
            team.Id = Convert.ToInt32(record["Id"]);
            team.Name = record["Name"].ToString();
            team.Description = record["Description"].ToString();
            return team;
        }

        protected override void SetTable()
        {
            Table = "Team";
        }
    }
}
