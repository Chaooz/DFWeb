
namespace DarkFactorCoreNet.Models
{
    public class UserModel
    {
        

        public enum UserErrorCode
        {
            OK,
            UserDoesNotExist,
            UserExistsAlready,
            CodeError,
            WrongPassword,
            PinCodeDoesNotMatch,
            MustChangePassword,
            PasswordDoesNotMatch
        
        }

        public int UserId;
        public string Username;
        public string Password;
        public string Email;
        public byte[]  Salt;
        public string Token;
        public bool MustChangePassword;
        public bool IsLoggedIn;
        public AccessLevel UserAccessLevel;
    }
}