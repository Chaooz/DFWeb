using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using DFWeb.BE.Models;
using DFWeb.BE.Provider;

namespace DarkFactorCoreNet.Editor
{
    public class ImageSelectorModel : PageModel
    {
        public int pageId;
        public int sectionId;

        public int prevImagePageId;
        public int nextImagePageId;

        public IList<ImageModel> imageList;
        private IImageProvider imageProvider;

        public ImageSelectorModel(IImageProvider imageProvider)
        {
            this.imageProvider = imageProvider;
            imageList = new List<ImageModel>();
        }

        public void OnGet(int pageId, int sectionId = 0, int imagePageId = 0)
        {
            this.pageId = pageId;
            this.sectionId = sectionId;

            this.prevImagePageId = Math.Max(0,imagePageId - 1);
            this.nextImagePageId = imagePageId + 1;

            imageList = imageProvider.GetImages(9,imagePageId);
            if ( imageList == null )
            {
                // Reroute to main page
            }
        }
    }
}
