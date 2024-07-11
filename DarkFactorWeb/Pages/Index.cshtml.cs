using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class IndexModel : BasePageModel
    {
        public PageListModel mainPageItems;

        public IndexModel(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider, IImageProvider imageProvider)
         : base(pageProvider,menuProvider, loginProvider, imageProvider)
        {
        }

        override
        public void OnGet(int id)
        {
            base.OnGet(1);
            mainPageItems = GetSubPages();
        }

        //
        // Get all articles on this page
        // TODO: Rename this to ArticleTeaserModel
        //
        private PageListModel GetSubPages()
        {
            PageListModel model = new PageListModel();
            model.Title = "Main Page";
            model.Pages = pageProvider.GetNewArticles(10);

            var userInfo = _loginProvider.GetLoginInfo();
            if ( userInfo != null )
            {
                model.ShowEditor = userInfo.UserAccessLevel >= (int)AccessLevel.Editor;
            }

            return model;
        }
    }
}
