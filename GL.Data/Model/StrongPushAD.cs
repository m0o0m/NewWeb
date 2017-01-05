using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class StrongPushAD
    {
        public int ID { get; set; }
        /// <summary>
        /// 系统选择（1IOS,2Android）
        /// </summary>
        public int Plat { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public int Agent { get; set; }
        /// <summary>
        /// 应用选择
        /// </summary>
        public int App { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 弹开方式（1每次打开弹出,2每天首次打开弹出）
        /// </summary>
        public int Type { get; set; }

        public string CreateTime { get; set; }

    }


    public class StrongPushADRecord
    {
        /// <summary>
        /// 系统选择（1IOS,2Android）
        /// </summary>
        public int Plat { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public int Agent { get; set; }
        /// <summary>
        /// 应用选择
        /// </summary>
        public int App { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// NewUrl
        /// </summary>
        public string NewUrl { get; set; }
        /// <summary>
        /// 弹开方式（1每次打开弹出,2每天首次打开弹出）
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 新的弹开方式（1每次打开弹出,2每天首次打开弹出）
        /// </summary>
        public int NewType { get; set; }

        public string Username { get; set; }
        public string CreateTime { get; set; }

      

    }


   
}
