
namespace DFWeb.BE.Models
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
        public string Token;
        public bool IsLoggedIn;
        public AccessLevel UserAccessLevel;
    }
}