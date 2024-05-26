using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class BasePageModel : MenuPageModel
    {
        public PageContentModel pageModel;
        public List<PageListModel> relatedPages;
        public int pageId;

        protected IPageProvider pageProvider;
        protected IImageProvider _imageProvider;


        public BasePageModel(   IPageProvider pageProvider, 
                                IMenuProvider menuProvider, 
                                ILoginProvider loginProvider,
                                IImageProvider imageProvider) : base(menuProvider, loginProvider)
        {
            this.pageProvider = pageProvider;
            _imageProvider = imageProvider;
            pageId = 0;
        }

        virtual
        public void OnGet(int id)
        {
            base.GetMenuData(id);

            pageModel = pageProvider.GetPage(id);
            relatedPages = GetRelatedPages(id);
            pageId = id;

            if ( pageModel != null )
            {
                pageModel.ImageModel = _imageProvider.GetImage(pageModel.ImageId);
            }
        }

        protected List<PageListModel> GetRelatedPages(int id)
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
    }
}
