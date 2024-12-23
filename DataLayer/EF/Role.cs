using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Role", Schema = "Accsess")]
    public partial class Role
    {
        public Role()
        {
            Access = new HashSet<Access>();
            User = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Discription { get; set; }
        public short? Type { get; set; }

        [InverseProperty("FkRoleNavigation")]
        public virtual ICollection<Access> Access { get; set; }
        [InverseProperty("FkRoleNavigation")]
        public virtual ICollection<User> User { get; set; }
    }
}
