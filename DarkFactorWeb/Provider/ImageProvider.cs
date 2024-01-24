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
    public interface IImageProvider
    {
        bool AddImage(int pageId, String filename, byte[] data);
        bool DeleteImage(int imageId);
        ImageModel GetImage(int imageId);
    }

    public class ImageProvider : IImageProvider
    {
        private IUserSessionProvider _userSession;
        private ILoginRepository _loginRepository;
        private IImageRepository _imageRepository;

        public ImageProvider(
            IUserSessionProvider userSession, 
            ILoginRepository loginRepository,
            IImageRepository imageRepository)
        {
            _userSession = userSession;
            _loginRepository = loginRepository;
            _imageRepository = imageRepository;
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

        public bool AddImage(int pageId, String filename, byte[] data)
        {
            if ( !CanEditPage() )
            {
                return false;
            }
 
            return _imageRepository.AddImage(pageId,filename, data);
        }

        public bool DeleteImage(int imageId)
        {
            if ( !CanEditPage() )
            {
                return false;
            } 
            return _imageRepository.DeleteImage(imageId);
        }

        public ImageModel GetImage(int imageId)
        {
            return _imageRepository.GetImage(imageId);
        }
    }
}