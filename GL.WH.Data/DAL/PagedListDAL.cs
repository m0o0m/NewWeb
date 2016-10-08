using GL.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using GL.Dapper;
using GL.Command.DBUtility;

namespace GL.Data.DAL
{
    public class PagedListDAL<T>
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;
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
    }
}