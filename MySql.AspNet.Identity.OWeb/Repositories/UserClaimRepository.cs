﻿using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using MySql.Data.MySqlClient;

namespace MySql.AspNet.Identity.OWeb.Repositories
{
    public class UserClaimRepository<TUser>  where TUser : IdentityUser
    {
        private readonly string _connectionString;
        public UserClaimRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(TUser user, Claim claim)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                        {
                            {"@UserId", user.Id},
                            {"@ClaimType", claim.Type},
                            {"@ClaimValue",claim.Value}
                        };

                MySqlHelper.ExecuteNonQuery(conn, @"INSERT INTO CustomerUserClaims(UserId,ClaimType,ClaimValue) VALUES(@UserId,@ClaimType,@ClaimValue)", parameters);
            }
        }

        public void Delete(TUser user, Claim claim)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@UserId", user.Id},
                    {"@ClaimType", claim.Type},
                    {"@ClaimValue", claim.Value}
                };

                MySqlHelper.ExecuteNonQuery(conn,
                    @"DELETE FROM CustomerUserClaims WHERE UserId=@UserId AND ClaimType=@ClaimType AND ClaimValue=@ClaimValue", parameters);
            }
        }

        public List<IdentityUserClaim> PopulateClaims(string userId)
        {
            var claims = new List<IdentityUserClaim>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@Id", userId}
                };

                var reader = MySqlHelper.ExecuteReader(conn, CommandType.Text,
                    @"SELECT ClaimType,ClaimValue FROM CustomerUserClaims WHERE UserId=@Id", parameters);
                while (reader.Read())
                {
                    claims.Add(new IdentityUserClaim() { ClaimType = reader[0].ToString(), ClaimValue = reader[1].ToString() });
                }

            }
            return claims;
        }
    }
}
