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
        [Route("AddImage")]
        public async Task<IActionResult> AddImage([FromForm] int pageId, [FromForm] List<IFormFile> files)
        {
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    //using (var memoryStream = System.IO.File.Create(filePath))
                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);
                        var fileArray = memoryStream.ToArray();

                        _imageProvider.AddImage(pageId, formFile.FileName, fileArray);
                    }
                }
            }
            return Redirect("/admin/edit?id=" + pageId);
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
        public string GetImage([FromForm] int imageId)
        {
            return _imageProvider.GetImage(imageId);
        }

    }
}