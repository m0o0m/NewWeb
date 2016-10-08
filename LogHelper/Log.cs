namespace LogHelper
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Web;

    public sealed class Log : IDisposable
    {
        private static Queue<Msg> msgs;
        private static string path;
        private static bool state;
        private static DateTime TimeSign;
        private static LogType type;
        private static StreamWriter writer;
        private static string formatPath = "~/log/";

        public Log() : this(LogType.Daily)
        {
        }

        public Log(LogType t)
        {
            if (msgs == null)
            {
                state = true;
                //path = string.Format(@"{0}\{1}", Application.StartupPath, this.GetFilename());

                if (Directory.Exists(HttpContext.Current.Server.MapPath(formatPath)) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(formatPath));
                }

                path = HttpContext.Current.Server.MapPath(this.GetFilename());
                type = t;
                msgs = new Queue<Msg>();
                new Thread(new ThreadStart(this.work)).Start();
            }
        }



        public void Dispose()
        {
            state = false;
        }

        private void FileClose()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
                writer = null;
            }
        }

        private void FileOpen()
        {
            writer = new StreamWriter(path, true, Encoding.UTF8);
        }

        private void FileWrite(Msg msg)
        {
            try
            {
                if (writer == null)
                {
                    this.FileOpen();
                }
                else
                {
                    if (DateTime.Now >= TimeSign)
                    {
                        this.FileClose();
                        this.FileOpen();
                    }
                    writer.Write(msg.Datetime);
                    writer.Write('\t');
                    writer.WriteLine(msg.Type);
                    writer.WriteLine(msg.Text);
                    writer.WriteLine();
                    writer.Flush();
                }
            }
            catch (Exception exception)
            {
                Console.Out.Write(exception);
            }
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

        private void work()
        {
            while (true)
            {
                if (msgs.Count > 0)
                {
                    Msg msg = null;
                    lock (msgs)
                    {
                        msg = msgs.Dequeue();
                    }
                    if (msg != null)
                    {
                        this.FileWrite(msg);
                    }
                }
                else if (state)
                {
                    Thread.Sleep(1);
                }
                else
                {
                    this.FileClose();
                }
            }
        }

        public void Write(Msg msg)
        {
            if (msg != null)
            {
                lock (msgs)
                {
                    msgs.Enqueue(msg);
                }
            }
        }



        public void Write(SqlException e, MsgType type)
        {
            this.Write(new Msg(string.Format("Sql::Source:[{0}]\n StackTrace:[{1}]\n Message:[{2}]\n", e.Source, e.StackTrace, e.Message), type));
        }

        public void Write(Exception e, MsgType type)
        {
            this.Write(new Msg(string.Format("Ex::Source:[{0}]\n StackTrace:[{1}]\n Message:[{2}]\n", e.Source, e.StackTrace, e.Message), type));
        }

        public void Error(Exception e)
        {
            this.Write(new Msg(string.Format("Ex::Source:[{0}]\n StackTrace:[{1}]\n Message:[{2}]\n", e.Source, e.StackTrace, e.Message), MsgType.Error));
        }

        public void Error(Exception e, string Url)
        {
            this.Write(new Msg(string.Format("Ex::Url:[{3}]\n Source:[{0}]\n StackTrace:[{1}]\n Message:[{2}]\n", e.Source, e.StackTrace, e.Message, Url), MsgType.Error));
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

