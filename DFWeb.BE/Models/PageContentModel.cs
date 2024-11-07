using System;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DarkFactorCoreNet.Models
{
    public class PageContentModel : TeaserPageContentModel
    {
        public string ContentText { get; set; }
        // public HtmlString HtmlContent { get; set; }
        // public HtmlString HtmlTeaser { get; set; }
        public string Command { get; set; }
        public string RelatedTags { get; set; }
    }
}
