using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("CategorySetting", Schema = "Miscellaneous")]
    public partial class CategorySetting
    {
        public CategorySetting()
        {
            Setting = new HashSet<Setting>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string GroupName { get; set; }
        [StringLength(100)]
        public string Discription { get; set; }

        [InverseProperty("FkCategorySettingNavigation")]
        public virtual ICollection<Setting> Setting { get; set; }
    }
}
