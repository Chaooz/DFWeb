using System;
using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using System.Linq;

using DarkFactorCoreNet.Repository;

namespace DarkFactorCoreNet.Controllers
{
    public class MenuCollector
    {
        public List<MenuItem> menuItems;
        private MenuRepository menuRepository;

        public MenuCollector()
        {
            menuRepository = new MenuRepository();
            menuItems = menuRepository.GetAllItems();

            /*
            menuItems.Add(new MenuItem() { ID = 1, ParentID = 0, Name = "H O M E", IsPublished = true });
            menuItems.Add(new MenuItem() { ID = 2, ParentID = 0, Name = "Codemonkey Blog", IsPublished = false });
            menuItems.Add(new MenuItem() { ID = 3, ParentID = 2, Name = ".Net Core 2 Website", IsPublished = false });
            menuItems.Add(new MenuItem() { ID = 9, ParentID = 2, Name = ".Heml for dummies", IsPublished = true });
            menuItems.Add(new MenuItem() { ID = 4, ParentID = 0, Name = "Games", IsPublished = true });
            menuItems.Add(new MenuItem() { ID = 5, ParentID = 0, Name = "Apps", IsPublished = true });
            menuItems.Add(new MenuItem() { ID = 7, ParentID = 4, Name = "Noid", IsPublished = true });
            menuItems.Add(new MenuItem() { ID = 8, ParentID = 4, Name = "Valyrian Adventures", IsPublished = true });
            menuItems.Add(new MenuItem() { ID = 10, ParentID = 8, Name = "Screenshot", IsPublished = true });
            */
        }

        public int GetDefaultId()
        {
            return menuItems[0].ID;
        }

        public List<MenuItem> GetTree( int pageId )
        {
            List<int> selectedTree = new List<int>();
            GetSelectedTree(selectedTree, pageId);

            List<MenuItem> menuTreeList = new List<MenuItem>();

            foreach( var id in selectedTree)
            {
                var menuItem = GetMenuItem(id);
                menuTreeList.Add(menuItem);
            }
            return menuTreeList;
        }

        public List<MenuItem> SelectItem( int selectedItemId )
        {
            // Create the expanded tree
            List<int> selectedTree = new List<int>();
            GetSelectedTree(selectedTree, selectedItemId);

            // Add top nodes
            List<MenuItem> visibleItems = new List<MenuItem>();
            AddItemsWithParent(visibleItems, selectedTree, selectedItemId, 180);
            return visibleItems;
        }

        private void AddItemsWithParent(List<MenuItem> visibleItems, List<int> selectedTree, int parentId, int width )
        {
            int selectedId = selectedTree.LastOrDefault();
            foreach (var menuItem in menuItems)
            {
                if (menuItem.ParentID == parentId)
                {
                    menuItem.MenuClass = GetMenuClass(menuItem.IsPublished, parentId == 0, selectedId == menuItem.ID);
                    menuItem.Width = width;
                    visibleItems.Add(menuItem);

                    // Expand child tree
                    if ( selectedTree.Contains( menuItem.ID ) )
                    {
                        AddItemsWithParent(visibleItems, selectedTree, menuItem.ID, width - 20);
                    }
                }
            }
        }

        private string GetMenuClass(bool isPublished, bool isMainItem, bool isSelected)
        {
            if (isSelected && isMainItem)
            {
                return MenuItem.CLASS_SELECTED;
            }
            else if ( isSelected && !isMainItem )
            {
                return MenuItem.CLASS_SELECTED_SUB;
            }
            else if ( isPublished && isMainItem )
            {
                return MenuItem.CLASS_MENU;
            }
            else if ( isPublished && !isMainItem )
            {
                return MenuItem.CLASS_SUBMENU;
            }
            else if ( !isPublished && isMainItem )
            {
                return MenuItem.CLASS_DRAFTMENU;
            }
            else
            {
                return MenuItem.CLASS_DRAFTSUBMENU;
            }
        }

        private void GetSelectedTree(List<int> expandedTree, int itemId)
        {
            if ( itemId != 0)
            {
                var menuItem = GetMenuItem(itemId);
                if (menuItem != null && menuItem.ParentID != 0)
                {
                    GetSelectedTree(expandedTree, menuItem.ParentID);
                }
                expandedTree.Add(itemId);
            }
        }

        private MenuItem GetMenuItem( int menuId )
        {
            return menuItems.Where(x => x.ID == menuId).FirstOrDefault();
        }
    }
}
