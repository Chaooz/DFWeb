using System;
using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class MainPage : BasePageModel
    {
        public List<TeaserPageContentModel> mainPageItems;

        public MainPage(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider) 
        : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
        }

        override
        public void OnGet(int id)
        {
            base.OnGet(id);
            mainPageItems = GetPageArticles(id);
        }

        virtual
        public List<TeaserPageContentModel> GetPageArticles(int pageId)
        {
            return pageProvider.GetPagesWithParentId(pageId);
        }
    }
}
