using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
    public class StudentRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IStudentRepository _studentRepository;

        public StudentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase("TestDatabase")  // Sử dụng cùng một tên database
                            .Options;

            _context = new ApplicationDbContext(options, null!);
            _context.Database.EnsureDeleted();  // Xóa database hiện tại trước khi tạo mới
            _context.Database.EnsureCreated();  // Tạo schema mới
            _context.Seed();  // Seed lại dữ liệu

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

            //Arrange
            string name = "Tran Thi B";
            // Act
            var students = await _studentRepository.GetStudentsByNameAsync(name);
            var student = students.FirstOrDefault();
            // Assert
            Assert.NotNull(student);
            Assert.Equal(name, student.Name);
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
                   ProgramId = TestDataSeeder.programs[0].Id,
                   StatusId = TestDataSeeder.statuses[0].Id,
                   FacultyId = TestDataSeeder.faculties[1].Id,
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
                   ProgramId = TestDataSeeder.programs[1].Id,
                   StatusId = TestDataSeeder.statuses[1].Id,
                   FacultyId = TestDataSeeder.faculties[1].Id,
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

            // Add deleted student back
            await _studentRepository.AddStudentAsync(new List<Student> { TestDataSeeder.students[1] });
        }

        [Fact]
        public async Task IsEmailExistAsync_EmailExist_ReturnsTrue()
        {
            // Arrange
            string email = "nguyenvana@example.com";

            // Act
            var result = await _studentRepository.IsEmailDuplicateAsync(email);

            // Assert
            Assert.True(result);
        }
    }
}
