using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EF
{
    [Table("Message", Schema = "Miscellaneous")]
    public partial class Message
    {
        [Key]
        public int Id { get; set; }
        [Column("FK_UseSender")]
        public int FkUseSender { get; set; }
        [Column("FK_UserReceiver")]
        public int FkUserReceiver { get; set; }
        [Required]
        [StringLength(10)]
        public string Date { get; set; }
        public bool Readed { get; set; }
        [Required]
        [StringLength(500)]
        public string Text { get; set; }
        [Column("type")]
        public short? Type { get; set; }

        [ForeignKey(nameof(FkUseSender))]
        [InverseProperty(nameof(User.MessageFkUseSenderNavigation))]
        public virtual User FkUseSenderNavigation { get; set; }
        [ForeignKey(nameof(FkUserReceiver))]
        [InverseProperty(nameof(User.MessageFkUserReceiverNavigation))]
        public virtual User FkUserReceiverNavigation { get; set; }
    }
}
