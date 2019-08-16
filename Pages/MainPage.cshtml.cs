using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Controllers;

namespace DarkFactorCoreNet.Pages
{
    public class MainPage : BasePageModel
    {
        override
        protected List<PageListModel> GetArticleSection(int id)
        {
            List<PageListModel> model = new List<PageListModel>();

            PageListModel row1 = new PageListModel();
            PageListModel row2 = new PageListModel();
            PageListModel row3 = new PageListModel();

            int index = 0;
            var subPages = GetSubPages(id);
            foreach( var page in subPages.Pages )
            {
                int m = index % 3;
                switch( m )
                {
                    case 0: row1.Pages.Add(page); break;
                    case 1: row2.Pages.Add(page); break;
                    case 2: row3.Pages.Add(page); break;
                }
                index++;
            }

            model.Add(row1);
            model.Add(row2);
            model.Add(row3);

            return model;
        }

        private PageListModel GetSubPages(int parentId)
        {
            PageListModel model = new PageListModel();
            model.Pages = pageController.GetPagesWithParentId(parentId);
            return model;
        }
    }
}
