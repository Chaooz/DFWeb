
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Core;
using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
       
        ILoginRepository _loginRepository;
        Microsoft.AspNetCore.Http.HttpContext _context;

        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUser([FromForm] UserLoginModel loginModel)
        {
            var ret = _loginRepository.LoginUser(loginModel.username,loginModel.password);
            switch(ret)
            {
                case LoggedInUser.UserErrorCode.UserDoesNotExist:
                    return Redirect("/Login/LoginFailed");
                case LoggedInUser.UserErrorCode.WrongPassword:
                    return Redirect("/Login/LoginFailed");
                default:
                    return Redirect("/");
           }
        }

        [HttpPost]
        [Route("ChangePassStep1")]
        public IActionResult ChangePassStep1([FromForm] string email)
        {
            return Redirect("/Login/ChangePassStep2");
        }

        [HttpPost]
        [Route("ChangePassStep2")]
        public IActionResult ChangePassStep2([FromForm] string code)
        {
            var ret = _loginRepository.VerifyPinCode(code);
            switch(ret)
            {
                case LoggedInUser.UserErrorCode.PinCodeDoesNotMatch:
                    return Redirect("/Login/ChangePassStep2?errorId=" + ret.ToString());
                default:
                    return Redirect("/Login/ChangePassStep3");
           }
        }

        [HttpPost]
        [Route("ChangePassStep3")]
        public IActionResult ChangePassStep3([FromForm] UserLoginModel loginModel)
        {
            return Redirect("/");
        }
    }
}