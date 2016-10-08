using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;


namespace GL.Data.DAL
{
    public class OperLogDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");


        internal static IEnumerable<OperLog> GetModelByIDList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<OperLog> i = cn.Query<OperLog>(@" select * from 515game.Q_OperLog");
                cn.Close();
                return i;
            }
        }



        internal static int InsertOperLog(OperLog oper)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
insert into 515game.Q_OperLog(IP,CreateTime,UserAccount,UserName,LeftMenu,OperType,OperDetail) values(
 @IP, @CreateTime,@UserAccount,@UserName,@LeftMenu,@OperType,@OperDetail
);
", oper);
                cn.Close();
                return i;
            }
        }


        public static int InsertFreezeLog(FreezeLog model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
insert into 515game.Q_FreezeLog(CreateTime,UserID,IP,IMei,TimeSpan,OperUserName,Reason,Type) values(
  @CreateTime,@UserID,@IP,@IMei,@TimeSpan,@OperUserName,@Reason,@Type
);
", model);
                cn.Close();
                return i;
            }
        }


        //OperConfig

        internal static OperConfig GetOperConfigExist(string url,string param) {
//            select* from 515game.Q_OperConfig
//where Action = '/Base/DayReport' and Method = 'Get' and Active = 1 and
// ParamName like 'Channels:%StartDate:%%ExpirationDate:%'

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"
select * from 515game.Q_OperConfig
where Action ='" + url + @"' and Method = 'Get' and Active = 1
{0}
", string.IsNullOrEmpty(param)? " and ParamName='' " : " and  ParamName like '"+param+@"' ");



                var i = cn.Query<OperConfig>(str.ToString());
                cn.Close();
                return i.FirstOrDefault();
            }
        }



    }
}
