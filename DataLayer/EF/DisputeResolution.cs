using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("DisputeResolution", Schema = "Accounting")]
    public partial class DisputeResolution
    {
        [Key]
        public int Id { get; set; }
        [Column("FK_Invoice")]
        public int FkInvoice { get; set; }
        [Column("FK_User")]
        public int? FkUser { get; set; }
        [Column("FK_BusinessOwner")]
        public int? FkBusinessOwner { get; set; }
        [Column("FK_Marketer")]
        public int? FkMarketer { get; set; }
        [Column("writer")]
        public short Writer { get; set; }
        [Required]
        [StringLength(1000)]
        public string Discription { get; set; }
        [StringLength(100)]
        public string FileDocumentAdress { get; set; }
        [Required]
        [StringLength(10)]
        public string Date { get; set; }
        [Column("disputerwhoPerson")]
        public short? DisputerwhoPerson { get; set; }

        [ForeignKey(nameof(FkBusinessOwner))]
        [InverseProperty(nameof(BusinessOwner.DisputeResolution))]
        public virtual BusinessOwner FkBusinessOwnerNavigation { get; set; }
        [ForeignKey(nameof(FkInvoice))]
        [InverseProperty(nameof(Invoice.DisputeResolution))]
        public virtual Invoice FkInvoiceNavigation { get; set; }
        [ForeignKey(nameof(FkMarketer))]
        [InverseProperty(nameof(Marketer.DisputeResolution))]
        public virtual Marketer FkMarketerNavigation { get; set; }
        [ForeignKey(nameof(FkUser))]
        [InverseProperty(nameof(User.DisputeResolution))]
        public virtual User FkUserNavigation { get; set; }
    }
}
