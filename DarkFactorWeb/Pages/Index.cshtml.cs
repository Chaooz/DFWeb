using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class IndexModel : MainPage
    {
        public IndexModel(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider)
         : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
        }

        override
        public List<TeaserPageContentModel> GetPageArticles(int pageId)
        {
            return  pageProvider.GetNewArticles(10);
        }
    }
}
