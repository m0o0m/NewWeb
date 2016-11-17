using GL.Command.DBUtility;
using GL.Data.Model;
using GL.Data.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.Dapper;
using System.Data;

namespace GL.Data.DAL
{
    public class DaiLiDAL
    {
        internal static readonly string sqlconnectionString = PubConstant.GetConnectionString("connectionstring");

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");


        /// <summary>
        /// 得到当前系统代理还剩下多少库存
        /// </summary>
        /// <param name="bdv"></param>
        /// <returns></returns>
        internal static DailiKuCun GetDaiLiKuCun()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
                       select * from dailikucun;
                 ");

                IEnumerable<DailiKuCun> i = cn.Query<DailiKuCun>(str.ToString());
                cn.Close();
                return i.FirstOrDefault();
            }
        }



        internal static DailiKuCun GetDaiLiKuCun(int no)
        {
            string otherDBSqlCon = GetDaiLiConnection(no, 2);

            using (var cn = new MySqlConnection(otherDBSqlCon))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
                       select * from dailikucun;
                 ");

                IEnumerable<DailiKuCun> i = cn.Query<DailiKuCun>(str.ToString());
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        /// <summary>
        /// 修改当前系统代理的库存（比如发邮件减少代理的库存
        /// </summary>
        /// <param name="Gold">在原有基础上的增减值,比如Gold=-100，则在原有库存上减去100</param>
        /// <returns></returns>
        internal static int UpdateDaiLiKuCun(int no ,Int64 Gold) {
            string otherDBSqlCon = GetDaiLiConnection(no, 2);

            using (var cn = new MySqlConnection(otherDBSqlCon))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();
                //使用存储过程，这样方便回滚
                str.AppendFormat(@"sys_updateDaiLiKuCun");

                int i = cn.Execute(str.ToString(), param: new {
                    I_Gold = Gold
                }, commandType: CommandType.StoredProcedure);

                cn.Close();

                return i;
            }

           
        }

        internal static IEnumerable<DaiLiUsers> GetDaiLiUsers() {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
                       select * from dailiusers;
                 ");

                IEnumerable<DaiLiUsers> i = cn.Query<DaiLiUsers>(str.ToString());
                cn.Close();
                return i;
            }
        }

        internal static DaiLiUsers GetDaiLiSingleUsers(int no)
        {


            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                StringBuilder str = new StringBuilder();

                str.AppendFormat(@"
                       select * from dailiusers where no=@no;
                 ");

                IEnumerable<DaiLiUsers> i = cn.Query<DaiLiUsers>(str.ToString(), new
                {
                    no = no
                });
                cn.Close();
                return i.FirstOrDefault();
            }
        }



        private static string GetDaiLiConnection(int no,int dbType) {
            DaiLiUsers daliUsers = GetDaiLiSingleUsers(no);
            if (dbType == 1) {
                return daliUsers.GameDataDBConnect;
            } else if (dbType == 2) {
                return daliUsers.GserverinfoDBConnect;
            }
            else {
                return daliUsers.RecordDBConnect;
            }

           
        }
        internal static int UpdateFlowDesc(int no,string flowNos) {

            IEnumerable<S_Desc> dbFlows = GetFlowDesc(no);
            dbFlows = dbFlows.Where(m => m.IsCheck == 1);

            string[] updateFlows = flowNos.Split(',');
            List<int> addOper = new List<int>();
            List<int> deleteOper = new List<int>();
            for (int i = 0; i < updateFlows.Length; i++) {
                int temp = Convert.ToInt32( updateFlows[i].Trim());
                S_Desc isExist = dbFlows.Where(m => m.Type == temp).FirstOrDefault();
                if (isExist == null)//数据库中不存在,加的存在，操作就是添加
                {
                    addOper.Add(temp);
                }
            }
            foreach (S_Desc item in dbFlows)
            {
                bool isExist = updateFlows.Contains(item.Type.ToString());
                if (!isExist)//数据库中存在，加的不存在,操作就是删除
                {
                    deleteOper.Add(item.Type);
                }
            }

            //组装sql语句
            string sql = "";
            if (deleteOper.Count() > 0)
            {
                sql += @" delete from "+database2+".dailikucunno where No in ( ";
                for (int i = 0; i < deleteOper.Count(); i++) {
                    if (i == deleteOper.Count() - 1)
                    {
                        sql += deleteOper[i]+ "  );";
                    }
                    else {
                        sql += deleteOper[i] + ",";
                    }
                }
             
            }


            if (addOper.Count() > 0)
            {
                sql += @" insert into "+ database2 + ".dailikucunno(No,NoName) ";
                for (int i = 0; i < addOper.Count(); i++)
                {
                    if (i == addOper.Count() - 1)
                    {
                        sql += " select " + addOper[i] + ",'' ; ";
                    }
                    else
                    {
                        sql += " select " + addOper[i] + ",'' union all ";
                    }
                }

            }




            string otherDBSqlCon = GetDaiLiConnection(no, 3);

            //检测
            if (sql != "")
            {
                using (var cn = new MySqlConnection(otherDBSqlCon))
                {
                    cn.Open();
                    int i = cn.Execute(sql);
                    cn.Close();
                    return i;
                }
            }

            return 1;
        }



        internal static IEnumerable<S_Desc> GetFlowDesc(int no)
        {
            string otherDBSqlCon = GetDaiLiConnection(no,3);


            using (var cn = new MySqlConnection(otherDBSqlCon))
            {
                cn.Open();
                StringBuilder str = new StringBuilder();
                str.AppendFormat(@"
select * from (
select a.Type,a.Type_Id,a.UserOper,a.pType,
case
when b.`No` is NULL then 0
else 1
END as IsCheck
 from "+database3+@".S_Desc as a
LEFT JOIN "+database2+@".dailikucunno as b on  a.Type=b.`No`) as b
where b.Type_Id=1 and  b.UserOper not like '%数据回滚%'
;
");
                IEnumerable<S_Desc> i = cn.Query<S_Desc>(str.ToString());
                cn.Close();
                return i;
            }
        }


    }
}
