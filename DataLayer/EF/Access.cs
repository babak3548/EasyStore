using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Access", Schema = "Accsess")]
    public partial class Access
    {
        [Key]
        public int Id { get; set; }
        public long DisplayMode { get; set; }
        [Column("FK_Role")]
        public int FkRole { get; set; }
        [Column("FK_Filed")]
        public int FkFiled { get; set; }

        [ForeignKey(nameof(FkFiled))]
        [InverseProperty(nameof(Filed.Access))]
        public virtual Filed FkFiledNavigation { get; set; }
        [ForeignKey(nameof(FkRole))]
        [InverseProperty(nameof(Role.Access))]
        public virtual Role FkRoleNavigation { get; set; }
    }
}
