using System;
using APEXAContracting.DataAccess.DotNetCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using APEXAContracting.Model.Entity;
using APEXAContracting.DataAccess.DotNetCore.Audit;

namespace APEXAContracting.DataAccess
{
    public partial class DatabaseContext : BaseDbContext
    {
        //public TestDBContext()
        //{
        //}

        //public TestDBContext(DbContextOptions<TestDBContext> options)
        //    : base(options)
        //{
        //}

        public virtual DbSet<BusinessType> BusinessType { get; set; }
        public virtual DbSet<BusinessUnit> BusinessUnit { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<HealthStatus> HealthStatus { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<UvContractLength> UvContractLength { get; set; }
        public virtual DbSet<UvHealthStatusWeight> UvHealthStatusWeight { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=APEXAContracting.Database;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<BusinessUnit>(entity =>
            {
                entity.HasIndex(e => e.HierarchyPrefix);

                entity.HasIndex(e => new { e.Name, e.Name2 })
                    .HasName("UX_BusinessUnit_NAMES")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.HierarchyKey)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.HierarchyPrefix)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name2)
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.HasOne(d => d.BusinessType)
                    .WithMany(p => p.BusinessUnit)
                    .HasForeignKey(d => d.BusinessTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BusinessUnit_BusinessType");

                entity.HasOne(d => d.HealthStatus)
                    .WithMany(p => p.BusinessUnit)
                    .HasForeignKey(d => d.HealthStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BusinessUnit_HealthStatus");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasIndex(e => new { e.OfferedById, e.AcceptedBykey })
                    .HasName("UX_Contract_Participants")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AcceptedBykey).HasMaxLength(10);

                entity.Property(e => e.ContractName).HasMaxLength(255);

                entity.Property(e => e.ContractPath).IsUnicode(false);

                entity.Property(e => e.EffectedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExpiredOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(dateadd(year,(1),getdate()))");

                entity.Property(e => e.OfferedByKey).HasMaxLength(10);

                entity.HasOne(d => d.AcceptedBy)
                    .WithMany(p => p.ContractAcceptedBy)
                    .HasForeignKey(d => d.AcceptedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_AcceptedBy");

                entity.HasOne(d => d.OfferedBy)
                    .WithMany(p => p.ContractOfferedBy)
                    .HasForeignKey(d => d.OfferedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_OfferedBy");
            });

            modelBuilder.Entity<HealthStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Weight).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UvContractLength>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("uv_ContractLength");

                entity.Property(e => e.AcceptedBykey).HasMaxLength(10);

                entity.Property(e => e.ContractName).HasMaxLength(255);

                entity.Property(e => e.ContractPath).IsUnicode(false);

                entity.Property(e => e.EffectedOn).HasColumnType("datetime");

                entity.Property(e => e.ExpiredOn).HasColumnType("datetime");

                entity.Property(e => e.OfferedByKey).HasMaxLength(10);
            });

            modelBuilder.Entity<UvHealthStatusWeight>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("uv_HealthStatusWeight");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
