using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Comment", Schema = "Miscellaneous")]
    public partial class Comment
    {
        public Comment()
        {
            InverseFkCommentNavigation = new HashSet<Comment>();
        }

        [Key]
        [Display(Name = "شناسه")]
        public int Id { get; set; }
        [Column("FK_Comment")]
        [Display(Name = "کامنت پدر")]
        public int? FkComment { get; set; }
        [StringLength(50)]
        [Display(Name = "کلمات کلیدی")]
        public string KeyWord { get; set; }
        [StringLength(50)]
        [Display(Name = "نام کاربر")]
        public string FullName { get; set; }
        [StringLength(50)]
        [Display(Name = "ایمیل")]
        public string EmailOrMobile { get; set; }
        [Required]
        [Column("Text")]
        [Display(Name = "متن ")]
        [StringLength(600)]
        public string TextValue { get; set; }
        [Display(Name = "تعداد موافقین")]
        public float? VotePositive { get; set; }
        [Display(Name = " آیا نظر مثبت است")]
        public bool IsPositiveComment { get; set; }
        [Display(Name = "تعداد مخالفین")]
        public int VoteCount { get; set; }
        [StringLength(20)]
        [Display(Name = "آی پی کاربر")]
        public string ComputerIp { get; set; }
        [Display(Name = "فعال یا غیر فعال")]
        public bool? Active { get; set; }
        [Column("FK_Product")]
        [Display(Name = "شناسه محصول")]
        public int? FkProduct { get; set; }
        [Display(Name = "نوع کامنت  سوال یا نظر")]
        public short? CommentType { get; set; }

        [Display(Name = " تاریخ ثبت")]
        public DateTime RegisterDate { get; set; }

        [ForeignKey(nameof(FkComment))]
        [InverseProperty(nameof(Comment.InverseFkCommentNavigation))]
        public virtual Comment FkCommentNavigation { get; set; }
        [ForeignKey(nameof(FkProduct))]
        [InverseProperty(nameof(Product.Comment))]
        public virtual Product FkProductNavigation { get; set; }
        [InverseProperty(nameof(Comment.FkCommentNavigation))]
        public virtual ICollection<Comment> InverseFkCommentNavigation { get; set; }
    }
}
