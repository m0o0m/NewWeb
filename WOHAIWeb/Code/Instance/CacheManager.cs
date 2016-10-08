using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using GL.Data.BLL;
using GL.Data.Model;

namespace GL.Instance
{
    public sealed class CacheManager
    {
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        private static System.Timers.Timer cacheTimer = new System.Timers.Timer(60000);

        private CacheManager()
        {
            cacheTimer.AutoReset = true;
            cacheTimer.Enabled = true;
            cacheTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            cacheTimer.Start();
        }
        internal static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            ArrayList akeys = new ArrayList(paramCache.Keys);
            for (int i = 0; i < akeys.Count; i++)
            {
                if (((OnlineStatic)paramCache[akeys[i]]).IsLeave)
                {
                    OnlineStatic os = ((OnlineStatic)paramCache[akeys[i]]);
                    ManagerInfoBLL.UpdateState(new ManagerInfo { AdminID = os.Id, AdminMasterRight = masterRight.离线 });
                    paramCache.Remove(akeys[i]);
                }
            }
        }

        internal static CacheManager Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        internal int Count
        {
            get { return paramCache.Count; }
        }

        internal void Create(object key, object value)
        {
            paramCache.Add(key, value);
        }

        internal void Update(object key, object value)
        {
            paramCache[key] = value;
        }

        internal object Select(object key)
        {
            return paramCache[key];
        }

        internal void Delete(object key)
        {
            if (IsExist(key))
            {
                OnlineStatic os = ((OnlineStatic)paramCache[key]);
                ManagerInfoBLL.UpdateState(new ManagerInfo { AdminID = os.Id, AdminMasterRight = masterRight.离线 });
                paramCache.Remove(key);
            }
        }

        internal bool IsExist(object key)
        {
            return paramCache.ContainsKey(key);
        }
        internal Hashtable ParamCache
        {
            get { return paramCache; }
            set { paramCache = value; }
        }

        //嵌套类
        private class Nested
        {
            internal static readonly CacheManager instance;

            private Nested()
            { }

            static Nested()
            {
                instance = new CacheManager();
            }
        }



    }


}