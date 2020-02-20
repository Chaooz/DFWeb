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
    public class EditController : ControllerBase
    {
        IPageProvider _pageProvider;
        IEditPageProvider _editPageProvider;

        public EditController(IEditPageProvider editPageProvider, IPageProvider pageProvider)
        {
            _editPageProvider = editPageProvider;
            _pageProvider = pageProvider;
        }

        [HttpPost]
        [Route("MoveDown")]
        public IActionResult MoveDown([FromForm] int pageId)
        {
            PageContentModel page = _pageProvider.GetPage(pageId);
            if ( page != null )
            {
                _editPageProvider.MovePageDown(page);
                return Redirect("/mainpage?id=" + page.ParentId);
            }
            return Redirect("/mainpage?id=" + pageId);
        }

        [HttpPost]
        [Route("MoveUp")]
        public IActionResult MoveUp([FromForm] int pageId)
        {
            PageContentModel page = _pageProvider.GetPage(pageId);
            if ( page != null )
            {
                _editPageProvider.MovePageUp(page);
                return Redirect("/mainpage?id=" + page.ParentId);
            }
            return Redirect("/mainpage?id=" + pageId);
        }

        [HttpPost]
        [Route("EditPage")]
        public IActionResult EditPage([FromForm] int pageId)
        {
            if ( _editPageProvider.EditPage(pageId) )
            {
                return Redirect("/admin/edit?id=" + pageId);
            }
            return Redirect("/mainpage?id=" + pageId);
        }

        [HttpPost]
        [Route("SavePromo")]
        public IActionResult SavePromo([FromForm] int pageId)
        {
            PageContentModel page = _pageProvider.GetPage(pageId);
            if ( page != null )
            {
                _editPageProvider.MovePageUp(page);
                return Redirect("/mainpage?id=" + page.ParentId);
            }
            return Redirect("/mainpage?id=" + pageId);
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

                        _editPageProvider.AddImage(pageId, formFile.FileName, fileArray);
                    }
                }
            }
            return Redirect("/admin/edit?id=" + pageId);
        }

        [HttpPost]
        [Route("DeleteImage")]
        public IActionResult DeleteImage([FromForm] int pageId, [FromForm] int imageId)
        {
            _editPageProvider.DeleteImage(imageId);
            return Redirect("/admin/edit?id=" + pageId);
        }
    }
}