using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.IO;

namespace System
{
    public class SessionKey
    {
        /// <summary>
        /// 验证
        /// </summary>
        public const string Verification = "yanzhen";
        /// <summary>
        /// 用户资料
        /// </summary>
        public const string Login = "SigninUserInfo";
        /// <summary>
        /// 用户ID
        /// </summary>
        public const string UserId = "UserGuid";
        /// <summary>
        /// 错误提示缓存
        /// </summary>
        public const string ProError = "ProError";
        /// <summary>
        /// 编辑中的用户缓存
        /// </summary>
        public const string EditUser = "EditUser_{0}";

    }
    /// <summary> 
    /// SessionHelper类扩展类 
    /// </summary> 
    public static class SessionStateBaseHelper
    {
        public static T GetValue<T>(this HttpSessionStateBase session, string key, T valueIfNull = default(T))
        {
            object raw = session[key];
            if (raw == null)
                return valueIfNull;
            T value = (T)raw;
            if (value == null)
                return valueIfNull;
            return value;
        }

        public static void SetValue<T>(this HttpSessionStateBase session, string key, T ojb)
        {
            session.Remove(key);
            session.Add(key, ojb);
        }

        /// <summary> 
        /// 清楚session中信息 
        /// </summary> 
        public static void ClearSession<T>(this HttpSessionStateBase session, string Key)
        {
            session[Key] = default(T);
            session.Remove(Key);
            //SessionManager<T>.SetSessionObject(Key, default(T));
        }

        /// <summary> 
        /// 信息是否存在 
        /// </summary> 
        /// <returns></returns> 
        public static bool IsExist<T>(this HttpSessionStateBase session, string Key)
        {
            bool ret = false;
            T userInfo = session.GetValue<T>(Key);
            if (userInfo != null)
                ret = true;
            return ret;
        }

    }
    /// <summary> 
    /// SessionHelper类扩展类 
    /// </summary> 
    public static class SessionStateHelper
    {
        public static T GetValue<T>(this HttpSessionState session, string key, T valueIfNull = default(T))
        {
            object raw = session[key];
            if (raw == null)
                return valueIfNull;
            T value = (T)raw;
            if (value == null)
                return valueIfNull;
            return value;
        }

        public static void SetValue<T>(this HttpSessionState session, string key, T ojb)
        {
            session.Remove(key);
            session.Add(key, ojb);
        }

        /// <summary> 
        /// 清楚session中信息 
        /// </summary> 
        public static void ClearSession<T>(this HttpSessionState session, string Key)
        {
            session[Key] = default(T);
            session.Remove(Key);
            //SessionManager<T>.SetSessionObject(Key, default(T));
        }

        /// <summary> 
        /// 信息是否存在 
        /// </summary> 
        /// <returns></returns> 
        public static bool IsExist<T>(this HttpSessionState session, string Key)
        {
            bool ret = false;
            T userInfo = session.GetValue<T>(Key);
            if (userInfo != null)
                ret = true;
            return ret;
        }

    }

    public class ViewDataKey
    {
        /// <summary>
        /// 支付接口
        /// </summary>
        public const string PayInterface = "PayInterface";
    }
    /// <summary> 
    /// ViewDataHelper类扩展类 
    /// </summary> 
    public static class ViewDataHelper
    {
        public static T GetValue<T>(this ViewDataDictionary viewdata, string key, T valueIfNull = default(T))
        {
            object raw = viewdata[key];
            if (raw == null)
                return valueIfNull;
            T value = (T)raw;
            if (value == null)
                return valueIfNull;
            return value;
        }

        public static void SetValue<T>(this ViewDataDictionary viewdata, string key, T ojb)
        {
            viewdata.Remove(key);
            viewdata.Add(key, ojb);
        }

        /// <summary> 
        /// 清楚ViewData中信息 
        /// </summary> 
        public static void ClearViewData<T>(this ViewDataDictionary viewdata, string Key)
        {
            viewdata[Key] = default(T);
            viewdata.Remove(Key);
            //SessionManager<T>.SetSessionObject(Key, default(T));
        }

        /// <summary> 
        /// 信息是否存在 
        /// </summary> 
        /// <returns></returns> 
        public static bool IsExist<T>(this ViewDataDictionary viewdata, string Key)
        {
            bool ret = false;
            T userInfo = viewdata.GetValue<T>(Key);
            if (userInfo != null)
                ret = true;
            return ret;
        }
    }

    public class CookieKey
    {
        /// <summary>
        /// 返回链接
        /// </summary>
        public const string urlRedirect = "urlRedirect";
        /// <summary>
        /// 目标链接
        /// </summary>
        public const string JumpRedirect = "JumpRedirect";
        /// <summary>
        /// 用于存储 Forms 身份验证票证的 Cookie 名称。默认值是“.ASPXAUTH”
        /// </summary>
        public const string FormsCookieName = ".ASPXAUTH";
    }




    public static class ControllerExtensions
    {

        public static string PartialViewToString(this Controller controller)
        {
            return controller.PartialViewToString(null, null);
        }

        public static string PartialViewToString(this Controller controller, string viewName)
        {
            return controller.PartialViewToString(viewName, null);
        }

        public static string PartialViewToString(this Controller controller, object model)
        {
            return controller.PartialViewToString(null, model);
        }

        public static string PartialViewToString(this Controller controller, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            }
            controller.ViewData.Model = model;
            using (StringWriter stringWriter = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                return stringWriter.GetStringBuilder().ToString();
            }
        }
    }
    

}