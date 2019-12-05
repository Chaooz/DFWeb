using Microsoft.AspNetCore.Http;

namespace DarkFactorCoreNet.Provider
{
    public interface IUserSessionProvider
    {
        void RemoveSession();
        void SetToken(string token);

        string GetUsername();
    }

    public class UserSessionProvider : IUserSessionProvider
    {
        public static readonly string SessionKeyUserName = "WEBUSER";
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
            RemoveConfig("Username");
            RemoveConfig("Token");
        }

        public void SetToken(string token)
        {
            SetConfigString("Token", token);
        }

        public string GetUsername()
        {
            return GetConfigString("Username");
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
                context.Session.SetString(SessionKeyUserName + "." + keyName, value);
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
