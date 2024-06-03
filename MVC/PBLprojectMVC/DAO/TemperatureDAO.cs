using PBLprojectMVC.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBLprojectMVC.DAO
{
    public class TemperatureDAO : StandardDAO<TemperatureViewModel>
    {
        protected override SqlParameter[] CreateParameters(TemperatureViewModel temperature)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("Id", temperature.Id));
            parameters.Add(new SqlParameter("Time", temperature.Time));
            parameters.Add(new SqlParameter("Value", temperature.Value));
            parameters.Add(new SqlParameter("DeviceId", temperature.DeviceId));

            return parameters.ToArray();
        }

        protected override TemperatureViewModel CreateModel(DataRow record)
        {
            TemperatureViewModel temperatura = new TemperatureViewModel();
            temperatura.Id = Convert.ToInt32(record["Id"]);
            temperatura.Time = Convert.ToDateTime(record["Time"]);
            temperatura.Value = Convert.ToSingle(record["Value"]);
            temperatura.DeviceId = Convert.ToInt32(record["DeviceId"]);
            return temperatura;
        }

        protected override void SetTable()
        {
            Table = "Temperature";
        }

    }
}
