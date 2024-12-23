using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    public partial class MultiLanguage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string KeyLanguage { get; set; }
        public string PersianValue { get; set; }
    }
}
