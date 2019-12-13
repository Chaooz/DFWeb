
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

        Microsoft.AspNetCore.Http.HttpContext _context;

        public LoginController(ILoginProvider loginProvider)
        {
            _loginProvider = loginProvider;
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
            var userId = _loginProvider.CreatePasswordToken(email);
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