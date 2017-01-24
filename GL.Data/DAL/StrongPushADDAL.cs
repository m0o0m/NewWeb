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

        public static int UpdateStrongPushAD(LoginRegisterDataView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"Update "+ database3 + @".strongpushad set Url = @Url,Type=@Type where Plat = @Plat and Agent = @Agent ", new
                {
                    Url = model.Url,
                    Plat = model.Platform,
                    Agent = model.Channels,
                    Type = model.Type
                });

                cn.Close();

                return i;
            }
        }
        public static int AddStrongPushADRecord(StrongPushADRecord model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"insert into " + database3 + @".strongpushadrecord(Plat,Agent,Url,NewUrl,Type,NewType,CreateTime,Username) values (@Plat,@Agent,@Url,@NewUrl,@Type,@NewType,'" + DateTime.Now + @"',@Username)", new
                {
                    Plat = model.Plat,
                    Agent = model.Agent,
                    NewUrl = model.NewUrl,
                    Url = model.Url,
                      Type = model.Type,
                      NewType = model.NewType,
                    Username = model.Username
                });

                cn.Close();

                return i;
            }
        }
        public static int AddStrongPushAD(LoginRegisterDataView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"insert into "+ database3 + @".strongpushad(Plat,Agent,Url,Type,CreateTime) values (@Plat,@Agent,@Url,@Type,'" + DateTime.Now + @"')", new {
                    Plat = model.Platform,
                    Agent = model.Channels,
                    Url = model.Url,
                    Type = model.Type
                });

                cn.Close();

                return i;
            }
        }
        public static StrongPushAD GetStrongPushAD(LoginRegisterDataView model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                StringBuilder str = new StringBuilder();
           
                str.AppendFormat(@"
select * from " + database3 + @".strongpushad 
where Plat = @Plat and Agent = @Agent
ORDER BY CreateTime DESC;
                ");

                IEnumerable<StrongPushAD> i = cn.Query<StrongPushAD>(str.ToString(), new {
                    Plat = model.Platform,
                    Agent = model.Channels
                });
                cn.Close();
                return i.FirstOrDefault();
            }
        }


    }
}
