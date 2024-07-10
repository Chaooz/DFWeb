using System;
using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class MainPage : BasePageModel
    {
        public PageListModel mainPageItems;

        public MainPage(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider) 
        : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
        }

        override
        public void OnGet(int id)
        {
            base.OnGet(id);
            mainPageItems = GetSubPages(id);
        }

        //
        // Get all articles on this page
        // TODO: Rename this to ArticleTeaserModel
        //
        protected PageListModel GetSubPages(int parentId)
        {
            PageListModel model = new PageListModel();
            model.Title = "Main Page";
            model.Pages = pageProvider.GetPagesWithParentId(parentId);

            var userInfo = _loginProvider.GetLoginInfo();
            if ( userInfo != null )
            {
                model.ShowEditor = userInfo.UserAccessLevel >= (int)AccessLevel.Editor;
            }

            return model;
        }
    }
}
