using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Provider
{
    public interface IPageProvider
    {
        PageContentModel GetPage(int pageId);
        List<PageContentModel> GetPagesWithParentId(int parentId);
        List<PageContentModel> GetPagesWithTag(string tag);
        List<String> GetRelatedTags(int pageId);
        bool SwapSort(PageContentModel page1, PageContentModel page2);
    }

    public class PageProvider : IPageProvider
    {
        private IPageRepository pageRepository;

        public PageProvider(IPageRepository pageRepository)
        {
            this.pageRepository = pageRepository;
        }

        public PageContentModel GetPage(int pageId)
        {
            return pageRepository.GetPage(pageId);
        }

        public List<PageContentModel> GetPagesWithParentId(int parentId)
        {
            return pageRepository.GetPagesWithParentId(parentId);
        }

        public List<PageContentModel> GetPagesWithTag(string tag)
        {
            return pageRepository.GetPagesWithTag(tag);
        }

        public List<String> GetRelatedTags(int pageId)
        {
            return pageRepository.GetRelatedTags(pageId);
        }

        public bool SwapSort(PageContentModel page1, PageContentModel page2)
        {
            int swapSortId = page1.SortId;
            page1.SortId = page2.SortId;
            page2.SortId = swapSortId;

            bool didSave1 = pageRepository.SavePage(page1);
            bool didSave2 = pageRepository.SavePage(page2);
            return didSave1 && didSave2;
        }
    }
}