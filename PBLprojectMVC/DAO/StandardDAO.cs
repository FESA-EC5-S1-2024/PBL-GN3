using PBLprojectMVC.Models;
using System.Data;
using System.Data.SqlClient;

namespace PBLprojectMVC.DAO
{
    public abstract class StandardDAO<T> where T : StandardViewModel
    {
        public StandardDAO()
        {
            SetTable();
        }

        protected string Table { get; set; }
        protected string NameSpGetAll { get; set; } = "spGetAll";
        protected string NameSpDelete { get; set; } = "spDelete";
        protected abstract SqlParameter[] CreateParameters(T model, bool isInsert = false);
        protected abstract T CreateModel(DataRow registro);
        protected abstract void SetTable();

        public virtual void Insert(T model)
        {
            HelperDAO.ExecuteProc("spInsert_" + Table, CreateParameters(model, true));
        }

        public virtual void Update(T model)
        {
            HelperDAO.ExecuteProc("spUpdate_" + Table, CreateParameters(model, false));
        }

        public virtual void Delete(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("Table", Table)
            };
            HelperDAO.ExecuteProc(NameSpDelete, p);
        }

        public virtual T Select(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("Table", Table)
            };
            var resultTable = HelperDAO.ExecuteProcSelect("spSelect", p);
            if (resultTable.Rows.Count == 0)
                return null;
            else
                return CreateModel(resultTable.Rows[0]);
        }

        public virtual List<T> GetAll()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("Table", Table),
            };
            var resultTable = HelperDAO.ExecuteProcSelect(NameSpGetAll, p);
            List<T> list = new List<T>();
            foreach (DataRow record in resultTable.Rows)
                list.Add(CreateModel(record));

            return list;
        }
    }
}
