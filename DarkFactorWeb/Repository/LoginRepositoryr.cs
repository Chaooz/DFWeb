
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
        bool LoginUser(string username, string password);
    }

    public class LoginRepository : ILoginRepository
    {
        private readonly ILogger<LoginRepository> _logger;

        private HttpContext _httpContext;
        private IDFDatabase _database;
        private LoggedInUser _loggedInUser;

        public static readonly string SessionKeyName = "WEBUSER";

        public LoginRepository( IHttpContextAccessor httpContext, IDFDatabase database )
        {
            _database = database;
            _loggedInUser = null;
            if ( httpContext != null )
            {
                _httpContext = httpContext.HttpContext;
            }
            else
            {
                _httpContext = null;
            }
        }

        public bool IsLoggedIn()
        {
            if ( _httpContext != null )
            {
                var username = _httpContext.Session.GetString(SessionKeyName);
                return  (username != null);
            }
            _loggedInUser = null;
            return false;
        }

        public bool LoginUser(string username, string password)
        {
            if ( _httpContext != null )
            {
                //_logger.LogInformation("LoginUser ...");
                if ( username == null || password == null )
                {
                    return false;
                }

                string sql = string.Format("select password, acl from users where username = @username");

                var variables = DFDataBase.CreateVariables();
                variables.Add("@username", username);

                List<PageContentModel> pageList = new List<PageContentModel>();

                using (DFStatement statement = _database.ExecuteSelect(sql, variables))
                {
                    while (statement.ReadNext())
                    {
                        // TODO : Hash password
                        string dbPassword = statement.ReadString("password");
                        int acl = statement.ReadUInt32("acl");
                        if ( password.Equals( dbPassword) )
                        {
                            _loggedInUser = new LoggedInUser()
                            {
                                Username = username,
                                UserAccessLevel = (LoggedInUser.AccessLevel) acl
                            };

                            return true;
                        }
                    }
                }
            }
            return false;
        } 

        public int GetAccessLevel()
        {
            if ( IsLoggedIn() && _loggedInUser != null)
            {
                return (int)_loggedInUser.UserAccessLevel;
            }
            return (int)LoggedInUser.AccessLevel.Public;
        }
    }
}