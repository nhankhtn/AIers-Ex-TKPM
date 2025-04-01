using Microsoft.EntityFrameworkCore;
using StudentManagement.DAL.Data;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests
{
    public static class TestDataSeeder
    {
        public static List<Faculty> faculties = new List<Faculty>
        {
            new Faculty
            {
                Id = Guid.NewGuid(),
                Name = "Information Technology",
            },
            new Faculty
            {
                Id = Guid.NewGuid(),
                Name = "Business Administration",
            }
        };

        public static List<Program> programs = new List<Program>
        {
            new Program
            {
                Id = Guid.NewGuid(),
                Name = "Software Engineering",
            },
            new Program
            {
                Id = Guid.NewGuid(),
                Name = "Business Administration",
            }
        };

        public static List<StudentStatus> statuses = new List<StudentStatus>
        {
            new StudentStatus
            {
                Id = Guid.NewGuid(),
                Name = "Active",
            },
            new StudentStatus
            {
                Id = Guid.NewGuid(),
                Name = "Graduated",
            }
        };

        public static List<Student> students = new List<Student>()
            {
                new Student
                {
                    Id = "ST001",
                    Name = "Nguyen Van A",
                    DateOfBirth = new DateTime(2002, 5, 12),
                    Gender = Gender.Male,
                    Email = "nguyenvana@example.com",
                    Course = 2022,
                    Phone = "0123432234",
                    PermanentAddress = "Hanoi, Vietnam",
                    TemporaryAddress = "Ho Chi Minh City, Vietnam",
                    MailingAddress = "Hanoi, Vietnam",
                    ProgramId = programs[0].Id,
                    StatusId = statuses[0].Id,
                    FacultyId = faculties[0].Id,
                    Nationality = "Vietnam",
                    Identity = new Identity
                    {
                        Type = IdentityType.CCCD,
                        DocumentNumber = "1",
                    }
                },
                new Student
                {
                    Id = "ST002",
                    Name = "Tran Thi B",
                    DateOfBirth = new DateTime(2001, 8, 23),
                    Gender = Gender.Female,
                    Email = "tranthib@example.com",
                    Course = 2021,
                    Phone = "0987654321",
                    PermanentAddress = "Da Nang, Vietnam",
                    TemporaryAddress = "Hue, Vietnam",
                    MailingAddress = "Da Nang, Vietnam",
                    ProgramId = programs[0].Id,
                    StatusId = statuses[0].Id,
                    FacultyId = faculties[0].Id,
                    Nationality = "Vietnam",
                    Identity = new Identity
                    {
                        Type = IdentityType.CCCD,
                        DocumentNumber = "2",
                    }
                },
                new Student
                {
                    Id = "ST003",
                    Name = "Le Van C",
                    DateOfBirth = new DateTime(2003, 3, 15),
                    Gender = Gender.Male,
                    Email = "levanc@example.com",
                    Course = 2023,
                    Phone = "0345678901",
                    PermanentAddress = "Hai Phong, Vietnam",
                    TemporaryAddress = "Nha Trang, Vietnam",
                    MailingAddress = "Hai Phong, Vietnam",
                    ProgramId = programs[0].Id,
                    StatusId = statuses[0].Id,
                    FacultyId = faculties[0].Id,
                    Nationality = "Vietnam",
                    Identity = new Identity
                    {
                        Type = IdentityType.CCCD,
                        DocumentNumber = "3",
                    }
                },
                new Student
                {
                    Id = "ST004",
                    Name = "Pham Thi D",
                    DateOfBirth = new DateTime(2000, 12, 30),
                    Gender = Gender.Female,
                    Email = "phamthid@example.com",
                    Course = 2020,
                    Phone = "0456789012",
                    PermanentAddress = "Can Tho, Vietnam",
                    TemporaryAddress = "Vung Tau, Vietnam",
                    MailingAddress = "Can Tho, Vietnam",
                    ProgramId = programs[0].Id,
                    StatusId = statuses[0].Id,
                    FacultyId = faculties[0].Id,
                    Nationality = "Vietnam",
                    Identity = new Identity
                    {
                        Type = IdentityType.CCCD,
                        DocumentNumber = "4",
                    }
                },
                new Student
                {
                    Id = "ST005",
                    Name = "Hoang Van E",
                    DateOfBirth = new DateTime(1999, 7, 7),
                    Gender = Gender.Male,
                    Email = "hoangvane@example.com",
                    Course = 2019,
                    Phone = "0567890123",
                    PermanentAddress = "Quang Ninh, Vietnam",
                    TemporaryAddress = "Binh Duong, Vietnam",
                    MailingAddress = "Quang Ninh, Vietnam",
                    ProgramId = programs[0].Id,
                    StatusId = statuses[0].Id,
                    FacultyId = faculties[0].Id,
                    Nationality = "Vietnam",
                    Identity = new Identity
                    {
                        Type = IdentityType.CCCD,
                        DocumentNumber = "5",
                    }
                }
            };

        public static void Seed(this ApplicationDbContext _context)
        {
            _context.Faculties.AddRange(faculties);
            _context.Programs.AddRange(programs);
            _context.StudentStatuses.AddRange(statuses);
            _context.Students.AddRange(students);

            _context.SaveChanges();
        }
    }
}
