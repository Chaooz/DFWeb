using System;
using Microsoft.AspNetCore.Html;

namespace DarkFactorCoreNet.Models
{
    public class PageContentModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }

        public HtmlString HtmlContent { get; set; }

        public PageContentModel()
        {
        }
    }
}
