using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;


namespace GL.Data.DAL
{
    public class RoleDAL
    {
        public static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");
        public static readonly string sqlconnectionString2 = PubConstant.GetConnectionString("ConnectionString");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");


        internal static Role GetModelByID(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open(); 
                IEnumerable<Role> i = cn.Query<Role>(@"select * from Role where ID = @ID", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        internal static int UpdateFreezeNoSpeak(int userid) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"
update "+ database1 + @".Role set IsFreeze = 0  where ID = @ID and FreezeTime <='"+DateTime.Now+@"';
update "+ database1 + @".Role set NoSpeak = 0  where ID = @ID and SpeakTime <='"+DateTime.Now+@"';
", 
                    new Role { ID = userid});
                cn.Close();
                return i;
            }
        }

        internal static Role GetRoleByID(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"
                    select * from (
                    select a.* ,if(ifnull(b.IP ,'0') = '0' ,0 ,1) SwitchIP ,if(ifnull(c.Mac ,'0') = '0' ,0 ,1) SwitchMac ,d.IP LastLoginIP ,ifnull(t.agentname ,'默认渠道') AgentName ,ifnull(r.NextMoney ,0) lastMoney
                    from (select * from Role where ID = @ID ) a
                        left join "+ database1 + @".RoleOrder r on a.id = r.id
                        left join "+ database2+ @".AgentUsers t on a.agent =t.id
                        left join BG_BanIPList b on a.CreateIP = b.IP
                        left join BG_BanMacList c on a.CreateMac = c.Mac
                        left join BG_LoginRecord d on a.id = d.userid order by d.LoginTime desc limit 1)t;", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }



        internal static Role GetVRoleByID(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"
                    select * from (
                    select d.AgentName as LoginAgent, a.* ,if(ifnull(b.IP ,'0') = '0' ,0 ,1) SwitchIP ,if(ifnull(c.Mac ,'0') = '0' ,0 ,1) SwitchMac ,d.IP LastLoginIP ,ifnull(t.agentname ,'默认渠道') AgentName ,ifnull(r.NextMoney ,0) lastMoney
                    from (select * from Role where ID = @ID ) a
                        left join "+ database1 + @".RoleOrder r on a.id = r.id
                        left join "+ database2+ @".AgentUsers t on a.agent =t.id
                        left join BG_BanIPList b on a.CreateIP = b.IP
                        left join BG_BanMacList c on a.CreateMac = c.Mac
                        left join V_LoginRecord d on a.id = d.userid order by d.LoginTime desc limit 1)t;", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static Role GetGiftByID(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"
                    select ifnull(sum(case when (date_add(Createtime ,interval s.AttrDays day) >= now()) then 1 else 0 end) ,0) Gift
                        ,ifnull(sum(case when (date_add(Createtime ,interval s.AttrDays day) >= now()) then 0 else 1 end) ,0) GiftExpire
                    from S_Template s 
                        join Gift t on s.TempalteID = t.Static and t.id = @ID;", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        

        internal static Role GetModelByOpenID(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"select ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP, NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze from Role where OpenID = @OpenID", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }
        
        internal static IEnumerable<Role> GetModelByIDList(string UserID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"select ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP, NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac, ClubID from Role where find_in_set(Id, @ID) ", new { ID = UserID });
                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<Role> GetModelByIDs(string UserID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"select ID, Account, Email, Tel, TrueName, Identity, Agent, Password, Gold, Diamond, Zicard, Telfare, MaxNoble, ShowGift, NickName, Gender, Country, Province, City, FigureUrl, IsYellowVip, IsYellowVipYear, YellowVipLevel, IsYellowHighVip, PF, OpenID, IOpenID, Invkey, Itime, LoginDevice, LastModify, CreateTime, CreateIP, NoSpeak, (CASE  WHEN IsFreeze=0  THEN 0 when IsFreeze>=1 then 1  ELSE IsFreeze END) as IsFreeze, SafeBox, SafePwd, CreateMac, ClubID from Role where Id in ( "+ UserID + ") ", new { ID = UserID });
                cn.Close();
                return i;
            }
        }


