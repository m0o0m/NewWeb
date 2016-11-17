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

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static IEnumerable<OperLog> GetModelByIDList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<OperLog> i = cn.Query<OperLog>(@" select * from "+ database1 + @".Q_OperLog where OperType <> '操作'");
                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<OperLog> GetModelByIDListForAll()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<OperLog> i = cn.Query<OperLog>(@" select * from " + database1 + @".Q_OperLog");
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
insert into "+ database1 + @".Q_OperLog(IP,CreateTime,UserAccount,UserName,LeftMenu,OperType,OperDetail) values(
 @IP, @CreateTime,@UserAccount,@UserName,@LeftMenu,@OperType,@OperDetail
);
", oper);
                cn.Close();
                return i;
            }
        }
        //UpdateASPNetUserReset
        internal static int UpdateASPNetUserReset(ASPNetUserLimit model)
        {
          
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
update "+ database2+ @".aspnetuserslimit set ErrorNum=0, LimitTime = NOW(),AllErrorNum=0 
where Username=@Username;", model);
                cn.Close();
                return i;
            }
        }

        internal static int UpdateASPNetUserLimit(ASPNetUserLimit model)
        {
            ASPNetUserLimit limit = GetASPNetUserLimit(model);
            if (limit == null) {
                return  AddASPNetUserLimit(model);
            }


            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
update "+ database2 + @".aspnetuserslimit set ErrorNum=ErrorNum+1, LimitTime =DATE_ADD( NOW(),INTERVAL 5 MINUTE),AllErrorNum=AllErrorNum+1 
where Username=@Username;", model);
                cn.Close();
                return i;
            }
        }

        internal static ASPNetUserLimit GetASPNetUserLimit(ASPNetUserLimit model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                var i = cn.Query<ASPNetUserLimit>(@"
select * from "+ database2 + @".aspnetuserslimit 
where Username='"+model.Username+@"';");
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static int AddASPNetUserLimit(ASPNetUserLimit model)
        {



            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                var i = cn.Execute(@"
insert into "+ database2 + @".aspnetuserslimit(Username,ErrorNum,LimitTime,AllErrorNum)
VALUES('" + model.Username+@"',1,DATE_ADD(NOW(),INTERVAL 5 MINUTE),1);
");
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
insert into "+ database1+ @".Q_FreezeLog(CreateTime,UserID,IP,IMei,TimeSpan,OperUserName,Reason,Type) values(
  @CreateTime,@UserID,@IP,@IMei,@TimeSpan,@OperUserName,@Reason,@Type
);
", model);
                cn.Close();
                return i;
            }
        }


        //OperConfig

        internal static OperConfig GetOperConfigExist(string url,string param) {
//            select* from gamedata.Q_OperConfig
//where Action = '/Base/DayReport' and Method = 'Get' and Active = 1 and
// ParamName like 'Channels:%StartDate:%%ExpirationDate:%'

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"
select * from "+ database1 + @".Q_OperConfig
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
