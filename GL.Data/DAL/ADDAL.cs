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
    public  class ADDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        internal static int Add(ADInfo model)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@"515game.sp_update_channel");
                cn.Query<ADInfo>(str.ToString(), param: new { V_IP = model.IP, V_Channel =model.ChannlID, V_URL=model.Url }, commandType: CommandType.StoredProcedure);

                return 1;
            }


           
        }

        internal static int AddClickInfo(ADInfo model)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append(@"515game.sp_click_channel");
                cn.Query<ADInfo>(str.ToString(), param: new { V_IP = model.IP, V_Channel = model.ChannlID, V_URL = model.Url }, commandType: CommandType.StoredProcedure);

                return 1;
            }



        }


        public static int Add(DMModel model) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
insert into AD_DM(appkey,ifa,ifamd5,mac,macmd5,source,iddate,CreateTime,Flag) 
VALUES(@Appkey,@Ifa,@Ifamd5,@Mac,@MacMD5,@Source,@Iddate,@CreateTime,@Flag)", model);
                cn.Close();
                return i;
            }
        }
        public static int Update(DMModel model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
update AD_DM set Flag=1 where  id = @Id", model);
                cn.Close();
                return i;
            }
        }

        public static int Delete(DMModel model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"delete from AD_DM where id = @Id", model);
                cn.Close();
                return i;
            }
        }


        public static DMModel GetDMModel(string appkey)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<DMModel> i = cn.Query<DMModel>(@"select * from AD_DM where appkey = '"+ appkey+"'");
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        public static DMModel GetDMReapeatModel(DMModel model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                string sql = "select * from AD_DM where appkey=@Appkey and ( macMD5 =@MacMD5 or  mac =@Mac or ifamd5 =@Ifamd5 or  ifa =@Ifa )";
            


                IEnumerable<DMModel> i = cn.Query<DMModel>(sql, model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        public static DMModel GetDMModel(DMModel model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                string sql = "select * from AD_DM where 1=1 ";
                if (string.IsNullOrEmpty(model.Ifa)) { //如果ifa为空

                    if (string.IsNullOrEmpty(model.Ifamd5))//如果Ifamd5为空
                    {
                        if (string.IsNullOrEmpty(model.Mac)||model.Mac== "02:00:00:00:00:00")
                        {
                            if (string.IsNullOrEmpty(model.MacMD5))
                            {
                                return null;
                            }
                            else {
                                sql += " and macMD5 =@MacMD5";
                            }
                        }
                        else {
                            sql += " and mac =@Mac";
                        }
                    }
                    else
                    {//如果Ifamd5不为空
                        sql += " and ifamd5 =@Ifamd5";
                    }

                }else
                {//如果ifa不为空
                    sql += " and ifa =@Ifa";
                }



                IEnumerable<DMModel> i = cn.Query<DMModel>(sql,model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

    }
}
