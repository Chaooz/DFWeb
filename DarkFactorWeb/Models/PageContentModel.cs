using System;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DarkFactorCoreNet.Models
{
    public class PageContentModel
    {
        public int ID { get; set; }
        public int ParentId { get; set; }
        public string PromoTitle { get; set; }
        public string PromoText { get; set; }
        public string ContentTitle { get; set; }
        public string ContentText { get; set; }
        public string Image { get; set; }
        public int SortId { get; set; }
        public int Acl { get; set; }
        public List<ImageModel> ImageList { get; set; }

        // Additional html properties
        public HtmlString HtmlContent { get; set; }
        public HtmlString HtmlTeaser { get; set; }
        public string Command { get; set; }

        public IList<TagModel> Tags{ get; set; }

        public bool IsEdit;

        public PageContentModel()
        {
            Tags = new List<TagModel>();
            IsEdit = false;
        }
    }
}
