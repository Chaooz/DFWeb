
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
        public IActionResult LoginUser([FromForm] string username, [FromForm] string password)
        {
            var ret = _loginProvider.LoginUser(username,password);
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
            var didSucceed = _loginProvider.VerifyPasswordToken(code);
            if ( didSucceed )
            {
                return Redirect("/Login/ChangePassStep3");
            }
            return Redirect("/Login/ChangePassStep1");
        }

        [HttpPost]
        [Route("ChangePassStep3")]
        public IActionResult ChangePassStep3([FromForm] string password, [FromForm] string password2)
        {
            if ( string.IsNullOrEmpty( password ) || string.IsNullOrEmpty( password2 ) || !password.Equals(password2) )
            {
                return Redirect("/Login/ChangePassStep3");
            }

            var errorCode = _loginProvider.ChangePassword(password);
            if ( errorCode == UserModel.UserErrorCode.OK )
            {
                _loginProvider.Logout();
                return Redirect("/");
            }
            return Redirect("/Login/ChangePassStep1");
       }
    }
}