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
    [Route("api/edit")]
    [ApiController]
    public class EditController : ControllerBase
    {
        private PageRepository pageRepository;

        public EditController()
        {
            pageRepository = new PageRepository();
        }

        [HttpPost]
        public IActionResult SavePage([FromForm] PageContentModel pageContent)
        {
            if (pageRepository.SavePage(pageContent))
            {
                return Redirect("/page?id=" + pageContent.ID);
            }
            else
            {
                return Redirect("/edit?id=" + pageContent.ID);
            }
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PageContentModel>> GetTodoItem(long id)
        {
            PageContentModel model = new PageContentModel()
            {
                ID = (int)id,
                Title = "HAH"
            };

            return model;
        }
    }
}
