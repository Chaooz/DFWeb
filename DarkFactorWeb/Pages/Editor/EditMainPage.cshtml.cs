using System;
using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;
using Microsoft.AspNetCore.Mvc;

namespace DarkFactorCoreNet.Pages
{
    public class EditMainPage : MainPage
    {
        public bool ShowEditor;

        public EditMainPage( IEditPageProvider editPageProvider, IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider) 
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
                ShowEditor = userInfo.UserAccessLevel >= (int)AccessLevel.Editor;
            }
        }
    }
}
