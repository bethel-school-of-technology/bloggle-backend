using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace collaby_backend.Models
{
    public partial class ApplicationUserDb : DbContext
    {
        public ApplicationUserDb()
        {
        }

        public ApplicationUserDb(DbContextOptions<ApplicationUserDb> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=C:/repos/collaby-backend/source/user.db;foreign keys=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .IsUnique();

                entity.Property(e => e.Id);

                entity.Property(e => e.DateCreated)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("VARCHAR(40)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("VARCHAR(65)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("VARCHAR(40)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
