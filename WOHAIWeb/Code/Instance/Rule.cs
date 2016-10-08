using GL.Data.Model;
using GL.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WOHAIWeb.Code.Instance
{
    public class Rule
    {
        private static HttpSessionState Session
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }

        internal static void Create(ManagerInfo cust)
        {

            Session.SetValue<int>(SessionKey.UserId, cust.AdminID);
            AuthRules.SetAuthCookie(new SessionData { UserID = cust.AdminID.ToString(), SessionID = Session.SessionID });
            OnlineStaticHelper.Create(Session.SessionID, new OnlineStatic { Id = cust.AdminID, LastTime = DateTime.Now });

            //OnlineStaticHelper.Create(Session.SessionID, new OnlineStatic { Id = cust.AdminID, LastTime = DateTime.Now });
        }

        internal static OnlineStatic Select()
        {
            

            //return OnlineStaticHelper.Select(Session.SessionID);
            return OnlineStaticHelper.Select(Session.SessionID);
        }

        internal static void Action()
        {
            OnlineStatic os = OnlineStaticHelper.Select(Session.SessionID);
            os.LastTime = DateTime.Now;
            OnlineStaticHelper.Update(Session.SessionID, os);
        }
        internal static void Dispose()
        {
            //OnlineStatic os = OnlineStaticHelper.Select(Session.SessionID);
            //os.LastTime = DateTime.Now;
            //OnlineStaticHelper.Update(Session.SessionID, os);

            OnlineStaticHelper.Delete(Session.SessionID);
            AuthRules.SignOut();
            Session.Abandon();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));



        }


    }

    public class AuthRules
    {
        public static void SetAuthCookie(SessionData sd)
        {
            SetAuthCookie(sd, false, FormsAuthentication.FormsCookiePath);
        }
        public static void SetAuthCookie(SessionData sd, bool createPersistentCookie)
        {
            SetAuthCookie(sd, createPersistentCookie, FormsAuthentication.FormsCookiePath);
        }

        public static void SetAuthCookie(SessionData sd, bool createPersistentCookie, string strCookiePath)
        {
            FormsAuthentication.SetAuthCookie(sd.UserID, createPersistentCookie, strCookiePath);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, sd.UserID, DateTime.Now, DateTime.Now.AddDays(1), createPersistentCookie, sd.SessionID);
            FormsIdentity identity = new FormsIdentity(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            HttpContext.Current.Response.Cookies.Add(cookie);

        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }

    }

    public class SessionData
    {
        public string UserID { get; set; }
        public string SessionID { get; set; }
    }



}