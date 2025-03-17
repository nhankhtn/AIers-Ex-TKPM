using Microsoft.EntityFrameworkCore;
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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Identity> Identities { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<StudentStatus> StudentStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //

            modelBuilder.Entity<Student>()
                .Property(s => s.Gender)
                .HasConversion<int>();

            modelBuilder.Entity<Identity>()
                .Property(i => i.IdentityType)
                .HasConversion<int>();

            // Relationships

            modelBuilder.Entity<Address>()
                .HasIndex(a => a.StudentId)
                .IsUnique();

            modelBuilder.Entity<Identity>()
                .HasIndex(i => i.StudentId)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Faculty)
                .WithMany(f => f.Students)
                .HasForeignKey(s => s.FacultyId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Program)
                .WithMany(p => p.Students)
                .HasForeignKey(s => s.ProgramId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Status)
                .WithMany(ss => ss.Students)
                .HasForeignKey(s => s.StatusId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Address)
                .WithOne(a => a.Student)
                .HasForeignKey<Address>(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasOne(a => a.Identity)
                .WithOne(s => s.Student)
                .HasForeignKey<Identity>(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);


            SeedData(modelBuilder);

        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentStatus>().HasData(
                    new StudentStatus { Id = 1, Code = "ACT", Name = "Active" },
                    new StudentStatus { Id = 2, Code = "IAC", Name = "Inactive" }
                );

            modelBuilder.Entity<Faculty>().HasData(
                    new Faculty { Id = 1, Code="CNTT", Name = "Information Technology" },
                    new Faculty { Id = 2, Code="BA", Name = "Business Administration" }
                );

            modelBuilder.Entity<Program>().HasData(
                    new Program { Id = 1, Code="SE", Name = "Software Engineering" },
                    new Program { Id = 2, Code="CS", Name = "Computer Science" },
                    new Program { Id = 3, Code="BA", Name = "Business Administration" }
                );
        }
    }
}
