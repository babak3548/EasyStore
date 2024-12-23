using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Entity", Schema = "Accsess")]
    public partial class Entity
    {
        public Entity()
        {
            Filed = new HashSet<Filed>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string PersianName { get; set; }

        [InverseProperty("FkEntityNavigation")]
        public virtual ICollection<Filed> Filed { get; set; }
    }
}
