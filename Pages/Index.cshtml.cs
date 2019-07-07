using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Models;

namespace aspnetcoreapp.Pages
{
    public class IndexModel : PageModel
    {
        public List<MenuItem> list;

        public void OnGet()
        {
            list = new List<MenuItem>();
            list.Add(new MenuItem() { ID = 1, ParentID = 0, Name = "H O M E", MenuClass = MenuItem.CLASS_MENU });
            list.Add(new MenuItem() { ID = 2, ParentID = 0, Name = "Codemonkey Blog", MenuClass = MenuItem.CLASS_DRAFTMENU });
            list.Add(new MenuItem() { ID = 3, ParentID = 2, Name = ".Net Core 2 Website", MenuClass = MenuItem.CLASS_DRAFTSUBMENU });
        }

        public ActionResult OnGetPartial() =>

            new PartialViewResult
            {
                ViewName = "Menu",
                ViewData = ViewData,
            };
        
    }
}
