using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UILayer.Models
{
    public class YektanetProduct
    {
        public string title { get; set; } //  : "رب گوجه فرنگی قوطی بزرگ",
        public string sku { get; set; } //    : "D153-Ij89d", // شناسه محصول
        public string[] category { get; set; } //    : ["مواد غذایی", "رب"],
        public int price { get; set; } //   : 11300, // تومان
        public string brand { get; set; }//    : "تبرک",
        public int discount { get; set; } //   : 30, // درصد
        public string image { get; set; } //  : 'https://www.yektanet.com/yektanet-logo.jpg',
        public bool isAvailable { get; set; } // : true, // محصول در حال حاضر موجود است
    }
    //yektanet("product", "detail", productInfo)
}
