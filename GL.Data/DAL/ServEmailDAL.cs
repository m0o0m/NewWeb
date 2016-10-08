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
    public class ServEmailDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;
        internal static int Insert(ServEmail servEmail)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"insert into ServEmail(ServEmailTime,ServEmailTitle,ServEmailContent,ServEmailAuthor)values (@ServEmailTime,@ServEmailTitle,@ServEmailContent,@ServEmailAuthor)", servEmail);

                cn.Close();
                
                return i;
            }

        
        
        
        
        }

        internal static int Delete(ServEmail servEmail)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"delete from ServEmail where ServEmailID = @ServEmailID", servEmail);

                cn.Close();

                return i;
            }
        }
    }
}
