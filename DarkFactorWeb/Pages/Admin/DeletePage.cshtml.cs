using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using DarkFactorCoreNet.Controllers;

namespace DarkFactorCoreNet.Pages
{
    public class DeleteModel : BasePageModel
    {
        public DeleteModel(IPageCollector pageController, IMenuCollector menuController) : base(pageController,menuController)
        {
        }
    }
}
