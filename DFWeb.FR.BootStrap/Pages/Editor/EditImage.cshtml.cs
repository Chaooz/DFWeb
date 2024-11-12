using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using DFWeb.BE.Models;
using DFWeb.BE.Provider;
using DFWeb.FR.Models;

namespace DarkFactorCoreNet.Pages
{
    public class EditImageModel : MenuPageModel
    {
        private IImageProvider imageProvider;

        public ImageModel imageModel;
        public int pageId;
        public int imageId;

        public EditImageModel(IMenuProvider menuProvider,IImageProvider imageProvider, ILoginProvider loginProvider) : base(menuProvider, loginProvider)
        {
            this.imageProvider = imageProvider;
        }

        public void OnGet(int pageId, int imageId)
        {
            base.GetMenuData(pageId);

            this.pageId = pageId;
            this.imageId = imageId;
            this.imageModel = imageProvider.GetImage(imageId);
        }

        public IActionResult OnPostAsync([FromForm] int pageId, [FromForm] int imageId, [FromForm] string filename )
        {
            this.imageProvider.UpdateImage(imageId, filename);
            return Redirect("/Editor/ImageSelector?pageId=" + pageId);
        }
    }
}
