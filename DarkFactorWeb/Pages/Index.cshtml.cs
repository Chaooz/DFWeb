using DarkFactorCoreNet.Provider;

namespace DarkFactorCoreNet.Pages
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IPageProvider pageProvider, IMenuProvider menuProvider, ILoginProvider loginProvider) : base(pageProvider,menuProvider, loginProvider)
        {
        }
    }
}
