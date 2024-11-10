using System;

using DFCommonLib.DataAccess;

using DFWeb.BE.Models;

namespace DFWeb.BE.Repository
{
    public interface ILoginRepository
    {
        AccessLevel GetAccessForUser(string username);
    }

    public class LoginRepository : ILoginRepository
    {
        private IDbConnectionFactory _connection;

        public LoginRepository(IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public AccessLevel GetAccessForUser(string username)
        {
            AccessLevel accessLevel = AccessLevel.Public;

            if ( string.IsNullOrEmpty(username) )
            {
                return accessLevel;
            }

            string sql = string.Format("select acl from users where username = @username");
            using (var cmd = _connection.CreateCommand(sql))
            {
                cmd.AddParameter("@username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        accessLevel = (AccessLevel) Convert.ToInt32(reader["acl"]);
                    }
                }
            }
            return accessLevel;
        }     
    }
}