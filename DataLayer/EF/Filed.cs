using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Filed", Schema = "Accsess")]
    public partial class Filed
    {
        public Filed()
        {
            Access = new HashSet<Access>();
            Languge = new HashSet<Languge>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public short? PartialType { get; set; }
        public short OrderByValue { get; set; }
        [Column("FK_Entity")]
        public int FkEntity { get; set; }

        [ForeignKey(nameof(FkEntity))]
        [InverseProperty(nameof(Entity.Filed))]
        public virtual Entity FkEntityNavigation { get; set; }
        [InverseProperty("FkFiledNavigation")]
        public virtual ICollection<Access> Access { get; set; }
        [InverseProperty("FkFiledNavigation")]
        public virtual ICollection<Languge> Languge { get; set; }
    }
}
