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
    public class PageCollector
    {
        private PageRepository pageRepository;

        public PageCollector()
        {
            pageRepository = new PageRepository();
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