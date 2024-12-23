using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EFLog
{
    public partial class Log
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? UserId { get; set; }
        [StringLength(50)]
        public string UserInfo { get; set; }
        public short? LogType { get; set; }
        public string Message { get; set; }
        public string InnerMessage { get; set; }
        public string StackTrace { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string TraceIdentifier { get; set; }
        //
        [Column(TypeName = "datetime2(7)")]
        public DateTime RegisterDate { get; set; }
    }
}
