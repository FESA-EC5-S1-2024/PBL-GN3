using PBLprojectMVC.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBLprojectMVC.DAO
{
    public class DeviceDAO : StandardDAO<DeviceViewModel>
    {
        protected override SqlParameter[] CreateParameters(DeviceViewModel device)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("Id", device.Id));
            parameters.Add(new SqlParameter("Name", device.Name));
            parameters.Add(new SqlParameter("Type", device.Type));
            parameters.Add(new SqlParameter("Image", HelperDAO.NullAsDbNull(device.ImageByte)));
            parameters.Add(new SqlParameter("TeamId", device.TeamId));

            return parameters.ToArray();
        }

        protected override DeviceViewModel CreateModel(DataRow record)
        {
            DeviceViewModel device = new DeviceViewModel();
            device.Id = Convert.ToInt32(record["Id"]);
            device.Name = record["Name"].ToString();
            device.Type = record["Type"].ToString();
            device.TeamId = Convert.ToInt32(record["TeamId"]);
            if (record.Table.Columns.Contains("TeamName"))
                device.TeamName = record["TeamName"].ToString();
            if (record["Image"] != DBNull.Value)
                device.ImageByte = record["Image"] as byte[];
            return device;
        }

        protected override void SetTable()
        {
            Table = "Device";
        }

    }
}
