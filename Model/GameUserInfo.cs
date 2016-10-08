using System;
namespace GS.Model
{
	/// <summary>
	/// GameUserInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class GameUserInfo
	{
		public GameUserInfo()
		{}
		#region Model
		private int _guuserid;
		private int _guparentuserid;
		private int _guhighuserid0;
		private int _guhighuserid1;
		private int _guhighuserid2;
		private int _guhighuserid3;
		private int _guhighuserid4;
		private string _guaccount;
		private int _gulevel;
		private int _gustate;
		private string _gupasswd;
		private string _guname;
		private string _guemail;
		private int? _guidentifycode;
		private string _gutel;
		private int _gufaceid;
		private int _gustatecongealflag;
		private int _guuserright;
		private int _gumasterright;
		private decimal _gumescore;
		private decimal _gumidscore;
		private decimal _gulowscore;
		private decimal _guoccupancyrate;
		private bool _guoccupancyrate_noflag;
		private decimal _gulowoccupancyrate_max;
		private bool _gulowoccupancyrate_max_noflag;
		private decimal _gulowoccupancyrate_min;
		private bool _gulowoccupancyrate_min_noflag;
		private decimal _gutaxoccupancyrate=0M;
		private decimal _gurollbackrate;
		private int _gubetlimit;
		private long _guregistertime;
		private int _gulessusercount;
		private int _guextend_userright=0;
		private decimal _guextend_totalbetscore=0M;
		private string _guextend_realname;
		private string _guextend_idcardno;
		private long _guextend_birthday=1900-1-1;
		private int _guextend_sex=0;
		private string _guextend_address="北京市";
		private string _guextend_signature="好懒呀，还没有个人签名。";
		private decimal _guextend_money1=0M;
		private int _guextend_lottery1=0;
		private decimal _guextend_score2=0M;
		private decimal _guextend_money2=0M;
		private int _guextend_lottery2=0;
		private decimal _guextend_gametime=0M;
		private int _guextend_rechargeamount=0;
		private int _guextend_receivingdaily=0;
		private string _guextend_bandpasswd= "123123";
		/// <summary>
		/// 
		/// </summary>
		public int GUUserID
		{
			set{ _guuserid=value;}
			get{return _guuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUParentUserID
		{
			set{ _guparentuserid=value;}
			get{return _guparentuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUHighUserID0
		{
			set{ _guhighuserid0=value;}
			get{return _guhighuserid0;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUHighUserID1
		{
			set{ _guhighuserid1=value;}
			get{return _guhighuserid1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUHighUserID2
		{
			set{ _guhighuserid2=value;}
			get{return _guhighuserid2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUHighUserID3
		{
			set{ _guhighuserid3=value;}
			get{return _guhighuserid3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUHighUserID4
		{
			set{ _guhighuserid4=value;}
			get{return _guhighuserid4;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUAccount
		{
			set{ _guaccount=value;}
			get{return _guaccount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GULevel
		{
			set{ _gulevel=value;}
			get{return _gulevel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUState
		{
			set{ _gustate=value;}
			get{return _gustate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUPasswd
		{
			set{ _gupasswd=value;}
			get{return _gupasswd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUName
		{
			set{ _guname=value;}
			get{return _guname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUEmail
		{
			set{ _guemail=value;}
			get{return _guemail;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? GUIdentifyCode
		{
			set{ _guidentifycode=value;}
			get{return _guidentifycode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUTel
		{
			set{ _gutel=value;}
			get{return _gutel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUFaceID
		{
			set{ _gufaceid=value;}
			get{return _gufaceid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUStateCongealFlag
		{
			set{ _gustatecongealflag=value;}
			get{return _gustatecongealflag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUUserRight
		{
			set{ _guuserright=value;}
			get{return _guuserright;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUMasterRight
		{
			set{ _gumasterright=value;}
			get{return _gumasterright;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GUMeScore
		{
			set{ _gumescore=value;}
			get{return _gumescore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GUMidScore
		{
			set{ _gumidscore=value;}
			get{return _gumidscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GULowScore
		{
			set{ _gulowscore=value;}
			get{return _gulowscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GUOccupancyRate
		{
			set{ _guoccupancyrate=value;}
			get{return _guoccupancyrate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool GUOccupancyRate_NoFlag
		{
			set{ _guoccupancyrate_noflag=value;}
			get{return _guoccupancyrate_noflag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GULowOccupancyRate_Max
		{
			set{ _gulowoccupancyrate_max=value;}
			get{return _gulowoccupancyrate_max;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool GULowOccupancyRate_Max_NoFlag
		{
			set{ _gulowoccupancyrate_max_noflag=value;}
			get{return _gulowoccupancyrate_max_noflag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GULowOccupancyRate_Min
		{
			set{ _gulowoccupancyrate_min=value;}
			get{return _gulowoccupancyrate_min;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool GULowOccupancyRate_Min_NoFlag
		{
			set{ _gulowoccupancyrate_min_noflag=value;}
			get{return _gulowoccupancyrate_min_noflag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GUTaxOccupancyRate
		{
			set{ _gutaxoccupancyrate=value;}
			get{return _gutaxoccupancyrate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GURollbackRate
		{
			set{ _gurollbackrate=value;}
			get{return _gurollbackrate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUBetLimit
		{
			set{ _gubetlimit=value;}
			get{return _gubetlimit;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long GURegisterTime
		{
			set{ _guregistertime=value;}
			get{return _guregistertime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GULessUserCount
		{
			set{ _gulessusercount=value;}
			get{return _gulessusercount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUExtend_UserRight
		{
			set{ _guextend_userright=value;}
			get{return _guextend_userright;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GUExtend_TotalBetScore
		{
			set{ _guextend_totalbetscore=value;}
			get{return _guextend_totalbetscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUExtend_RealName
		{
			set{ _guextend_realname=value;}
			get{return _guextend_realname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUExtend_IDCardNo
		{
			set{ _guextend_idcardno=value;}
			get{return _guextend_idcardno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long GUExtend_Birthday
		{
			set{ _guextend_birthday=value;}
			get{return _guextend_birthday;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUExtend_Sex
		{
			set{ _guextend_sex=value;}
			get{return _guextend_sex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUExtend_Address
		{
			set{ _guextend_address=value;}
			get{return _guextend_address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUExtend_Signature
		{
			set{ _guextend_signature=value;}
			get{return _guextend_signature;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GUExtend_Money1
		{
			set{ _guextend_money1=value;}
			get{return _guextend_money1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUExtend_Lottery1
		{
			set{ _guextend_lottery1=value;}
			get{return _guextend_lottery1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GUExtend_Score2
		{
			set{ _guextend_score2=value;}
			get{return _guextend_score2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GUExtend_Money2
		{
			set{ _guextend_money2=value;}
			get{return _guextend_money2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUExtend_Lottery2
		{
			set{ _guextend_lottery2=value;}
			get{return _guextend_lottery2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal GUExtend_GameTime
		{
			set{ _guextend_gametime=value;}
			get{return _guextend_gametime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUExtend_RechargeAmount
		{
			set{ _guextend_rechargeamount=value;}
			get{return _guextend_rechargeamount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GUExtend_ReceivingDaily
		{
			set{ _guextend_receivingdaily=value;}
			get{return _guextend_receivingdaily;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GUExtend_BandPasswd
		{
			set{ _guextend_bandpasswd=value;}
			get{return _guextend_bandpasswd;}
		}
		#endregion Model

	}
}

