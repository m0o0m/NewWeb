using GL.Command.DBUtility;
using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;
#if MWeb
using GL.Data.View;
#endif

namespace GL.Data.BLL
{
    public class RoleBLL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");


#if MWeb
        public static PagedList<Role> GetListByPage(MemberSeachView msv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = msv.PageIndex;
            pq.PageSize = 10;

            switch (msv.SeachType)
            {
                case seachType.查询游戏币区间:

                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where (Gold + SafeBox) between {0} and {1}", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where (Gold + SafeBox) between {2} and {3} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询游戏币等于:
                     pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where (Gold + SafeBox) = {0}", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                     pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where (Gold + SafeBox) = {2} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                   break;
                case seachType.查询游戏币大于:
                     pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where (Gold + SafeBox) > {0}", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                     pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where (Gold + SafeBox) > {2} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                   break;
                case seachType.查询钻石区间:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where Diamond between {0} and {1}", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where Diamond between {2} and {3}  order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询钻石等于:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where Diamond = {0}", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where Diamond = {2} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询钻石大于:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where Diamond > {0}", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where Diamond > {2} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询VIP等级:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where MaxNoble = {0}", msv.Lv), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where MaxNoble = {2} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Lv);
                    break;
                case seachType.IP地址:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where CreateIP = '{0}'", msv.Value), sqlconnectionString);
                    pq.Sql = string.Format(@"
update "+database1+@"Role set IsFreeze = 0  where   CreateIP = '{2}'  and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where   CreateIP = '{2}'   and SpeakTime <='" + DateTime.Now + @"';

select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where CreateIP = '{2}' order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    break;
                case seachType.禁言:
                    if (string.IsNullOrEmpty(msv.Value.ToString()) && msv.Value.ToString() != "0")
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where NoSpeak = 1", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where NoSpeak = 1 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);

                    }
                    else
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where NoSpeak = 1 and ID in ( select ID from Role where ID = '{0}' or Account='{0}' or NickName='{0}' ) ", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where  (ID = '{2}') and FreezeTime <='"+DateTime.Now+@"';
update "+ database1 + @".Role set NoSpeak = 0  where (ID = '{2}') and SpeakTime <='"+DateTime.Now+@"';
select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where NoSpeak = 1 and ID in ( select ID from Role where ID = '{2}' or Account='{2}' or NickName='{2}') order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }


                    break;
                case seachType.封号:
                    if (string.IsNullOrEmpty(msv.Value.ToString()) && msv.Value.ToString() != "0")
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where IsFreeze != 0", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where IsFreeze !=0 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                    else {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where IsFreeze !=0 and ID in (select ID from Role where ID = '{0}' or Account='{0}' or NickName='{0}' )", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where  (ID = '{2}') and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where (ID = '{2}') and SpeakTime <='" + DateTime.Now + @"';
select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where IsFreeze !=0 and ID in (select ID from Role where ID = '{2}' or Account='{2}' or NickName='{2}') order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                        break;
                case seachType.会员账号:
                    if (!string.IsNullOrEmpty(msv.Value.ToString()) && msv.Value.ToString() != "0")
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where Account = '{0}'", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where   Account='{2}'  and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where  Account='{2}'  and SpeakTime <='" + DateTime.Now + @"';

select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where Account = '{2}' order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                    else
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(@"select count(0) from Role", sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
                    }
                    break;
                case seachType.会员昵称:
                    if (!string.IsNullOrEmpty(msv.Value.ToString()) && msv.Value.ToString() != "0")
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where locate ('{0}' ,NickName) > 0", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where   locate ('{2}' ,NickName) > 0  and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where  locate ('{2}' ,NickName) > 0  and SpeakTime <='" + DateTime.Now + @"';

select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where locate ('{2}' ,NickName) > 0 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                    else
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(@"select count(0) from Role", sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
                    }
                    break;
                default:

                    if (Convert.ToInt32(msv.Value) > 0)
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where ID = {0}", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where    ID = {2}  and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where   ID = {2}  and SpeakTime <='" + DateTime.Now + @"';

