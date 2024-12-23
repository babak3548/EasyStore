using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Invoice", Schema = "Accounting")]
    public partial class Invoice
    {
        public Invoice()
        {
          //  Accounting = new HashSet<Accounting>();
            BridgeInvoiceProduct = new HashSet<BridgeInvoiceProduct>();
            DisputeResolution = new HashSet<DisputeResolution>();
        }

        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        [Column(TypeName = "datetime2(7)")]
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }
        [Column(TypeName = "datetime2(7)")]
        [Display(Name = "تاریخ به روز رسانی")]
        public DateTime UpdateDate { get; set; }
        [Column(TypeName = "datetime2(7)")]
        [Display(Name = "تاریخ خروج از انبار")]
        public DateTime SendingDate { get; set; }

        [Display(Name = "مدت زمان پردازش کالا")]
        public short ProcessingDays { get; set; }

        [Column(TypeName = "tinyint")]
        [Display(Name = "وضعیت")]
        public InvoiceStatus Status { get; set; }
        

        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name ="مجموع مبلغ")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal TotalSumProductPrice { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        [DefaultValue(0)]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        [Display(Name = "مبلغ پرداختی کاربر")]
        public decimal PaymentToCountinue { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "هزینه ارسال")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal ShippingCost { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [DefaultValue(0)]
        [Display(Name = "تخفیف")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal Discount { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [DefaultValue(0)]
        [Display(Name = "مالیات")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal Vat { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "هزینه انصراف")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal RejectedCost {get ; set;}

        [Column("FK_User")]
        [Display(Name = "شناسه کاربر")]
        public int FkUser { get; set; }
        [Column("FK_BusinessOwner")]
        [Display(Name = "صاحب کسب و کار")]
        public int FkBusinessOwner { get; set; }
        [Display(Name = "روش پرداخت")]
        public PaymentType PaymentType { get; set; }
        // یادداشت های مربوط به سفارش، مانند توضیح نحوه ارسال.
        [Display(Name = "یادداشت برای فروشنده")]
        public string NoteForBusinessOwner { get; set; }
        // در صورتی که نیاز به اطلاع رسانی  در مورد سفارش بود می تونیم از این قسمت استفاده نماییم 
        // پیش فرض استفاده نمی شود
        [Display(Name = "یادداشت برای کاربر")]
        public string NoteForUser { get; set; }
        [Column("FK_Marketer")]
        public Nullable<int> FkMarketer { get; set; }



        #region delFiels
        [StringLength(10)]
        [Display(Name = "زمان پرداخت")]
        public string TimeBankPayInfo { get; set; }
        [Column(TypeName = "tinyint")]
        [DefaultValue(0)]
        [Display(Name = "بانک مرجع")]
        public Banks PaymentBankCode { get; set; }
        [StringLength(50)]
        [Display(Name = "شناسه تراکنش مرجع")]
        public string TransctionRefrenceId { get; set; }
        #endregion delFields


        [StringLength(50)]
        [Display(Name = "شماره پیگیری ارسال")]
        public string TracingShippingNumber { get; set; }
        [Column(TypeName = "tinyint")]
        [DefaultValue(0)]
        public ShippingCompanies ShippingCompany { get; set; }
        [StringLength(250)]
        public string CommentForBusinessman { get; set; }
        [Display(Name = "تاریخچه وضعیت")]
        public string HistoryStateAndDescription { get; set; }

        #region adreesInfo
        [Display(Name = "کد استان تحویل")]
        [Column("FK_Province")]
        public Nullable<int> FkProvince { get; set; }
        [Display(Name = "آدرس تحویل")]
        public string DeliveryAddress { get; set; }
        [Display(Name = "نام شهر")]
        public string DeliveryCityName { get; set; }
        [Display(Name = "نام تحویل گیرنده")]
        public string DeliveryName { get; set; }
        [Display(Name = "نام خانوادگی تحویل گیرنده")]
        public string DeliveryLastName { get; set; }
        [Display(Name = "کد پستی")]
        public string DeliveryPostCode { get; set; }

        [Display(Name = "تلفن تحویل گیرنده")]
        public string DeliveryTel { get; set; }
        [Display(Name = "موبایل تحویل گیرنده")]
        public string DeliveryMobile { get; set; }
        [Display(Name = "نام شرکت تحویل گیرنده")]
        public string DeliveryCompanyName { get; set; }
        #endregion adressInfo

        [ForeignKey(nameof(FkBusinessOwner))]
        [InverseProperty(nameof(BusinessOwner.Invoice))]
        public virtual BusinessOwner FkBusinessOwnerNavigation { get; set; }
        [ForeignKey(nameof(FkMarketer))]
        [InverseProperty(nameof(Marketer.Invoice))]
        public virtual Marketer FkMarketerNavigation { get; set; }
        [ForeignKey(nameof(FkProvince))]
        [InverseProperty(nameof(Province.Invoice))]
        public virtual Province FkProvinceNavigation { get; set; }
        [ForeignKey(nameof(FkUser))]
        [InverseProperty(nameof(User.Invoice))]
        public virtual User FkUserNavigation { get; set; }

        [InverseProperty("FkInvoiceNavigation")]
        public virtual ICollection<BridgeInvoiceProduct> BridgeInvoiceProduct { get; set; }
        [InverseProperty("FkInvoiceNavigation")]
        public virtual ICollection<DisputeResolution> DisputeResolution { get; set; }
        [InverseProperty("FkInvoiceNavigation")]
        public virtual ICollection<PaymentLog> PaymentLogs { get; set; }
        //[Display(Name = "رنگ")]
        //public string Color { get; set; }
    }
}
