using System;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DarkFactorCoreNet.Models
{
    public class ArticleSectionModel
    {
        public int ID { get; set; }
        public int PageId { get; set; }
        public string Text { get; set; }
        public int ImageId { get; set; }
        public int SortId { get; set; }
        public int Layout { get; set; }

        public ArticleSectionModel()
        {
        }
    }
}
