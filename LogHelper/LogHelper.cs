namespace LogHelper
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Windows.Forms;
    using System.Web;

    public class LogHelper
    {
        private static string path;
        private static DateTime TimeSign;
        private static LogType type;
        private static StreamWriter writer;
        private static string formatPath = "~/log/";

        public LogHelper()
        {
            type = LogType.Daily;
        }

        public LogHelper(LogType t)
        {
            type = t;
        }

        private string GetFilename()
        {
            DateTime now = DateTime.Now;
            string format = "";
            switch (type)
            {
                case LogType.Daily:
                    TimeSign = new DateTime(now.Year, now.Month, now.Day);
                    TimeSign = TimeSign.AddDays(1.0);
                    format = "yyyyMMdd'.log'";
                    break;

                case LogType.Weekly:
                    TimeSign = new DateTime(now.Year, now.Month, now.Day);
                    TimeSign = TimeSign.AddDays(7.0);
                    format = "yyyyMMdd'.log'";
                    break;

                case LogType.Monthly:
                    TimeSign = new DateTime(now.Year, now.Month, 1);
                    TimeSign = TimeSign.AddMonths(1);
                    format = "yyyyMM'.log'";
                    break;

                case LogType.Annually:
                    TimeSign = new DateTime(now.Year, 1, 1);
                    TimeSign = TimeSign.AddYears(1);
                    format = "yyyy'.log'";
                    break;
            }
            return formatPath + now.ToString(format);
        }

        public void Write(Msg msg)
        {
            try
            {
                //StreamWriter writer = new StreamWriter(string.Format(@"c:\{1}", this.GetFilename()), true);
                StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath(this.GetFilename()), true);
                writer.BaseStream.Seek(0L, SeekOrigin.End);
                writer.WriteLine(string.Format("****类型:[{2}]\n*******\n{0}\n****{1}\n", msg.Datetime, msg.Text, msg.Type));
                writer.Flush();
                writer.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public void Write(SqlException e, MsgType type)
        {
            this.Write(new Msg(string.Format("Sql::Source:[{0}]\n StackTrace:[{1}]\n Message:[{2}]", e.Source, e.StackTrace, e.Message), type));
        }

        public void Write(Exception e, MsgType type)
        {
            this.Write(new Msg(string.Format("Ex::Source:[{0}]\n StackTrace:[{1}]\n Message:[{2}]", e.Source, e.StackTrace, e.Message), type));
        }

        public void Write(string text, MsgType type)
        {
            this.Write(new Msg(text, type));
        }

        public void Write(DateTime dt, string text, MsgType type)
        {
            this.Write(new Msg(dt, text, type));
        }



    }
}

