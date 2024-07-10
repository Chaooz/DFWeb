using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class FullPageModel : BasePageModel
    {
        public IList<ArticleSectionModel> articleSections;

        public FullPageModel(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider)
         : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
            articleSections = new List<ArticleSectionModel>();
        }

        override
        public void OnGet(int id)
        {
            base.OnGet(id);
            articleSections = pageProvider.GetArticleSections(id);
        }
    }
}
