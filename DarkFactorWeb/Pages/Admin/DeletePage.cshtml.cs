using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using DFWeb.BE.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class DeleteModel : BasePageModel
    {
        public DeleteModel(IPageProvider pageProvider, 
            IMenuProvider menuProvider, 
            ILoginProvider loginProvider,
            IImageProvider imageProvider) : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
        }

        override
        public void OnGet(int id)
        {
            base.OnGet(id);
            Title = "Delete page";
        }
    }
}
