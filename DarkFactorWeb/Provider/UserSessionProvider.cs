using Microsoft.AspNetCore.Http;
using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Provider
{
    public interface IUserSessionProvider
    {
        void RemoveSession();
        void SetUser(UserModel user);

        string GetUsername();
        string GetToken();
        bool IsLoggedIn();
    }

    public class UserSessionProvider : IUserSessionProvider
    {
        public static readonly string SessionKeyUserName = "WEBUSER";

        public static readonly string SessionUsernameKey = "Username";
        public static readonly string SessionTokenKey = "Token";
        public static readonly string SessionIsLoggedIn = "IsLoggedIn";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSessionProvider( IHttpContextAccessor httpContext )
        {
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

        public void RemoveSession()
        {
            RemoveConfig(SessionUsernameKey);
            RemoveConfig(SessionTokenKey);
            RemoveConfig(SessionIsLoggedIn);
        }

        public void SetUser(UserModel user)
        {
            SetConfigString(SessionUsernameKey, user.Username);
            SetConfigString(SessionTokenKey, user.Token);
            SetConfigInt(SessionIsLoggedIn, user.IsLoggedIn ? 1 : 0);
        }

        public string GetUsername()
        {
            return GetConfigString(SessionUsernameKey);
        }

        public string GetToken()
        {
            return GetConfigString(SessionTokenKey);
        }

        public bool IsLoggedIn()
        {
            var IsLoggedIn = GetConfigInt(SessionIsLoggedIn);
            if ( IsLoggedIn != null && IsLoggedIn == 1 )
            {
                return true;
            }
            return false;
        }

        private string GetConfigString(string keyName)
        {
            var context = GetContext();
            if ( context != null )
            {
                return context.Session.GetString(SessionKeyUserName + "." + keyName);
            }
            return null;
        }

        private void SetConfigString(string keyName, string value)
        {
            var context = GetContext();
            if ( context != null )
            {
                if ( value != null )
                {
                    context.Session.SetString(SessionKeyUserName + "." + keyName, value);
                } 
                else
                {
                    context.Session.Remove(SessionKeyUserName + "." + keyName);
                }
            }
        }

        private int? GetConfigInt(string keyName)
        {
            var context = GetContext();
            if ( context != null )
            {
                return context.Session.GetInt32(SessionKeyUserName + "." + keyName);
            }
            return null;
        }

        private void SetConfigInt(string keyName, int? value)
        {
            var context = GetContext();
            if ( context != null )
            {
                if ( value != null )
                {
                    context.Session.SetInt32(SessionKeyUserName + "." + keyName, value.GetValueOrDefault());
                } 
                else
                {
                    context.Session.Remove(SessionKeyUserName + "." + keyName);
                }
            }
        }

        private void RemoveConfig(string keyName)
        {
            var context = GetContext();
            if ( context != null )
            {
                context.Session.Remove(SessionKeyUserName + "." + keyName);
            }
        }
    }
}
