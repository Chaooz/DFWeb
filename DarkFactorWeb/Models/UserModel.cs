
namespace DarkFactorCoreNet.Models
{
    public class UserModel
    {
        public enum AccessLevel
        {
            Public,
            Member,
            Editor,
            Admin
        }

        public enum UserErrorCode
        {
            OK,
            UserDoesNotExist,
            UserExistsAlready,
            CodeError,
            WrongPassword,
            PinCodeDoesNotMatch,
            MustChangePassword
        
        }

        public int UserId;
        public string Username;
        public string Password;
        public string Email;
        public byte[]  Salt;
        public string Token;
        public bool MustChangePassword;
        public AccessLevel UserAccessLevel;
    }
}