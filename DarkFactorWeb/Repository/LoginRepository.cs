
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Repository.Database;

using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Repository
{
    public interface ILoginRepository
    {
        UserModel GetUserWithUsername(string username);
        UserModel GetUserWithEmail(string email);
        UserModel.UserErrorCode ChangePassword(string username, string password);
    }

    public class LoginRepository : ILoginRepository
    {
        private readonly ILogger<LoginRepository> _logger;

        private IDFDatabase _database;

        public LoginRepository( IDFDatabase database )
        {
            _database = database;
        }


        public UserModel GetUserWithUsername(string username)
        {
            if ( string.IsNullOrEmpty(username) )
            {
                return null;
            }

            string sql = string.Format("select * from users where username = @username");

            var variables = DFDataBase.CreateVariables();
            variables.Add("@username", username);

            return GetUser( sql, variables);
        }

        public UserModel GetUserWithEmail(string email)
        {
            if ( string.IsNullOrEmpty(email) )
            {
                return null;
            }

            string sql = string.Format("select * from users where email = @email");

            var variables = DFDataBase.CreateVariables();
            variables.Add("@email", email);

            return GetUser( sql, variables);
        }

        private UserModel GetUser(string sql,  Dictionary<string, object>  variables )
        {
            List<PageContentModel> pageList = new List<PageContentModel>();
            using (DFStatement statement = _database.ExecuteSelect(sql, variables))
            {
                while (statement.ReadNext())
                {
                    UserModel user = new UserModel()
                    {
                        Username = statement.ReadString("username"),
                        Password = statement.ReadString("password"),
                        Email = statement.ReadString("email"),
                        UserAccessLevel = (UserModel.AccessLevel) statement.ReadUInt32("acl")
                    };
                    return user;
                }
            }
            return null;
        }

        public UserModel.UserErrorCode ChangePassword(string username, string password)
        {
            if ( string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) )
            {
                return UserModel.UserErrorCode.UserDoesNotExist;
            }

            UserModel user = GetUserWithUsername(username);
            if ( user == null )
            {
                return UserModel.UserErrorCode.UserDoesNotExist;
            }

            // Update password
            string salt = "123";

            string sql = string.Format("update users set salt=@salt, password=@password where username=@username");

            var variables = DFDataBase.CreateVariables();
            variables.Add("@salt", salt);
            variables.Add("@password", password);
            variables.Add("@username", username);

            int numRows = _database.ExecuteUpdate(sql, variables);
            if ( numRows == 1 )
            {
                return UserModel.UserErrorCode.OK;
            }

            return UserModel.UserErrorCode.CodeError;
        }

    }
}