using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using DataTables.DatabaseUtil;

namespace DataTables.DatabaseUtil.Sqlserver
{
    public class Query : DataTables.Query
    {
        public Query(Database db, string type)
            : base(db, type)
        {
        }


        /// <summary>
        /// Create LIMIT / OFFSET for SQL Server 2012+. Note that this will only work
        /// with SQL Server 2012 or newer due to the use of the OFFSET and FETCH NEXT
        /// keywords
        /// </summary>
        /// <returns>Limit / offset string</returns>
        override protected string _BuildLimit()
        {
            string limit = "";

            if (_offset != -1)
            {
                limit = " OFFSET " + _offset + " ROWS";
            }

            if (_limit != -1)
            {
                if (_offset == -1)
                {
                    limit += " OFFSET 0 ROWS";
                }

                limit += " FETCH NEXT " + _limit + " ROWS ONLY";
            }

            return limit;
        }


        override protected void _Prepare(string sql)
        {
            DbParameter param;
            var provider = DbProviderFactories.GetFactory(_db.Adapter());
            var cmd = provider.CreateCommand();

            // On insert we need to get the table's primary key value in
            // an 'output' statement, so it can be used
            if (_type == "insert")
            {
                var pkeyCmd = provider.CreateCommand();

                // We need to find out what the primary key column name and type is
                pkeyCmd.CommandText = @"
                    SELECT
	                    INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME as column_name,
	                    INFORMATION_SCHEMA.COLUMNS.DATA_TYPE as data_type,
                        INFORMATION_SCHEMA.COLUMNS.CHARACTER_MAXIMUM_LENGTH as data_length
                    FROM
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                    JOIN
                        INFORMATION_SCHEMA.COLUMNS ON
	                        INFORMATION_SCHEMA.COLUMNS.table_name = INFORMATION_SCHEMA.KEY_COLUMN_USAGE.table_name
	                        AND INFORMATION_SCHEMA.COLUMNS.column_name = INFORMATION_SCHEMA.KEY_COLUMN_USAGE.column_name
                    WHERE
                        OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 
	                    AND INFORMATION_SCHEMA.KEY_COLUMN_USAGE.table_name = @table
                ";
                pkeyCmd.Connection = _db.Conn();
                pkeyCmd.Transaction = _db.DbTransaction;

                param = pkeyCmd.CreateParameter();
                param.ParameterName = "@table";
                param.Value = _table[0];
                pkeyCmd.Parameters.Add(param);

                using (var dr = pkeyCmd.ExecuteReader())
                {
                    // If the table doesn't have a primary key field, we can't get
                    // the inserted pkey!
                    if (dr.HasRows && dr.Read())
                    {
                        // Insert into a temporary table so we can select from it.
                        // This is required for tables which have a trigger on insert
                        // See thread 29556. We can't just use 'SELECT SCOPE_IDENTITY()'
                        // since the primary key might not be an identify column
                        sql = dr["data_length"] != DBNull.Value ?
                            "DECLARE @T TABLE ( insert_id " + dr["data_type"] + " (" + dr["data_length"] + ") ); " + sql :
                            "DECLARE @T TABLE ( insert_id " + dr["data_type"] + " ); " + sql;
                        sql = sql.Replace(" VALUES (",
                            " OUTPUT INSERTED." + dr["column_name"] + " as insert_id INTO @T VALUES (");
                        sql += "; SELECT insert_id FROM @T";
                    }
                }
            }

            cmd.CommandText = sql;
            cmd.Connection = _db.Conn();
            cmd.Transaction = _db.DbTransaction;

            // Bind values
            for (int i = 0, ien = _bindings.Count; i < ien; i++)
            {
                var binding = _bindings[i];

                param = cmd.CreateParameter();
                param.ParameterName = binding.Name;
                param.Value = binding.Value ?? DBNull.Value;

                if (binding.Type != null)
                {
                    param.DbType = binding.Type;
                }

                cmd.Parameters.Add(param);
            }

            _stmt = cmd;
        }


        override protected DataTables.Result _Exec()
        {
            var dt = new System.Data.DataTable();

            using (var dr = _stmt.ExecuteReader())
            {
                dt.Load(dr);
            }

            return new Sqlserver.Result(_db, dt, this);
        }
    }
}