select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where ID = {2} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                    else
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(@"select count(0) from Role", sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
                    }//FreezeTime,SpeakTime,
                    break;
            }



            PagedList<Role> obj = new PagedList<Role>(DAL.PagedListDAL<Role>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }



        public static PagedList<Role> GetListByPage_No007(MemberSeachView msv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = msv.PageIndex;
            pq.PageSize = 10;

            switch (msv.SeachType)
            {
                case seachType.查询游戏币区间:

                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where (Gold + SafeBox) between {0} and {1} and Agent!=10010", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where (Gold + SafeBox) between {2} and {3} and Agent!=10010  order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询游戏币等于:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where (Gold + SafeBox) = {0} and Agent!=10010", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where (Gold + SafeBox) = {2} and Agent!=10010 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询游戏币大于:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where (Gold + SafeBox) > {0} and Agent!=10010", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where (Gold + SafeBox) > {2} and Agent!=10010 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询钻石区间:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where Diamond between {0} and {1} and Agent!=10010", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where Diamond between {2} and {3} and Agent!=10010 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询钻石等于:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where Diamond = {0} and Agent!=10010", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where Diamond = {2} and Agent!=10010 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询钻石大于:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where Diamond > {0} and Agent!=10010", msv.UpperLimit, msv.LowerLimit), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where Diamond > {2} and Agent!=10010 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.UpperLimit, msv.LowerLimit);
                    break;
                case seachType.查询VIP等级:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where MaxNoble = {0} and Agent!=10010", msv.Lv), sqlconnectionString);
                    pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where MaxNoble = {2} and Agent!=10010 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Lv);
                    break;
                case seachType.IP地址:
                    pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where CreateIP = '{0}'", msv.Value), sqlconnectionString);
                    pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where   CreateIP = '{2}'  and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where   CreateIP = '{2}'   and SpeakTime <='" + DateTime.Now + @"';

select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where CreateIP = '{2}' order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    break;
                case seachType.禁言:
                    if (string.IsNullOrEmpty(msv.Value.ToString()) && msv.Value.ToString() != "0")
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where NoSpeak = 1", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where NoSpeak = 1 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);

                    }
                    else
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where NoSpeak = 1 and ID in ( select ID from Role where ID = '{0}' or Account='{0}' or NickName='{0}' ) ", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where  (ID = '{2}' ) and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where (ID = '{2}' ) and SpeakTime <='" + DateTime.Now + @"';
select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where NoSpeak = 1 and ID in ( select ID from Role where ID = '{2}' or Account='{2}' or NickName='{2}') order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }


                    break;
                case seachType.封号:
                    if (string.IsNullOrEmpty(msv.Value.ToString()) && msv.Value.ToString() != "0")
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where IsFreeze != 0", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where IsFreeze !=0 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                    else
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where IsFreeze !=0 and ID in (select ID from Role where ID = '{0}' or Account='{0}' or NickName='{0}' )", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where  (ID = '{2}' ) and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where (ID = '{2}' ) and SpeakTime <='" + DateTime.Now + @"';
select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where IsFreeze !=0 and ID in (select ID from Role where ID = '{2}' or Account='{2}' or NickName='{2}') order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                    break;
                case seachType.会员账号:
                    if (!string.IsNullOrEmpty(msv.Value.ToString()) && msv.Value.ToString() != "0")
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where Account = '{0}'", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where   Account='{2}'  and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where  Account='{2}'  and SpeakTime <='" + DateTime.Now + @"';

select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where Account = '{2}' order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                    else
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(@"select count(0) from Role", sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
                    }
                    break;
                case seachType.会员昵称:
                    if (!string.IsNullOrEmpty(msv.Value.ToString()) && msv.Value.ToString() != "0")
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where locate ('{0}' ,NickName) > 0", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where   locate ('{2}' ,NickName) > 0  and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where  locate ('{2}' ,NickName) > 0  and SpeakTime <='" + DateTime.Now + @"';

