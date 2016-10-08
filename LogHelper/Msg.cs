namespace LogHelper
{
    using System;

    public class Msg
    {
        private DateTime datetime;
        private string text;
        private MsgType type;

        public Msg() : this("", MsgType.Unknown)
        {
        }

        public Msg(string t, MsgType p) : this(DateTime.Now, t, p)
        {
        }

        public Msg(DateTime dt, string t, MsgType p)
        {
            this.datetime = dt;
            this.type = p;
            this.text = t;
        }

        public override string ToString()
        {
            return (this.datetime.ToString() + "\t" + this.text + "\n");
        }

        public DateTime Datetime
        {
            get
            {
                return this.datetime;
            }
            set
            {
                this.datetime = value;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        public MsgType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}

