using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DarkFactorCoreNet.Models
{
    public class UserLoginModel
    {
        public string username{ get; set;}
        public string password{ get; set;}

        public UserLoginModel()
        {
        }
    }
}
