using System;
using Microsoft.AspNetCore.Html;

namespace DarkFactorCoreNet.Models
{
    public class PageContentModel
    {
        public int ID { get; set; }
        public int ParentId { get; set; }
        public string PromoTitle { get; set; }
        public string PromoText { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int SortId { get; set; }
        public bool IsPublished { get; set; }

        // Additional html properties
        public HtmlString HtmlContent { get; set; }
        public HtmlString HtmlTeaser { get; set; }
        public string Command { get; set; }
    }
}
