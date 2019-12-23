using System;
using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class MainPage : BasePageModel
    {
        public PageListModel mainPageItems;

        public MainPage(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider) : base(pageProvider,menuProvider, loginProvider)
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
