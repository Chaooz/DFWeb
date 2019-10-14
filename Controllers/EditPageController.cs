using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Repository;

namespace DarkFactorCoreNet.Controllers
{
    [Route("api/editor")]
    [ApiController]
    public class EditPageController : ControllerBase
    {
        private PageRepository pageRepository;

        public EditPageController()
        {
            pageRepository = new PageRepository();
        }

        [HttpGet]
        public PageContentModel GetModel( int pageId )
        {
            PageContentModel model = pageRepository.GetPage(pageId);
            return model;
        }

        [HttpPost]
        public IActionResult UpdatePageData([FromForm] PageContentModel pageContentModel)
        {
            bool didSuceed = false;
            switch(pageContentModel.Command )
            {
                case "save":
                    didSuceed = pageRepository.SavePage(pageContentModel);
                    return Redirect("/admin/preview?id=" + pageContentModel.ID);
                case "move_up":
                    didSuceed = pageRepository.MovePageUp(pageContentModel.ID);
                    break;
                case "move_down":
                    didSuceed = pageRepository.MovePageDown(pageContentModel.ID);
                    break;
                case "create_page":
                    didSuceed = pageRepository.CreatePage(pageContentModel.ID);
                    break;
                case "create_child_page":
                    didSuceed = pageRepository.CreateChildPage(pageContentModel.ID);
                    break;
                case "delete_page":
                    pageContentModel.ID = pageRepository.DeletePage(pageContentModel.ID);
                    break;
            }

            if (pageContentModel != null && pageContentModel.ID != 0 )
            {
                return Redirect("/page?id=" + pageContentModel.ID);
            }
            return Redirect("/");
        }
    }
}
