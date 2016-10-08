using System;
namespace GS.Model
{
	/// <summary>
	/// ManagerInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ManagerInfo
	{
		public ManagerInfo()
		{}
		#region Model
		private int _adminid;
		private string _adminaccount;
		private string _adminname;
		private string _adminpasswd;
		private DateTime? _operdate;
		private int? _adminmasterright;
		/// <summary>
		/// 
		/// </summary>
		public int AdminID
		{
			set{ _adminid=value;}
			get{return _adminid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdminAccount
		{
			set{ _adminaccount=value;}
			get{return _adminaccount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdminName
		{
			set{ _adminname=value;}
			get{return _adminname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdminPasswd
		{
			set{ _adminpasswd=value;}
			get{return _adminpasswd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? OperDate
		{
			set{ _operdate=value;}
			get{return _operdate ?? DateTime.Now;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AdminMasterRight
		{
			set{ _adminmasterright=value;}
			get{return _adminmasterright ?? 0;}
		}
		#endregion Model

	}
}

