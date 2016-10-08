using GL.Command.DBUtility;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Redis.Data
{
    public class ServiceStackManage
    {
        // private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private static ConnectionMultiplexer redis = ServiceStackManage.CreateRedisManager();
        //private static PooledRedisClientManager prcm = ServiceStackManage.CreateRedisManager(
        //   new string[] { "192.168.1.6:6379" },   //读写服务器
        //   new string[] { "192.168.1.6:6379" }    //只读服务器
        //);


        /// <summary>
        /// 创建Redis连接池管理对象
        /// </summary>
        //public static ConnectionMultiplexer CreateRedisManager(string[] readWriteHosts, string[] readOnlyHosts)
        //public static ConnectionMultiplexer CreateRedisManager()
        //{

        //    string configString = PubConstant.GetConnectionString("RedisConnectionString");
        //    var options = ConfigurationOptions.Parse(configString);
        //    options.ClientName = "RedisGate";
        //    options.AllowAdmin = true;
        //    return ConnectionMultiplexer.Connect(options);

        //    //支持读写分离，均衡负载
        //    //return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
        //    //{
        //    //    MaxWritePoolSize = 5,
        //    //    //“写”链接池数
        //    //    MaxReadPoolSize = 5,
        //    //    //“读”链接池数
        //    //    AutoStart = true,
        //    //});
        //}


        private static string constr = PubConstant.GetConnectionString("RedisConnectionString");

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            var options = ConfigurationOptions.Parse(constr);
            options.ClientName = "RedisGate";
            options.AllowAdmin = true;
            options.Password = "jx3role";



            return ConnectionMultiplexer.Connect(options);
        });

        public static ConnectionMultiplexer Redis
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        public static bool Set(string key, RedisValue val)
        {
            return Redis.GetDatabase().SetAdd(key, val);
        }


        /// <summary>
        /// 读取数据
        /// </summary>
        public static RedisValue[] Get(params RedisKey[] key)
        {
            return Redis.GetDatabase().StringGet(key);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        public static RedisValue[] GetHash(string key)
        {
            return Redis.GetDatabase().HashValues(key);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        public static HashEntry[] GetHashEntry(string key)
        {
            return Redis.GetDatabase().HashGetAll(key);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        public static RedisValue Get(string key)
        {
            return Redis.GetDatabase().StringGet(key);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public static bool Remove(string key)
        {
            return Redis.GetDatabase().KeyDelete(key);
        }
    }


}