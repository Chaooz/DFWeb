using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Controllers;

namespace DarkFactorCoreNet.Pages
{
    public class LoginModel : BasePageModel
    {
        private ILoginProvider _loginProvider;

        public LoginModel(ILoginProvider loginProvider,
            IPageProvider pageProvider, IMenuProvider menuProvider) : base(pageProvider,menuProvider)
        {
            _loginProvider = loginProvider;
        }

        public IActionResult OnPostAsync([FromForm] String username, String password)
        {
            _loginProvider.LoginUser(username, password);
            return Redirect("/");
        }
    }
}
