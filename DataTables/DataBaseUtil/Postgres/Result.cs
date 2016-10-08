using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;

namespace DataTables.DatabaseUtil.Postgres
{
    public class Result : DataTables.Result
    {
        public Result(Database db, System.Data.DataTable dt, Query q)
            : base(db, dt, q)
        {
        }

        override public string InsertId()
        {
            if (_dt.Rows.Count > 0 && _dt.Columns.Contains("dt_pkey"))
            {
                return _dt.Rows[0]["dt_pkey"].ToString();
            }

            return null;
        }
    }
}
