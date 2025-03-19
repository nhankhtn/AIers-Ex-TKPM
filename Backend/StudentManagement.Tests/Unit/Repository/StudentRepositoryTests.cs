using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManagement.DAL.Data;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
namespace StudentManagement.Tests.Unit.Repository
{
    public class StudentRepositoryTests
    {
        //private readonly ApplicationDbContext _context;
        //private readonly IStudentRepository _studentRepository;
        //public StudentRepositoryTests()
        //{
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: "StudentManagement")
        //        .Options;
        //    _context = new ApplicationDbContext(options);
        //    _studentRepository = new StudentRepository(_context);

        //    _context.Students.AddRange(SeedData());
        //    _context.SaveChanges();

        //}

        //private Student[] SeedData()
        //{
        //    List<Student> students = new List<Student>()
        //    {
        //        //new Student { Id = "22120249", Name = "Nguyen Van A", DateOfBirth = new DateTime(2004, 1, 1), Email = "nguyenvana@gmail.com",
        //        //    Gender = Gender.Male, Faculty=Faculty.Law, Course="2022", Address="ktx", Phone="0123456789", Program="chuong trinh chuan", Status = StudentStatus.Studying },
        //        //new Student { Id = "22120250", Name = "Nguyen Van B", DateOfBirth = new DateTime(2004, 1, 1), Email = "nguyenvanb@gmail.com",
        //        //    Gender = Gender.Male, Faculty=Faculty.Law, Course="2022", Address="ktx", Phone="0123456789", Program="chuong trinh chuan", Status = StudentStatus.Studying }
        //    };

        //    return students.ToArray();
        //}

        //[Fact]
        //public async Task GetAllStudentsAsync_StudentsExist_ReturnsAllStudents()
        //{
        //    // Arrange
        //    int page = 1;
        //    int pageSize = 10;
        //    string key = string.Empty;
        //    // Act
        //    var students = await _studentRepository.GetAllStudentsAsync(page, pageSize, key);
        //    // Assert
        //    Assert.NotNull(students.students);
        //    Assert.Equal(2, students.total);
        //}
        //[Fact]
        //public async Task GetStudentByIdAsync_StudentExist_ReturnsStudent()
        //{
        //    // Arrange
        //    string studentId = "22120249";
        //    // Act
        //    var student = await _studentRepository.GetStudentByIdAsync(studentId);
        //    // Assert
        //    Assert.NotNull(student);
        //    Assert.Equal(studentId, student.Id);
        //}
        //[Fact]
        //public async Task GetStudentByNameAsync_StudentExist_ReturnsStudent()
        //{
        //    //Arrange
        //    string name = "Nguyen Van A";
        //    // Act
        //    var students = await _studentRepository.GetStudentsByNameAsync(name);
        //    // Assert
        //    Assert.Equal(name, students.FirstOrDefault().Name);
        //}

        ////[Fact]
        ////public async Task AddStudentAsync_StudentNotExist_AddStudent()
        ////{
        ////    // Arrange
        ////    var student = new Student
        ////    {
        ////        Id = "22120251",
        ////        Name = "Nguyen Van C",
        ////        DateOfBirth = new DateTime(2004, 1, 1),
        ////        Email = "nguyenvanc@gmail.com",
        ////        Gender = Gender.Male,
        ////        Faculty = Faculty.Law,
        ////        Course = "2022",
        ////        Address = "ktx",
        ////        Phone = "0123456789",
        ////        Program = "chuong trinh chuan",
        ////        Status = StudentStatus.Studying
        ////    };
        ////    // Act
        ////    var result = await _studentRepository.AddStudentAsync(student);

        ////    // Assert

        ////    Assert.True(result);
        ////}

        ////[Fact]
        ////public async Task UpdateStudentAsync_StudentExist_UpdateStudent()
        ////{
        ////    // Arrange
        ////    var student = new Student
        ////    {
        ////        Id = "22120251",
        ////        Name = "Nguyen Van C",
        ////        DateOfBirth = new DateTime(2004, 1, 1),
        ////        Email = "nguyenvanc@gmail.com",
        ////        Gender = Gender.Male,
        ////        Faculty = Faculty.Law,
        ////        Course = "2022",
        ////        Address = "ktx1",
        ////        Phone = "0123456789",
        ////        Program = "chuong trinh chuan",
        ////        Status = StudentStatus.Studying
        ////    };
        ////    // Act
        ////    await _studentRepository.AddStudentAsync(student);
        ////    student.Address = "ktx2";
        ////    var result = await _studentRepository.UpdateStudentAsync(student);

        ////    // Assert
        ////    Assert.True(result);
        ////}

        //[Fact]
        //public async Task DeleteStudentAsync_StudentExist_DeleteStudent()
        //{
        //    // Arrange
        //    string studentId = "22120249";
        //    // Act
        //    var result = await _studentRepository.DeleteStudentAsync(studentId);
        //    // Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public async Task IsEmailExistAsync_EmailExist_ReturnsTrue()
        //{
        //    // Arrange
        //    string email = "nguyenvana@gmail.com";

        //    // Act
        //    var result = await _studentRepository.IsEmailExistAsync(email);

        //    // Assert
        //    Assert.True(result);


        //}
    }
}
