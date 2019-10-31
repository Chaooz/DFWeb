
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
       
        ILoginProvider _loginProvider;
        Microsoft.AspNetCore.Http.HttpContext _context;

        public LoginController(ILoginProvider loginProvider)
        {
            _loginProvider = loginProvider;
        }

        [HttpPost]
        public IActionResult LoginUser([FromForm] UserLoginModel loginModel)
        {
            _loginProvider.LoginUser(loginModel.username,loginModel.password);
            return Redirect("/");
        }

    }
}