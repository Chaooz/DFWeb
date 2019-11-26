
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Repository.Database;

using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Controllers
{
    public interface ILoginRepository
    {
        bool IsLoggedIn();
        LoggedInUser.UserErrorCode LoginUser(string username, string password);
        LoggedInUser.UserErrorCode ChangePassword(string username, string password, string pinCode);
    }

    public class LoginRepository : ILoginRepository
    {
        private readonly ILogger<LoginRepository> _logger;

        private IDFDatabase _database;
        private LoggedInUser _loggedInUser;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public static readonly string SessionKeyUserName = "WEBUSER";
        public static readonly string SessionKeyPinName = "WEBUSERPIN";

        public LoginRepository( IHttpContextAccessor httpContext, IDFDatabase database )
        {
            _database = database;
            _loggedInUser = null;

            _httpContextAccessor = httpContext;
        }

        private HttpContext GetContext()
        {
            if ( _httpContextAccessor != null )
            {
                return _httpContextAccessor.HttpContext;
            }
            return null;
        }

        public bool IsLoggedIn()
        {
            return GetLoggedInUser() != null;
        }

        public LoggedInUser GetLoggedInUser()
        {
            var context = GetContext();
            if ( context != null && _loggedInUser == null)
            {
                var username = context.Session.GetString(SessionKeyUserName);
                if ( username != null )
                {
                    _loggedInUser = GetUser(username);
                }
            }
            return _loggedInUser;
        }

        public LoggedInUser.UserErrorCode LoginUser(string username, string password)
        {
            var context = GetContext();
            if ( context != null )
            {
                //_logger.LogInformation("LoginUser ...");
                if ( username == null || password == null )
                {
                    return LoggedInUser.UserErrorCode.UserDoesNotExist;
                }

                LoggedInUser user = GetUser(username);
                if ( user == null )
                {
                    return LoggedInUser.UserErrorCode.UserDoesNotExist;
                }

                // TODO. encrypt password
                if ( password.Equals( user.Password) )
                {
                    SetLoggedInUser(user);
                    return LoggedInUser.UserErrorCode.OK;
                }
                return LoggedInUser.UserErrorCode.WrongPassword;
            }
            return LoggedInUser.UserErrorCode.CodeError;
        } 

        public bool SetPinCode(String pinCode)
        {
            var context = GetContext();
            if ( context != null )
            {
                context.Session.SetString(SessionKeyPinName,pinCode);
                return true;
            }
            return false;
        }

        // TODO : Return errorcode/ok
        public LoggedInUser.UserErrorCode ChangePassword(string username, string password, string pinCode)
        {
            var context = GetContext();
            if ( context == null )
            {
                return LoggedInUser.UserErrorCode.CodeError;
            }

            // Verify that the PIN code matches
            string storedPinCode = context.Session.GetString(SessionKeyPinName);
            if ( string.IsNullOrEmpty(pinCode) )
            {
                return LoggedInUser.UserErrorCode.CodeError;
            }

            if ( !storedPinCode.Equals(pinCode))
            {
                return LoggedInUser.UserErrorCode.PinCodeDoesNotMatch;
            }

            if ( string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) )
            {
                return LoggedInUser.UserErrorCode.UserDoesNotExist;
            }

            LoggedInUser user = GetUser(username);
            if ( user == null )
            {
                return LoggedInUser.UserErrorCode.UserDoesNotExist;
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
                return LoggedInUser.UserErrorCode.OK;
            }

            return LoggedInUser.UserErrorCode.CodeError;
        }

        private LoggedInUser GetUser(string username)
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

        private LoggedInUser GetUserWithEmail(string email)
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



        private LoggedInUser GetUser(string sql,  Dictionary<string, object>  variables )
        {
            List<PageContentModel> pageList = new List<PageContentModel>();
            using (DFStatement statement = _database.ExecuteSelect(sql, variables))
            {
                while (statement.ReadNext())
                {
                    LoggedInUser user = new LoggedInUser()
                    {
                        Username = statement.ReadString("username"),
                        Password = statement.ReadString("password"),
                        UserAccessLevel = (LoggedInUser.AccessLevel) statement.ReadUInt32("acl")
                    };
                    return user;
                }
            }
            return null;
        }

        public int GetAccessLevel()
        {
            var user = GetLoggedInUser();
            if ( user != null )
            {
                return (int)user.UserAccessLevel;
            }
            return (int)LoggedInUser.AccessLevel.Public;
        }

        private bool SetLoggedInUser(LoggedInUser user)
        {
            var context = GetContext();
            if ( context != null )
            {
                _loggedInUser = user;
                context.Session.SetString(SessionKeyUserName,user.Username);
                return true;
            }
            return false;
        }
    }
}