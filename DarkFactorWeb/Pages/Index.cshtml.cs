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
        public void OnGet(int id)
        {
            base.OnGet(1);
        }
    }
}
