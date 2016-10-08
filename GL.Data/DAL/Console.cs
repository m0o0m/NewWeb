using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Data.View;
using GL.Command.DBUtility;
using MySql.Data.MySqlClient;
using GL.Dapper;
using GL.Data.Model;

namespace GL.Data.DAL
{
    public class Console
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameRecord");
        internal static int AddModel(ModelBaseData model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                if(model.ID == 0)
                {
                    cn.Open();
                    int i = cn.Execute(@"insert into record.S_DataModel(ModelName ,Para ,Model ,Createtime ) select @ModelName ,@Para ,@Model ,now();", model);
                    cn.Close();
                    return i;
                }
                else { 
                    cn.Open();
                    int i = cn.Execute(@"update record.S_DataModel set ModelName = @ModelName ,Para = @Para ,Model = @Model ,Createtime = now() where id = @ID ;", model);
                    cn.Close();
                    return i;
                }
            }
        }



        public static IEnumerable<CommonIDName> GetMasterOper()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                //记录查询次数
               
                IEnumerable<CommonIDName> i = cn.Query<CommonIDName>("select DISTINCT Proj as Name from record.BG_ScaleRecord");
                cn.Close();
                return i;
            }
        }

    }
}