using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace collaby_backend.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=collabyDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsDraft)
                    .HasColumnName("isDraft")
                    .HasColumnType("BOOLEAN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("VARCHAR(1000)");

                entity.Property(e => e.PostId).HasColumnName("postID");

                entity.Property(e => e.ReportCount).HasColumnName("reportCount");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateCreated)
                    .HasColumnName("dateCreated")
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("datetime('now')");

                entity.Property(e => e.DateModified)
                    .HasColumnName("dateModified")
                    .HasColumnType("DATETIME");

                entity.Property(e => e.IsDraft)
                    .HasColumnName("isDraft")
                    .HasColumnType("BOOLEAN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("VARCHAR(12500)");

                entity.Property(e => e.RatingCount).HasColumnName("ratingCount");

                entity.Property(e => e.RatingValue)
                    .HasColumnName("ratingValue")
                    .HasColumnType("DOUBLE");

                entity.Property(e => e.TotalComments).HasColumnName("totalComments");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.PostId).HasColumnName("postID");

                entity.Property(e => e.Ratings)
                    .HasColumnName("rating")
                    .HasColumnType("NUMERIC(1,1)");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateCreated)
                    .HasColumnName("dateCreated")
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("datetime('now')");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("VARCHAR(12500)");

                entity.Property(e => e.PostId).HasColumnName("postID");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("VARCHAR(40)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("firstName")
                    .HasColumnType("VARCHAR(40)");

                entity.Property(e => e.Img)
                    .HasColumnName("img")
                    .HasColumnType("CHAR(10000)");

                entity.Property(e => e.IsAdmin)
                    .HasColumnName("isAdmin")
                    .HasColumnType("BOOLEAN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("isDeleted")
                    .HasColumnType("BOOLEAN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("lastName")
                    .HasColumnType("VARCHAR(40)");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasColumnType("VARCHAR(40)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("VARCHAR(40)");

                entity.Property(e => e.TotalPosts).HasColumnName("totalPosts");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName")
                    .HasColumnType("VARCHAR(40)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
