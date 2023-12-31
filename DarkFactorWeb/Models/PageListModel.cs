﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DarkFactorCoreNet.Models
{
    public class PageListModel
    {
        public string Title { get; set; }
        public List<PageContentModel> Pages;

        public bool ShowEditor;

        public PageListModel()
        {
            Pages = new List<PageContentModel>();
            ShowEditor = false;
        }
    };
}
