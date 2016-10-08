using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class BoardDetail_Left
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 总牌局数
        /// </summary>
        public double TotalBoard { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public double TotalCount { get; set; }
        /// <summary>
        /// 总回收
        /// </summary>
        public double TotalRecive { get; set; }

    }

    public class BoardDetail_Right {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 游戏ID
        /// </summary>
        public int GameID { get; set; }
        /// <summary>
        /// 房间分类
        /// </summary>
        public string RoomCategory { get; set; }
        /// <summary>
        /// 牌局总数
        /// </summary>
        public double BoardNum { get; set; }
        /// <summary>
        /// 玩牌人数
        /// </summary>
        public double BoardCount { get; set; }
        /// <summary>
        /// 玩牌率
        /// </summary>
        public double BoardLv { get; set; }
        /// <summary>
        /// 牌局比例
        /// </summary>
        public double BoardRate { get; set; }
        /// <summary>
        /// 人均牌局数
        /// </summary>
        public double BoardNumPer { get; set; }
        /// <summary>
        /// 人均牌局时长
        /// </summary>
        public double BoardTimePerMan { get; set; }
        /// <summary>
        /// 牌局平均时长
        /// </summary>
        public double BoardTimePer { get; set; }
        /// <summary>
        /// 牌局平均流通
        /// </summary>
        public double BoardFlowPer { get; set; }
        /// <summary>
        /// 回收
        /// </summary>
        public double CallBack { get; set; }
        /// <summary>
        /// 牌局总时长
        /// </summary>
        public double BoardTime { get; set; }

    }
}
