using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DarkFactorCoreNet.Provider
{
    public interface IImageProvider
    {
        Task<uint> UploadImage(int pageId,  List<IFormFile> files);
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


        public async Task<uint> UploadImage(int pageId,  List<IFormFile> files)
        {
            if ( !CanEditPage() )
            {
                return 0;
            }

            // Only upload the first file
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //using (var memoryStream = System.IO.File.Create(filePath))
                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);
                        var fileArray = memoryStream.ToArray();

                        return _imageRepository.UploadImage(pageId,formFile.FileName, fileArray);
                    }
                }
            }
            return 0;
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