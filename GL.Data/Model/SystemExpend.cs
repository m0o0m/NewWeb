using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.Model
{
    public class SystemExpend
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string RecordTime { get; set; }
        /// <summary>
        /// 系统消耗
        /// </summary>
        public string Chip { get; set; }
        /// <summary>
        /// 中发白消耗
        /// </summary>
        public string ChipScale { get; set; }
        /// <summary>
        /// 十二生肖消耗
        /// </summary>
        public string ChipScale1 { get; set; }
        /// <summary>
        /// 奔驰宝马消耗
        /// </summary>
        public string ChipCar { get; set; }
        /// <summary>
        /// 小马快跑消耗
        /// </summary>
        public string ChipHorse { get; set; }
        /// <summary>
        /// 德州扑克消耗
        /// </summary>
        public string ChipTexas { get; set; }
        /// <summary>
        /// 拼手牌消耗
        /// </summary>
        public string ChipSpell { get; set; }
        /// <summary>
        /// 互动道具消耗
        /// </summary>
        public string ChipTools { get; set; }
        /// <summary>
        /// 购买礼物消耗
        /// </summary>
        public string ChipGift { get; set; }
        /// <summary>
        /// 喇叭消耗
        /// </summary>
        public string ChipLoud { get; set; }

    }
}
