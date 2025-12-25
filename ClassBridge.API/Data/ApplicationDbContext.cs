using ClassBridge.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassBridge.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<TeacherStrength> TeacherStrengths { get; set; }
        public DbSet<TeacherAvailability> TeacherAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Teacher configuration
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Teacher)
                    .HasForeignKey<Teacher>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Bio).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Qualification).IsRequired().HasMaxLength(200);
                entity.Property(e => e.HourlyRate).HasColumnType("decimal(18,2)");
            });

            // Parent configuration
            modelBuilder.Entity<Parent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Parent)
                    .HasForeignKey<Parent>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PostalCode).IsRequired().HasMaxLength(10);
            });

            // Child configuration
            modelBuilder.Entity<Child>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(e => e.ParentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Grade).IsRequired().HasMaxLength(50);
            });

            // TeacherStrength configuration
            modelBuilder.Entity<TeacherStrength>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Teacher)
                    .WithMany(t => t.Strengths)
                    .HasForeignKey(e => e.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // TeacherAvailability configuration
            modelBuilder.Entity<TeacherAvailability>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Teacher)
                    .WithMany(t => t.Availabilities)
                    .HasForeignKey(e => e.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}