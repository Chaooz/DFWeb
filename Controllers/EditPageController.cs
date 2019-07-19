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

        [HttpPost]
        public IActionResult UpdatePageData([FromForm] EditPageModel editPageModel)
        {
            bool didSuceed = false;
            switch( editPageModel.Command )
            {
                case "save":
                    didSuceed = pageRepository.SavePage(editPageModel);
                    break;

                case "create_child_page":
                    didSuceed = pageRepository.CreateChildPage(editPageModel.ID);
                    break;
            }

            if ( editPageModel != null && editPageModel.ID != 0 )
            {
                return Redirect("/page?id=" + editPageModel.ID);
            }
            return Redirect("/");
        }
    }
}
