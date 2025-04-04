using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StudentManagement.DAL.Data;
using StudentManagement.DAL.Migrations;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests
{
    public static class TestDbContextFactory
    {
        public static Guid Guid1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static Guid Guid2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
        public static Guid Guid3 = Guid.Parse("00000000-0000-0000-0000-000000000003");
        public static Guid Guid4 = Guid.Parse("00000000-0000-0000-0000-000000000004");
        public static Guid Guid5 = Guid.Parse("00000000-0000-0000-0000-000000000005");

        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options, null!);

            context.Database.EnsureCreated();
            context.CreateSeedData();
            return context;
        }

        private static void CreateSeedData(this ApplicationDbContext context)
        {
            context.Students.RemoveRange(context.Students);
            context.StudentStatuses.RemoveRange(context.StudentStatuses);
            context.Programs.RemoveRange(context.Programs);
            context.Faculties.RemoveRange(context.Faculties);
            context.Settings.RemoveRange(context.Settings);
            
            var faculties = new List<Faculty>
            {
                new Faculty { Id = Guid1, Name = "Faculty 1" },
                new Faculty { Id = Guid2, Name = "Faculty 2" },
                new Faculty { Id = Guid3, Name = "Faculty 3" }
            };

            var programs = new List<Program>
            {
                new Program { Id = Guid1, Name = "Program 1" },
                new Program { Id = Guid2, Name = "Program 2" },
                new Program { Id = Guid3, Name = "Program 3" }
            };

            var statuses = new List<StudentStatus>
            {
                new StudentStatus { Id = Guid1, Name = "Active", Order = 0 },
                new StudentStatus { Id = Guid2, Name = "Inactive", Order = 1 }
            };

            var settings = new List<Setting>
            {
                new Setting { EmailDomain = "@gmail.com", EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"}
            };


            var students = new List<Student>
            {
                new Student { Id = "ST001", Name = "John Doe", DateOfBirth = new DateTime(2000, 1, 1), Gender = Gender.Male, Email = "john@gmail.com", Course = 2022, Phone = "+84323456789", PermanentAddress = "Address 1", ProgramId = programs[0].Id, StatusId = statuses[0].Id, FacultyId = faculties[0].Id, Nationality = "USA", Identity = new Identity { DocumentNumber = "111" } },
                new Student { Id = "ST002", Name = "Jane Doe", DateOfBirth = new DateTime(2001, 2, 2), Gender = Gender.Female, Email = "jane@gmail.com", Course = 2022, Phone = "+84334567890", PermanentAddress = "Address 2", ProgramId = programs[1].Id, StatusId = statuses[1].Id, FacultyId = faculties[1].Id, Nationality = "UK", Identity = new Identity { DocumentNumber = "222" } },
                new Student { Id = "ST003", Name = "Alice Smith", DateOfBirth = new DateTime(2002, 3, 3), Gender = Gender.Female, Email = "alice@gmail.com", Course = 2023, Phone = "+84345678901", PermanentAddress = "Address 3", ProgramId = programs[2].Id, StatusId = statuses[0].Id, FacultyId = faculties[1].Id, Nationality = "Canada", Identity = new Identity { DocumentNumber = "333" } },
                new Student { Id = "ST004", Name = "Bob Brown", DateOfBirth = new DateTime(2003, 4, 4), Gender = Gender.Male, Email = "bob@gmail.com", Course = 2024, Phone = "+84956789012", PermanentAddress = "Address 4", ProgramId = programs[0].Id, StatusId = statuses[0].Id, FacultyId = faculties[2].Id, Nationality = "Australia", Identity = new Identity { DocumentNumber = "444" } },
                new Student { Id = "ST005", Name = "Charlie Johnson", DateOfBirth = new DateTime(2004, 5, 5), Gender = Gender.Male, Email = "charlie@gmail.com", Course = 2025, Phone = "+84367890123", PermanentAddress = "Address 5", ProgramId = programs[1].Id, StatusId = statuses[0].Id, FacultyId = faculties[2].Id, Nationality = "Germany", Identity = new Identity { DocumentNumber = "555" } }
            };

            context.Faculties.AddRange(faculties);
            context.Programs.AddRange(programs);
            context.StudentStatuses.AddRange(statuses);
            context.Settings.AddRange(settings);
            context.Students.AddRange(students);

            context.SaveChanges();
        }
    }
}
