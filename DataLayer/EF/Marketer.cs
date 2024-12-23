using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Marketer", Schema = "Shapping")]
    public partial class Marketer
    {
        public Marketer()
        {
            BridgeBusinessOwnerMarketer = new HashSet<BridgeBusinessOwnerMarketer>();
            BusinessOwner = new HashSet<BusinessOwner>();
            DisputeResolution = new HashSet<DisputeResolution>();
            InverseFkMarketerNavigation = new HashSet<Marketer>();
            Invoice = new HashSet<Invoice>();
        }

        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "نام")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "ویژگی برجسته ")]
        [StringLength(500)]
        public string Specialty { get; set; }
        [StringLength(250)]
        [Display(Name = "کلمات کلیدی")]
        public string WordKey { get; set; }
        [Column("FK_User")]
        [Display(Name = "نام کاربری")]
        public int FkUser { get; set; }
        [Column("FK_Marketer")]
        [Display(Name = "بازاریاب پدر")]
        public int? FkMarketer { get; set; }
        [StringLength(100)]
        [Display(Name = "عکس")]
        public string Image { get; set; }
        [Display(Name = "آدرس اینترنتی")]
        [StringLength(50)]
        public string WebSiteAddress { get; set; }
        [StringLength(25)]
        [Display(Name = "تلفن")]
        public string Tel { get; set; }
        [Display(Name = "فعال")]
        public bool? Active { get; set; }
        [StringLength(100)]
        [Display(Name = "واتش اپ یا موبایل دوم")]
        public string Yahoo { get; set; }
        [StringLength(50)]
        [Display(Name = "ایمیل")]
        public string Gmail { get; set; }
        [StringLength(200)]
        [Display(Name = "آدرس اینستاگرام کانال تلگرام یا صفحه اجتماعی")]
        public string Skype { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "پورسانت پرداخت یا هزینه پایه تبلیغ")]
        public decimal? PaymentPorsantAmount { get; set; }

        [ForeignKey(nameof(FkMarketer))]
        [InverseProperty(nameof(Marketer.InverseFkMarketerNavigation))]
        public virtual Marketer FkMarketerNavigation { get; set; }
        [ForeignKey(nameof(FkUser))]
        [InverseProperty(nameof(User.Marketer))]
        public virtual User FkUserNavigation { get; set; }
        [InverseProperty("FkMarketerNavigation")]
        public virtual ICollection<BridgeBusinessOwnerMarketer> BridgeBusinessOwnerMarketer { get; set; }
        [InverseProperty("FkMarketerNavigation")]
        public virtual ICollection<BusinessOwner> BusinessOwner { get; set; }
        [InverseProperty("FkMarketerNavigation")]
        public virtual ICollection<DisputeResolution> DisputeResolution { get; set; }
        [InverseProperty(nameof(Marketer.FkMarketerNavigation))]
        public virtual ICollection<Marketer> InverseFkMarketerNavigation { get; set; }
        [InverseProperty("FkMarketerNavigation")]
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
