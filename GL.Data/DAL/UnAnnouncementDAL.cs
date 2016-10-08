using GL.Command.DBUtility;
using GL.Data.Model;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using GL.Dapper;

namespace GL.Data.DAL
{
    /// <summary>
    /// Unrelated_Announcement
    /// </summary>
    public class UnAnnouncementDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        internal static string GetModel()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<string> i = cn.Query<string>(@"select Content from Unrelated_Announcement where ID = 1");
                cn.Close();
                return i.FirstOrDefault();
            }
        }


        internal static int Update(string value)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"update Unrelated_Announcement set Content = @Content where ID = 1", new { Content = value } );
                cn.Close();
                return i;
            }
        }
        //UpdateGameAnnouncement

        internal static int UpdateGameAnnouncement(string sql)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(sql);
                cn.Close();
                return i;
            }
        }

        internal static List<GL.Data.Model.GameAnnouncement> GetGameAnnouncement(string sql) {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<GL.Data.Model.GameAnnouncement> i = cn.Query<GL.Data.Model.GameAnnouncement>(sql);
                cn.Close();
                return i.ToList();
            }
        }
    }
}
