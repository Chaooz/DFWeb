using System;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DarkFactorCoreNet.Models
{
    public class MainPageContentModel
    {
        public int PageID { get; set; }
        public string Title { get; set; }
        public int Acl { get; set; }
        public string RelatedTags { get; set; }
    }
}
