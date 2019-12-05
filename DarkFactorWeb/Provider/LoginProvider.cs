
using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Provider
{
    public interface ILoginProvider
    {
        UserModel.UserErrorCode LoginUser(string username, string password);
        UserModel.UserErrorCode CreatePasswordToken(string email);
    }

    public class LoginProvider
    {
        IUserSessionProvider _userSession;

        ILoginRepository _loginRepository;

        public LoginProvider(IUserSessionProvider userSession, ILoginRepository loginRepository)
        {
            _userSession = userSession;
            _loginRepository = loginRepository;
        }

        public bool IsLoggedIn()
        {
            return GetLoggedInUser() != null;
        }

        public LoggedInUser GetLoggedInUser()
        {
            string username = _userSession.GetUsername();
            if ( username != null )
            {
                var user = _loginRepository.GetUserWithUsername(username);
                return null;
            }
            return null;
        }

        UserModel.UserErrorCode LoginUser(string username, string password)
        {
            var user = _loginRepository.GetUserWithUsername(username);
            if ( user != null )
            {
                return UserModel.UserErrorCode.OK;
            }
            return UserModel.UserErrorCode.UserDoesNotExist;
        }

        public UserModel.UserErrorCode CreatePasswordToken(string email)
        {
            _userSession.RemoveSession();

            UserModel userModel = _loginRepository.GetUserWithEmail(email);
            if ( userModel == null )
            {
                return UserModel.UserErrorCode.UserDoesNotExist;
            }

            // TODO : Generate random token
            string randomToken = "1337";
            _userSession.SetToken(randomToken);
            return UserModel.UserErrorCode.OK;
        }
    }
}