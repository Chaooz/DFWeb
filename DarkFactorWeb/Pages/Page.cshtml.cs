using System.Collections.Generic;
using DFWeb.BE.Models;
using DFWeb.BE.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class Page : FullPageModel
    {
        public Page(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider)
         : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
        }
    }
}
