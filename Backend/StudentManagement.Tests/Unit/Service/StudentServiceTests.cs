using AutoMapper;
using Moq;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Services;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Unit.Service
{
    public class StudentServiceTests
    {
        //moq
        private readonly Mock<IStudentRepository> _mockStudentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IStudentService _studentService;

        public StudentServiceTests()
        {
            _mockStudentRepository = new Mock<IStudentRepository>();
            _mockMapper = new Mock<IMapper>();
            _studentService = new StudentService(_mockStudentRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllStudentsAsync_StudentsExist_ReturnsAllStudents()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            string key = "key";
            var students = new List<Student>
            {
                new Student { Id = "1", Name = "Student 1", Address="ktx", Course="2022", DateOfBirth = new DateTime(2024, 1, 1),
                    Email="student1@gmail.com", Faculty=Faculty.Japanese, Gender=Gender.Female, Phone="0389277431", Program="chuong trinh chuan", Status=StudentStatus.Studying }
            };
            var total = students.Count;
            var getStudentsDto = new GetStudentsDto
            {
                Students = new List<StudentDTO>
                {
                    new StudentDTO { Id = "1", Name = "Student 1", DateOfBirth = new DateTime(2024, 1, 1),
                        Email="student1@gmail.com", Faculty=2, Gender=1, Phone="0389277431", Program="chuong trinh chuan", Status=0}
                },
                Total = total,
                PageIndex = page,
                PageSize = pageSize
            };
            _mockStudentRepository.Setup(repo => repo.GetAllStudentsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync((students, total));
            _mockMapper.Setup(x => x.Map<IEnumerable<StudentDTO>>(It.IsAny<List<Student>>())).Returns(getStudentsDto.Students);

            // Act
            var result = await _studentService.GetAllStudentsAsync(page, pageSize, key);
            // Assert

            Assert.Equal(getStudentsDto.Students, result.Data.Students);
        }

        [Fact]
        public async Task GetStudentByIdAsync_HasStudentId_ReturnStudentDto()
        {
            // Arrange
            var studentId = "1";
            var student = new Student
            {
                Id = "1",
                Name = "Student 1",
                Address = "ktx",
                Course = "2022",
                DateOfBirth = new DateTime(2024, 1, 1),
                Email = "student1@gmail.com",
                Faculty = Faculty.Japanese,
                Gender = Gender.Female,
                Phone = "0389277431",
                Program = "chuong trinh chuan",
                Status = StudentStatus.Studying
            };
            var studentDto = new StudentDTO
            {
                Id = "1",
                Name = "Student 1",
                DateOfBirth = new DateTime(2024, 1, 1),
                Email = "student1@gmail.com",
                Faculty = 2,
                Gender = 1,
                Phone = "0389277431",
                Program = "chuong trinh chuan",
                Status = 0
            };
            _mockStudentRepository.Setup(repo => repo.GetStudentByIdAsync(It.IsAny<string>())).ReturnsAsync(student);
            _mockMapper.Setup(x => x.Map<StudentDTO>(It.IsAny<Student>())).Returns(studentDto);

            // Act
            var result = await _studentService.GetStudentByIdAsync(studentId);
            // Assert
            Assert.NotNull(result);

        }
    }
}
