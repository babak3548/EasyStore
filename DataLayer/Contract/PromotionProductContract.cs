using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Contract
{
   public class PromotionProductContract
    {
        public int PromotionId { get; set; }
        public DateTime PromotionExpireDateTime { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string Image { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public decimal BeforDiscountPrice { get; set; }
        public decimal Price { get; set; }
        public string Discription { get; set; }
        public string MadeInCountry { get; set; }
        public int Available { get; set; }
        public string AvailableColors { get; set; }
        public string Dimansion { get; set; }
        public int FkCategory { get; set; }
    }
}
