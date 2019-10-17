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
    public class MainPage : BasePageModel
    {
        public PageListModel mainPageItems;

        public MainPage(IPageCollector pageController, IMenuCollector menuController) : base(pageController,menuController)
        {
        }

        override
        public void OnGet(int id)
        {
            base.OnGet(id);

            mainPageItems = GetSubPages(id);
        }

        override
        protected List<PageListModel> GetArticleSection(int id)
        {
            List<PageListModel> model = new List<PageListModel>();

            var aboutMe = GetPagesWithTag("Thor");
            if (aboutMe.Pages.Count > 0)
            {
                model.Add(aboutMe);
            }

            var aboutDarkFactor = GetPagesWithTag("DarkFactor");
            if (aboutDarkFactor.Pages.Count > 0)
            {
                model.Add(aboutDarkFactor);
            }

            return model;
        }

        private PageListModel GetSubPages(int parentId)
        {
            PageListModel model = new PageListModel();
            model.Pages = pageController.GetPagesWithParentId(parentId);
            return model;
        }
    }
}
