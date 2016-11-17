using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;
using System.Data;
using Webdiyer.WebControls.Mvc;
using GL.Common;

namespace GL.Data.DAL
{
    public class WaterMarginDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static IEnumerable<WaterMargin> GetWaterMarginList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from "+ database3 + @".cfg_slotconfig;
                ;");

                IEnumerable<WaterMargin> i = cn.Query<WaterMargin>(str.ToString());
                cn.Close();
                return i;
            }
        }


        public static IEnumerable<SSwitch> GetSSwitchList(string id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from "+ database1+ @".S_Switch
where id in ( "+id+@")
                ;");

                IEnumerable<SSwitch> i = cn.Query<SSwitch>(str.ToString());
                cn.Close();
                return i;
            }
        }



        internal static int UpdateSSwitch(List<SSwitch> models)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                //奖池para2不修改，采用另外一种方式
                foreach (SSwitch item in models)
                {
                    str.AppendFormat(@"
update "+ database1+ @".S_Switch set ISOpen={0},para1={1},para6={3}
where id ={4};",
item.ISOpen, item.para1, item.para2, item.para6,
item.ID
);
                }
                int i = cn.Execute(str.ToString());
                cn.Close();
                return i;
            }
        }


        public static IEnumerable<ArcadeGameStock> GetArcadeGameStockList(string id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from "+ database1 + @".ArcadeGameStock
where id in ( " + id + @")
                ;");

                IEnumerable<ArcadeGameStock> i = cn.Query<ArcadeGameStock>(str.ToString());
                cn.Close();
                return i;
            }
        }



        internal static int UpdateArcadeGameStock(List<ArcadeGameStock> models)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                foreach (ArcadeGameStock item in models)
                {
                    str.AppendFormat(@"
update "+ database1 + @".ArcadeGameStock set
StockCordon={1},
StockIsOpen={2},
Param1={3},
Param2={4},
Param3={5},
Param4={6},
Param5={7},
Param6={8},
Param7={9},
CreateTime = '{10}'
where id ={11};",
item.StockValue, item.StockCordon, item.StockIsOpen, 
item.Param1, item.Param2, item.Param3, item.Param4, item.Param5, item.Param6, item.Param7,
item.CreateTime,item.ID
);
                }
                int i = cn.Execute(str.ToString());
                cn.Close();
                return i;
            }
        }



        internal static int UpdateWatermargin(List<WaterMargin> models)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                foreach (WaterMargin item in models)
                {
                    str.AppendFormat(@"
update "+ database3+ @".cfg_slotconfig set Hatchet={0},Gun={1},Knife={2},Lu={3},Lin={4},Song={5},God={6},Hall={7},Outlaws={8},Wine={9},CreateTime='{10}'
where ColumnNo ={11} and Type={12}
;",
item.Hatchet,item.Gun,item.Knife,item.Lu,item.Lin,item.Song,item.God,item.Hall,item.Outlaws,item.Wine,item.CreateTime,
item.ColumnNO,item.Type
);
                }
               int  i = cn.Execute(str.ToString());
                cn.Close();
                return i;
            }
        }


        public static MarryConfig GetMarryConfig(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from "+ database3+ @".cfg_maryconfig where id = @id;
                ;");

                IEnumerable<MarryConfig> i = cn.Query<MarryConfig>(str.ToString(), new {
                    id = id
                });
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        public static Int64 GetHuiShou(string createtime) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                //204
                str.AppendFormat(@"  
select sum(chip) as Obj from "+ database3 + @".Clearing_UserMoneyRecord where recordtype = 204 and recordTime = '" + createtime + @"'
                ;");

                IEnumerable<SingleData> i = cn.Query<SingleData>(str.ToString());
                cn.Close();
                SingleData d = i.FirstOrDefault();
                if (d == null)
                {
                    return 0;
                }
                else {
                    return Convert.ToInt64( i.FirstOrDefault().Obj);
                }
              
            }
        }



        public static Int64 GetComHuiShou(string createtime,int recordType)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                //204
                str.AppendFormat(@"  
select sum(chip) as Obj from "+ database3 + @".Clearing_UserMoneyRecord where recordtype = " + recordType + " and recordTime = '" + createtime + @"'
                ;");

                IEnumerable<SingleData> i = cn.Query<SingleData>(str.ToString());
                cn.Close();
                SingleData d = i.FirstOrDefault();
                if (d == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt64(i.FirstOrDefault().Obj);
                }

            }
        }



        public static IEnumerable<BiBeiConfig> GetBiBeiConfig()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from "+ database3 + @".config_bibei
                ;");

                IEnumerable<BiBeiConfig> i = cn.Query<BiBeiConfig>(str.ToString());
                cn.Close();
                return i;
            }
        }


        /// <summary>
        /// 修改小玛丽配置
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        internal static int UpdateMary(int id,Int64 maryUPLimit,Int64 maryDownLimit, Int64 bibei1, Int64 bibei2, Int64 bibei3, Int64 bibei4, Int64 bibei5, Int64 bibein)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

              
                str.AppendFormat(@"
update "+ database3 + @".cfg_maryconfig set Lowerlimit=@Lowerlimit,Uperlimit=@Uperlimit,CreateTime=@CreateTime
where id = @id;
update "+ database3 + @".config_bibei set GapWeight = @bibei1 where GameRound = 0;
update "+ database3 + @".config_bibei set GapWeight = @bibei2 where GameRound = 1;
update "+ database3 + @".config_bibei set GapWeight = @bibei3 where GameRound = 2;
update "+ database3 + @".config_bibei set GapWeight = @bibei4 where GameRound = 3;
update "+ database3 + @".config_bibei set GapWeight = @bibei5 where GameRound = 4;
update "+ database3 + @".config_bibei set GapWeight = @bibein where GameRound = 5;
"
);
             
                int i = cn.Execute(str.ToString(), new {
                    id = id,
                    Uperlimit = maryUPLimit,
                    Lowerlimit = maryDownLimit,
                    CreateTime = DateTime.Now.ToString(),
                    bibei1 = bibei1,
                    bibei2 = bibei2,
                    bibei3 = bibei3,
                    bibei4 = bibei4,
                    bibei5 = bibei5,
                    bibein = bibein
                });
                cn.Close();
                return i;
            }
        }

    }
}
