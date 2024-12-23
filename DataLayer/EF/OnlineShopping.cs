using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataLayer.EF
{
    public partial class OnlineShopping : DbContext
    {
        public OnlineShopping()
        {

        }

        public OnlineShopping(DbContextOptions<OnlineShopping> options)
            : base(options)
        {

        }

        public virtual DbSet<Access> Access { get; set; }
        public virtual DbSet<Accounting> Accounting { get; set; }
        public virtual DbSet<BridgeBusinessOwnerMarketer> BridgeBusinessOwnerMarketer { get; set; }
        public virtual DbSet<BridgeInvoiceProduct> BridgeInvoiceProduct { get; set; }
        public virtual DbSet<BridgeProvinceBusinessOwner> BridgeProvinceBusinessOwner { get; set; }
        public virtual DbSet<BusinessOwner> BusinessOwner { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategorySetting> CategorySetting { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Content> Content { get; set; }
        public virtual DbSet<DisputeResolution> DisputeResolution { get; set; }
        public virtual DbSet<Entity> Entity { get; set; }
        public virtual DbSet<Filed> Filed { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<Languge> Languge { get; set; }
        public virtual DbSet<Marketer> Marketer { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<PromotionProduct> PromotionProduct { get; set; }
        public virtual DbSet<BrigeProductCategoryProperty> BrigeProductCategories { get; set; }
        public virtual DbSet<VwProduct> VwProduct { get; set; }
        public virtual DbSet<MultiLanguage> MultiLanguage { get; set; }
        public virtual DbSet<PaymentLog> PaymentLogs { get; set; }
        public virtual DbSet<CategoryProperty> CategoryProperties { get; set; }
        public virtual DbSet<Wish> Wishes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                ///#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //  optionsBuilder.UseSqlServer("Server=.\\SQL2;Database=ShoppingCenters;user=farid;password=123456");
                optionsBuilder.UseSqlServer("Server=.;Database=ShoppingCenters;user=babak;password=123456");
            }
        }
        //.HasIndex(p => new {p.FirstColumn , p.SecondColumn}).IsUnique();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Access>(entity =>
            {
                entity.HasIndex(e => new { e.FkFiled, e.FkRole })
                    .HasName("IX_Access")
                    .IsUnique();

                entity.HasOne(d => d.FkFiledNavigation)
                    .WithMany(p => p.Access)
                    .HasForeignKey(d => d.FkFiled)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Access_Filed");

                entity.HasOne(d => d.FkRoleNavigation)
                    .WithMany(p => p.Access)
                    .HasForeignKey(d => d.FkRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Access_Role");
            });

            modelBuilder.Entity<Accounting>(entity =>
            {


                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.Accounting)
                    .HasForeignKey(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Accounting_User");
            });

            modelBuilder.Entity<BridgeBusinessOwnerMarketer>(entity =>
            {
                entity.HasIndex(e => new { e.FkBusinessOwner, e.FkMarketer })
                    .HasName("IX_Bridge_BusinessOwner_Marketer")
                    .IsUnique();

                entity.HasOne(d => d.FkBusinessOwnerNavigation)
                    .WithMany(p => p.BridgeBusinessOwnerMarketer)
                    .HasForeignKey(d => d.FkBusinessOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bridge_BusinessOwner_Marketer_BusinessOwner");

                entity.HasOne(d => d.FkMarketerNavigation)
                    .WithMany(p => p.BridgeBusinessOwnerMarketer)
                    .HasForeignKey(d => d.FkMarketer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bridge_BusinessOwner_Marketer_Marketer");
            });

            modelBuilder.Entity<BridgeInvoiceProduct>(entity =>
            {
                entity.HasOne(d => d.FkInvoiceNavigation)
                    .WithMany(p => p.BridgeInvoiceProduct)
                    .HasForeignKey(d => d.FkInvoice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bridge_Invoice_Product_Invoice");

                entity.HasOne(d => d.FkProductNavigation)
                    .WithMany(p => p.BridgeInvoiceProduct)
                    .HasForeignKey(d => d.FkProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bridge_Invoice_Product_Product");
            });

            modelBuilder.Entity<BridgeProvinceBusinessOwner>(entity =>
            {
                entity.HasIndex(e => new { e.FkBusinessOwner, e.FkProvince })
                    .HasName("IX_Bridge_Province_BusinessOwner")
                    .IsUnique();

                entity.HasOne(d => d.FkBusinessOwnerNavigation)
                    .WithMany(p => p.BridgeProvinceBusinessOwner)
                    .HasForeignKey(d => d.FkBusinessOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bridge_Province_BusinessOwner_BusinessOwner");

                entity.HasOne(d => d.FkProvinceNavigation)
                    .WithMany(p => p.BridgeProvinceBusinessOwner)
                    .HasForeignKey(d => d.FkProvince)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bridge_Province_BusinessOwner_Province");
            });

            modelBuilder.Entity<BusinessOwner>(entity =>
            {
                entity.HasIndex(e => e.FkUser)
                    .HasName("IX_BusinessOwner")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("BusinessOwnerNameKey")
                    .IsUnique();

                entity.HasOne(d => d.FkMarketerNavigation)
                    .WithMany(p => p.BusinessOwner)
                    .HasForeignKey(d => d.FkMarketer)
                    .HasConstraintName("FK_BusinessOwner_Marketer");

                entity.HasOne(d => d.FkProvinceNavigation)
                    .WithMany(p => p.BusinessOwner)
                    .HasForeignKey(d => d.FkProvince)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BusinessOwner_Province");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithOne(p => p.BusinessOwner)
                    .HasForeignKey<BusinessOwner>(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BusinessOwner_User");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.IdsParent)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.FkCategoryNavigation)
                    .WithMany(p => p.InverseFkCategoryNavigation)
                    .HasForeignKey(d => d.FkCategory)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.FkCommentNavigation)
                    .WithMany(p => p.InverseFkCommentNavigation)
                    .HasForeignKey(d => d.FkComment)
                    .HasConstraintName("FK_Comment_Comment");

                entity.HasOne(d => d.FkProductNavigation)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.FkProduct)
                    .HasConstraintName("FK_Comment_Product");
            });

            modelBuilder.Entity<DisputeResolution>(entity =>
            {
                entity.Property(e => e.DisputerwhoPerson).HasComment("اگر در آینده لازم باشد که چه کسی از کی شکایت کرده لازم می شود ");

                entity.HasOne(d => d.FkBusinessOwnerNavigation)
                    .WithMany(p => p.DisputeResolution)
                    .HasForeignKey(d => d.FkBusinessOwner)
                    .HasConstraintName("FK_DisputeResolution_BusinessOwner");

                entity.HasOne(d => d.FkInvoiceNavigation)
                    .WithMany(p => p.DisputeResolution)
                    .HasForeignKey(d => d.FkInvoice)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DisputeResolution_Invoice");

                entity.HasOne(d => d.FkMarketerNavigation)
                    .WithMany(p => p.DisputeResolution)
                    .HasForeignKey(d => d.FkMarketer)
                    .HasConstraintName("FK_DisputeResolution_Marketer");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.DisputeResolution)
                    .HasForeignKey(d => d.FkUser)
                    .HasConstraintName("FK_DisputeResolution_User");
            });

            modelBuilder.Entity<Filed>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.FkEntity })
                    .HasName("IX_Filed")
                    .IsUnique();

                entity.HasOne(d => d.FkEntityNavigation)
                    .WithMany(p => p.Filed)
                    .HasForeignKey(d => d.FkEntity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Filed_Entity");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                //  entity.Property(e => e.Writer).HasComment("کد اولین رکورد مربوط به شکایت را نگه می دارد و این کد فقط توسط ایجاد کننده شکایت قابل پاک شدن است");

                entity.HasOne(d => d.FkBusinessOwnerNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.FkBusinessOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_BusinessOwner");

                entity.HasOne(d => d.FkMarketerNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.FkMarketer)
                    .HasConstraintName("FK_Invoice_Marketer");

                entity.HasOne(d => d.FkProvinceNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.FkProvince)
                    .HasConstraintName("FK_Invoice_Province");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_User1");
            });

            modelBuilder.Entity<Languge>(entity =>
            {
                entity.HasIndex(e => new { e.FkFiled, e.Name })
                    .HasName("IX_Languge")
                    .IsUnique();

                entity.HasOne(d => d.FkFiledNavigation)
                    .WithMany(p => p.Languge)
                    .HasForeignKey(d => d.FkFiled)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Languge_Filed");
            });

            modelBuilder.Entity<Marketer>(entity =>
            {
                entity.HasIndex(e => e.FkUser)
                    .HasName("IX_Marketer")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("MarketerNameKey")
                    .IsUnique();

                entity.HasOne(d => d.FkMarketerNavigation)
                    .WithMany(p => p.InverseFkMarketerNavigation)
                    .HasForeignKey(d => d.FkMarketer)
                    .HasConstraintName("FK_Marketer_Marketer");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithOne(p => p.Marketer)
                    .HasForeignKey<Marketer>(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Marketer_User");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(d => d.FkUseSenderNavigation)
                    .WithMany(p => p.MessageFkUseSenderNavigation)
                    .HasForeignKey(d => d.FkUseSender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_User");

                entity.HasOne(d => d.FkUserReceiverNavigation)
                    .WithMany(p => p.MessageFkUserReceiverNavigation)
                    .HasForeignKey(d => d.FkUserReceiver)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_User1");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.FkBusinessOwner })
                    .HasName("IX_Product")
                    .IsUnique();
                entity.HasIndex(e => new { e.NameForUrll })
                   .HasName("IX_NameForUrll")
                   .IsUnique();

                entity.HasOne(d => d.FkBusinessOwnerNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.FkBusinessOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_BusinessOwner");

                entity.HasOne(d => d.FkCategoryNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.FkCategory)
                    .HasConstraintName("FK_Product_Category");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasOne(d => d.FkCategorySettingNavigation)
                    .WithMany(p => p.Setting)
                    .HasForeignKey(d => d.FkCategorySetting)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Setting_CategorySetting");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("IX_User")
                    .IsUnique();

                entity.HasOne(d => d.FkRoleNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.FkRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });
            modelBuilder.Entity<Content>()
        .HasIndex(b => b.Name)
         .HasName("IX_Name")
        .IsUnique();

            modelBuilder.Entity<VwProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwProduct", "Shapping");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
