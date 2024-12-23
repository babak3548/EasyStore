using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Bridge_Invoice_Product", Schema = "Accounting")]
    public partial class BridgeInvoiceProduct
    {
        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [Display(Name = "تعداد")]
        public int Count { get; set; }
        [Display(Name = "رنگ")]
        public string  Colore { get; set; }
        [StringLength(100)]
        [Display(Name = "تصویر")]
        public string Image { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "قیمت قبل از تخفیف (ریال)")]
        [DisplayFormat(DataFormatString = "{0:#.####}")]
        public decimal BeforDiscountPrice { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "قیمت خرید (ریال)")]
        [DisplayFormat(DataFormatString = "{0:#,##.##}")]
        [Required]
        public decimal BuyPrice { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "قیمت فروش (ریال)")]
        [DisplayFormat(DataFormatString = "{0:#,##.##}")]
        [Required]
        public decimal Price { get; set; }
        [Column("FK_Product")]
        [Display(Name = "کالا")]
        public int FkProduct { get; set; }
        [Column("FK_Invoice")]
        public int FkInvoice { get; set; }
        [Column(TypeName = "tinyint")]
        [Display(Name = "وضعیت")]
        public InvoiceDetilasStatus InvoiceDetilasState { get; set; }
        [Display(Name = "تاریخچه")]
        //تاریخچه وضعیت و توضیحات را نگه می دارد
        public string HistoryStateAndDescription { get; set; }

        [ForeignKey(nameof(FkInvoice))]
        [InverseProperty(nameof(Invoice.BridgeInvoiceProduct))]
        public virtual Invoice FkInvoiceNavigation { get; set; }
        [ForeignKey(nameof(FkProduct))]
        [InverseProperty(nameof(Product.BridgeInvoiceProduct))]
        public virtual Product FkProductNavigation { get; set; }
    }
}
