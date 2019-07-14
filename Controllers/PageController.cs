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
    public class PageController
    {
        private PageRepository pageRepository;

        public PageController()
        {
            pageRepository = new PageRepository();
        }

        public PageContentModel GetPage(int pageId)
        {
            return pageRepository.GetPage(pageId);
        }
    }
}