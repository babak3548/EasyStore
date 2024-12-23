using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
 public  class PromotionProductModel
    {
        public int PromotionId { get; set; }
        public PromotionTypes PromotionType { get; set; }
        public int? FkProduct { get; set; }
         public int Order { get; set; }
        public DateTime ExpireDateTime { get; set; }

        public string ProductName { get; set; }
        public string ProductNameForUrl { get; set; }
        public string ProductImage { get; set; }
        public string ProductImage1 { get; set; }
        public decimal Price { get; set; }
        public int? FkCategory { get; set; }
        public string CategoryName { get; set; }


        public string ProductImage2 { get; set; }
        public string ProductImage3 { get; set; }
        public string ProductImage4 { get; set; }
   
        public string AvailableColors { get; set; }
        public string Discription { get; set; }
        public string AbstractDiscription { get; set; }
        
        public decimal BeforDiscountPrice { get; set; }
        public int Available { get; set; }
        public string ProductBrand { get; set; }
    }
}
