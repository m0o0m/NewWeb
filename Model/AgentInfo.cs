using System;
namespace GS.Model
{
	/// <summary>
	/// AgentInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class AgentInfo
	{
		public AgentInfo()
		{}
		#region Model
		private int _agentid;
		private string _agentaccount;
		private string _agentname;
		private string _agentpasswd;
		private string _agentqq;
		private string _agenttel;
		private string _agentemail;
		private decimal? _deposit;
		private decimal? _initialamount;
		private decimal? _amountavailable;
		private decimal? _havaamount;
		private int? _higherlevel;
		private int? _lowerlevel;
		private int? _agentstate;
		private int? _onlinestate;
		private DateTime? _registertime;
		private string _loginip;
		private DateTime? _logintime;
		private decimal? _recharge;
		private decimal? _drawing;
		private string _drawingpasswd;
		private int? _revenuemodel;
        private string _jurisdictionID;

        public string JurisdictionID
        {
            get { return _jurisdictionID; }
            set { _jurisdictionID = value; }
        }
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
		public string AgentAccount
		{
			set{ _agentaccount=value;}
			get{return _agentaccount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AgentName
		{
			set{ _agentname=value;}
			get{return _agentname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AgentPasswd
		{
			set{ _agentpasswd=value;}
			get{return _agentpasswd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AgentQQ
		{
			set{ _agentqq=value;}
			get{return _agentqq;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AgentTel
		{
			set{ _agenttel=value;}
			get{return _agenttel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AgentEmail
		{
			set{ _agentemail=value;}
			get{return _agentemail;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Deposit
		{
			set{ _deposit=value;}
			get{return _deposit;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? InitialAmount
		{
			set{ _initialamount=value;}
			get{return _initialamount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? AmountAvailable
		{
			set{ _amountavailable=value;}
			get{return _amountavailable;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? HavaAmount
		{
			set{ _havaamount=value;}
			get{return _havaamount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? HigherLevel
		{
			set{ _higherlevel=value;}
			get{return _higherlevel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? LowerLevel
		{
			set{ _lowerlevel=value;}
			get{return _lowerlevel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AgentState
		{
			set{ _agentstate=value;}
			get{return _agentstate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OnlineState
		{
			set{ _onlinestate=value;}
			get{return _onlinestate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? RegisterTime
		{
			set{ _registertime=value;}
			get{return _registertime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LoginIP
		{
			set{ _loginip=value;}
			get{return _loginip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? LoginTime
		{
			set{ _logintime=value;}
			get{return _logintime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Recharge
		{
			set{ _recharge=value;}
			get{return _recharge;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Drawing
		{
			set{ _drawing=value;}
			get{return _drawing;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DrawingPasswd
		{
			set{ _drawingpasswd=value;}
			get{return _drawingpasswd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RevenueModel
		{
			set{ _revenuemodel=value;}
			get{return _revenuemodel;}
		}
		#endregion Model

	}
}

