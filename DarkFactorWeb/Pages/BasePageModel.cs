using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

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
        protected ILoginProvider _loginProvider;
        protected IImageProvider _imageProvider;

        public UserInfoModel UserInfoModel { get; set; }

        public BasePageModel(   IPageProvider pageProvider, 
                                IMenuProvider menuProvider, 
                                ILoginProvider loginProvider,
                                IImageProvider imageProvider)
        {
            this.menuProvider = menuProvider;
            this.pageProvider = pageProvider;
            _loginProvider = loginProvider;
            _imageProvider = imageProvider;
            pageId = 0;
        }

        virtual
        public void OnGet(int id)
        {
            menuItems = menuProvider.SelectItem(id);
            treeList = menuProvider.GetTree(id);
            pageModel = pageProvider.GetPage(id);
            articleSectionModel = GetArticleSection(id);
            UserInfoModel = _loginProvider.GetLoginInfo();
            pageId = id;

            if ( pageModel != null )
            {
                pageModel.ImageModel = _imageProvider.GetImage(pageModel.ImageId);
            }
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
