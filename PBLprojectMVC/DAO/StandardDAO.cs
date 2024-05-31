using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBLprojectMVC.Models;
using System.Data.SqlClient;
using System.Data;

namespace PBLprojectMVC.DAO
{
    public abstract class StandardDAO<T> where T : StandardViewModel
    {

        protected abstract SqlParameter[] CreateParameters(T model);
        protected abstract T MountModel(DataRow row);

        protected string Table { get; set; } = "";

        public virtual void Insert(T model)
        {
            HelperDAO.ExecuteProc("spInsert_" + Table, CreateParameters(model));
        }

        public virtual void Update(T model)
        {
            HelperDAO.ExecuteProc("spUpdate_" + Table, CreateParameters(model));
        }

        public virtual void Delete(int id)
        {

            var p = new SqlParameter[1]{
                new SqlParameter("@Id", id),
            };
            
            HelperDAO.ExecuteProc("spDelete_" + Table, p);
        }

        public virtual T? Get(int id)
        {
            var p = new SqlParameter[]{
                new SqlParameter("@Id", id),
            };

            var table = HelperDAO.ExecuteProcSelect("spGet_" + Table, p);

            if (table.Rows.Count != 0)
                return MountModel(table.Rows[0]);
            else
                return null;
        }

        public virtual List<T> GetAll()
        {

            var table = HelperDAO.ExecuteProcSelect("spGetAll_" + Table, null);
            List<T> list = new List<T>();
            
            foreach (DataRow row in table.Rows)
                list.Add(MountModel(row));

            return list;

        }
        
    }
}