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
        public List<MenuItem> treeList;
        public List<MenuItem> menuItems;
        public PageContentModel pageModel;
        public List<PageListModel> articleSectionModel;
        public int pageId;

        protected MenuCollector menuController;
        protected PageCollector pageController;

        public BasePageModel()
        {
            menuController = new MenuCollector();
            pageController = new PageCollector();
            pageId = 0;
        }

        virtual
        public void OnGet(int id)
        {
            menuItems = menuController.SelectItem(id);
            treeList = menuController.GetTree(id);
            pageModel = pageController.GetPage(id);
            articleSectionModel = GetArticleSection(id);
            pageId = id;
        }

        virtual
        protected List<PageListModel> GetArticleSection(int id)
        {
            return null;
        }

        protected PageListModel GetPagesWithTag(string tag)
        {
            return GetPagesWithTag(tag, tag);
        }

        protected PageListModel GetPagesWithTag(string title, string tag)
        {
            PageListModel pageListModel = new PageListModel();
            pageListModel.Title = title;
            pageListModel.Pages = pageController.GetPagesWithTag(tag);
            return pageListModel;
        }

        public ActionResult OnGetPartial() =>

            new PartialViewResult
            {
                ViewName = "Menu",
                ViewData = ViewData,
            };

    }
}
