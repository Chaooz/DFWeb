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
        public List<TeaserPageContentModel> relatedPages;
        public int PageId;
        public string Title;

        public string EditUrl;

        protected IPageProvider pageProvider;
        protected IImageProvider _imageProvider;

        public BasePageModel(   IPageProvider pageProvider, 
                                IMenuProvider menuProvider, 
                                ILoginProvider loginProvider,
                                IImageProvider imageProvider) : base(menuProvider, loginProvider)
        {
            this.pageProvider = pageProvider;
            _imageProvider = imageProvider;
            PageId = 0;
        }

        virtual
        public void OnGet(int id)
        {
            base.GetMenuData(id);
            string relatedTags = "";
            pageModel = pageProvider.GetPage(id);
            if ( pageModel != null )
            {
                Title = pageModel.ContentTitle;
                relatedTags = pageModel.RelatedTags;
            }
            relatedPages = GetRelatedPages(relatedTags);
            PageId = id;
        }

        protected List<TeaserPageContentModel> GetRelatedPages(string relatedTags)
        {
            List<TeaserPageContentModel> relatedPages = new List<TeaserPageContentModel>();

            var tagList = relatedTags.Split(" ");
            foreach( string tag in tagList)
            {
                var pagesWithTag = pageProvider.GetPagesWithTag(tag);
                if (pagesWithTag.Count > 0)
                {
                    relatedPages.AddRange(pagesWithTag);
                }
            }
            return relatedPages;
        }
    }
}
