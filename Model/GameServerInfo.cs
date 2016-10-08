using System;
namespace GS.Model
{
	/// <summary>
	/// GameServerInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class GameServerInfo
	{
		public GameServerInfo()
		{}
		#region Model
		private int _serverid;
		private int _kindid;
		private int _typeid;
		private string _servername;
		private int _sortid;
		private bool _enable;
		private int _maxusercount;
		/// <summary>
		/// 
		/// </summary>
		public int ServerID
		{
			set{ _serverid=value;}
			get{return _serverid;}
		}
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
		public string ServerName
		{
			set{ _servername=value;}
			get{return _servername;}
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
		public int MaxUserCount
		{
			set{ _maxusercount=value;}
			get{return _maxusercount;}
		}
		#endregion Model

	}
}