select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where locate ('{2}' ,NickName) > 0 order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                    else
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(@"select count(0) from Role", sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
                    }
                    break;
                default:

                    if (Convert.ToInt32(msv.Value) > 0)
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where ID = {0}", msv.Value), sqlconnectionString);
                        pq.Sql = string.Format(@"
update "+ database1 + @".Role set IsFreeze = 0  where    ID = {2}  and FreezeTime <='" + DateTime.Now + @"';
update "+ database1 + @".Role set NoSpeak = 0  where   ID = {2}  and SpeakTime <='" + DateTime.Now + @"';

select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role where ID = {2} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, msv.Value);
                    }
                    else
                    {
                        pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(@"select count(0) from Role", sqlconnectionString);
                        pq.Sql = string.Format(@"select FreezeTime,SpeakTime,ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP,  (CASE  WHEN NoSpeak=0  THEN 0 when NoSpeak>=1 then 1  ELSE NoSpeak END) as NoSpeak,(CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac from Role order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
                    }//FreezeTime,SpeakTime,
                    break;
            }



            PagedList<Role> obj = new PagedList<Role>(DAL.PagedListDAL<Role>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        public static int UpdateIsFreeze(string ip, isSwitch isFreeze)
        {
            return RoleDAL.UpdateIsFreeze(ip, isFreeze);
        }


     



        public static int GetRecordCount(int id)
        {
            return DAL.PagedListDAL<GameUserInfo>.GetRecordCount(string.Format(@"select count(0) from "+ database1 + @".Role where Agent = {0}", id));
        }


        public static int GetRecordCountNotFreeze()
        {
            return DAL.PagedListDAL<GameUserInfo>.GetRecordCount(string.Format(@"
select count(0) from "+database1+@".Role where IsFreeze = 0
"));
        }


        public static PagedList<Role> GetListByPage(int page, int id)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;


            pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where Agent = {0}", id), sqlconnectionString);
            pq.Sql = string.Format(@"select ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP, NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac, ClubID from Role where Agent = {2} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, id);



            PagedList<Role> obj = new PagedList<Role>(DAL.PagedListDAL<Role>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static PagedList<Role> GetRouletteDuiHuanInfo(int page, int id)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;


            pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;select count(0) from Role where Post >0  {0}", id>0? "and id = "+ id : "" ), sqlconnectionString);
            pq.Sql = string.Format(@"SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;select ID,TrueName,   Tel   ,  QQNum  ,  Post  ,  Address   
                       from Role where Post >0  {2} order by ID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, id>0? " and ID = "+id : "");



            PagedList<Role> obj = new PagedList<Role>(DAL.PagedListDAL<Role>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }




        /// <summary>
        /// 通过会员id，账号，匿名查询会员id
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static long GetIdByIdOrAccoOrNName(string where) {
          
            return RoleDAL.GetIdByIdOrAccoOrNName(where);
        }

#endif

#if MWeb || Redis

        public static Role GetModelByAcc(Role role)
        {
            return RoleDAL.GetModelByAcc(role);
        }

        public static IEnumerable<Role> GetModelByIDList(string UserID)
        {
            return RoleDAL.GetModelByIDList(UserID);
        }

        //GetModelByIDs
        public static IEnumerable<Role> GetModelByIDs(string UserID)
        {
            return RoleDAL.GetModelByIDs(UserID);
        }


        public static IEnumerable<Role> GetMasterLevelModels() {
            return RoleDAL.GetMasterLevelModels();
        }


        public static PagedList<Role> GetListByCreateTime(int page, DateTime startTime, DateTime endTime)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;


            pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where CreateTime >= '{0}' and CreateTime<'{1}'" , startTime,endTime), sqlconnectionString);
            pq.Sql = string.Format(@"select ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP, NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac, ClubID 
                                     from Role where CreateTime >= '{2}' and CreateTime<'{3}' order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, startTime, endTime);



            PagedList<Role> obj = new PagedList<Role>(DAL.PagedListDAL<Role>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        public static PagedList<Role> GetListByCreateTime(BaseDataView bdv)
        {
            PagerQuery pq = new PagerQuery();
          

          

            if (bdv.Channels > 0)
            {
                pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where CreateTime >= '{0}' and CreateTime<'{1}' and Agent in ({2})", bdv.StartDate, bdv.ExpirationDate, bdv.UserList), sqlconnectionString);
                pq.Sql = string.Format(@"select ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP, NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac, ClubID 
                                     from Role where CreateTime >= '{2}' and CreateTime<'{3}' and Agent in ({4})  order by CreateTime desc ", pq.StartRowNumber, pq.PageSize, bdv.StartDate, bdv.ExpirationDate, bdv.UserList);
            }
            else {
                pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format(@"select count(0) from Role where CreateTime >= '{0}' and CreateTime<'{1}' and Agent!=10010", bdv.StartDate, bdv.ExpirationDate), sqlconnectionString);
                pq.Sql = string.Format(@"select ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP, NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac, ClubID 
                                     from Role where CreateTime >= '{2}' and CreateTime<'{3}'  and Agent!=10010 order by CreateTime desc ", pq.StartRowNumber, pq.PageSize, bdv.StartDate, bdv.ExpirationDate);
            }

            var d = DAL.PagedListDAL<Role>.GetListByPage(pq, sqlconnectionString);

            PagedList<Role> obj = new PagedList<Role>(d, 1, d.Count()+1, d.Count()+1);
            return obj;
        }

#endif

        public static Role GetModelByOpenID(Role role)
        {
            return RoleDAL.GetModelByOpenID(role);
        }

        public static Role GetModelByID(Role role)
        {
            return RoleDAL.GetModelByID(role);
        }

        public static Role GetRoleByString(Role str)
        {
            //查询角色表的时候更新禁言 封号状态
            RoleDAL.UpdateFreezeNoSpeak(Convert.ToInt32(str.ID));


            return RoleDAL.GetRoleByID(str);
        }


        public static Role GetVRoleByString(Role str)
        {
            //查询角色表的时候更新禁言 封号状态
            RoleDAL.UpdateFreezeNoSpeak(Convert.ToInt32(str.ID));


            return RoleDAL.GetVRoleByID(str);
        }

        public static Role GetGiftByString(Role str)
        {
            return RoleDAL.GetGiftByID(str);
        }
        public static int UpdateRole(Role model)
        {
            return RoleDAL.UpdateRole(model);
        }

        public static int UpdateRoleNoSpeak(int nospeak, DateTime speakTime, int id)
        {
            return RoleDAL.UpdateRoleNoSpeak(nospeak, speakTime, id);
        }
        public static int UpdateRoleNoFreeze(int isfreeze, DateTime freezeTime, int id)
        {
            return RoleDAL.UpdateRoleNoFreeze(isfreeze, freezeTime, id);
        }

        public static int UpdateRolePwd(string pwd, int id)
        {
            return RoleDAL.UpdateRolePwd(pwd, id);
        }
        
        public static int UpdateRoleSafePwd(string pwd, int id)
        {
            return RoleDAL.UpdateRolePwd(pwd, id);
        }
        public static int UpdateRoleClub(Role model)
        {
            return RoleDAL.UpdateRoleClub(model);
        }
        

        public static int AddMD5Flow(GL.Data.Model.MD5Flow model)
        {
            return RoleDAL.AddMD5Flow(model);
        }

        public static IEnumerable<MD5Flow> GetMD5Flow(MD5Flow model) {
            return RoleDAL.GetMD5Flow(model);
        }
    }
}
