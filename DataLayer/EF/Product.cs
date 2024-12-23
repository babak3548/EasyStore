using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Product", Schema = "Shapping")]
    public partial class Product
    {
        public Product()
        {
            BridgeInvoiceProduct = new HashSet<BridgeInvoiceProduct>();
            Comment = new HashSet<Comment>();
        }

        [Key]
        [Display(Name ="شناسه")]
        public int Id { get; set; }
      
        [StringLength(200)]
        [Display(Name = " آدرس محصول در سایت، بهتر به لاتین پر شود")]
    //    [Index("IX_NameForUrll", 1, IsUnique = true)]
        public string NameForUrll { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "عنوان کالا")]
        public string Name { get; set; }
        [StringLength(50)]
        [Required]
        [Display(Name = "برند")]
        public string Brand { get; set; }
        [StringLength(100)]
        [Display(Name = "تصویر اصلی")]
        public string Image { get; set; }
        [StringLength(100)]
        [Display(Name = "تصویر1")]
        public string Image1 { get; set; }
        
        [Display(Name = "تصویر لیستی")]
        public string Image2 { get; set; }
        [StringLength(100)]
        [Display(Name = "تصویر3")]
        public string Image3 { get; set; }
        [StringLength(100)]
        [Display(Name = "تصویر4")]
        public string Image4 { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "قیمت قبل از تخفیف (ریال)")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal BeforDiscountPrice { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "قیمت خرید (ریال)")]
        [DisplayFormat(DataFormatString= "{0:#,##.##}")]
        [Required]
        public decimal BuyPrice { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "قیمت فروش (ریال)")]
        [DisplayFormat(DataFormatString = "{0:#,##.##}")]
        [Required]
        public decimal Price { get; set; }
        [StringLength(10)]
        [Display(Name = "تاریخ ثبت به استرینق")]
        public string RegisterDate { get; set; }
        [Column(TypeName = "datetime2(7)")]
        [Display(Name = "تاریخ ثبت  ")]
        public DateTime AddDate { get; set; }
        [Column(TypeName = "datetime2(7)")]
        [Display(Name = "تاریخ به روز رسانی")]
        public DateTime UpdateDate { get; set; }
        [Required]
        [Display(Name = "توضیحات")]
        public string Discription { get; set; }
        [Column("FK_BusinessOwner")]
        [Display(Name = "فروشنده")]
        [Required]
        public int FkBusinessOwner { get; set; }
        [StringLength(50)]
        [Display(Name = "کشور سازنده")]
        [Required]
        public string MadeInCountry { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "قیمت فروش عمده (ریال)")]
        public decimal? CountPrice { get; set; }
        [Display(Name = "حداقل تعداد در فروش عمده")]
        public int? MinCountForPrice { get; set; }
        [Column("available")]
        [Display(Name = "موجودی انبار")]
        [Required]
        public int Available { get; set; }
        [Column("AvailableColors")]
        [Display(Name = "رنگهای موجود")]
        public string AvailableColors { get; set; }
        [Display(Name = "پورسانت بازاریاب")]
        public float PersentMarkater { get; set; }
        [Display(Name = "قابل مرجوع تا چند روز(اختیاری-7 روز)")]
        public byte? AcceptReturnDay { get; set; }
   
        [MinLength(30)]
        [Display(Name = " چکیده توضیحات ")]
        public string ShippingDiscription { get; set; } //to do rename
        [Display(Name = "حداکثر زمان آماده سازی (روز - اجباری)")]
        public byte MaxShippingDay { get; set; }
        [Display(Name = "حداقل زمان آماده سازی(روز - اختیاری)")]
        public byte MinShippingDay { get; set; }
       // [StringLength(maximumLength:)]
        [Display(Name = "کلمات کلیدی")]
        public string WordKey { get; set; }
        [Display(Name = "ابعاد کالا به سانتی متر (طول*عرض*ارتفاع)")]
        [Required]
        public string Dimansion { get; set; }
      //  [Required]
        [StringLength(20)]
        [Display(Name = "واحد فروش اختیاری(کیلو، گرم، بسته، دستگاه)")]
        public string? UnitBuy { get; set; }
        [Column("wightUnitBuyWithKG")]
        [Display(Name = "وزن کالا (گرم)")]
        public double WightUnitBuyWithKg { get; set; }
        [Display(Name = "فعال برای فروش و نمایش")]
        public bool Active { get; set; }
        [Column("FK_Category")]
        [Display(Name = "گروه کالا")]
        [Required]
        public int FkCategory { get; set; }
        [Display(Name = "الویت نمایش (از بزرگتر به کوچک -حداکثر1000)")]
        public int RankShow { get; set; }
        [Display(Name = "رتبه پرفروش بودن نسبت به بقیه")]
        public int? RankSelling { get; set; }
        [Column("FK_Content")]
        [Display(Name = "محتوای خاص مخصوص کالا(اختیاری)")]
        public int? FkContent { get; set; }

        [Column("CalculatedStar")]
        [Display(Name = "ستاره محاسبه شده از کامنت ها")]
        public float CalculatedStar { get; set; }
        [Column("UserStar")]
        [Display(Name = "ستاره (عداد بین 1 تا 5 باشد)")]
        public float UserStar { get; set; }

        [Display(Name = "توضیحات تگ متا")]
        public string MetaDescription { get; set; }
        [Display(Name = "آدرس ویدیو")]
        public string VideoUrl{ get; set; }

        [ForeignKey(nameof(FkBusinessOwner))]
        [InverseProperty(nameof(BusinessOwner.Product))]
        public virtual BusinessOwner FkBusinessOwnerNavigation { get; set; }
        [ForeignKey(nameof(FkCategory))]
        [InverseProperty(nameof(Category.Product))]
        public virtual Category FkCategoryNavigation { get; set; }
        [InverseProperty("FkProductNavigation")]
        public virtual ICollection<BridgeInvoiceProduct> BridgeInvoiceProduct { get; set; }
      //  [InverseProperty("FkProductNavigation")]
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<BrigeProductCategoryProperty> BrigeProductCategories { get; set; }
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; }
        [InverseProperty(nameof(Wish.Product))] // اشاره به لیست پروپرتی پروداکت در ویش دارد
        public virtual ICollection<Wish> Wishes { get; set; }
    }
}
