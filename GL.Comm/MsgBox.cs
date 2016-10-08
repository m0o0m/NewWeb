using System;
using System.Web;
using System.Web.UI;

namespace GL.Common
{
    public class MsgBox
    {
        public static string Alert(string _Msg)
        {
            string s = "<script language=javascript>";
            s = (s + "alert('" + _Msg + "');") + "</script>";
            return s;
        }

        public static string Alert(string _Msg, bool b)
        {
            string s = "<script language=javascript>";
            s = s + "alert('" + _Msg + "');";
            if (b)
            {
                s = s + "window.close();";
            }
            s = s + "</script>";
            return s;
        }

        public static string Alert(string _Msg, string URL)
        {
            string s = "<script language=javascript>";
            s = ((s + "alert('" + _Msg + "');") + "window.location='" + URL + "';") + "</script>";
            return s;
        }

        public static string Alert_Close(string _Msg)
        {
            string s = "<script language=javascript>";
            s = (s + "alert('" + _Msg + "');") + "window.close();" + "</script>";
            return s;
        }

        public static string Alert_History(string _Msg, int BackLong)
        {
            string s = "<script language=javascript>";
            object obj2 = s + "alert('" + _Msg + "');";
            s = string.Concat(new object[] { obj2, "history.go('", BackLong, "')" }) + "</script>";
            return s;
        }

        public static string Alert_ReloadWin(string _Msg)
        {
            string s = "<script language=javascript>";
            s = (s + "alert('" + _Msg + "');") + "window.opener.location.href=window.opener.location.href;window.close();" + "</script>";
            return s;
        }


        public static string Confirm(string _Msg, string URL)
        {
            string s = "<script language=javascript>";
            string str2 = s;
            s = (str2 + "var retValue=window.confirm('" + _Msg + "');if(retValue){window.location='" + URL + "';}") + "</script>";
            return s;
        }


    }

}
