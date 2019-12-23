using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class EditModel : BasePageModel
    {
        private IPageRepository pageRepository;

        public EditModel(
            IPageRepository pageRepository, 
            IPageProvider pageProvider, 
            IMenuProvider menuProvider,
            ILoginProvider loginProvider) : base(pageProvider,menuProvider, loginProvider)
        {
            this.pageRepository = pageRepository;
        }

        public IActionResult OnPostAsync([FromForm] PageContentModel pageContentModel)
        {
            bool didSuceed = false;
            switch(pageContentModel.Command )
            {
                case "save":
                    didSuceed = pageRepository.SavePage(pageContentModel);
                    return Redirect("/admin/preview?id=" + pageContentModel.ID);
                case "move_up":
                    didSuceed = pageRepository.MovePageUp(pageContentModel.ID);
                    break;
                case "move_down":
                    didSuceed = pageRepository.MovePageDown(pageContentModel.ID);
                    break;
                case "create_page":
                    didSuceed = pageRepository.CreatePage(pageContentModel.ID);
                    break;
                case "create_child_page":
                    didSuceed = pageRepository.CreateChildPage(pageContentModel.ID);
                    break;
                case "delete_page":
                    pageContentModel.ID = pageRepository.DeletePage(pageContentModel.ID);
                    break;
            }

            if (pageContentModel != null && pageContentModel.ID != 0 )
            {
                return Redirect("/page?id=" + pageContentModel.ID);
            }
            return Redirect("/");
        }
    }
}
