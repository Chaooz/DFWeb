using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

using DFWeb.BE.Models;
using DFWeb.BE.Repository;

namespace DFWeb.BE.Provider
{
    public interface IPageProvider
    {
        int GetMainPageId();
        PageContentModel GetPage(int pageId);
        List<TeaserPageContentModel> GetPagesWithParentId(int parentId);
        List<TeaserPageContentModel> GetPagesWithTag(string tag);
        List<TeaserPageContentModel> GetNewArticles(int maxArticles);
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

        public int GetMainPageId()
        {
            return pageRepository.GetMainPageId();
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

        public List<TeaserPageContentModel> GetNewArticles(int maxArticles)
        {
            return pageRepository.GetNewArticles(maxArticles);
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