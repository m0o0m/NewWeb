using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class RobotControl {
        //安全数据监控
        public RobotSystemStatu RobotSystemStatu { get; set; }
        //在线人数
        public RobotGameStatu RobotGameStatu { get; set; }
        public RobotPlayerLimit RobotPlayerLimit { get; set; }

        public RobotDown RobotDown { get; set; }
    }

    public class RobotDown {
        public bool Texas { get; set; }

        public bool ZFB { get; set; }

        public bool ZODIAC { get; set;}

        public bool CARS { get; set; }

        public bool TEXAS_EX { get; set; }

    }

    /// <summary>
    /// 安全数据监控
    /// </summary>
    public class RobotSystemStatu
    {
        /// <summary>
        /// cpu使用
        /// </summary>
        public string CPUPercent { get; set; }
        /// <summary>
        /// 程序内存占用
        /// </summary>
        public string MemUsed { get; set; }
        /// <summary>
        /// 总内存占用
        /// </summary>
        public string MemTotal { get; set; }
        /// <summary>
        /// 端口流量
        /// </summary>
        public string FlowRate { get; set; }
        /// <summary>
        /// 程序连接数
        /// </summary>
        public int ConnCnt { get; set; }
        /// <summary>
        /// 程序总连接数
        /// </summary>
        public int ConnLimit { get; set; }
    }
    /// <summary>
    /// 在线人数
    /// </summary>
    public class RobotGameStatu {
        /// <summary>
        /// 所有游戏在线人数和
        /// </summary>
       public double All { get; set; }
        /// <summary>
        /// 德州扑克在线人数
        /// </summary>
        public double Texas { get; set; }
        /// <summary>
        /// 中发白在线人数
        /// </summary>
        public double ZFB { get; set; }
        /// <summary>
        /// 十二生肖在线人数
        /// </summary>
        public double ZODIAC { get; set; }
        /// <summary>
        /// 奔驰宝马在线人数
        /// </summary>
        public double CARS { get; set; }
        /// <summary>
        /// 百人德州在线人数
        /// </summary>
        public double TEXAS_EX { get; set; }
    }

    public class RobotPlayerLimit
    {
        /// <summary>
        /// 所有游戏
        /// </summary>
        public List<uint> AllLimit { get; set; }
        /// <summary>
        /// 德州扑克
        /// </summary>
        public List<uint> TexasLimit { get; set; }
        /// <summary>
        /// 中发白
        /// </summary>
        public List<uint> ZFBLimit { get; set; }
        /// <summary>
        /// 十二生肖
        /// </summary>
        public List<uint> ZODIACLimit { get; set; }
        /// <summary>
        /// 奔驰宝马
        /// </summary>
        public List<uint> CARSLimit { get; set; }
        /// <summary>
        /// 百人德州
        /// </summary>
        public List<uint> TEXAS_EXLimit { get; set; }
    }
}
