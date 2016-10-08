using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace DataTables.DatabaseUtil.Sqlite
{
    class Result : DataTables.Result
    {
        public Result(Database db, System.Data.DataTable dt, Query q)
            : base(db, dt, q)
        {
        }

        override public string InsertId()
        {
            if (_dt.Rows.Count > 0)
            {
                var provider = DbProviderFactories.GetFactory(_db.Adapter());
                var cmd = provider.CreateCommand();

                cmd.CommandText = "select last_insert_rowid()";
                cmd.Connection = _db.Conn();

                return (string)cmd.ExecuteScalar();
            }

            return null;
        }
    }
}
