using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.View
{
    public class GameRecordView
    {

        public long UserID { get; set; }
        public int Page { get; set; }
        public string StartDate { get; set; }
        public string ExpirationDate { get; set; }
        public object DataList { get; set; }
        public object Data { get; set; }
        public seachType SeachType { get; set; }
        public int commandID { get; set; }
        public int Channels { get; set; }
        public string UserList { get; set; }

        public int ItemID { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public string SearchExt { get; set; }
        public int SearchExter { get; set; }
        public int Gametype { get; set; }

        public string PageList { get; set; }

        public int UserType { get; set; }


        public string GametypeS { get; set; }

    }


}
