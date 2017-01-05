using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;
using System.Data;


namespace GL.Data.DAL
{
    public class StrongPushADDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");


        internal static IEnumerable<StrongPushADRecord> GetStrongPushADRecord(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from "+database3+@".strongpushadrecord ORDER BY CreateTime DESC;
                ");

                IEnumerable<StrongPushADRecord> i = cn.Query<StrongPushADRecord>(str.ToString());
                cn.Close();
                return i;
            }
        }

    }
}
