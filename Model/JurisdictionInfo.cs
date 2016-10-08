using System;
namespace GS.Model
{
	/// <summary>
	/// JurisdictionInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class JurisdictionInfo
	{
		public JurisdictionInfo()
		{}
		#region Model
		private int _jurisdictionid;
		private string _jurisdictionname;
		private int? _state;
		/// <summary>
		/// 
		/// </summary>
		public int JurisdictionID
		{
			set{ _jurisdictionid=value;}
			get{return _jurisdictionid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string JurisdictionName
		{
			set{ _jurisdictionname=value;}
			get{return _jurisdictionname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

