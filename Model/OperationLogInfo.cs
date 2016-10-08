using System;
namespace GS.Model
{
	/// <summary>
	/// OperationLogInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class OperationLogInfo
	{
		public OperationLogInfo()
		{}
		#region Model
		private int _id;
		private int? _operateid;
		private int? _operatetype;
		private DateTime? _operatetime;
		private string _remark;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OperateID
		{
			set{ _operateid=value;}
			get{return _operateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OperateType
		{
			set{ _operatetype=value;}
			get{return _operatetype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? OperateTime
		{
			set{ _operatetime=value;}
			get{return _operatetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

