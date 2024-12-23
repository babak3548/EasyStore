using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Setting", Schema = "Miscellaneous")]
    
    public partial class Setting
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]

        public string Name { get; set; }
        [StringLength(50)]
        public string GroupName { get; set; }
        [StringLength(50)]
        public string Value { get; set; }

        [Column("FK_CategorySetting")]
        public int FkCategorySetting { get; set; }

        [ForeignKey(nameof(FkCategorySetting))]
        [InverseProperty(nameof(CategorySetting.Setting))]
        public virtual CategorySetting FkCategorySettingNavigation { get; set; }
    }
}
