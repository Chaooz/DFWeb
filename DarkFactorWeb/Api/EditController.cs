using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Core;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Provider;

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
            _editPageProvider.EditPage(pageId);
            return null;
        }

    }
}