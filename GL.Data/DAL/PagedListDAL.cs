using GL.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using GL.Dapper;
using GL.Command.DBUtility;
using System.Data;

namespace GL.Data.DAL
{
    public class PagedListDAL<T>
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");
        internal static IList<T> GetListByPage(PagerQuery pq)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                
                IEnumerable<T> csc = cn.Query<T>(pq.Sql);

                cn.Close();

                return csc.ToList();
            }



        }



        internal static IList<T> GetListByPage(PagerQuery pq,object param)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                IEnumerable<T> csc = cn.Query<T>(pq.Sql, param);

                cn.Close();

                return csc.ToList();
            }



        }


        internal static IList<T> GetListByPage(PagerQuery pq, string _sqlconnectionString)
        {

            using (var cn = new MySqlConnection(_sqlconnectionString))
            {
                cn.Open();

                IEnumerable<T> csc = cn.Query<T>(pq.Sql);

                cn.Close();

                return csc.ToList();
            }



        }


        /// <summary>
        /// 总页数
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        internal static int GetRecordCount(string sql)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Query<int>(sql).Single();
                cn.Close();
                return i;
            }
        }



        internal static int GetRecordCount(string sql,object param)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Query<int>(sql,param).Single();
                cn.Close();
                return i;
            }
        }



        /// <summary>
        /// 总页数
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        internal static int GetRecordCount(string sql, string _sqlconnectionString)
        {
            using (var cn = new MySqlConnection(_sqlconnectionString))
            {
                cn.Open();
                int i = cn.Query<int>(sql).Single();
                cn.Close();
                return i;
            }
        }

        internal static IEnumerable<object> GetModelData(string sql ,int id ,string _sqlconnectionString)
        {
            using (var cn = new MySqlConnection(_sqlconnectionString))
            {
                cn.Open();
                //记录查询次数
                cn.Execute(string.Format(@"update "+ database3 + @".S_DataModel set CheckCount = CheckCount + 1 where id = {0};", id));
                IEnumerable<object> i = cn.Query<object>(sql);
                cn.Close();
                return i;
            }
        }
    }
}
