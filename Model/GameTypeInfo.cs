using System;
namespace GS.Model
{
	/// <summary>
	/// GameTypeInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class GameTypeInfo
	{
		public GameTypeInfo()
		{}
		#region Model
		private int _typeid;
		private int _imageid;
		private string _typename;
		private int _sortid;
		private bool _enable;
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
		public int ImageID
		{
			set{ _imageid=value;}
			get{return _imageid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TypeName
		{
			set{ _typename=value;}
			get{return _typename;}
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
		#endregion Model

	}
}

