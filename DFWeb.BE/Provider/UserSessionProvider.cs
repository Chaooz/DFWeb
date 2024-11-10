using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Pqc.Crypto.Ntru;

using DFWeb.BE.Models;

using AccountClientModule.Provider;

namespace DFWeb.BE.Provider
{
    public interface IUserSessionProvider
    {
        void RemoveSession();
        void SetUser(UserModel user);

        UserModel GetUser();

        string GetUsername();
        string GetToken();
        bool IsLoggedIn();
        bool CanEditPage();
    }

    public class UserSessionProvider : DFSessionProvider, IUserSessionProvider
    {
        public static readonly string SessionUsernameKey = "Username";
        public static readonly string SessionTokenKey = "Token";
        public static readonly string SessionIsLoggedIn = "IsLoggedIn";

        public static readonly string SessionAccessLevel = "AccessLevel";

        public UserSessionProvider( IHttpContextAccessor httpContext ) : base("WEBUSER", httpContext)
        {
        }

        public void RemoveSession()
        {
            RemoveConfig(SessionUsernameKey);
            RemoveConfig(SessionTokenKey);
            RemoveConfig(SessionIsLoggedIn);
            RemoveConfig(SessionAccessLevel);
        }

        public void SetUser(UserModel user)
        {
            SetConfigString(SessionUsernameKey, user.Username);
            SetConfigString(SessionTokenKey, user.Token);
            SetConfigInt(SessionIsLoggedIn, user.IsLoggedIn ? 1 : 0);
            SetConfigInt(SessionAccessLevel, (int) user.UserAccessLevel );
        }

        public UserModel GetUser()
        {
            var token = GetConfigString(SessionTokenKey);
            if ( token == null )
            {
                return null;
            }

            UserModel user = new UserModel();
            user.Token = token;
            user.Username = GetConfigString(SessionUsernameKey);
            user.IsLoggedIn = GetConfigInt(SessionIsLoggedIn) == 1 ? true : false;

            var accessLevel = GetConfigInt(SessionAccessLevel);
            if (accessLevel != null)
            {
                user.UserAccessLevel =(AccessLevel) accessLevel;
            }
            else
            {
                user.UserAccessLevel = AccessLevel.Public;
            }

            return user;
        }

        public string GetUsername()
        {
            return GetConfigString(SessionUsernameKey);
        }

        public string GetToken()
        {
            return GetConfigString(SessionTokenKey);
        }

        public bool CanEditPage()
        {
            if ( IsLoggedIn() )
            {
                var accessLevel = GetConfigInt(SessionAccessLevel);
                if ( accessLevel != null && accessLevel >= (int) AccessLevel.Editor )
                {
                    return true;
                }
            }
            return false;
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
