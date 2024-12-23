using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("BusinessOwner", Schema = "Shapping")]
    public partial class BusinessOwner
    {
        public BusinessOwner()
        {
            BridgeBusinessOwnerMarketer = new HashSet<BridgeBusinessOwnerMarketer>();
            BridgeProvinceBusinessOwner = new HashSet<BridgeProvinceBusinessOwner>();
            DisputeResolution = new HashSet<DisputeResolution>();
            Invoice = new HashSet<Invoice>();
            Product = new HashSet<Product>();
        }

        [Display(Name = "شناسه")]
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [StringLength(250)]
        [Display(Name = "نوع فعالیت")]
        public string TypeActivity { get; set; }
      //  [StringLength(10)]
        [Display(Name = "کلمات کلید زمینه فعالیت فروشنده")]
        public string WordKey { get; set; }
        //[StringLength(250)]
        [Display(Name = "آدرس فروشنده")]
        public string Address { get; set; }
        
        [StringLength(10)]
        [Display(Name = "کد ملی ")]
        public string NationalCode { get; set; }
        [Column("FK_User")]
        [Display(Name = "نام کاربری")]
        public int? FkUser { get; set; }
        [Column("FK_Province")]
        [Display(Name = "استان")]
        public int FkProvince { get; set; }
        [Column("FK_Marketer")]
        [Display(Name = "بازاریاب")]
        public int? FkMarketer { get; set; }
        [StringLength(100)]
        [Display(Name = "آدس فایل مجوز فروشنده ")]
        public string DocumentFile { get; set; }
        [StringLength(100)]
        [Display(Name = "عکس")]
        public string Image { get; set; }
        [StringLength(50)]
        [Display(Name = "آدرس وب سایت")]
        public string WebSiteAddress { get; set; }
    
        [Display(Name = " توضیحات در مورد معاملات یا خود فروشنده")]
        public string Discription { get; set; }
        [Display(Name = "فعال")]
        public bool? Active { get; set; }
        [StringLength(25)]
        [Display(Name = "تلفن")]
        public string Tel { get; set; }
        [StringLength(50)]
        [Display(Name = "واتس آپ یا موبایل دوم")]
        public string WhatsApp { get; set; }
        [StringLength(50)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [StringLength(50)]
        [Display(Name = "اینستاگرام یا تلگرام کانال یا وب سایت")]
        public string InstaOrOtherSocial { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "تخفیف نسبت به تک فروشی")]
        public decimal? PaymentPorsantAmount { get; set; }
        [Display(Name = "نوع فروش")]
        public TypeSell TypeSells { get; set; }  //
        [Column("TypeShippingBussinessOwner")]
        [Display(Name = "نوع ارسال")]
        public int TypeShippingBussinessOwner { get; set; }



     
        [ForeignKey(nameof(FkMarketer))]
        [InverseProperty(nameof(Marketer.BusinessOwner))]
        public virtual Marketer FkMarketerNavigation { get; set; }
        [ForeignKey(nameof(FkProvince))]
        [InverseProperty(nameof(Province.BusinessOwner))]
        public virtual Province FkProvinceNavigation { get; set; }
        [ForeignKey(nameof(FkUser))]
        [InverseProperty(nameof(User.BusinessOwner))]
        public virtual User FkUserNavigation { get; set; }
        [InverseProperty("FkBusinessOwnerNavigation")]
        public virtual ICollection<BridgeBusinessOwnerMarketer> BridgeBusinessOwnerMarketer { get; set; }
        [InverseProperty("FkBusinessOwnerNavigation")]
        public virtual ICollection<BridgeProvinceBusinessOwner> BridgeProvinceBusinessOwner { get; set; }
        [InverseProperty("FkBusinessOwnerNavigation")]
        public virtual ICollection<DisputeResolution> DisputeResolution { get; set; }
        [InverseProperty("FkBusinessOwnerNavigation")]
        public virtual ICollection<Invoice> Invoice { get; set; }
        [InverseProperty("FkBusinessOwnerNavigation")]
        public virtual ICollection<Product> Product { get; set; }
    }
}
