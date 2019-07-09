using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Controllers;

namespace aspnetcoreapp.Pages
{
    public class IndexModel : PageModel
    {
        public List<MenuItem> menuItems;

        private MenuController menuController;
        private PageController pageController;

        public IndexModel()
        {
            menuController = new MenuController();
            pageController = new PageController();
        }

        //public void OnGet()
        //{
        //    menuItems = menuController.GetDefaultSelection();
        //}

        public void OnGet(int id)
        {
            menuItems = menuController.SelectItem(id);
        }

        public ActionResult OnGetPartial() =>

            new PartialViewResult
            {
                ViewName = "Menu",
                ViewData = ViewData,
            };
        
    }
}
