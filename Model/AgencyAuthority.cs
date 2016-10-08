using System;
namespace GS.Model
{
	/// <summary>
	/// AgencyAuthority:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class AgencyAuthority
	{
		public AgencyAuthority()
		{}
		#region Model
		private int _agentid;
		private int _jurisdictionid;
		/// <summary>
		/// 
		/// </summary>
		public int AgentID
		{
			set{ _agentid=value;}
			get{return _agentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int JurisdictionID
		{
			set{ _jurisdictionid=value;}
			get{return _jurisdictionid;}
		}
		#endregion Model

	}
}

