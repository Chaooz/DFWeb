using System.Collections.Generic;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class Page : BasePageModel
    {
        public IList<ArticleSectionModel> articleSections;

        public Page(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider)
         : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
            articleSections = new List<ArticleSectionModel>();
        }

        override
        public void OnGet(int id)
        {
            base.OnGet(id);
            articleSections = pageProvider.GetArticleSections(id);

            // Load images. TODO: Do this in Javascript
            foreach (ArticleSectionModel section in articleSections)
            {
                if (section.ImageId != 0 )
                {
                    section.ImageModel = _imageProvider.GetImage(section.ImageId);
                }
            }
        }
    }
}
