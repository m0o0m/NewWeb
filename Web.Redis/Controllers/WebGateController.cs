using GL.Data;
using GL.Data.BLL;
using GL.Data.Model;
using log4net;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace Web.Redis.Controllers
{
    [RoutePrefix("api/WebGate")]
    public class WebGateController : ApiController
    {
        private ILog log = LogManager.GetLogger("WebGate");
        /// <summary>
        /// 网关查询
        /// </summary>
        /// <returns>网关地址</returns>
        [Route("GetGate")]
        public Gate GetGate()
        {


           var gatelist =  GateInfoBLL.GetModel();
            if (gatelist.Count() <= 1)
            {
                var _gate = gatelist.FirstOrDefault();
                if (_gate == null)
                {
                    return new Gate
                    {
                        G = string.Concat(0, ":", 0),
                        Z = string.Concat(0, ":", 0),
                        H = string.Concat(0, ":", 0)
                    };
                }
                return new Gate
                {
                    G = string.Concat(_gate.GIP, ":", _gate.GPort),
                    Z = string.Concat(_gate.ZIP, ":", _gate.ZPort),
                    H = string.Concat(_gate.HIP, ":", _gate.HPort)
                };

            }



            var res = ServiceStackManage.Redis.GetDatabase().HashGetAll("GateNum");

            //var id = (from x in res
            //          where (int)x.Value < 3
            //          orderby x.Value descending
            //          select x.Name).FirstOrDefault();
            //var id = (from x in res
            //          orderby x.Value descending
            //          select x.Name).FirstOrDefault();

            //var id = (from x in res
            //          orderby x.Value descending
            //          select Convert.ToInt32(x.Name)).ToList();
            //log.Error(String.Join("\n ", res.Select(x => JsonConvert.SerializeObject(x)).ToList()));

            var gatelist1 = (from x in gatelist
                             join y in res on x.ID equals Convert.ToInt32(y.Name) into odr
                             from o in odr.DefaultIfEmpty()
                             select new GateInfo
                             {
                                 ID = x.ID,
                                 GIP = x.GIP,
                                 GPort = x.GPort,
                                 HIP = x.HIP,
                                 HPort = x.HPort,
                                 ZIP = x.ZIP,
                                 ZPort = x.ZPort,
                                 Limit = x.Limit,
                                 Type = x.Type,
                                 Description = x.Description,
                                 Num = Convert.ToInt32(o.Value)
                             });
            //log.Error(String.Join("\n ", gatelist1.Select(x=> " Description" + x.Description + " Num" + x.Num).ToList()));

            var gate = (from x in gatelist1
                       where x.Num < x.Limit
                       orderby x.ID ascending
                       select x).FirstOrDefault();
            //log.Error(" Description" + gate.Description + " Num" + gate.Num );

            if (gate == null)
            {
                gate = (from x in gatelist1
                        orderby x.Num ascending
                        select x).FirstOrDefault();
            }
            //log.Error(JsonConvert.SerializeObject(gate));


            return new Gate
            {
                G = string.Concat(gate.GIP, ":", gate.GPort),
                Z = string.Concat(gate.ZIP, ":", gate.ZPort),
                H = string.Concat(gate.HIP, ":", gate.HPort)
            };
        }

        [Route("GetGate")]
        public Gate GetGate([FromUri]string id, [FromUri]string account, [FromUri]string openid)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                if (string.IsNullOrWhiteSpace(account))
                {
                    if (string.IsNullOrWhiteSpace(openid))
                    {
                        return GetGate();
                    }
                    var res1 = RoleBLL.GetModelByOpenID(new Role { OpenID = openid });
                    if (res1 == null)
                    {
                        return GetGate();
                    }
                    id = res1.ID.ToString();
                }
                var res2 = RoleBLL.GetModelByAcc(new Role { Account = account });
                if (res2 == null)
                {
                    return GetGate();
                }
                id = res2.ID.ToString();
            }
            var res = ServiceStackManage.Redis.GetDatabase().HashScan("UserGate", id, 5, CommandFlags.None);
            var GateID = (from x in res
                          where x.Name == id
                          select Convert.ToInt32(x.Value)).FirstOrDefault();


            var gate = GateInfoBLL.GetModelByID(GateID);
            //log.Info(id + "\n " + JsonConvert.SerializeObject(gate));

            if (gate == null)
            {
                return GetGate();
                //var aaa = GetGate();
                //log.Info(id + "\n " + JsonConvert.SerializeObject(aaa));
                //return aaa;
            }
            //log.Info(id + "\n Description" + gate.Description + " Num" + gate.Num);

            return new Gate
            {
                G = string.Concat(gate.GIP, ":", gate.GPort),
                Z = string.Concat(gate.ZIP, ":", gate.ZPort),
                H = string.Concat(gate.HIP, ":", gate.HPort)
            };
        }



        [Route("GetCheck")]
        public object GetCheck()
        {
            var res = ServiceStackManage.Redis.GetDatabase().Ping();
            return res;
        }

        //[Route("GeList")]
        //public IEnumerable<GateW> GetList()
        //{

        //    var res = ServiceStackManage.GetHashEntry("GateNum");
        //    //var res = ServiceStackManage.Redis.GetDatabase().Ping();

        //    return (from x in res
        //            select new GateW
        //            {
        //                key = x.Name,
        //                value = x.Value
        //            }).ToList();
        //}

        //public class GateW
        //{
        //    public string key { get; set; }
        //    public string value { get; set; }
        //}

        //public Gate GetGate(string id)
        //{
        //    var res = ServiceStackManage.Get(string.Format("Gi_{0}_gid", id));
        //    if (res.IsNullOrEmpty)
        //    {
        //        return null;
        //    }
        //    var gate = GateInfoBLL.GetModelByID(Convert.ToInt32(res));
        //    return new Gate
        //    {
        //        G = string.Concat(gate.GIP, ":", gate.GPort),
        //        Z = string.Concat(gate.ZIP, ":", gate.ZPort),
        //        H = string.Concat(gate.HIP, ":", gate.HPort)
        //    };
        //}



        //public Gate Get()
        //{



        //string res = ServiceStackManage.Get<string>("Gi_1_num");

        //var db = ServiceStackManage.Redis.GetDatabase();


        //var result = db.HashScan("Gi_*_num", "*", 10, 1, 30, CommandFlags.None).ToList();
        //var result = db.HashScan("Gi_1_num", "*", 30, CommandFlags.None).ToList();

        //if (result.Any())
        //{
        //    //转换成ids                                
        //    var ids = result.ToList().Select<SortedSetEntry, RedisKey>(i => i.ToString());
        //    //按照keys获取value                
        //    var values = db.StringGet(ids.ToArray());
        //    //构造List Json以加速解析                
        //    var portsJson = new StringBuilder("[");
        //    values.ToList().ForEach(item =>
        //    {
        //        if (!string.IsNullOrWhiteSpace(item))
        //        {
        //            portsJson.Append(item).Append(",");
        //        }
        //    });
        //    portsJson.Append("]");
        //    //users = JsonConvert.DeserializeObject<List<GateInfo>>(portsJson.ToString());
        //}
        //return null;

        //}


        // GET api/values/5
        //public RedisValue Get(string id)
        //{

        //    return ServiceStackManage.Get(id);
        //}

        //public RedisValue[] Get(params string[] id)
        //{

        //    return ServiceStackManage.Get(id.Select<string, RedisKey>(x => x).ToArray());
        //}

        //public void CreateTerminalCache(List<User> users)
        //{
        //    if (users == null) return;
        //    var db = ConnectionMultiplexer.GetDatabase();
        //    var sourceData = new List<KeyValuePair<RedisKey, RedisValue>>();
        //    //构造集合数据            
        //    var list = users.Select(item =>
        //    {
        //        var value = JsonConvert.SerializeObject(item);
        //        //构造原始数据                
        //        sourceData.Add(new KeyValuePair<RedisKey, RedisValue>("capqueen:users:" + item.Id, value));
        //        //构造数据                    
        //        return new SortedSetEntry(item.Name, item.Id);
        //    });
        //    //添加进有序集合，采用name - id             
        //    db.SortedSetAdd("capqueen:users:index", list.ToArray());
        //    //添加港口数据key-value            
        //    db.StringSet(sourceData.ToArray(), When.Always, CommandFlags.None);
        //}

        ////然后搜索的时候如下:
        //public List<User> GetUserByWord(string words)
        //{
        //    var db = ConnectionMultiplexer.GetDatabase();
        //    //搜索            
        //    var result = db.SortedSetScan("capqueen:users:index", words + "*", 10, 1, 30, CommandFlags.None).Take(30).ToList();
        //    var users = new List<User>(); if (result.Any())
        //    {
        //        //转换成ids                                
        //        var ids = result.ToList().Select<SortedSetEntry, RedisKey>(i => i.ToString());
        //        //按照keys获取value                
        //        var values = db.StringGet(ids.ToArray());
        //        //构造List Json以加速解析                
        //        var portsJson = new StringBuilder("[");
        //        values.ToList().ForEach(item =>
        //        {
        //            if (!string.IsNullOrWhiteSpace(item)) { portsJson.Append(item).Append(","); }
        //        }); portsJson.Append("]"); users = JsonConvert.DeserializeObject<List<User>>(portsJson.ToString());
        //    }
        //    return users;
        //}

    }

}
