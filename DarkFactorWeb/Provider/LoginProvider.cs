
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
        ICookieProvider _cookieProvider;

        public LoginProvider(IUserSessionProvider userSession, 
                            IAccountClient accountClient,
                            ILoginRepository loginRepository,
                            ICookieProvider cookieProvider)
        {
            _userSession = userSession;
            _accountClient = accountClient;
            _loginRepository = loginRepository;
            _cookieProvider = cookieProvider;

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
                return userInfo;
            }

            var loginToken = _cookieProvider.GetCookie("DFToken");
            if ( loginToken != null )
            {
                LoginTokenData loginTokenData = new LoginTokenData()
                {
                    token = loginToken
                };
                var accountData = _accountClient.LoginToken(loginTokenData);
                return SetLoggedInAccount(accountData);
            }

            return userInfo;
        }
        
        public UserModel GetLoggedInUser()
        {
            return _userSession.GetUser();
        }

        public AccountData.ErrorCode LoginUser(string username, string password)
        {
            LoginData loginData = new LoginData()
            {
                username = username,
                password = password
            };
            var accountData = _accountClient.LoginAccount(loginData);
            SetLoggedInAccount(accountData);
            return accountData.errorCode;
        }

        private UserInfoModel SetLoggedInAccount(AccountData accountData)
        {
            if  ( accountData.errorCode == AccountData.ErrorCode.OK )
            {
                AccessLevel accessLevel = _loginRepository.GetAccessForUser(accountData.nickname);

                UserModel userModel = new UserModel()
                {
                    Username = accountData.nickname,
                    IsLoggedIn = true,
                    UserAccessLevel = accessLevel,
                    Token = accountData.token
                };

                _userSession.SetUser(userModel);
                _cookieProvider.RemoveCookie("DFToken");
                _cookieProvider.SetCookie("DFToken", accountData.token);

                UserInfoModel userInfo = new UserInfoModel()
                {
                    IsLoggedIn = true,
                    UserAccessLevel = (int) userModel.UserAccessLevel,
                    Handle = userModel.Username
                };
                return userInfo;
            }
            _userSession.RemoveSession();
            _cookieProvider.RemoveCookie("DFToken");
            return null;
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