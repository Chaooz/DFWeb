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
        IImageProvider _imageProvider;

        public EditController(IEditPageProvider editPageProvider, IPageProvider pageProvider, IImageProvider imageProvider)
        {
            _editPageProvider = editPageProvider;
            _pageProvider = pageProvider;
            _imageProvider = imageProvider;
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
                return Redirect("/Editor/EditPage?id=" + pageId);
            }
            return Redirect("/mainpage?id=" + pageId);
        }

        [HttpPost]
        [Route("DeletePage")]
        public IActionResult DeletePage([FromForm] int pageId)
        {
            if ( _editPageProvider.DeletePage(pageId) )
            {
                return Redirect("/");
            }            
            return Redirect("/page?id=" + pageId);
        }

        [HttpPost]
        [Route("AddImage")]
        public IActionResult AddImage([FromForm] int pageId, [FromForm] uint imageId)
        {
            if ( _editPageProvider.AddImage(pageId, imageId) )
            {
                return Redirect("/Editor/EditPage?id=" + pageId);
            }
            return Redirect("/Editor/EditPage?id=" + pageId + "&error=1");
        }

        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] int pageId, [FromForm] List<IFormFile> files)
        {
            uint imageId = await _imageProvider.UploadImage(pageId,files);
            if ( imageId != 0 &&  _editPageProvider.AddImage(pageId, imageId) )
            {
                return Redirect("/Editor/EditPage?id=" + pageId);
            }
            return Redirect("/Editor/EditPage?id=" + pageId + "&error=1");
        }

        [HttpPost]
        [Route("ChangeAccess")]
        public IActionResult ChangeAccess([FromForm] int pageId, [FromForm] int acl)
        {
            if ( _editPageProvider.ChangeAccess(pageId, acl) )
            {
                return Redirect("/Editor/EditPage?id=" + pageId);
            }
            return Redirect("/Editor/EditPage?id=" + pageId + "&error=2");
       }
    }
}