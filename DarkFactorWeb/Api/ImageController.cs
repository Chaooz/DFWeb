using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Core;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DarkFactorCoreNet.Api
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
            return Redirect("/admin/edit?id=" + pageId);
        }

        [HttpPost]
        [Route("GetImage")]
        public ImageModel GetImage([FromForm] int imageId)
        {
            return _imageProvider.GetImage(imageId);
        }
    }
}