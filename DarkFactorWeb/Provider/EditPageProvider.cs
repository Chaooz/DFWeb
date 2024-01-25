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
        bool MovePageUp(PageContentModel page);
        bool MovePageDown(PageContentModel page);
        bool EditPage(int pageId);
        bool AddImage(int pageID, uint imageId);
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

        public bool MovePageUp(PageContentModel page)
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
            PageContentModel swapPage = pageList.OrderBy(x => x.SortId).Where(x => x.SortId < page.SortId).LastOrDefault();
            if (swapPage == null)
            {
                return false;
            }

            return SwapSort(page,swapPage);
       }

        public bool MovePageDown(PageContentModel page)
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
            PageContentModel swapPage = pageList.OrderBy(x => x.SortId).Where(x => x.SortId > page.SortId).FirstOrDefault();
            if (swapPage == null)
            {
                return false;
            }

            return SwapSort(page,swapPage);
        }

        private bool SwapSort(PageContentModel page1, PageContentModel page2)
        {
            int swapSortId = page1.SortId;
            page1.SortId = page2.SortId;
            page2.SortId = swapSortId;

            bool didSave1 = _pageRepository.SavePage(page1);
            bool didSave2 = _pageRepository.SavePage(page2);
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
    }
}