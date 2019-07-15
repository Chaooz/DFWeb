using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Controllers;

namespace DarkFactorCoreNet.Pages
{
    public class BasePageModel : PageModel
    {
        public List<MenuItem> menuItems;
        public PageContentModel pageModel;

        protected MenuController menuController;
        protected PageController pageController;

        public BasePageModel()
        {
            menuController = new MenuController();
            pageController = new PageController();
        }

        public void OnGet(int id)
        {
            menuItems = menuController.SelectItem(id);
            pageModel = pageController.GetPage(id);
        }

        public ActionResult OnGetPartial() =>

            new PartialViewResult
            {
                ViewName = "Menu",
                ViewData = ViewData,
            };

    }
}
