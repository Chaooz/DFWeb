using System;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace DarkFactorCoreNet.Models
{
    public class TeaserPageContentModel
    {
        public int ID { get; set; }
        public int ParentId { get; set; }
        public string PromoTitle { get; set; }
       public string PromoText { get; set; }
        public string ContentTitle { get; set; }
        public int ImageId { get; set; }
        public int SortId { get; set; }
        public int Acl { get; set; }

        // TODO : Refactor this away
        public bool IsEdit;
    }
}