        public static IEnumerable<Role> GetMasterLevelModels()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"select ID, Account from Role where MasterLevel!=0 ");
                cn.Close();
                return i;
            }
        }



        internal static Role GetModelByAcc(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"select ID, FigureUrl from Role where Account = @Account", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        internal static long GetIdByIdOrAccoOrNName(string fieldValue) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<Role> i = cn.Query<Role>(@"select ID  from Role where Account = @Account or ID=@Account or NickName=@Account", new Role { Account=fieldValue});
                cn.Close();
                Role r = i.FirstOrDefault();
                if (r != null)
                {
                    return r.ID;
                }
                else {
                    return 0;
                }
            }
          
        }


        internal static int UpdateIsFreeze(string ip, isSwitch isFreeze)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update Role set IsFreeze = @IsFreeze where CreateIP = @CreateIP", new Role { CreateIP = ip, IsFreeze = isFreeze });
                cn.Close();
                return i;
            }
        }


        internal static string GetGiveAcc(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat("SELECT Account from Role where ID = {0}", id);

                object i = cn.ExecuteScalar(str.ToString());


                cn.Close();
                return (i ?? string.Empty).ToString();
            }
        }

        internal static int UpdateRole(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update "+ database1 + @".Role set ExtInfo = @ExtInfo where id = @ID ;", model);
                cn.Close();
                return i; 
            }
        }
        internal static int UpdateRoleNoSpeak(int nospeak, DateTime speakTime, int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update "+ database1 + @".Role set NoSpeak = @NoSpeak,SpeakTime=@SpeakTime  where id = @ID ;", new
                {
                    NoSpeak = nospeak,
                    SpeakTime = speakTime,
                    ID = id
                });
                cn.Close();
                return i;
            }
        }
        internal static int UpdateRolePwd(string pwd, int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update "+ database1 + @".Role set SafePwd = @Password  where id = @ID ;", new
                {
                    Password = pwd,
                    ID = id
                });
                cn.Close();
                return i;
            }
        }


        internal static int UpdateRoleSafePwd(string pwd, int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update "+ database1 + @".Role set SafePwd = @Password  where id = @ID ;", new
                {
                    Password = pwd,
                    ID = id
                });
                cn.Close();
                return i;
            }
        }


        internal static int UpdateRoleNoFreeze(int isfreeze, DateTime freezeTime, int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update "+ database1 + @".Role set IsFreeze = @IsFreeze,FreezeTime=@FreezeTime  where id = @ID ;", new
                {
                    IsFreeze = isfreeze,
                    FreezeTime = freezeTime,
                    ID = id
                });
                cn.Close();
                return i;
            }
        }
        internal static int UpdateRoleClub(Role model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update "+ database1 + @".Role left join ClubInfo b on b.id = @ClubID  set Clubid = ifnull(b.id ,Clubid) where "+ database1 + @".Role.id = @ID ;
                            replace into ClubUser(userid ,ClubID ,AgentID ,NickName ,CreateTime) 
                            select @ID ,@ClubID ,@Agent ,@NickName ,now() from ClubInfo where id = @ClubID;"
                    , model);
                cn.Close();
                return i;
            }
        }
        
        internal static int AddMD5Flow(GL.Data.Model.MD5Flow model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString2))
            {
                cn.Open();
                int i = cn.Execute(@"INSERT INTO MD5Flow (userid,md5,CreateTime,Category) VALUES 
(@userid, @md5, @CreateTime, @Category)", model);
                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<MD5Flow> GetMD5Flow(MD5Flow model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString2))
            {
                cn.Open();
                IEnumerable<MD5Flow> i = cn.Query<MD5Flow>(@"select * from MD5Flow where userid=@userid and md5=@md5 ", model);
                cn.Close();
                return i;
            }
        }



    }
}
