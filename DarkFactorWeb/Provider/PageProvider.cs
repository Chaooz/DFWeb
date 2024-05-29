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
        List<TeaserPageContentModel> GetPagesWithParentId(int parentId);
        List<TeaserPageContentModel> GetPagesWithTag(string tag);
        List<String> GetRelatedTags(int pageId);
        IList<ArticleSectionModel> GetArticleSections(int pageId);
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

        public List<TeaserPageContentModel> GetPagesWithParentId(int parentId)
        {
            return pageRepository.GetPagesWithParentId(parentId);
        }

        public List<TeaserPageContentModel> GetPagesWithTag(string tag)
        {
            return pageRepository.GetPagesWithTag(tag);
        }

        public List<String> GetRelatedTags(int pageId)
        {
            return pageRepository.GetRelatedTags(pageId);
        }

        public IList<ArticleSectionModel> GetArticleSections(int pageId)
        {
            return pageRepository.GetArticleSections(pageId);
        }
    }
}