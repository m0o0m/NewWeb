using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    /// <summary>
    /// 代理商库存表
    /// </summary>
    public class DailiKuCun
    {
        /// <summary>
        /// 代理商编号
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// 代理商所有的库存金币
        /// </summary>
        public Int64 Gold { get; set; }
    }

    /// <summary>
    /// 代理商扣钱的流水编号表
    /// </summary>
    public class DaiLiKuCunNo {
        /// <summary>
        /// 流水编号
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// 流水名称
        /// </summary>
        public string NoName { get; set; }
    }
    /// <summary>
    /// 代理商库存表
    /// </summary>
    public class DaiLiUsers {
        /// <summary>
        /// 代理商编号
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// 代理商的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// GameData代理商的数据库连接
        /// </summary>
        public string GameDataDBConnect { get; set; }
        /// <summary>
        /// Gserverinfo代理商的数据库连接
        /// </summary>
        public string GserverinfoDBConnect { get; set; }
        /// <summary>
        /// Record代理商的数据库连接
        /// </summary>
        public string RecordDBConnect { get; set; }
        /// <summary>
        /// 代理商的服务器地址
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// 代理商的服务器端口
        /// </summary>
        public int ServerPort { get; set; }
        /// <summary>
        /// 代理商游戏地址
        /// </summary>
        public string GameUrl { get; set; }
        /// <summary>
        /// 代理商的后台系统地址
        /// </summary>
        public string MWebUrl { get; set; }
    }

    public class S_Desc {
        public int Type { get; set; }

        public int Type_Id { get; set; }

        public string UserOper { get; set; }

        public int pType { get; set; }

        public int OrderNo { get; set; }

        public int IsCheck { get; set; }

    }

}
