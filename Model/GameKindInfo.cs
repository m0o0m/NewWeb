using System;
namespace GS.Model
{
	/// <summary>
	/// GameKindInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class GameKindInfo
	{
		public GameKindInfo()
		{}
		#region Model
		private int _kindid;
		private int _typeid;
		private string _kindname;
		private int _sortid;
		private bool _enable;
		private int _processtype;
		private int _maxversion;
		private int _tablecount;
		private int _cellscore;
		private int _highscore;
		private int _lessscore;
		private decimal _taxrate;
		private int _aiusercount=0;
		private int _ailevel=100;
		private int _betpool=100000;
		/// <summary>
		/// 
		/// </summary>
		public int KindID
		{
			set{ _kindid=value;}
			get{return _kindid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int TypeID
		{
			set{ _typeid=value;}
			get{return _typeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string KindName
		{
			set{ _kindname=value;}
			get{return _kindname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int SortID
		{
			set{ _sortid=value;}
			get{return _sortid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enable
		{
			set{ _enable=value;}
			get{return _enable;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ProcessType
		{
			set{ _processtype=value;}
			get{return _processtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int MaxVersion
		{
			set{ _maxversion=value;}
			get{return _maxversion;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int TableCount
		{
			set{ _tablecount=value;}
			get{return _tablecount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CellScore
		{
			set{ _cellscore=value;}
			get{return _cellscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int HighScore
		{
			set{ _highscore=value;}
			get{return _highscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int LessScore
		{
			set{ _lessscore=value;}
			get{return _lessscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal TaxRate
		{
			set{ _taxrate=value;}
			get{return _taxrate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AIUserCount
		{
			set{ _aiusercount=value;}
			get{return _aiusercount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int AILevel
		{
			set{ _ailevel=value;}
			get{return _ailevel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BetPool
		{
			set{ _betpool=value;}
			get{return _betpool;}
		}
		#endregion Model

	}
}

