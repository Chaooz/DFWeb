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
        private IEditPageProvider editPageProvider;

        public EditModel(
            IEditPageProvider editPageProvider, 
            IImageProvider imageProvider,
            IPageProvider pageProvider, 
            IMenuProvider menuProvider,
            ILoginProvider loginProvider) : base(pageProvider,menuProvider, loginProvider,imageProvider)
        {
            this.editPageProvider = editPageProvider;
        }

        override
        public void OnGet(int id)
        {
            base.OnGet(id);

            if ( pageModel != null )
            {
                pageModel.IsEdit = true;
            }
        }

        public IActionResult OnPostAsync([FromForm] PageContentModel pageContentModel)
        {
            bool didSuceed = false;
            switch(pageContentModel.Command )
            {
                case "save":
                    didSuceed = editPageProvider.SaveFullPage(pageContentModel);
                    return Redirect("/page?id=" + pageContentModel.ID);
                case "create_page":
                    didSuceed = editPageProvider.CreatePage(pageContentModel.ID,"New page");
                    break;
                case "create_child_page":
                    didSuceed = editPageProvider.CreateChildPage(pageContentModel.ID, "New page");
                    break;
                case "delete_page":
                    didSuceed = editPageProvider.DeletePage(pageContentModel.ID);
                    break;
            }

            if (pageContentModel != null && pageContentModel.ID != 0 )
            {
                return Redirect("/page?id=" + pageContentModel.ID);
            }
            return Redirect("/");
        }

        public IActionResult OnPostCreateArticleSection([FromForm] int pageId)
        {
            editPageProvider.CreateArticleSection(pageId, "New section", "New content");
            return Redirect("/Editor/EditPage?id=" + pageId);
        }

        public IActionResult OnPostUpdateArticleSection([FromForm] int pageId)
        {
            editPageProvider.CreateArticleSection(pageId, "New section", "New content");
            return Redirect("/Editor/EditPage?pageId=" + pageId);
        }
    }
}
