
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
        
        }

        public string Username;
        public string Password;
        public string Email;
        public AccessLevel UserAccessLevel;
    }
}