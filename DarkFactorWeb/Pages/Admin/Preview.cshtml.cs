using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Controllers;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class PreviewModel : BasePageModel
    {
        public PreviewModel(IPageProvider pageProvider, IMenuProvider menuProvider) : base(pageProvider,menuProvider)
        {
        }

        override
        protected List<PageListModel> GetArticleSection(int id)
        {
            List<PageListModel> model = new List<PageListModel>();

            var subPages = GetSubPages(id);
            if (subPages.Pages.Count > 0)
            {
                model.Add(subPages);
            }

            return model;
        }

        private PageListModel GetSubPages(int id)
        {
            PageListModel model = new PageListModel();
            model.Title = "Sidemenu view";

            var page = pageProvider.GetPage(id);
            if ( page != null )
            {
                model.Pages.Add(page);
            }

            return model;
        }
    }
}
