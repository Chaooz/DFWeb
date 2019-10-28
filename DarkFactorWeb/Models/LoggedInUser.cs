
namespace DarkFactorCoreNet.Models
{
    public class LoggedInUser
    {
        public enum AccessLevel
        {
            Public,
            Member,
            Editor,
            Admin
        }

        public string Username;
        public AccessLevel UserAccessLevel;
    }
}