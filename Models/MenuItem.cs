using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DarkFactorCoreNet.Models
{
    public class MenuItem
    {
        public static string CLASS_MENU = "menu";
        public static string CLASS_SUBMENU = "submenu";
        public static string CLASS_DRAFTMENU = "menu_editor";
        public static string CLASS_DRAFTSUBMENU = "submenu_editor";

        public int ID;
        public int ParentID;
        public string Name;
        public bool IsPublished;
        public string MenuClass;

        public MenuItem()
        {
        }
    };
}
