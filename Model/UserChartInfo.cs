using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Model
{
    /// <summary>
    /// UserChartInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UserChartInfo
    {
        public UserChartInfo()
        { }
        #region Model
        private long _ucid;
        private long _ucgameroundid;
        private int _ucuserid;
        private int _ucchairid;
        private decimal _uctotalbetscore;
        private decimal _uctotalwinscore;
        private decimal _uctotaltaxscore = 0M;
        private decimal _ucwinscoreoccupancy_high;
        private decimal _ucwinscoreoccupancy_self;
        private decimal _ucwinscoreoccupancy_less;
        private decimal _uctaxscoreoccupancy_high = 0M;
        private decimal _uctaxscoreoccupancy_self = 0M;
        private decimal _uctaxscoreoccupancy_less = 0M;
        private decimal _ucvalidbetscore_total;
        private decimal _ucvalidbetscore_high;
        private decimal _ucvalidbetscore_highrollback;
        private decimal _ucvalidbetscore_less;
        private decimal _ucvalidbetscore_lessrollback;
        private decimal _ucpaidscore_high;
        private decimal _ucpaidscore_less;
        private byte[] _ucdetailbetscore;
        /// <summary>
        /// 
        /// </summary>
        public long UCID
        {
            set { _ucid = value; }
            get { return _ucid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long UCGameRoundID
        {
            set { _ucgameroundid = value; }
            get { return _ucgameroundid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UCUserID
        {
            set { _ucuserid = value; }
            get { return _ucuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UCChairID
        {
            set { _ucchairid = value; }
            get { return _ucchairid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCTotalBetScore
        {
            set { _uctotalbetscore = value; }
            get { return _uctotalbetscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCTotalWinScore
        {
            set { _uctotalwinscore = value; }
            get { return _uctotalwinscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCTotalTaxScore
        {
            set { _uctotaltaxscore = value; }
            get { return _uctotaltaxscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCWinScoreOccupancy_High
        {
            set { _ucwinscoreoccupancy_high = value; }
            get { return _ucwinscoreoccupancy_high; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCWinScoreOccupancy_Self
        {
            set { _ucwinscoreoccupancy_self = value; }
            get { return _ucwinscoreoccupancy_self; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCWinScoreOccupancy_Less
        {
            set { _ucwinscoreoccupancy_less = value; }
            get { return _ucwinscoreoccupancy_less; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCTaxScoreOccupancy_High
        {
            set { _uctaxscoreoccupancy_high = value; }
            get { return _uctaxscoreoccupancy_high; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCTaxScoreOccupancy_Self
        {
            set { _uctaxscoreoccupancy_self = value; }
            get { return _uctaxscoreoccupancy_self; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCTaxScoreOccupancy_Less
        {
            set { _uctaxscoreoccupancy_less = value; }
            get { return _uctaxscoreoccupancy_less; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCValidBetScore_Total
        {
            set { _ucvalidbetscore_total = value; }
            get { return _ucvalidbetscore_total; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCValidBetScore_High
        {
            set { _ucvalidbetscore_high = value; }
            get { return _ucvalidbetscore_high; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCValidBetScore_HighRollback
        {
            set { _ucvalidbetscore_highrollback = value; }
            get { return _ucvalidbetscore_highrollback; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCValidBetScore_Less
        {
            set { _ucvalidbetscore_less = value; }
            get { return _ucvalidbetscore_less; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCValidBetScore_LessRollback
        {
            set { _ucvalidbetscore_lessrollback = value; }
            get { return _ucvalidbetscore_lessrollback; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCPaidScore_High
        {
            set { _ucpaidscore_high = value; }
            get { return _ucpaidscore_high; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal UCPaidScore_Less
        {
            set { _ucpaidscore_less = value; }
            get { return _ucpaidscore_less; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] UCDetailBetScore
        {
            set { _ucdetailbetscore = value; }
            get { return _ucdetailbetscore; }
        }
        #endregion Model

    }
}
