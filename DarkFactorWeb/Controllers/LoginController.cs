
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
        public IActionResult LoginUser([FromForm] UserLoginModel loginModel)
        {
            _loginRepository.LoginUser(loginModel.username,loginModel.password);
            return Redirect("/");
        }

    }
}