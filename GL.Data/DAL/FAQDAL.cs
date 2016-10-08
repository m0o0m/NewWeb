using GL.Command.DBUtility;
using GL.Data.Model;
using GL.Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.DAL
{
    public class FAQDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;
        internal static IEnumerable<FAQ> GetList()
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<FAQ> i = cn.Query<FAQ>(@"select * from FAQ order by Id");
                cn.Close();
                return i;
            }
        }




        internal static int Add(FAQ model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"insert into FAQ( faqtype, faqcontent, faqtitle, operdate)values (@faqtype, @faqcontent, @faqtitle, @operdate)", model);
                cn.Close();
                return i;
            }

        }

        internal static int Delete(FAQ model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"delete from FAQ where Id = @Id", model);
                cn.Close();
                return i;
            }
        }

        internal static FAQ GetModelByID(FAQ model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                IEnumerable<FAQ> i = cn.Query<FAQ>(@"select * from FAQ where Id = @Id", model);
                cn.Close();
                return i.FirstOrDefault();
            }
        }

        internal static int Update(FAQ model)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();
                int i = cn.Execute(@"UPDATE FAQ SET faqtype =@faqtype faqcontent =@faqcontent faqtitle =@faqtitle operdate =@operdate where Id = @Id", model);
                cn.Close();
                return i;
            }
        }



    }
}
