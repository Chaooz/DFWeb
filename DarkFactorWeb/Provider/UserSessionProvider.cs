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
    }

    public class UserSessionProvider : IUserSessionProvider
    {
        public static readonly string SessionKeyUserName = "WEBUSER";

        public static readonly string SessionUsernameKey = "Username";
        public static readonly string SessionTokenbKey = "Token";

        private readonly IHttpContextAccessor _httpContextAccessor;

        private LoggedInUser _loggedInUser;

        public UserSessionProvider( IHttpContextAccessor httpContext )
        {
            _httpContextAccessor = httpContext;
            _loggedInUser = null;
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
            RemoveConfig(SessionTokenbKey);
        }

        public void SetUser(UserModel user)
        {
            SetConfigString(SessionUsernameKey, user.Username);
            SetConfigString(SessionTokenbKey, user.Token);
        }

        public string GetUsername()
        {
            return GetConfigString(SessionUsernameKey);
        }

        public string GetToken()
        {
            return GetConfigString(SessionTokenbKey);
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
