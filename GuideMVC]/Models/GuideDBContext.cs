using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GuideMVC_.Models
{
    public partial class GuideDBContext : IdentityDbContext<ApplicationUser>
    {
        public GuideDBContext(DbContextOptions<GuideDBContext> options) : base(options)
        {
        }

        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<RelativeType> RelativeTypes { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<UserRelative> UserRelatives { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=GuideDBCodeFirst;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<RelativeType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(m => m.Person)
                .WithOne(m => m.ApplicationUser)
                .HasForeignKey<Person>(p=>p.UserId);
            
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.Firstname).HasMaxLength(50);

                entity.Property(e => e.Lastname).HasMaxLength(50);

                entity.Property(e => e.Middlename).HasMaxLength(50);

                entity.Property(e => e.PassportNumber).HasMaxLength(4);

                entity.Property(e => e.PassportSeries).HasMaxLength(6);
                
                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Genders");
            });

            modelBuilder.Entity<UserRelative>(entity =>
            {
                entity.HasKey(e => new { e.ToUserId, e.FromUserId, e.RelativeTypeId });

                entity.HasOne(d => d.FromPerson)
                    .WithMany(p => p.UserRelativeFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRelatives_User1");

                entity.HasOne(d => d.RelativeType)
                    .WithMany(p => p.UserRelatives)
                    .HasForeignKey(d => d.RelativeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRelatives_RelativeTypes");

                entity.HasOne(d => d.ToPerson)
                    .WithMany(p => p.UserRelativeToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRelatives_User");
            });

           
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
