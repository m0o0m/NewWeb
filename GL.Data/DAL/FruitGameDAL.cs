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
    public class FruitGameDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("ConnectionStringForGameData");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");

        internal static IEnumerable<FruitGameExplodeConfig> GetExplodeConfig()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from "+ database3 + @".fg_explodeconfig;
                ;");

                IEnumerable<FruitGameExplodeConfig> i = cn.Query<FruitGameExplodeConfig>(str.ToString());
                cn.Close();
                return i;
            }

        }

        public static IEnumerable<FruitPotConfig> GetFruitPotConfig(string id,int gameID)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from "+ database1+ @".FruiteBigPot where id in ("+id+@") and gameid = "+gameID+@"
                ;");

                IEnumerable<FruitPotConfig> i = cn.Query<FruitPotConfig>(str.ToString());
                cn.Close();
                return i;
            }
        }



        internal static IEnumerable<FruitBibeiConfig> GetBiBeiConfig()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
select * from "+ database3 + @".fg_config_bibei;
                ;");

                IEnumerable<FruitBibeiConfig> i = cn.Query<FruitBibeiConfig>(str.ToString());
                cn.Close();
                return i;
            }
        }



        internal static int UpdateExplodeConfig(FruitGameExplodeConfig models)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


               
                    str.AppendFormat(@"
update "+ database3 + @".fg_explodeconfig set 
SmallApple={0},SmallOrange={1},SmallMango={2},SmallRing={3},
SmallWatermalon={4},SmallDoubleStar={5},SmallDoubleSeven={6},Apple={7},
Orange={8},Mango={9},Ring={10},Watermalon={11},
DoubleStar={12},DoubleSeven={13},SmallBar={14},BigBar={15},
Normal={16},Lucky={17},Random={18},SmallThree={19},
BigThree={20},Bigfour={21},Zong={22},TianNv={23},
TianLong={24},JiuBao={25},GrandSlam={26},OpenTrain={27},
CreateTime='{28}'
where  Type={29}
;",
models.SmallApple, models.SmallOrange, models.SmallMango, models.SmallRing,
models.SmallWatermalon, models.SmallDoubleStar, models.SmallDoubleSeven, models.Apple,
models.Orange, models.Mango, models.Ring, models.Watermalon,
models.DoubleStar, models.DoubleSeven, models.SmallBar, models.BigBar,
models.Normal, models.Lucky, models.Random, models.SmallThree,
models.BigThree, models.Bigfour, models.Zong, models.TianNv,
models.TianLong, models.JiuBao, models.GrandSlam, models.OpenTrain,
models.CreateTime,models.Type
);
              
                int i = cn.Execute(str.ToString());
                cn.Close();
                return i;
            }
        }

        public static int UpdateFruitPotConfig(List<FruitPotConfig> models)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                foreach (FruitPotConfig item in models)
                {
                    str.AppendFormat(@"
update "+ database1+ @".FruiteBigPot set UpdateTime='{0}',Open={1},Critical={2}
where Id ={3}
;",
DateTime.Now.ToString(),
item.Open,
item.Critical,
item.Id
);
                }
                int i = cn.Execute(str.ToString());
                cn.Close();
                return i;
            }
        }

        public static int UpdateFruitBibei(List<FruitBibeiConfig> models)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();


                foreach (FruitBibeiConfig item in models)
                {
                    str.AppendFormat(@"
update "+ database3 + @".fg_config_bibei set GameNum1={0},GameNum2={1},GameNum3={2},GameNumn={3}
where SeasonID ={4}
;",
item.GameNum1,
item.GameNum2,
item.GameNum3,
item.GameNumn,
item.SeasonID
);
                }
                int i = cn.Execute(str.ToString());
                cn.Close();
                return i;
            }
        }




    }
}
