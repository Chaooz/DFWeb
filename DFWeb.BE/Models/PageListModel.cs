using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DFWeb.BE.Models
{
    public class PageListModel
    {
        public string Title { get; set; }
        public string RelatedTags{ get; set; }
        public List<TeaserPageContentModel> Pages;

        public bool ShowEditor;

        public PageListModel()
        {
            Pages = new List<TeaserPageContentModel>();
            ShowEditor = false;
        }
    };
}
