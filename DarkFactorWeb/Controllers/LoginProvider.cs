
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Controllers
{
    public interface ILoginProvider
    {
        bool IsLoggedIn();
        bool LoginUser(string username, string password);
    }

    public class LoginProvider : ILoginProvider
    {
        private readonly ILogger<LoginProvider> _logger;

        private HttpContext _httpContext;

        private LoggedInUser _loggedInUser;

        public static readonly string SessionKeyName = "WEBUSER";

        public LoginProvider( IHttpContextAccessor httpContext )
        {
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

                // Do an actual check against service or repository for this user
                if ( username.Equals("chaoz") && password.Equals("mypass"))
                {
                    _loggedInUser = new LoggedInUser()
                    {
                        Username = username,
                        // Hack
                        UserAccessLevel = LoggedInUser.AccessLevel.Admin
                    };

                    _httpContext.Session.SetString(SessionKeyName, username);
                    return true;
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