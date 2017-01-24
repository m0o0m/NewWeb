using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Command.DBUtility;
using GL.Data.Model;
using System.Data;
using GL.Common;
using Webdiyer.WebControls.Mvc;
using GL.Data.View;
using MySql.Data.MySqlClient;
using GL.Dapper;

namespace GL.Data.DAL
{

    public class MemberCenterDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");
        public static readonly string sqlconnectionStringForRecord = PubConstant.GetConnectionString("ConnectionStringForGameRecord");
        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");
        public static readonly string sqlconnection = PubConstant.ConnectionString;

        public static PagedList<Role> GetAllListByPage(int PageIndex, string userID)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = PageIndex;
            pq.PageSize = 10;
            PagedList<Role> obj = null;
            if (string.IsNullOrEmpty(userID))
            {
                pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount("select count(0) from Role", sqlconnectionString);
                pq.Sql = string.Format(@"SELECT r.ID,NickName,Agent,Gold,Diamond,SafeBox,CreateTime,LastModify,CreateMac,AgentName,
                (CASE WHEN NoSpeak=0 THEN 0 WHEN NoSpeak>=1 THEN 1 ELSE NoSpeak END) AS NoSpeak, 
(CASE WHEN IsFreeze=0 THEN 0 WHEN IsFreeze>=1 THEN 1 ELSE IsFreeze END) AS IsFreeze from Role r LEFT JOIN {2}.AgentUsers a on r.agent=a.id ORDER BY r.ID  DESC LIMIT {0}, {1}", pq.StartRowNumber, pq.PageSize, database2);
                obj = new PagedList<Role>(DAL.PagedListDAL<Role>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            }
            else
            {
                pq.RecordCount = 1;
                pq.CurrentPage = 1;
                pq.Sql = string.Format(@"SELECT r.ID,NickName,Agent,Gold,Diamond,SafeBox,CreateTime,LastModify,CreateMac,AgentName,
                (CASE WHEN NoSpeak=0 THEN 0 WHEN NoSpeak>=1 THEN 1 ELSE NoSpeak END) AS NoSpeak, 
(CASE WHEN IsFreeze=0 THEN 0 WHEN IsFreeze>=1 THEN 1 ELSE IsFreeze END) AS IsFreeze from Role r LEFT JOIN {1}.AgentUsers a on r.agent=a.id where r.id={0} ORDER BY r.ID  ", userID, database2);
                obj = new PagedList<Role>(DAL.PagedListDAL<Role>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            }
            return obj;

        }


        //GetDataForAbnormalUser
        public static PagedList<Role> GetDataForAbnormalUser(int PageIndex, string type)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = PageIndex;
            pq.PageSize = 10;
            //  int StartRowNumber = (PageIndex - 1) * pq.PageSize;
            PagedList<Role> obj = null;
            string SqlWhere = "";
            if (type.Trim() == "yichang")
            {
                SqlWhere = " NoSpeak>0 and IsFreeze>0 ";
            }
            if (type.Trim() == "fenghao")
            {
                SqlWhere = " IsFreeze>0 ";
            }
            if (type.Trim() == "jinyan")
            {
                SqlWhere = " NoSpeak>0 ";
            }
            pq.RecordCount = DAL.PagedListDAL<Role>.GetRecordCount(string.Format("select count(0) from Role Where  {0}", SqlWhere), sqlconnectionString);
            pq.Sql = string.Format(@"SELECT ID,NickName,Agent,Gold,Diamond,SafeBox,CreateTime,LastModify,CreateMac, 
                (CASE WHEN NoSpeak=0 THEN 0 WHEN NoSpeak>=1 THEN 1 ELSE NoSpeak END) AS NoSpeak, 
(CASE WHEN IsFreeze=0 THEN 0 WHEN IsFreeze>=1 THEN 1 ELSE IsFreeze END) AS IsFreeze FROM Role WHERE  ID IN (SELECT ID FROM Role Where {2}) 
ORDER BY ID DESC LIMIT {0}, {1}", pq.StartRowNumber, pq.PageSize, SqlWhere);
            obj = new PagedList<Role>(DAL.PagedListDAL<Role>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }



        public static PagedList<BlackList> GetListByPageForMac(int page, ValueView vv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            // int StartRowNumber = (page - 1) * pq.PageSize;
            if (!string.IsNullOrEmpty(vv.Value))
            {
                pq.RecordCount = DAL.PagedListDAL<BlackList>.GetRecordCount(string.Format(@"select count(0) from BG_BanMacList where Mac = '{0}'", vv.Value), sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_BanMacList where Mac = '{2}'order by CreateTime limit {0}, {1}", pq.StartRowNumber, pq.PageSize, vv.Value);

            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<BlackList>.GetRecordCount(@"select count(0) from BG_BanMacList", sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_BanMacList order by CreateTime limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
            }


            PagedList<BlackList> obj = new PagedList<BlackList>(DAL.PagedListDAL<BlackList>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        public static PagedList<BlackList> GetListByPageFor(int page, ValueView vv)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            //  int StartRowNumber = (page - 1) * pq.PageSize;
            if (!string.IsNullOrEmpty(vv.Value))
            {
                pq.RecordCount = DAL.PagedListDAL<BlackList>.GetRecordCount(string.Format(@"select count(0) from BG_BanIPList where IP = '{0}'", vv.Value), sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_BanIPList where IP = '{2}'order by CreateTime limit {0}, {1}", pq.StartRowNumber, pq.PageSize, vv.Value);

            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<BlackList>.GetRecordCount(@"select count(0) from BG_BanIPList", sqlconnectionString);
                pq.Sql = string.Format(@"select * from BG_BanIPList order by CreateTime limit {0}, {1}", pq.StartRowNumber, pq.PageSize);
            }


            PagedList<BlackList> obj = new PagedList<BlackList>(DAL.PagedListDAL<BlackList>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        public static int GetSendEmailCount(string id)
        {
            int count = 0;
            using (var cn = new MySqlConnection(sqlconnection))
            {
                cn.Open();
                count = cn.Query<int>(string.Format("select COUNT(*) from UserEmail where UEUserID={0}", id)).Single();
                cn.Close();
            }
            return count;
        }


        public static int UpdateRemarksname(string id, string name)
        {
            int ishave = 0;
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                ishave = cn.Query<int>(string.Format("select count(*) from remarksname  where UserId ={0}", id)).Single();
                cn.Close();
            }
            if (ishave > 0)
            {
                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();
                    int i = cn.Execute(string.Format("update remarksname set Name='{0}' where UserId ={1}", name, id));
                    cn.Close();
                    return i;
                }
            }
            else
            {
                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();
                    int i = cn.Execute(string.Format("insert into remarksname(UserId,Name) values({0},'{1}')", id, name));
                    cn.Close();
                    return i;
                }
            }

        }


        public static string GetRemarksNameByID(string id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                //    string Rname = cn.Query<string>(string.Format("", id));
                //  int Rname = cn.Execute(string.Format("select Name from remarksname  where UserId ={0}", id));
                string Rname = cn.ExecuteScalar<string>(string.Format("select Name from remarksname  where UserId ={0}", id));
                cn.Close();
                return Rname;
            }

        }

        public static int UpdateNickName(string id, string name)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(string.Format("update Role set NickName='{0}' where id ={1}", name, id));
                cn.Close();
                return i;
            }
        }


        //     public static IEnumerable<ScaleGameRecord> GetListForScale(GameRecordView vbd)   DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), 
        public static long GetScaleForZyinkui(int zID)
        {
            GameRecordView vbd = new GameRecordView { StartDate = DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") };

            long score = 0;
            using (var cn = new MySqlConnection(sqlconnectionStringForRecord))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("select * from BG_ScaleGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");
                IEnumerable<ScaleGameRecord> data = cn.Query<ScaleGameRecord>(str.ToString());
                cn.Close();
                foreach (ScaleGameRecord m in data)
                {
                    if (int.Parse(m.UserData.Split(',').ToList()[0]) == zID)
                    {
                        List<string> wanjia = m.UserData.Split('_').ToList();
                        wanjia.RemoveAt(0);//除去庄家
                        wanjia.RemoveAt(wanjia.Count - 1);//   去除最后一个元素  因为是空的
                        for (int i = 0; i < wanjia.Count; i++)
                        {
                            score = score + int.Parse(wanjia[i].Split(',')[5]);
                        }
                    }
                }
            }

            return score;

        }


        public static long GetListForZodiacyinkui(int zID)
        {
            IEnumerable<ZodiacGameRecord> data = null;
            long score = 0;
            GameRecordView vbd = new GameRecordView { StartDate = DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") };
            using (var cn = new MySqlConnection(sqlconnectionStringForRecord))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from BG_ZodiacGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");

                data = cn.Query<ZodiacGameRecord>(str.ToString());
                cn.Close();
            }

            foreach (ZodiacGameRecord m in data)
            {
                if (int.Parse(m.UserData.Split(',').ToList()[0]) == zID)
                {
                    score = score + int.Parse(m.UserData.Split(',').ToList()[5]);
                }
            }
            return score;
        }



        public static long GetListForCaryinkui(int zID)
        {
            IEnumerable<CarGameRecord> data = null;
            long score = 0;
            GameRecordView vbd = new GameRecordView { StartDate = DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") };
            using (var cn = new MySqlConnection(sqlconnectionStringForRecord))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("select * from BG_CarGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");

                data = cn.Query<CarGameRecord>(str.ToString());
                cn.Close();

            }
            foreach (CarGameRecord m in data)
            {
                if (int.Parse(m.UserData.Split(',').ToList()[0]) == zID)
                {
                    score = score + int.Parse(m.UserData.Split(',').ToList()[5]);   //庄家输赢
                }
            }
            return score;
        }


        public static long GetListForHundredyinkui(int zID)
        {
            long score = 0;

            IEnumerable<ScaleGameRecord> data = null;
            GameRecordView vbd = new GameRecordView { StartDate = DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), ExpirationDate = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") };
            using (var cn = new MySqlConnection(sqlconnectionStringForRecord))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();

                str.Append("select * from " + database3 + @".BG_TexProGameRecord where CreateTime between '" + vbd.StartDate + "' and '" + vbd.ExpirationDate + "' and CreateTime!='" + vbd.ExpirationDate + "' order by CreateTime desc ");

                data = cn.Query<ScaleGameRecord>(str.ToString());
                cn.Close();
            }
            foreach (ScaleGameRecord m in data)
            {
                if (int.Parse(m.UserData.Split(',').ToList()[0]) == zID)
                {
                    List<string> wanjia = m.UserData.Split('_').ToList();
                    wanjia.RemoveAt(0);//出去庄家
                    wanjia.RemoveAt(wanjia.Count - 1);//   去除最后一个元素  因为是空的
                    for (int i = 0; i < wanjia.Count; i++)
                    {
                        score = score + int.Parse(wanjia[i].Split(',')[2]);
                    }
                }
            }
            return score;
        }


        /// <summary>
        /// 德州扑克
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PagedList<TexasGameRecord> GetListByPageForTexas(GameRecordView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<TexasGameRecord>.GetRecordCount(string.Format(@" select count(*) from BG_TexasGameRecord where  ( UserID like '{2}%' or UserID like '%{2}%') and CreateTime between '{0}' and '{1}'", model.StartDate, model.ExpirationDate, model.SearchExt), sqlconnectionStringForRecord);
            pq.Sql = string.Format(@"
           select * from BG_TexasGameRecord where  ( UserID like '{4}%' or UserID like '%{4}%') and CreateTime between '{2}' and '{3}'
            order by CreateTime desc 
            limit {0}, {1};", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate, model.SearchExt);
            PagedList<TexasGameRecord> obj = new PagedList<TexasGameRecord>(DAL.PagedListDAL<TexasGameRecord>.GetListByPage(pq, sqlconnectionStringForRecord), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }


        /// <summary>
        /// 中发白
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PagedList<ScaleGameRecord> GetListByPageForScale(GameRecordView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;
            if (Convert.ToInt64(model.Data) > 0)
            {
                if (Convert.ToInt64(model.SearchExt) > 0)
                {
                    pq.RecordCount = DAL.PagedListDAL<ScaleGameRecord>.GetRecordCount(string.Format(@"select count(0) from BG_ScaleGameRecord where ( UserData like '{3}%' or UserData like '%_{3}%') and CreateTime between '{0}' and '{1}' and Round = {2}", model.StartDate, model.ExpirationDate, model.Data,model.SearchExt), sqlconnectionStringForRecord);
                    pq.Sql = string.Format(@"select * from BG_ScaleGameRecord where  ( UserData like '{5}%' or UserData like '%_{5}%') and CreateTime between '{2}' and '{3}' and Round = {4} order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate, model.Data,model.SearchExt);
                }
                else
                {
                    pq.RecordCount = DAL.PagedListDAL<ScaleGameRecord>.GetRecordCount(string.Format(@"select count(0) from BG_ScaleGameRecord where CreateTime between '{0}' and '{1}' and Round = {2}", model.StartDate, model.ExpirationDate, model.Data), sqlconnectionStringForRecord);
                    pq.Sql = string.Format(@"select * from BG_ScaleGameRecord where CreateTime between '{2}' and '{3}' and Round = {4} order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate, model.Data);
                }
            }
            else
            {
                if (Convert.ToInt64(model.SearchExt) > 0)
                {
                    pq.RecordCount = DAL.PagedListDAL<ScaleGameRecord>.GetRecordCount(string.Format(@"select count(0) from BG_ScaleGameRecord where ( UserData like '{2}%' or UserData like '%_{2}%') and CreateTime between '{0}' and '{1}'", model.StartDate, model.ExpirationDate, model.SearchExt), sqlconnectionStringForRecord);
                    pq.Sql = string.Format(@"select * from BG_ScaleGameRecord where ( UserData like '{4}%' or UserData like '%_{4}%') and CreateTime between '{2}' and '{3}' order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate, model.SearchExt);
                }
                else
                {
                    pq.RecordCount = DAL.PagedListDAL<ScaleGameRecord>.GetRecordCount(string.Format(@"select count(0) from BG_ScaleGameRecord where CreateTime between '{0}' and '{1}'", model.StartDate, model.ExpirationDate), sqlconnectionStringForRecord);
                    pq.Sql = string.Format(@"select * from BG_ScaleGameRecord where CreateTime between '{2}' and '{3}' order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate);
                }
            }
            PagedList<ScaleGameRecord> obj = new PagedList<ScaleGameRecord>(DAL.PagedListDAL<ScaleGameRecord>.GetListByPage(pq, sqlconnectionStringForRecord), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }

        /// <summary>
        /// 十二生肖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PagedList<ZodiacGameRecord> GetListByPageForZodiac(GameRecordView model)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = model.Page;
            pq.PageSize = 10;

            if (Convert.ToInt64(model.Data) > 0)
            {
                pq.RecordCount = DAL.PagedListDAL<ZodiacGameRecord>.GetRecordCount(string.Format(@"select count(0) from " + database3 + @".BG_ZodiacGameRecord where CreateTime between '{0}' and '{1}' and Round like '%{2}%'", model.StartDate, model.ExpirationDate, model.Data), sqlconnectionString);
                pq.Sql = string.Format(@"select * from " + database3 + @".BG_ZodiacGameRecord where CreateTime between '{2}' and '{3}' and Round = {4} order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate, model.Data);

            }
            else
            {
                pq.RecordCount = DAL.PagedListDAL<ZodiacGameRecord>.GetRecordCount(string.Format(@"select count(0) from " + database3 + @".BG_ZodiacGameRecord where CreateTime between '{0}' and '{1}'", model.StartDate, model.ExpirationDate), sqlconnectionString);
                pq.Sql = string.Format(@"select * from " + database3 + @".BG_ZodiacGameRecord where CreateTime between '{2}' and '{3}' order by CreateTime desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize, model.StartDate, model.ExpirationDate);
                
            }

            PagedList<ZodiacGameRecord> obj = new PagedList<ZodiacGameRecord>(DAL.PagedListDAL<ZodiacGameRecord>.GetListByPage(pq, sqlconnectionString), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }




    }
}
