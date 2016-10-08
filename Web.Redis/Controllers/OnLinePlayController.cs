using GL.Data;
using GL.Data.BLL;
using GL.Data.Model;
using log4net;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using Web.Redis.Models;

namespace Web.Redis.Controllers
{



    [RoutePrefix("api/OnLinePlay")]
    public class OnLinePlayController : ApiController
    {
        [Route("GetOnLineUser")]
        // GET: OnLinePlay
        public object GetOnLineUser([FromUri]string pageSize, [FromUri]string pageIndex)
        {
                ILog log = LogManager.GetLogger("OnLinePlay");
                //默认分页配置
                if (string.IsNullOrEmpty(pageSize)) { pageSize = "10"; }
                if (string.IsNullOrEmpty(pageIndex)) { pageIndex = "1"; }


                Cache ca = HttpContext.Current.Cache;
                List<RankModel> rankAll = new List<RankModel>();
                try
                {
                    rankAll = ca["onlineList"] as List<RankModel>;
                }
                catch(Exception ex) {
                    log.Error("OnLinePlay(读取在线玩家):"+ex.Message);
                    rankAll = new List<RankModel>();
                }

            if (rankAll == null) {
                log.Error("OnLinePlay(读取在线玩家):读取的缓存转换为null"  );
                string rJson = OnLineInfoBLL.GetNewJson();
                rankAll = (List<RankModel>)JsonConvert.DeserializeObject<List<RankModel>>(rJson);
            }
            int onlineUserTotal = rankAll.Count();
            if (onlineUserTotal == 0) {
                log.Error("OnLinePlay(读取在线玩家):读取的缓存转换为长度为0");
                string rJson = OnLineInfoBLL.GetNewJson();
                rankAll = (List<RankModel>)JsonConvert.DeserializeObject<List<RankModel>>(rJson);
            }







                int pageSizeInt = Convert.ToInt32(pageSize); //取多少行数据
                int pageIndexInt = Convert.ToInt32(pageIndex);//取第几页
                int pageTotal = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(onlineUserTotal * 1.0 / pageSizeInt)));//总页数

                int skip = (pageIndexInt - 1) * pageSizeInt;//跳过多少行
                                                            //取需要的数据
                IEnumerable<RankModel> onlineUser = (
                                    from a in rankAll
                                    select a
                                  ).OrderByDescending(m => m.VipGrade).Skip(skip).Take(pageSizeInt);

                List<RankListData> ModelList = new List<RankListData>();
                foreach (RankModel item in onlineUser)
                {
                    string[] rs = item.Value.ToString().Split('_');
                    RankListData data = new RankListData();
                    data.dwFaceID = Convert.ToInt32(rs[2]);
                    data.strFaceUrl = rs[1];
                    data.dwUserID = Convert.ToInt32(item.Name);
                    data.gameID = GameId(rs[5]);
                    data.isOnline = IsOnline(rs[5]);
                    data.llValue = Convert.ToDouble(rs[3]);
                    data.strRoleName = rs[0];
                    data.vipGrade = Convert.ToInt32(rs[4]);
                    ModelList.Add(data);
                }

                return Json(new
                {
                    dwType = 0,
                    list = ModelList,
                    showNum = pageSizeInt,
                    totalPage = pageTotal,
                    curPage = pageIndex
                });

       




        }



        //13 中发白  14 十二生肖   15 德州扑克
        private int GameId(string value)
        {
            if (value[0] == '1')
            {
                return 13;
            }
            else if (value[1] == '1')
            {
                return 14;
            }
            else if (value[2] == '1')
            {
                return 15;
            }
            else
            {
                return 0;
            }
        }
        private bool IsOnline(string value)
        {
            if (value[0] == '1' || value[1] == '1' || value[2] == '1')
            {
                return true;
            }
            else
            {
                return false;
            }
        }





    }
}