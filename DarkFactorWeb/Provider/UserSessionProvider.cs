using Microsoft.AspNetCore.Http;
using DarkFactorCoreNet.Models;
using AccountClientModule.Provider;

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

    public class UserSessionProvider : DFSessionProvider, IUserSessionProvider
    {
        public static readonly string SessionUsernameKey = "Username";
        public static readonly string SessionTokenKey = "Token";
        public static readonly string SessionIsLoggedIn = "IsLoggedIn";


        public UserSessionProvider( IHttpContextAccessor httpContext ) : base("WEBUSER", httpContext)
        {
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
    }
}
