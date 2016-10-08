using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Redis.Models
{
    public class RankListData
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int dwUserID { get; set; }
        /// <summary>
        /// vip等级
        /// </summary>
        public int vipGrade { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool isOnline { get; set; }
        /// <summary>
        /// 游戏id
        /// </summary>
        public int gameID { get; set; }
        /// <summary>
        /// 金币
        /// </summary>
        public double llValue { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string strRoleName { get; set; }
        /// <summary>
        /// 头像id
        /// </summary>
        public int dwFaceID { get; set; }
        /// <summary>
        /// 头像url
        /// </summary>
        public string strFaceUrl { get; set; }


    }

    public class RankModel {
        public string Name { get; set; }
        public string Value { get; set; }
        public int VipGrade { get; set; }
    }

    public class RankListResModel
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int dwType { get; set; }
        /// <summary>
        /// 列表数据
        /// </summary>
        public List<RankListData> list { get; set; }
        /// <summary>
        /// 实际显示的数量
        /// </summary>
        public int showNum { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage { get; set; }
    }
}