using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Languge", Schema = "Accsess")]
    public partial class Languge
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Value { get; set; }
        [StringLength(150)]
        public string Title { get; set; }
        [Column("FK_Filed")]
        public int FkFiled { get; set; }

        [ForeignKey(nameof(FkFiled))]
        [InverseProperty(nameof(Filed.Languge))]
        public virtual Filed FkFiledNavigation { get; set; }
    }
}
