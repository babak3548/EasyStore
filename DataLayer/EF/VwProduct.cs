using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    public partial class VwProduct
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Brand { get; set; }
        [StringLength(100)]
        public string Image { get; set; }
        [StringLength(100)]
        public string Image1 { get; set; }
        [StringLength(100)]
        public string Image2 { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }
        [Required]
        [StringLength(10)]
        public string RegisterDate { get; set; }
        [StringLength(500)]
        public string Discription { get; set; }
        [Column("FK_BusinessOwner")]
        public int FkBusinessOwner { get; set; }
        [StringLength(50)]
        public string MadeInCountry { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal? CountPrice { get; set; }
        public int? MinCountForPrice { get; set; }
        [Column("available")]
        public int Available { get; set; }
        public float PersentMarkater { get; set; }
        public byte? AcceptReturnDay { get; set; }
        [StringLength(500)]
        public string ShippingDiscription { get; set; }
        [StringLength(250)]
        public string WordKey { get; set; }
        public bool SellOrBuy { get; set; }
        [Required]
        [StringLength(20)]
        public string UnitBuy { get; set; }
        [Column("wightUnitBuyWithKG")]
        public int WightUnitBuyWithKg { get; set; }
        public bool? Active { get; set; }
        [Column("FK_Category")]
        public int? FkCategory { get; set; }
    }
}
