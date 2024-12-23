using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.EF
{
    [Table("CategoryProperty", Schema = "Shapping")]
    public class CategoryProperty
    {
        public CategoryProperty()
        {
        }

        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "مقدار نام اجباری می باشد")]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "توضیحات")]
        [StringLength(500)]
        public string Discription { get; set; }
        [Display(Name = "گروه")]
        public int FKCategory { get; set; }

        [ForeignKey(nameof(FKCategory))]
        [Display(Name = "گروه")]
        public virtual Category FkCategoryNavigation { get; set; }

        public virtual ICollection<BrigeProductCategoryProperty> BrigeProductCategories { get; set; }




    }
}
