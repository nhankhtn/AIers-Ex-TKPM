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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<StudentStatus> StudentStatuses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Identity> Identities { get; set; }
        //public DbSet<StudentNationalities> Nationalities { get; set; }

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
                .HasOne(a => a.Identity)
                .WithOne(s => s.Student)
                .HasForeignKey<Identity>(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Student>()
            //    .HasOne(a => a.Nationalities)
            //    .WithOne(s => s.Student)
            //    .HasForeignKey<StudentNationalities>(a => a.StudentId)
            //    .OnDelete(DeleteBehavior.Cascade);


            // SeedData(modelBuilder);

        }


        //private void SeedData(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<StudentStatus>().HasData(
        //            new StudentStatus { Id = Guid.NewGuid(), Code = "ACT", Name = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
        //            new StudentStatus { Id = Guid.NewGuid(), Code = "IAC", Name = "Inactive", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        //        );

        //    modelBuilder.Entity<Faculty>().HasData(
        //            new Faculty { Id = Guid.NewGuid(), Code="CNTT", Name = "Information Technology", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        //            new Faculty { Id = Guid.NewGuid(), Code="BA", Name = "Business Administration", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        //        );

        //    modelBuilder.Entity<Program>().HasData(
        //            new Program { Id = Guid.NewGuid(), Code="SE", Name = "Software Engineering", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        //            new Program { Id = Guid.NewGuid(), Code="CS", Name = "Computer Science", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        //            new Program { Id = Guid.NewGuid(), Code="BA", Name = "Business Administration", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        //        );
        //}
    }
}
