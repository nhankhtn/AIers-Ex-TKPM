using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.DAL.Data;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
namespace StudentManagement.Tests.Unit.Repository
{
    [Collection("StudentRepository")]
    public class StudentRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentRepository _studentRepository;

        public StudentRepositoryTests()
        {
            _context = TestDbContextFactory.Create();
            _studentRepository = new StudentRepository(_context);
        }

        [Fact]
        public async Task GetAllStudentsASync_StudentExist_ReturnsStudent()
        {

            // Act
            var students = await _studentRepository.GetAllStudentsAsync(1, 10, null, null, null, null);
            // Assert
            Assert.NotNull(students.students);
            Assert.Equal(5, students.total);
        }



        [Fact]
        public async Task GetStudentByIdAsync_StudentExist_ReturnsStudent()
        {

            // Arrange
            string studentId = "ST005";
            // Act
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            // Assert
            Assert.NotNull(student);
            Assert.Equal(studentId, student.Id);
        }

        [Fact]
        public async Task GetStudentByNameAsync_StudentExist_ReturnsStudent()
        {
            var std = await _studentRepository.GetStudentByIdAsync("ST001");
            Assert.NotNull(std);
            // Act
            var students = await _studentRepository.GetStudentsByNameAsync(std.Name);
            var student = students.FirstOrDefault();
            // Assert
            Assert.NotNull(student);
            Assert.Equal(std.Name, student.Name);
        }

        [Fact]
        public async Task AddStudentAsync_StudentNotExist_AddStudent()
        {


            // Arrange
            List<Student> newStudents = new List<Student>()
            {
               new Student
               {
                   Id = "ST006",
                   Name = "Le Van Do Ki",
                   DateOfBirth = new DateTime(2001, 8, 23),
                   Gender = Gender.Male,
                   Email = "levandoki@example.com",
                   Course = 2021,
                   Phone = "0987654321",
                   PermanentAddress = "Da Nang, Vietnam",
                   TemporaryAddress = "Hue, Vietnam",
                   MailingAddress = "Da Nang, Vietnam",
                   ProgramId = TestDbContextFactory.Guid1,
                   StatusId = TestDbContextFactory.Guid1,
                   FacultyId = TestDbContextFactory.Guid1,
                   Nationality = "Vietnam",
                   Identity = new Identity
                   {
                       Type = IdentityType.CCCD,
                       DocumentNumber = "6",
                   }

               },
               new Student
               {
                   Id = "ST007",
                   Name = "Le Nan Do",
                   DateOfBirth = new DateTime(2000, 7, 10),
                   Gender = Gender.Male,
                   Email = "lenando@example.com",
                   Course = 2022,
                   Phone = "0987654321",
                   PermanentAddress = "Da Nang, Vietnam",
                   TemporaryAddress = "Hue, Vietnam",
                   MailingAddress = "Da Nang, Vietnam",
                   ProgramId = TestDbContextFactory.Guid1,
                   StatusId = TestDbContextFactory.Guid2,
                   FacultyId = TestDbContextFactory.Guid2,
                   Nationality = "Vietnam",
                   Identity = new Identity
                   {
                       Type = IdentityType.CCCD,
                       DocumentNumber = "7",
                   }
               }
            };
            var result = await _studentRepository.AddStudentAsync(newStudents);
            var students = await _studentRepository.GetAllStudentsAsync(1, 10, null, null, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(7, students.total);
            
            // Delete added students
            foreach (var student in newStudents)
            {
                await _studentRepository.DeleteStudentAsync(student.Id);
            }
        }

        [Fact]
        public async Task UpdateStudentAsync_StudentExist_UpdateStudent()
        { 
            // Act
            var student = await _studentRepository.GetStudentByIdAsync("ST001");
            Assert.NotNull(student);
            var newAddress = "KTX";
            student.PermanentAddress = newAddress;
            var result = await _studentRepository.UpdateStudentAsync(student);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newAddress, result.PermanentAddress);
        }

        [Fact]
        public async Task DeleteStudentAsync_StudentExist_DeleteStudent()
        {
            // Arrange
            string studentId = "ST002";
            // Act
            await _studentRepository.DeleteStudentAsync(studentId);
            // Assert
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            Assert.Null(student);
        }

        [Fact]
        public async Task IsEmailExistAsync_EmailExist_ReturnsTrue()
        {
            // Arrange
            string email = "john@gmail.com";

            // Act
            var result = await _studentRepository.GetStudentByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
        }
    }
}
