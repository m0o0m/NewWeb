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
    public class BaseDataDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.ConnectionString;

        internal static IEnumerable<BaseDataInfo> GetRegisteredUsers(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( CreateTime, '%Y-%m-%d') as date, count(0) as count FROM role where date(CreateTime) >= date(@StartDate) and date(CreateTime) < date(@ExpirationDate)");

                switch (bdv.Terminals)
                {
                    case terminals.页游:
                        str.AppendFormat(" and LoginDevice = {0}", (int)terminals.页游);
                        break;
                    case terminals.安卓:
                        str.AppendFormat(" and LoginDevice = {0}", (int)terminals.安卓);
                        break;
                    case terminals.IOS:
                        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                        break;
                    case terminals.所有终端:
                    default:
                        break;
                }

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );");
                        break;
                    case groupby.按月:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m' );");
                        break;
                }


                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);


                //SELECT DATE_FORMAT( CreateTime, '%Y-%m-%d') as date, count(0) as count FROM role where UNIX_TIMESTAMP(CreateTime) >= UNIX_TIMESTAMP(@StartDate) and UNIX_TIMESTAMP(CreateTime) <= UNIX_TIMESTAMP(@ExpirationDate) GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );
                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<BaseDataInfo> GetActiveUsers(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( CreateTime, '%Y-%m-%d') as date, sum(count) as count FROM bg_activeusers where date(CreateTime) >= date(@StartDate) and date(CreateTime) < date(@ExpirationDate)");

                switch (bdv.Terminals)
                {
                    case terminals.页游:
                        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                        break;
                    case terminals.安卓:
                        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                        break;
                    case terminals.IOS:
                        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                        break;
                    case terminals.所有终端:
                    default:
                        str.Append(" and (type = 1 or type = 2)");
                        break;
                }

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );");
                        break;
                    case groupby.按月:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m' );");
                        break;
                }


                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);


                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<BaseDataInfoForOnlinePlay> GetOnlinePlay(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT CreateTime as date, sum(Online)/count(Online) as online, sum(Playing)/count(Playing) as playing FROM bg_onlineplaying where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d')");

                //switch (bdv.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d %H' );");
                        break;
                    case groupby.按月:
                        str.Append(" GROUP BY DATE_FORMAT( CreateTime, '%Y-%m-%d' );");
                        break;
                }


                IEnumerable<BaseDataInfoForOnlinePlay> i = cn.Query<BaseDataInfoForOnlinePlay>(str.ToString(), bdv);


                cn.Close();
                return i;
            }
        }

        internal static int GetAllPlayCount(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("SELECT sum(Count) FROM (SELECT Count FROM playcounttexas union all SELECT Count from playcountland) A;");
                int i = cn.Query<int>(str.ToString()).First();
                cn.Close();
                return i;
            }
        }


        internal static object GetAllUser(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.Append("SELECT count(0) FROM role;");
                int i = cn.Query<int>(str.ToString()).First();
                cn.Close();
                return i;
            }
        }


        internal static IEnumerable<BaseDataInfo> GetPlayCount(BaseDataView bdv)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( @StartDate, '%Y-%m-%d') as date, sum(count) as count FROM (SELECT Count, CreateTime FROM playcounttexas union all SELECT Count,CreateTime from playcountland) A ");

                //switch (bdv.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                switch (bdv.Groupby)
                {
                    case groupby.按日:
                        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                        break;
                    case groupby.按月:
                        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                        break;
                }


                IEnumerable<BaseDataInfo> i = cn.Query<BaseDataInfo>(str.ToString(), bdv);


                cn.Close();
                return i;
            }
        }






        internal static IEnumerable<BaseDataInfoForBankruptcyRate> GetBankruptcyRate(BaseDataView vbd)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append("SELECT DATE_FORMAT( curdate(), '%Y-%m-%d') as date, a.count as count, b.count as activeuser FROM (SELECT count(0) as count FROM role where Gold > 10) as a, (SELECT count(0) as count FROM role where Gold <= 10) as b;");

                //switch (vbd.Terminals)
                //{
                //    case terminals.页游:
                //        str.AppendFormat(" and type = {0}", (int)terminals.页游);
                //        break;
                //    case terminals.安卓:
                //        str.AppendFormat(" and type = {0}", (int)terminals.安卓);
                //        break;
                //    case terminals.IOS:
                //        str.AppendFormat(" and type = {0}", (int)terminals.IOS);
                //        break;
                //    case terminals.所有终端:
                //    default:
                //        str.Append(" and (type = 1 or type = 2)");
                //        break;
                //}

                //switch (vbd.Groupby)
                //{
                //    case groupby.按日:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m-%d') = DATE_FORMAT(@StartDate, '%Y-%m-%d');");
                //        break;
                //    case groupby.按月:
                //        str.Append(" where DATE_FORMAT( CreateTime, '%Y-%m') = DATE_FORMAT(@StartDate, '%Y-%m');");
                //        break;
                //}


                IEnumerable<BaseDataInfoForBankruptcyRate> i = cn.Query<BaseDataInfoForBankruptcyRate>(str.ToString(), vbd);


                cn.Close();
                return i;
            }
        }





        internal static IEnumerable<BaseDataInfoForDailyOutput> GetDailyOutput(BaseDataView vbd)
        {


            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.Append(@"SELECT DATE_FORMAT(CreateTime, '%Y-%m-%d') as date, sum(ChipChange) as Chip, sum(DiamondChange) as Diamond, sum(ScoreChange) as Score from bg_usermoneyrecord where `type` = 24 and CreateTime between @StartDate and @ExpirationDate group by DATE_FORMAT(CreateTime, '%Y-%m-%d');");
                //str.Append(@"SELECT sum(ChipChange) as Chip, sum(DiamondChange) as Diamond, `type` as ChipChangeType from bg_usermoneyrecord where  CreateTime between @StartDate and date_add( @StartDate, interval 1 day) group by `Type`;
                //select sum(Gold) as systemBargainingChip from role");


                IEnumerable<BaseDataInfoForDailyOutput> i = cn.Query<BaseDataInfoForDailyOutput>(str.ToString(), vbd);



                cn.Close();
                return i;
            }

        
        }
    }
}