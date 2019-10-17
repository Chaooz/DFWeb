using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkFactorCoreNet.Models;
using DarkFactorCoreNet.Controllers;

namespace DarkFactorCoreNet.Pages
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IPageCollector pageController, IMenuCollector menuController) : base(pageController,menuController)
        {
        }
    }
}
