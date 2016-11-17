using GL.Command.DBUtility;
using GL.Dapper;
using GL.Data.JWeb.Model;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.JWeb.DAL
{
    public class FishDAL
    {
        //FishInfoRecord

        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("connectionstringforgamedata");



        public static IEnumerable<FishInfoRecord> GetFishInfoRecord(int userid,int pageIndex)
        {
            int startRow = pageIndex > 1 ? ((pageIndex - 1) * 10) : 0;
            int pagelength = 10;
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from FishInfoRecord
where userid = @userid and (type=1 or type=0)
ORDER BY Createtime DESC 
limit @startRow,@pagelength
");
                IEnumerable<FishInfoRecord> i = cn.Query<FishInfoRecord>(str.ToString(), 
                    new { userid = userid, startRow= startRow, pagelength= pagelength });
                cn.Close();
                return i;
            }
        }

    }
}
