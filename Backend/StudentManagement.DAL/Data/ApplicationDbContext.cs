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
        public DbSet<Identity> Identity { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<StudentStatus> StudentStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //

            modelBuilder.Entity<Student>()
                .Property(s => s.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<Identity>()
                .Property(i => i.IdentityType)
                .HasConversion<string>();

            // Relationships

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
                .HasForeignKey<Address>(a => a.StudentId);

            modelBuilder.Entity<Student>()
                .HasOne(a => a.Identity)
                .WithOne(s => s.Student)
                .HasForeignKey<Identity>(a => a.StudentId);

        }
    }
}
