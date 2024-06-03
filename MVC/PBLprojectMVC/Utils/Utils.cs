using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PBLprojectMVC.Utils
{
    public static class UtilsParams
    {
        
        public static string Host(){
            try
            {
                string jsonString = File.ReadAllText("config.json");
                JObject config = JObject.Parse(jsonString);
                return config["Host"].ToString();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not read the connection string from the file.", ex);
            }
        }

        public static string Conn(){
            try
            {
                string jsonString = File.ReadAllText("config.json");
                JObject config = JObject.Parse(jsonString);
                return config["ConnectionStrings"]["DefaultConnection"].ToString();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not read the connection string from the file.", ex);
            }
        }
    }
}