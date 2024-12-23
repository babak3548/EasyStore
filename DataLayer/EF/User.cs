using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("User", Schema = "Accsess")]
    public partial class User
    {
        public User()
        {
            Accounting = new HashSet<Accounting>();
            DisputeResolution = new HashSet<DisputeResolution>();
            Invoice = new HashSet<Invoice>();
            MessageFkUseSenderNavigation = new HashSet<Message>();
            MessageFkUserReceiverNavigation = new HashSet<Message>();
        }
        [Display(Name = "شناسه")]
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "درخواست دریافت پیشنهادات وِیژ")]
        public bool SpicialOffer { get; set; }
        [Display(Name = "عضویت در خبر نامه")]
        public bool Newsletter { get; set; }
        [StringLength(50)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "تاریخ تولد")]
        public DateTime Birthday { get; set; }
        [Display(Name = "جنسیت")]
        public Gender Gender { get; set; }
        
        [Required]
        [StringLength(14)]
        [Display(Name = "موبایل")]
        public string Mobile { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "تاریخ ثبت")]
        public string RegisterDate { get; set; }
        [Column("FK_Role")]
        [Display(Name = "کد نقش کاربری")]
        public int FkRole { get; set; }
        [StringLength(16)]
        [Display(Name = "آی پی ایجاد کننده")]
        public string IpComputerCreator { get; set; }
        [Display(Name = " فاکتور جاری")]
        public int? CurrentInvoice { get; set; }
        [Display(Name = "فعال ")]
        public bool? Ative { get; set; }
        [StringLength(10)]
        [Display(Name = "کد فعال سازی")]
        public string AtivationCode { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "هش پسورد")]
        public string Password { get; set; }
        [StringLength(30)]
        [Display(Name = "پسورد موقت")]
        public string TempPassword { get; set; }
        [StringLength(16)]
        [Display(Name = "آخرین لاگین کننده")]
        public string IpComputerLast { get; set; }

        [ForeignKey(nameof(FkRole))]
        [InverseProperty(nameof(Role.User))]
        public virtual Role FkRoleNavigation { get; set; }
        [InverseProperty("FkUserNavigation")]
        public virtual BusinessOwner BusinessOwner { get; set; }
        [InverseProperty("FkUserNavigation")]
        public virtual Marketer Marketer { get; set; }
        [InverseProperty("FkUserNavigation")]
        public virtual ICollection<Accounting> Accounting { get; set; }
        [InverseProperty("FkUserNavigation")]
        public virtual ICollection<DisputeResolution> DisputeResolution { get; set; }
        [InverseProperty("FkUserNavigation")]
        public virtual ICollection<Invoice> Invoice { get; set; }
        [InverseProperty(nameof(Message.FkUseSenderNavigation))]
        public virtual ICollection<Message> MessageFkUseSenderNavigation { get; set; }
        [InverseProperty(nameof(Message.FkUserReceiverNavigation))]
        public virtual ICollection<Message> MessageFkUserReceiverNavigation { get; set; }
        public virtual ICollection<PaymentLog> PaymentLogs { get; set; }
        [InverseProperty(nameof(Wish.User))]
        public virtual ICollection<Wish> Wishes { get; set; }

        //Wish
        //   [Index("IX_Order_key", IsClustered = false ,IsUnique =true, Order =2)]
        [MaxLength(50)]
        [Display(Name = "نام خانوادگی")]
        public string Family { get; set; }
        [MaxLength(12)]
        [Display(Name = "تلفن")]
        public string Tel { get; set; }
        [MaxLength(20)]
        [Display(Name = "نام شهر")]
        public string CityName { get; set; }
        [MaxLength(1000)]
        [Display(Name = "آدرس")]
        public string Address { get; set; }
        [Display(Name = "کد استان")]
        public string FkProvince { get; set; }
        [MaxLength(16)]
        [Display(Name = "آی پی آخرین لاگین کننده")]
        public string IpComputer { get; set; }
    }
}
