using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Provider
{
    public interface IEditPageProvider
    {
        bool CreatePage( int pageId, string pageTitle );
        bool CreateChildPage( int parentPageId, string pageTitle );
        bool SaveFullPage(PageContentModel pageModel);
        bool MovePageUp(TeaserPageContentModel page);
        bool MovePageDown(TeaserPageContentModel page);
        bool EditPage(int pageId);
        bool AddImage(int pageID, uint imageId);
        bool DeletePage(int pageId);
        bool ChangeAccess(int pageId, int accessLevel);
    }

    public class EditPageProvider : IEditPageProvider
    {
        private IPageProvider _pageProvider;
        private IPageRepository _pageRepository;
        private IUserSessionProvider _userSession;
        private ILoginRepository _loginRepository;

        public EditPageProvider(
            IPageProvider pageProvider, 
            IUserSessionProvider userSession, 
            ILoginRepository loginRepository,
            IPageRepository pageRepository)
        {
            _pageProvider = pageProvider;
            _userSession = userSession;
            _loginRepository = loginRepository;
            _pageRepository = pageRepository;
        }

        public bool CreatePage( int pageId, string pageTitle )
        {
            if ( !CanEditPage() )
            {
                return false;
            }
            return _pageRepository.CreatePage(pageId,pageTitle);
        }
        
        public bool CreateChildPage( int parentPageId, string pageTitle )
        {
            if ( !CanEditPage() )
            {
                return false;
            }
            return _pageRepository.CreateChildPage(parentPageId,pageTitle);
        }


        public bool SaveFullPage(PageContentModel pageModel)
        {
            var editPage = _pageRepository.GetPage( pageModel.ID );
            editPage.Tags = pageModel.Tags;
            editPage.PromoText = pageModel.PromoText;
            editPage.PromoTitle = pageModel.PromoTitle;
            editPage.ContentText = pageModel.ContentText;
            editPage.ContentTitle = pageModel.ContentTitle;
            return _pageRepository.SavePage(editPage);
        }

        public bool MovePageUp(TeaserPageContentModel page)
        {
            if ( !CanEditPage() )
            {
                return false;
            }

            if ( page == null )
            {
                return false;
            }

            var pageList = _pageProvider.GetPagesWithParentId(page.ParentId);
            TeaserPageContentModel swapPage = pageList.OrderBy(x => x.SortId).Where(x => x.SortId < page.SortId).LastOrDefault();
            if (swapPage == null)
            {
                return false;
            }

            return SwapSort(page,swapPage);
       }

        public bool MovePageDown(TeaserPageContentModel page)
        {
            if ( !CanEditPage() )
            {
                return false;
            }

            if ( page == null )
            {
                return false;
            }

            var pageList = _pageProvider.GetPagesWithParentId(page.ParentId);
            TeaserPageContentModel swapPage = pageList.OrderBy(x => x.SortId).Where(x => x.SortId > page.SortId).FirstOrDefault();
            if (swapPage == null)
            {
                return false;
            }

            return SwapSort(page,swapPage);
        }

        private bool SwapSort(TeaserPageContentModel page1, TeaserPageContentModel page2)
        {
            var databasePage1 = _pageRepository.GetPage( page1.ID );
            var databasePage2 = _pageRepository.GetPage( page2.ID );

            databasePage1.SortId = page2.SortId;
            databasePage2.SortId = page1.SortId;

            bool didSave1 = _pageRepository.SavePage(databasePage1);
            bool didSave2 = _pageRepository.SavePage(databasePage2);
            return didSave1 && didSave2;
        }

        public bool EditPage(int pageId)
        {
            if ( !CanEditPage() )
            {
                return false;
            }
            return true;
        }

        private bool CanEditPage()
        {
            if ( !_userSession.IsLoggedIn() )
            {
                return false;
            }

            string username = _userSession.GetUsername();
            var userAccessLevel = _loginRepository.GetAccessForUser(username);
            if ( userAccessLevel < AccessLevel.Editor )
            {
                return false;
            }

            return true;
        }

        public bool AddImage(int pageID, uint imageId)
        {
            return _pageRepository.AddImage(pageID,imageId);
        }

        public bool DeletePage(int pageId)
        {
            if ( !CanEditPage() )
            {
                return false;
            }

            _pageRepository.DeletePage(pageId);
            return true;
        }

        public bool ChangeAccess(int pageId, int accessLevel)
        {
            if ( !CanEditPage() )
            {
                return false;
            }

            return _pageRepository.ChangeAccess(pageId,accessLevel);
        }
    }
}