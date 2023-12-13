using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineCourse.Models
{
    public class ProductModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PromotionPrice { get; set; }
        public Nullable<bool> IncludeVAT { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<long> CategoryID { get; set; }
        public string Detail { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifiDate { get; set; }
        public string ModifiedBy { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescriptions { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public string LinkVideo { get; set; }
        public string ListType { get; set; }
        public int CountVideo { get; set; }
        public int CountComment { get; set; }
        public int CountLearner { get; set; }

        public bool IsBought { get; set; }
    }
}