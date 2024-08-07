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
        Task<bool> UpdateImageData(int imageId,  List<IFormFile> files);
        bool DeleteImage(int imageId);
        ImageModel GetImage(int imageId);
        byte[] GetRawImage(int imageId);
        IList<ImageModel> GetImages(int imagesPrPage, int pageNumber);
        bool UpdateImage(int imageId, string filename);
    }

    public class ImageProvider : IImageProvider
    {
        private IUserSessionProvider _userSession;
        private IImageRepository _imageRepository;

        public ImageProvider(
            IUserSessionProvider userSession, 
            IImageRepository imageRepository)
        {
            _userSession = userSession;
            _imageRepository = imageRepository;
        }

        private bool CanEditPage()
        {
            return _userSession.CanEditPage();
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

        public async Task<bool> UpdateImageData(int imageId,  List<IFormFile> files)
        {
            if ( !CanEditPage() )
            {
                return false;
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

                        return _imageRepository.UpdateImageData(imageId,formFile.FileName, fileArray);
                    }
                }
            }
            return false;

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

        public byte[] GetRawImage(int imageId)
        {
            return _imageRepository.GetRawImage(imageId);
        }

        public IList<ImageModel> GetImages(int imagesPrPage, int pageNumber)
        {
            if ( CanEditPage() )
            {
                return _imageRepository.GetImages(imagesPrPage,pageNumber);
            } 
            return null;
        }

        public bool UpdateImage(int imageId, string filename)
        {
            return _imageRepository.UpdateImage(imageId, filename);
        }
    }
}