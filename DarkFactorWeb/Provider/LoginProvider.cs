
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq;

using DFCommonLib.Config;
using DFCommonLib.Utils;

using AccountClientModule.Client;
using AccountCommon.SharedModel;

using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.ConfigModel;

namespace DarkFactorCoreNet.Provider
{
    public interface ILoginProvider
    {
        UserInfoModel GetLoginInfo();
        AccountData.ErrorCode LoginUser(string username, string password);
        void Logout();
        
        ReturnData ResetPasswordWithEmail(string email);
        ReturnData ResetPasswordWithCode(string code);
        ReturnData ResetPasswordWithToken(string password);
    }

    public class LoginProvider : ILoginProvider
    {
        IUserSessionProvider _userSession;
        ILoginRepository _loginRepository;
        IAccountClient _accountClient;

        public LoginProvider(IUserSessionProvider userSession, 
                            IAccountClient accountClient,
                            ILoginRepository loginRepository)
        {
            _userSession = userSession;
            _accountClient = accountClient;
            _loginRepository = loginRepository;

            IConfigurationHelper configuration = DFServices.GetService<IConfigurationHelper>();
            var customer = configuration.GetFirstCustomer() as WebConfig;
            if ( customer != null )
            {
                _accountClient.SetEndpoint(customer.AccountServer);
            }
        }

        public UserInfoModel GetLoginInfo()
        {
            UserInfoModel userInfo = new UserInfoModel();

            var user = GetLoggedInUser();
            if ( user != null )
            {
                userInfo.IsLoggedIn = user.IsLoggedIn;
                userInfo.UserAccessLevel = (int) user.UserAccessLevel; 
                userInfo.Handle = user.Username;
            }
            return userInfo;
        }
        
        public UserModel GetLoggedInUser()
        {
            string username = _userSession.GetUsername();
            if ( username != null )
            {
                var user = _loginRepository.GetUserWithUsername(username);
                if ( user != null )
                {
                    user.IsLoggedIn = _userSession.IsLoggedIn();
                    user.Token = _userSession.GetToken();
                }
                return user;
            }
            return null;
        }

        public AccountData.ErrorCode LoginUser(string username, string password)
        {
            LoginData loginData = new LoginData()
            {
                username = username,
                password = password
            };
            var returnData = _accountClient.LoginAccount(loginData);
            if  ( returnData.errorCode == AccountData.ErrorCode.OK )
            {
                AccessLevel accessLevel = _loginRepository.GetAccessForUser( returnData.nickname );

                UserModel userModel = new UserModel()
                {
                    Username = returnData.nickname,
                    IsLoggedIn = true,
                    UserAccessLevel = accessLevel
                };

                _userSession.SetUser(userModel);
            }
            return returnData.errorCode;
        }

        public void Logout()
        {
            _userSession.RemoveSession();
        }

        public ReturnData ResetPasswordWithEmail(string email)
        {
            return _accountClient.ResetPasswordWithEmail(email);
        }

        public ReturnData ResetPasswordWithCode(string code)
        {
            return _accountClient.ResetPasswordWithCode(code);
        }

        public ReturnData ResetPasswordWithToken(string password)
        {
            return _accountClient.ResetPasswordWithToken(password);
        }
    }
}