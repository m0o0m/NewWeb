using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Model
{
    /// <summary>
    /// GameRoundInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class GameRoundInfo
    {
        public GameRoundInfo()
        { }
        #region Model
        private long _grid;
        private long _grstarttime;
        private int _grcalculatedflag;
        private int _grvalidflag;
        private int _grserverid;
        private int _grtableid;
        private int _grendreason;
        private byte[] _grenddata;
        /// <summary>
        /// 
        /// </summary>
        public long GRID
        {
            set { _grid = value; }
            get { return _grid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long GRStartTime
        {
            set { _grstarttime = value; }
            get { return _grstarttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GRCalculatedFlag
        {
            set { _grcalculatedflag = value; }
            get { return _grcalculatedflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GRValidFlag
        {
            set { _grvalidflag = value; }
            get { return _grvalidflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GRServerID
        {
            set { _grserverid = value; }
            get { return _grserverid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GRTableID
        {
            set { _grtableid = value; }
            get { return _grtableid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int GREndReason
        {
            set { _grendreason = value; }
            get { return _grendreason; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] GREndData
        {
            set { _grenddata = value; }
            get { return _grenddata; }
        }
        #endregion Model

    }
}
