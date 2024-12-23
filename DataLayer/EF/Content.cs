using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Content", Schema = "Accsess")]
    public partial class Content
    {
        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [StringLength(50)]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "محتوای اصلی")]
        public string ShowValue { get; set; }
        [Display(Name = "عکس بنر حداکثر 5 مگا بایت")]
        public string BanerImageAdress { get; set; }
        [Display(Name = "آدرس ویدیو حداکثر 30 مگا بایت")]
        public string VideoAdress { get; set; }
        [Display(Name = "الویت نمایش")]
        public int Position { get; set; }
        [Display(Name = "نوع محتوا ")]
        public ContentTypes ContentType { get; set; }
        [Display(Name = "نویسنده")]
        public string Writer  { get; set; }
        [Display(Name = "چکیده")]
        public string Abstract { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }
        [Display(Name = "تاریخ ویرایش")]
        public DateTime UpdateDate { get; set; }
    }
}
