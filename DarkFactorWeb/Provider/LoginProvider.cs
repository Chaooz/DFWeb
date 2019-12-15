
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq;

using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Provider
{
    public interface ILoginProvider
    {
        UserModel.UserErrorCode LoginUser(string username, string password);
        UserModel CreatePasswordToken(string email);
    }

    public class LoginProvider : ILoginProvider
    {
        IUserSessionProvider _userSession;

        ILoginRepository _loginRepository;

        const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const int CODE_LENGTH = 10;
        private static Random random = new Random();

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

        public UserModel.UserErrorCode LoginUser(string username, string password)
        {
            var user = _loginRepository.GetUserWithUsername(username);
            if ( user == null )
            {
                return UserModel.UserErrorCode.UserDoesNotExist;
            }

            if ( string.IsNullOrEmpty( user.Password ) || user.MustChangePassword ) 
            {
                return UserModel.UserErrorCode.MustChangePassword;
            }

            string hashedPassword = generateHash(password, user.Salt);
            if ( !user.Password.Equals( hashedPassword ) )
            {
                return UserModel.UserErrorCode.WrongPassword;
            }

            return UserModel.UserErrorCode.OK;
        }

        public UserModel CreatePasswordToken(string email)
        {
            _userSession.RemoveSession();

            UserModel userModel = _loginRepository.GetUserWithEmail(email);
            if ( userModel == null )
            {
                return null;
            }

            // Generate random activation code
            userModel.Token = new string(Enumerable.Repeat(CHARS, CODE_LENGTH)
            .Select(s => s[random.Next(s.Length)]).ToArray());

            // Remember this user
            _userSession.SetUser(userModel);

            // Todo Send email with code

            return userModel;
        }

        public UserModel.UserErrorCode ChangePassword(string password)
        {
            byte[] salt = generateSalt();
            string hashedPassword = generateHash(password,salt);
            string username = _userSession.GetUsername();

            return _loginRepository.ChangePassword(username,password);
        }

        public static string generateHash(string password, byte[] condiment) 
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: condiment,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public static byte[] generateSalt() 
        {
            // generate a 128-bit (16*8) salt using a secure PRNG
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create()) 
            {
                rng.GetBytes(salt);
            }
            return salt; 
        }
    }
}