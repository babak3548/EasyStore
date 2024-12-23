using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Province", Schema = "Miscellaneous")]
    public partial class Province
    {
        public Province()
        {
            BridgeProvinceBusinessOwner = new HashSet<BridgeProvinceBusinessOwner>();
            BusinessOwner = new HashSet<BusinessOwner>();
            Invoice = new HashSet<Invoice>();
        }

        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [StringLength(50)]
        [Display(Name = "نام استان")]
        public string Name { get; set; }
        [StringLength(50)]
        [Display(Name = "نام کشور")]
        public string Country { get; set; }

        [Column("Fk_Province")]
        [Display(Name = "کلید استان یا شهر در برگیرنده این محل")]
        public int? FkProvince { get; set; }

        [ForeignKey(nameof(FkProvince))]
        [InverseProperty(nameof(Province.InverseFkProvinceNavigation))]
        [Display(Name = "استان یا شهر در برگیرنده این محل")]
        public virtual Province FkProvinceNavigation { get; set; }
        [InverseProperty(nameof(Province.FkProvinceNavigation))]
        public virtual ICollection<Province> InverseFkProvinceNavigation { get; set; }


        [InverseProperty("FkProvinceNavigation")]
        public virtual ICollection<BridgeProvinceBusinessOwner> BridgeProvinceBusinessOwner { get; set; }
        [InverseProperty("FkProvinceNavigation")]
        public virtual ICollection<BusinessOwner> BusinessOwner { get; set; }
        [InverseProperty("FkProvinceNavigation")]
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
