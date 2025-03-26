using Microsoft.EntityFrameworkCore;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly List<AuditEntry> _auditEntries;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, List<AuditEntry> auditEntries) : base(options)
        {
            _auditEntries = auditEntries;
        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<StudentStatus> StudentStatuses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Identity> Identities { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<Student>()
                .Property(s => s.Gender)
                .HasConversion<string>()
                .HasDefaultValue(Gender.Male);

            modelBuilder.Entity<Identity>()
                .Property(i => i.Type)
                .HasConversion<string>()
                .HasDefaultValue(IdentityType.CCCD);


            // Relationships

            modelBuilder.Entity<Identity>()
                .HasIndex(i => i.StudentId)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Faculty)
                .WithMany(f => f.Students)
                .HasForeignKey(s => s.FacultyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Program)
                .WithMany(p => p.Students)
                .HasForeignKey(s => s.ProgramId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Status)
                .WithMany(ss => ss.Students)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Student>()
                .HasOne(a => a.Identity)
                .WithOne(s => s.Student)
                .HasForeignKey<Identity>(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Program>(b =>
            {
                b.HasData(
                    new Program
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111111"),
                        Name = "Đại trà"
                    },
                    new Program
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111112"),
                        Name = "Chất lượng cao"
                    },
                    new Program
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111113"),
                        Name = "Tiên tiến"
                    });
            });
            modelBuilder.Entity<StudentStatus>(b =>
            {
                b.HasData(
                    new StudentStatus
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111111"),
                        Name = "Đang học",
                        Order = 1,
                    },
                    new StudentStatus
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111112"),
                        Name = "Đã tốt nghiệp",
                        Order = 2,
                    },
                    new StudentStatus
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111113"),
                        Name = "Đã bảo lưu",
                        Order = 1,
                    },
                    new StudentStatus
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111114"),
                        Name = "Đã nghỉ học",
                        Order = 3,
                    });
            });
            modelBuilder.Entity<Faculty>(b =>
            {
                b.HasData(
                    new Faculty
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111111"),
                        Name = "Khoa Toán"
                    },
                    new Faculty
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111112"),
                        Name = "Khoa Công nghệ thông tin"
                    },
                    new Faculty
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111113"),
                        Name = "Khoa Hoá"
                    },
                    new Faculty
                    {
                        Id = new Guid("11111111-1111-1111-1111-111111111114"),
                        Name = "Khoa Lí"
                    });
            });
            modelBuilder.Entity<Setting>(b =>
            b.HasData(
                new Setting { Id =1 }
            ));

        }
    }
}
