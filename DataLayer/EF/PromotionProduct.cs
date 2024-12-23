using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
// ViewData["PromotionType"] = new SelectList(EnumUtility.EnumToList<PromotionTypes>(), "Id", "Name");
namespace DataLayer.EF
{
   public class PromotionProduct
    {
        public PromotionProduct()
        {
        }
        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [Display(Name = "نوع پرومشن")]
        public PromotionTypes PromotionType { get; set; }
        [Display(Name = "عنوان ها (هر قسمت با ویرگول ازهم جدا میشه ',')")]
        public string   Title{ get; set; }
        //[Display(Name = "آدرس محصول")]
        //public string NameForUrl { get; set; }
        [Display(Name = "کلمه کلیدی کمپین (درصورت وجود فیلتر روی این کلمه خواهد بود)")]
        public string Query { get; set; }
        [Display(Name = "شناسه محصول - برای سرگروه ها معنی ندارد")]
        [Column("FkProduct")]
        public int? FkProduct { get; set; }
        [Column("Order")]
        [Display(Name = "الویت نمایش ")]
        public int Order { get; set; }
        [Display(Name = "تاریخ انقضا")]
        public DateTime ExpireDateTime { get; set; }

        [ForeignKey(nameof(FkProduct))]
        [Display(Name = "محصول")]
        public virtual Product Product { get; set; }

        [StringLength(100)]
        [Display(Name = "تصویر در صورت نیاز-jpg,png,gif, 5-800000")]
        public string Image { get; set; }
        [Column("FkCategory")]
        [Display(Name = "شناسه گروه - فقط برای سرگروهها- و در صورت پر بود کالای آن نمایش داده میشه")]
        public int? FkCategory { get; set; }

        [ForeignKey(nameof(FkCategory))]
        [Display(Name = "گروه")]
        public virtual Category Category { get; set; }
    }
}
