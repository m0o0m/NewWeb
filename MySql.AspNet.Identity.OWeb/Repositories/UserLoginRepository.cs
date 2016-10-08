using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity;
using MySql.Data.MySqlClient;

namespace MySql.AspNet.Identity.OWeb.Repositories
{
    public class UserLoginRepository
    {
        private readonly string _connectionString;
        public UserLoginRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Insert(IdentityUser user, UserLoginInfo login)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@Id", user.Id},
                    {"@LoginProvider", login.LoginProvider},
                    {"@ProviderKey", login.ProviderKey}
                };

                MySqlHelper.ExecuteNonQuery(conn, @"INSERT INTO CustomerUserLogins(UserId,LoginProvider,ProviderKey) VALUES(@Id,@LoginProvider,@ProviderKey)", parameters);
            }
        }

        public void Delete(IdentityUser user, UserLoginInfo login)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@Id", user.Id},
                    {"@LoginProvider", login.LoginProvider},
                    {"@ProviderKey", login.ProviderKey}
                };

                MySqlHelper.ExecuteNonQuery(conn, @"DELETE FROM CustomerUserLogins WHERE UserId = @Id AND LoginProvider = @LoginProvider AND ProviderKey = @ProviderKey", parameters);
            }
        }

        public string GetByUserLoginInfo(UserLoginInfo login)
        {
            string userId;
            using (var conn = new MySqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@LoginProvider", login.LoginProvider},
                    {"@ProviderKey", login.ProviderKey}
                };
                var userIdObject = MySqlHelper.ExecuteScalar(conn, CommandType.Text,
                    @"SELECT UserId FROM CustomerUserLogins WHERE LoginProvider = @LoginProvider AND ProviderKey = @ProviderKey", parameters);
                userId = userIdObject == null
                    ? null
                    : userIdObject.ToString();
            }
            return userId;
        }

        public List<UserLoginInfo> PopulateLogins(string userId)
        {
            var listLogins = new List<UserLoginInfo>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@Id",userId}
                };

                var reader = MySqlHelper.ExecuteReader(conn, CommandType.Text,
                   @"SELECT LoginProvider,ProviderKey FROM CustomerUserLogins Where UserId = @Id", parameters);
                while (reader.Read())
                {
                    listLogins.Add(new UserLoginInfo(reader[0].ToString(), reader[1].ToString()));
                }
            }
            return listLogins;
        }

   
    }
}
