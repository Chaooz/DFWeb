using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using DarkFactorCoreNet.Provider;
using DarkFactorCoreNet.Models;

namespace DarkFactorCoreNet.Editor
{
    public class ImageSelectorModel : PageModel
    {
        public int pageId;

        public IList<ImageModel> imageList;
        private IImageProvider imageProvider;

        public ImageSelectorModel(IImageProvider imageProvider)
        {
            this.imageProvider = imageProvider;
            imageList = new List<ImageModel>();
        }

        public void OnGet(int pageId)
        {
            this.pageId = pageId;
            imageList = imageProvider.GetImages(10);
            if ( imageList == null )
            {
                // Reroute to main page
            }
        }
    }
}
