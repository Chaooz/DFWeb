using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class PreviewModel : BasePageModel
    {
        public PreviewModel(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider,IImageProvider imageProvider) 
        : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
        }
    }
}
