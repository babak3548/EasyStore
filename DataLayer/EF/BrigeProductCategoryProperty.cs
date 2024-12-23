using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.EF
{
    [Table("BrigeProductCategoryProperty", Schema = "Shapping")]
   public class BrigeProductCategoryProperty
    {
        public BrigeProductCategoryProperty()
        {
        }
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string value { get; set; }
        [Column("FkCategoryProperty")]
        public int FkCategoryProperty { get; set; }

        [Column("FkProduct")]
        public int FkProduct { get; set; }

        [ForeignKey(nameof(FkCategoryProperty))]
        public virtual CategoryProperty CategoryProperty { get; set; }


        [ForeignKey(nameof(FkProduct))]
        public virtual Product Product { get; set; }
    }
}
