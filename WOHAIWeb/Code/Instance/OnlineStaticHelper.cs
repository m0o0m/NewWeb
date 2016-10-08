using GL.Data.BLL;
using GL.Data.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GL.Instance
{
    public sealed class OnlineStaticHelper
    {


        public static void Create(string key, OnlineStatic value)
        {
            if (IsExist(key))
            {
                CacheManager.Instance.Update(key, value);
            }
            else
            {
                CacheManager.Instance.Create(key, value);
            }
        }
        public static void Update(string key, OnlineStatic value)
        {
            CacheManager.Instance.Update(key, value);
        }
        public static void Delete(string Key)
        {
            CacheManager.Instance.Delete(Key);
        }
        public static Hashtable GetList()
        {
            return CacheManager.Instance.ParamCache;
        }
        public static OnlineStatic Select(string key)
        {
            if (CacheManager.Instance.IsExist(key))
            {
                return (OnlineStatic)CacheManager.Instance.Select(key);
            }
            return new OnlineStatic() { Id = 0, LastTime = DateTime.Now };
        }
        public static bool IsExist(string key)
        {
            return CacheManager.Instance.IsExist(key);
        }



    }
}