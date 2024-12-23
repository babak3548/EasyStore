using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Accounting", Schema = "Accounting")]
    public partial class Accounting
    {
        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "بدهکار")]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Debtor { get; set; }
        [Display(Name = "بستانکار")]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Creditor { get; set; }
        [Column("FK_User")]
        [Display(Name = "شناسه کاربر")]
        public int FkUser { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "datetime2(7)")]
        [Display(Name = "تاریخ موثر")]
        public DateTime Date { get; set; }
        [Column(TypeName = "datetime2(7)")]
        [Display(Name = "تاریخ ایجاد")]
        public DateTime RegisterDate { get; set; }
        [InverseProperty("Accounting")]
        public virtual PaymentLog PaymentLog{ get; set; }

        [ForeignKey(nameof(FkUser))]
        [InverseProperty(nameof(User.Accounting))]
        public virtual User FkUserNavigation { get; set; }
    }
}
