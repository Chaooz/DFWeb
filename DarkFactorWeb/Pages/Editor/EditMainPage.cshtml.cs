using System;
using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;
using Microsoft.AspNetCore.Mvc;

namespace DarkFactorCoreNet.Pages
{
    public class EditMainPage : MainPage
    {
        private IEditPageProvider _editPageProvider;

        public EditMainPage( IEditPageProvider editPageProvider, IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider) 
        : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
            _editPageProvider = editPageProvider;
        }

        override
        public void OnGet(int id)
        {
            base.OnGet(id);

            var userInfo = _loginProvider.GetLoginInfo();
            if ( userInfo != null )
            {
                mainPageItems.ShowEditor = userInfo.UserAccessLevel >= (int)AccessLevel.Editor;
            }
        }

        public IActionResult OnPostAsync([FromForm] string pageId, [FromForm] string title, [FromForm] string relatedTags, [FromForm] string command )
        {
            switch(command )
            {
                case "save":
                    // didSuceed = _editPageProvider.SaveFullPage(pageContentModel);
                    return Redirect("/MainPage?id=" + pageId);
                // case "create_child_page":
                //     didSuceed = editPageProvider.CreateChildPage(pageContentModel.ID, "New page");
                //     break;
                // case "delete_page":
                //     didSuceed = editPageProvider.DeletePage(pageContentModel.ID);
                //     break;
            }
            return Redirect("/");
        }
    }
}
