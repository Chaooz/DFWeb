using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.Extensions.Configuration;

using DFWeb.BE.Models;
using DFWeb.BE.Provider;

namespace DFWeb.BE.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        IImageProvider _imageProvider;

        public ImageController(IImageProvider imageProvider)
        {
            _imageProvider = imageProvider;
        }
      
        // This action handles the form POST and the upload
        [HttpPost]
        [Route("UploadImage")]
        public async Task<uint> UploadImage([FromForm] int pageId, [FromForm] List<IFormFile> files)
        {        
            var ret = await _imageProvider.UploadImage(pageId,files);
            return ret;
        }

        [HttpPost]
        [Route("DeleteImage")]
        public IActionResult DeleteImage([FromForm] int pageId, [FromForm] int imageId)
        {
            _imageProvider.DeleteImage(imageId);
            return Redirect("/Editor/ImageSelector?pageId=" + pageId);
        }

        [HttpGet]
        [Route("GetImage")]
        public IActionResult  GetImage(int imageId)
        {
            var image = _imageProvider.GetRawImage(imageId);
            if ( image != null )
            {
                return File(image, "image/png");
            }
            return null;
        }

        // This action handles the form POST and the upload
        [HttpPost]
        [Route("UploadImageData")]
        public async Task<IActionResult> UploadImageData([FromForm] int pageId, [FromForm] int imageId, [FromForm] List<IFormFile> files)
        {        
            var ret = await _imageProvider.UpdateImageData(imageId,files);
            return Redirect("/Editor/EditImage?pageId=" + pageId + "&imageId=" + imageId);
        }
    }
}