using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DataLayer.EFLog
{
    public partial class EasyStoreLog : DbContext
    {
        public EasyStoreLog()
        {
        }

        public EasyStoreLog(DbContextOptions<EasyStoreLog> options)
            : base(options)
        {
        }

        public virtual DbSet<Log> Log { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

             string  constring = AppSetting.Configuration.GetConnectionString("ConnectionStringEasyStoreLog");

#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(constring);  //"Server=45.149.76.249;Database=EasyStoreLog;user=aliHassan;password=AliH@ssanHossein12400$"
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.TraceIdentifier).IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
