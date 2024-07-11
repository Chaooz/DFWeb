using System;
using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class EditMainPage : MainPage
    {
        public EditMainPage(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider) 
        : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
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
    }
}
