using PBLprojectMVC.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBLprojectMVC.DAO
{
    public class DeviceDAO : StandardDAO<DeviceViewModel>
    {
        protected override SqlParameter[] CreateParameters(DeviceViewModel device, bool isInsert = false)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!isInsert)
            {
                parameters.Add(new SqlParameter("Id", device.Id));
            }

            parameters.Add(new SqlParameter("Nome", device.Name));
            parameters.Add(new SqlParameter("Tipo", device.Type));

            return parameters.ToArray();
        }

        protected override DeviceViewModel CreateModel(DataRow record)
        {
            DeviceViewModel device = new DeviceViewModel();
            device.Id = Convert.ToInt32(record["Id"]);
            device.Name = record["Name"].ToString();
            device.Type = record["Type"].ToString();
            return device;
        }

        protected override void SetTable()
        {
            Table = "Device";
            NameSpGetAll = "spGetAll_Device";
        }

    }
}
