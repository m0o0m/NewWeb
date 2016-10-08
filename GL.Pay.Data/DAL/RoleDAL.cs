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
    public class RoleDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        internal static Role GetModelByID(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"select ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP, NoSpeak, IsFreeze from Role where ID = @ID", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        internal static Role GetModelByOpenID(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"select ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP, NoSpeak, IsFreeze from Role where OpenID = @OpenID", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }
    }
}
