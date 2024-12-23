using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Bridge_BusinessOwner_Marketer", Schema = "Shapping")]
    public partial class BridgeBusinessOwnerMarketer
    {
        [Key]
        public int Id { get; set; }
        [Column("FK_BusinessOwner")]
        public int FkBusinessOwner { get; set; }
        [Column("FK_Marketer")]
        public int FkMarketer { get; set; }
        [StringLength(500)]
        public string DiscriptionRequest { get; set; }
        public bool? AcceptRequest { get; set; }
        public bool? RequestFromBusinessOwner { get; set; }
        public bool? RequestFromMarketer { get; set; }
        public bool? RejectRequest { get; set; }
        [Column("date")]
        [StringLength(10)]
        public string Date { get; set; }

        [ForeignKey(nameof(FkBusinessOwner))]
        [InverseProperty(nameof(BusinessOwner.BridgeBusinessOwnerMarketer))]
        public virtual BusinessOwner FkBusinessOwnerNavigation { get; set; }
        [ForeignKey(nameof(FkMarketer))]
        [InverseProperty(nameof(Marketer.BridgeBusinessOwnerMarketer))]
        public virtual Marketer FkMarketerNavigation { get; set; }
    }
}
