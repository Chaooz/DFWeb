
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Core;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        ILoginProvider _loginProvider;
        IEmailProvider _emailProvider;

        Microsoft.AspNetCore.Http.HttpContext _context;

        public LoginController(ILoginProvider loginProvider, IEmailProvider emailProvider)
        {
            _loginProvider = loginProvider;
            _emailProvider = emailProvider;
        }

        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUser([FromForm] UserLoginModel loginModel)
        {
            var ret = _loginProvider.LoginUser(loginModel.username,loginModel.password);
            switch(ret)
            {
                case UserModel.UserErrorCode.UserDoesNotExist:
                    return Redirect("/Login/LoginFailed");
                case UserModel.UserErrorCode.WrongPassword:
                    return Redirect("/Login/LoginFailed");
                default:
                    return Redirect("/");
           }
        }

        [HttpPost]
        [Route("ChangePassStep1")]
        public IActionResult ChangePassStep1([FromForm] string email)
        {
            var userModel = _loginProvider.CreatePasswordToken(email);
            if( userModel != null )
            {
                // Send code on email
                EmailMessage message = new EmailMessage();
                message.ToAddresses.Add( new EmailAddress(){ Name = "Bla", Address = userModel.Email } );
                message.FromAddresses.Add( new EmailAddress() { Name = "DarkFactor", Address = "darkfactor@altibox.no" } );
                message.Subject = "DarkFactor : Code";
                message.Content = "Your code to reset your password is : " + userModel.Token;

                _emailProvider.Send( message );
            }
            return Redirect("/Login/ChangePassStep2");
        }

        [HttpPost]
        [Route("ChangePassStep2")]
        public IActionResult ChangePassStep2([FromForm] string code)
        {
            return Redirect("/Login/ChangePassStep3");
        }

        [HttpPost]
        [Route("ChangePassStep3")]
        public IActionResult ChangePassStep3([FromForm] UserLoginModel loginModel)
        {
            return Redirect("/");
        }
    }
}