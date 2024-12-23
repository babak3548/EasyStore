using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Bridge_Province_BusinessOwner", Schema = "Shapping")]
    public partial class BridgeProvinceBusinessOwner
    {
        [Key]
        public int Id { get; set; }
        [Column("FK_Province")]
        public int FkProvince { get; set; }
        [Column("FK_BusinessOwner")]
        public int FkBusinessOwner { get; set; }
        [Column("AnyXKG")]
        public float AnyXkg { get; set; }
        [Column("AnyXKGMony", TypeName = "decimal(18, 0)")]
        public decimal AnyXkgmony { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal? FreeGratherThanMony { get; set; }

        [ForeignKey(nameof(FkBusinessOwner))]
        [InverseProperty(nameof(BusinessOwner.BridgeProvinceBusinessOwner))]
        public virtual BusinessOwner FkBusinessOwnerNavigation { get; set; }
        [ForeignKey(nameof(FkProvince))]
        [InverseProperty(nameof(Province.BridgeProvinceBusinessOwner))]
        public virtual Province FkProvinceNavigation { get; set; }
    }
}
