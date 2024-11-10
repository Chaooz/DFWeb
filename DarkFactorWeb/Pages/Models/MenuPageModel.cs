using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DFWeb.BE.Models;
using DFWeb.BE.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class MenuPageModel : PageModel
    {
        public List<MenuItem> treeList;
        public List<MenuItem> menuItems;
        public UserInfoModel UserInfoModel { get; set; }

        protected IMenuProvider menuProvider;
        protected ILoginProvider _loginProvider;

        public MenuPageModel(IMenuProvider menuProvider, ILoginProvider loginProvider)
        {
            this.menuProvider = menuProvider;
            _loginProvider = loginProvider;
        }

        protected void GetMenuData(int id)
        {
            menuItems = menuProvider.SelectItem(id);
            treeList = menuProvider.GetTree(id);
            UserInfoModel = _loginProvider.GetLoginInfo();
        }

        public ActionResult OnGetPartial() =>

            new PartialViewResult
            {
                ViewName = "Menu",
                ViewData = ViewData,
            };

    }
}
