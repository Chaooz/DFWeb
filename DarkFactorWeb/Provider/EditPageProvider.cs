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
    }

    public class EditPageProvider : IEditPageProvider
    {
        private IPageProvider _pageProvider;
        private IUserSessionProvider _userSession;
        private ILoginRepository _loginRepository;

        public EditPageProvider(IPageProvider pageProvider, IUserSessionProvider userSession, ILoginRepository loginRepository)
        {
            _pageProvider = pageProvider;
            _userSession = userSession;
            _loginRepository = loginRepository;
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

            return _pageProvider.SwapSort(page,swapPage);
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

            return _pageProvider.SwapSort(page,swapPage);
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
            UserModel userModel = _loginRepository.GetUserWithUsername(username);
            if ( userModel == null )
            {
                return false;
            }

            if ( userModel.UserAccessLevel < AccessLevel.Editor )
            {
                return false;
            }

            return true;
        }
    }
}