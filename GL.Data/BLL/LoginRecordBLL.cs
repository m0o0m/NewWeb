using GL.Command.DBUtility;
using GL.Common;
using GL.Data.Model;
using GL.Data.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class LoginRecordBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");
        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        public static PagedList<LoginRecord> GetListByPage(int page, ValueView vv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;

            if (vv.Type == seachType.同IP登陆玩家)
            {
                pq.RecordCount = DAL.PagedListDAL<LoginRecord>.GetRecordCount(string.Format(@"select count(1) from(select ip from "+ database1 + @".V_LoginRecord  where LoginTime >= '{0}' and LoginTime < '{1}' group by ip ,date(LoginTime) having count(distinct userid) > 3 ) t", vv.StartDate, vv.ExpirationDate), sqlconnectionString);
                pq.Sql = string.Format(
                    @"select ip as IP ,count(distinct userid) as Acc ,date(LoginTime) as LoginTime ,group_concat(distinct userid separator '_') as UserIDS ,-1 Flag from "+ database1 + @".V_LoginRecord where LoginTime >= '{3}' and LoginTime < '{4}' group by ip ,date(LoginTime) having count(distinct userid) > 3 order by LoginTime ,Acc desc limit {0}, {1}"
                    , pq.StartRowNumber, pq.PageSize, vv.Value, vv.StartDate, vv.ExpirationDate);
            }
            else if (!string.IsNullOrEmpty(vv.Value))
            {
                switch (vv.Type)
                {
                    case seachType.NULL:
                        pq.RecordCount = DAL.PagedListDAL<LoginRecord>.GetRecordCount(string.Format(@"select count(0) from V_LoginRecord where UserID = '{0}' and LoginTime between '{1}' and '{2}'", vv.Value.Trim(), vv.StartDate, vv.ExpirationDate), sqlconnectionString);
                        pq.Sql = string.Format(
                            @"select a.*,b.CreateIP from (select * from V_LoginRecord where UserID = '{2}' and LoginTime between '{3}' and '{4}' order by V_LoginRecord.LoginTime desc limit {0}, {1} ) a join Role b on a.userid = b.id"
                            , pq.StartRowNumber, pq.PageSize, vv.Value.Trim(), vv.StartDate, vv.ExpirationDate);
                        break;
                    case seachType.IP地址:
                        pq.RecordCount = DAL.PagedListDAL<LoginRecord>.GetRecordCount(string.Format(@"select count(0) from V_LoginRecord where IP = '{0}' and LoginTime between '{1}' and '{2}'", vv.Value.Trim(), vv.StartDate, vv.ExpirationDate), sqlconnectionString);
                        pq.Sql = string.Format(
                            @"select a.*,b.CreateIP from (select * from V_LoginRecord where IP = '{2}' and LoginTime between '{3}' and '{4}' order by V_LoginRecord.LoginTime desc limit {0}, {1} ) a join Role b on a.userid = b.id"
                            , pq.StartRowNumber, pq.PageSize, vv.Value.Trim(), vv.StartDate, vv.ExpirationDate);
                        break;
                    case seachType.Mac地址:
                        pq.RecordCount = DAL.PagedListDAL<LoginRecord>.GetRecordCount(string.Format(@"select count(0) from V_LoginRecord where Mac = '{0}' and LoginTime between '{1}' and '{2}'", vv.Value.Trim(), vv.StartDate, vv.ExpirationDate), sqlconnectionString);
                        pq.Sql = string.Format(
                            @"select a.*,b.CreateIP from (select * from V_LoginRecord where Mac = '{2}' and LoginTime between '{3}' and '{4}' order by V_LoginRecord.LoginTime desc limit {0}, {1} ) a join Role b on a.userid = b.id"
                            , pq.StartRowNumber, pq.PageSize, vv.Value.Trim(), vv.StartDate, vv.ExpirationDate);
                        break;
                    case seachType.会员账号:
                        pq.RecordCount = DAL.PagedListDAL<LoginRecord>.GetRecordCount(string.Format(@"select count(0) from V_LoginRecord a join Role b on a.UserID = b.ID and b.Account = '{0}' where LoginTime between '{1}' and '{2}'", vv.Value.Trim(), vv.StartDate, vv.ExpirationDate), sqlconnectionString);
                        pq.Sql = string.Format(
                            @"select a.* ,b.CreateIP from V_LoginRecord a join Role b on a.UserID = b.ID and b.Account = '{2}' where a.LoginTime between '{3}' and '{4}' order by a.LoginTime desc limit {0}, {1}"
                            , pq.StartRowNumber, pq.PageSize, vv.Value.Trim(), vv.StartDate, vv.ExpirationDate);
                        break;
                    case seachType.会员昵称:
                        pq.RecordCount = DAL.PagedListDAL<LoginRecord>.GetRecordCount(string.Format(@"select count(0) from V_LoginRecord where NickName = '{0}' and LoginTime between '{1}' and '{2}'", vv.Value.Trim(), vv.StartDate, vv.ExpirationDate), sqlconnectionString);
                        pq.Sql = string.Format(
                            @"select a.*,b.CreateIP from (select * from V_LoginRecord where NickName = '{2}' and LoginTime between '{3}' and '{4}' order by V_LoginRecord.LoginTime desc limit {0}, {1} ) a join Role b on a.userid = b.id"
                            , pq.StartRowNumber, pq.PageSize, vv.Value.Trim(), vv.StartDate, vv.ExpirationDate);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<LoginRecord>.GetRecordCount(string.Format(@"select count(0) from V_LoginRecord where LoginTime between '{0}' and '{1}'", vv.StartDate, vv.ExpirationDate), sqlconnectionString);
                pq.Sql = string.Format(
                    @"select a.*,b.CreateIP from (select * from V_LoginRecord where LoginTime between '{2}' and '{3}' order by LoginTime desc limit {0}, {1} ) a join Role b on a.userid = b.id"
                    , pq.StartRowNumber, pq.PageSize, vv.StartDate, vv.ExpirationDate);

            }

            //if (string.IsNullOrWhiteSpace(vv.Value))
            //{
            //    pq.RecordCount = DAL.PagedListDAL<LoginRecord>.GetRecordCount(string.Format(@"select count(0) from V_LoginRecord where LoginTime between '{1}' and '{2}'", vv.Value, vv.StartDate, vv.ExpirationDate), sqlconnectionString);
            //    pq.Sql = string.Format(@"select b.* from(select IP from V_LoginRecord group by IP having count(IP) > 1)a join V_LoginRecord b on a.IP = b.IP where LoginTime between '{3}' and '{4}' order by a.IP desc,LoginTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, vv.Value, vv.StartDate, vv.ExpirationDate);
            //}

            //select b.* from(select ip from V_LoginRecord group by IP having count(ip) > 1)a join V_LoginRecord b on a.ip = b.IP order by a.IP desc;

            //pq.Sql = string.Concat(pq.Sql, " order by IP desc");


            PagedList<LoginRecord> obj = new PagedList<LoginRecord>(DAL.PagedListDAL<LoginRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static PagedList<LoginRepeat> GetRepeatByPage(ValueView vv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = vv.Page;
            pq.PageSize = 5;

            pq.RecordCount = DAL.PagedListDAL<LoginRepeat>.GetRecordCount(string.Format(@"select count(1) from(select ip from "+ database1 + @".V_LoginRecord  where LoginTime >= '{0}' and LoginTime < '{1}' group by ip ,date(LoginTime) having count(distinct userid) > 3 ) t", vv.StartDate, vv.ExpirationDate), sqlconnectionString);
            pq.Sql = string.Format(@"select ip as IP ,count(distinct userid) as Account ,date(LoginTime) as LoginTime ,group_concat(distinct userid separator '_') as UserID from "+ database1 + @".V_LoginRecord where LoginTime >= '{3}' and LoginTime < '{4}' group by ip ,date(LoginTime) having count(distinct userid) > 3 order by 3 ,2 desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, vv.Value, vv.StartDate, vv.ExpirationDate);

            PagedList<LoginRepeat> obj = new PagedList<LoginRepeat>(DAL.PagedListDAL<LoginRepeat>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }
        public static LoginRecord GetModel(Role model)
        {
            return DAL.LoginRecordDAL.GetModel(model);
        }
    }
}
