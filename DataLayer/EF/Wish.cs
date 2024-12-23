using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
  public  class Wish
    {
        [Key]
        public int Id { get; set; }
        public int FkUser { get; set; }
        public int FkProduct { get; set; }
        [Column(TypeName = "datetime2(7)")]
        public DateTime RegisterDate { get; set; }
        public bool IsDeleted { get; set; }
        [InverseProperty("Wishes")]
        public virtual User User { get; set; }
        [ForeignKey(nameof(FkProduct))]
     
        [InverseProperty("Wishes")] // اشاره به لیست در کلاس پروداکت دارد
        public virtual Product Product { get; set; }
 

    }
}
