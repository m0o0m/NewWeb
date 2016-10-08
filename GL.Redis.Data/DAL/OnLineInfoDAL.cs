using GL.Command.DBUtility;
using GL.Dapper;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.DAL
{
    public class OnLineInfoDAL
    {

        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        internal static string GetNewJson()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<OnLineInfo> i = cn.Query<OnLineInfo>(@"select  CreateTime,OnLineInfoJson from 515game.OnLineInfo order by createTime DESC limit 0,1");
                cn.Close();
                OnLineInfo o = i.FirstOrDefault();
                if (o == null)
                {
                    return "";
                }
                else {
                    return o.OnLineInfoJson;
                }
            }
        }

        internal static int InsertNewJson(OnLineInfo info )
        {
            string newjosn = GetNewJson();
           
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = 0;
                if (string.IsNullOrEmpty(newjosn))
                {
                    i = cn.Execute(@"insert into 515game.OnLineInfo values(@CreateTime,@OnLineInfoJson)", new OnLineInfo { CreateTime = info.CreateTime, OnLineInfoJson = info.OnLineInfoJson });
                }
                else {

                    i = cn.Execute(@"

insert into 515game.OnLineInfo values(@CreateTime,@OnLineInfoJson);
delete from 515game.OnLineInfo where CreateTime!=@CreateTime;                                  
", new OnLineInfo { CreateTime = info.CreateTime, OnLineInfoJson = info.OnLineInfoJson });
                }
             
                cn.Close();
                return i;
            }
        }




    }
}
