using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Category", Schema = "Shapping")]
    public partial class Category
    {
        public Category()
        {
            InverseFkCategoryNavigation = new HashSet<Category>();
            Product = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage ="مقدار نام اجباری می باشد")]
        [Display(Name = "نام گروه")]
        public string Name { get; set; }

       
        [Required(ErrorMessage = "مقدار نام اجباری می باشد")]
        [Display(Name = "عنوان صفحه برای سو")]
        public string TitlePage { get; set; }
        public PromotionCategory PromotionType { get; set; }
        
        [StringLength(500)]
        [Display(Name = "نام انگلیسی برای آدرس")]
        public string Discription { get; set; }
        [Column("FK_Category")]
        public int? FkCategory { get; set; }
        [Column("Ids_Parent")]
        [StringLength(50)]
        [Display(Name = "شناسه های والد")]
        public string IdsParent { get; set; }
        [StringLength(100)]
        [Display(Name = "تصویر")]
        public string Image { get; set; }
        public bool  Active { get; set; }

        [ForeignKey(nameof(FkCategory))]
        [InverseProperty(nameof(Category.InverseFkCategoryNavigation))]
        public virtual Category FkCategoryNavigation { get; set; }
        [InverseProperty(nameof(Category.FkCategoryNavigation))]
        public virtual ICollection<Category> InverseFkCategoryNavigation { get; set; }
      //  [InverseProperty("FKCategory")]
     //   public virtual ICollection<CategoryProperty> InverseFKCategoryProperty { get; set; }

        //
        [InverseProperty("FkCategoryNavigation")]
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<PromotionProduct> PromotionProducts { get; set; }
        

        
    }
}
