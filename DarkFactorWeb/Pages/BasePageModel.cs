﻿using System;
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

        protected IMenuProvider menuProvider;
        protected IPageProvider pageProvider;

        public BasePageModel(IPageProvider pageProvider, IMenuProvider menuProvider)
        {
            this.menuProvider = menuProvider;
            this.pageProvider = pageProvider;
            pageId = 0;
        }

        virtual
        public void OnGet(int id)
        {
            menuItems = menuProvider.SelectItem(id);
            treeList = menuProvider.GetTree(id);
            pageModel = pageProvider.GetPage(id);
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

        protected List<String> GetRelatedTags(int pageId)
        {
            return pageProvider.GetRelatedTags(pageId);
        }

        protected PageListModel GetPagesWithTag(string title, string tag)
        {
            PageListModel pageListModel = new PageListModel();
            pageListModel.Title = title;
            pageListModel.Pages = pageProvider.GetPagesWithTag(tag);
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