using System;
using Microsoft.AspNetCore.Html;

namespace DarkFactorCoreNet.Models
{
    public class PageContentModel
    {
        public int ID;
        public string Title;
        public HtmlString Content;
        public bool IsPublished;

        public PageContentModel()
        {
        }
    }
}
