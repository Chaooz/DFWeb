using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Controllers
{
    public interface IPageCollector
    {
        PageContentModel GetPage(int pageId);
        List<PageContentModel> GetPagesWithParentId(int parentId);
        List<PageContentModel> GetPagesWithTag(string tag);
    }

    public class PageCollector : IPageCollector
    {
        private IPageRepository pageRepository;

        public PageCollector(IPageRepository pageRepository)
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
    }
}