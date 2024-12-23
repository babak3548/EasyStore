using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.EF
{
    public class PaymentLog
    {
        [Key]
        [Display(Name = "شناسه")]
        public long Id { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "وضعیت")]
        public PayLogType Status { get; set; }
        [Display(Name = "شناسه کاربر")]
        public long UserId { get; set; }
        [Display(Name = "شناسه فاکتور")]
        public Nullable<int> InvoiceId { get; set; }
        [Display(Name = "شناسه حسابداری")]
        public Nullable<int> AccountingId { get; set; }
        [Column(TypeName = "datetime2(7)")]
        [Display(Name = "تاریخ ایجاد")]
        public System.DateTime CreateDate { get; set; }
        [Column(TypeName = "datetime2(7)")]
        [Display(Name = "تاریخ ویرایش")]
        public System.DateTime UpdateDate { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        [Display(Name = "مبلغ")]
        public decimal Amount { get; set; }
        [Column(TypeName = "tinyint")]
        [Display(Name = "درگاه بانک")]
        public Banks PaymentBankCode { get; set; }
        [ForeignKey(nameof(InvoiceId))]
        [InverseProperty(nameof(Invoice.PaymentLogs))]
        public virtual Invoice FkInvoiceNavigation { get; set; }

        [ForeignKey(nameof(AccountingId))]
        [InverseProperty("PaymentLog")]
        public virtual Accounting Accounting { get; set; }
        public virtual User User { get; set; }
    }
}
