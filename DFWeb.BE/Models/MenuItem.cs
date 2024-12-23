﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DFWeb.BE.Models
{
    public class MenuItem
    {
        public static string CLASS_SELECTED = "selectedmenu";
        public static string CLASS_SELECTED_SUB = "selectedsubmenu";
        public static string CLASS_MENU = "menu";
        public static string CLASS_SUBMENU = "submenu";
        public static string CLASS_DRAFTMENU = "menu_editor";
        public static string CLASS_DRAFTSUBMENU = "submenu_editor";

        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public bool IsPublished { get; set; }

        // Temp classes
        public string MenuClass;
        public int Width;

        public MenuItem()
        {
        }
    };
}
