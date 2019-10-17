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

        public MainPage(IPageProvider pageProvider, IMenuProvider menuProvider) : base(pageProvider,menuProvider)
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

            List<String> tagList = GetRelatedTags(id);
            foreach( String tag in tagList)
            {
                var tagPage = GetPagesWithTag(tag);
                if (tagPage.Pages.Count > 0)
                {
                    model.Add(tagPage);
                }
            }
            return model;
        }

        private PageListModel GetSubPages(int parentId)
        {
            PageListModel model = new PageListModel();
            model.Pages = pageProvider.GetPagesWithParentId(parentId);
            return model;
        }
    }
}
