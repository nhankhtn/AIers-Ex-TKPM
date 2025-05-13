using AutoMapper;
using Castle.Core.Smtp;
using Moq;
using StudentManagement.BLL.Checker;
using StudentManagement.BLL.DTOs.Identity;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Mapping;
using StudentManagement.BLL.Services;
using StudentManagement.BLL.Services.EmailService;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.BLL.Validators;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.SettingRepository;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.Domain.Enums;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Unit.Service
{
    public class StudentServiceTests
    {
        private readonly IStudentChecker _studentChecker;
        private readonly IStudentValidator _studentValidator;
        private readonly IStudentService _studentService;

        public StudentServiceTests()
        {
            var _context = TestDbContextFactory.Create();

            var _facultyRepository = new FacultyRepository(_context);
            var _programRepository = new ProgramRepository(_context);
            var _studentStatusRepository = new StudentStatusRepository(_context);
            var _studentRepository = new StudentRepository(_context);
            var _settingRepository = new SettingRepository(_context);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>(); // Add the MappingProfile here
            });
            var _mapper = mapperConfig.CreateMapper(); // Create the IMapper instance

            _studentChecker = new StudentChecker(
                _studentRepository,
                _facultyRepository,
                _programRepository,
                _studentStatusRepository
                );

            _studentValidator = new StudentValidator(
                new EmailService(_settingRepository)
                );

            _studentService = new StudentService(
                _studentRepository,
                _facultyRepository,
                _studentStatusRepository,
                _programRepository,
                _studentValidator,
                _studentChecker,
                _mapper
                );
        }


        [Fact]
        public async Task GetAllStudentsAsync_StudentsExist_ReturnsAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync(1, 10, null, null, null, null);
            Assert.NotNull(students.Data);
            Assert.NotNull(students.Data.Data);
            Assert.Equal(5, students.Data.Total);
        }

        [Fact]
        public async Task AddStudentsAsync_ValidStudents_ShouldReturnAcceptableAndUnacceptableLists()
        {
            var students = new List<StudentDTO>
            {
                new StudentDTO { Id = "ST006", Name = "John Doe", DateOfBirth = new DateTime(2000, 1, 1), Gender = "Male", Email = "john232@gmail.com", Course = 2022, Phone = "+84363459789", PermanentAddress = "Address 1", Program = TestDbContextFactory.Guid1.ToString(), Status = TestDbContextFactory.Guid1.ToString(), Faculty = TestDbContextFactory.Guid1.ToString(), Nationality = "USA", Identity = new IdentityDTO { DocumentNumber = "666"} },
                new StudentDTO { Id = "ST007", Name = "John Doe", DateOfBirth = new DateTime(2000, 1, 1), Gender = "Male", Email = "john2223@gmail.com", Course = 2022, Phone = "+84363459700", PermanentAddress = "Address 1", Program = TestDbContextFactory.Guid1.ToString(), Status = TestDbContextFactory.Guid1.ToString(), Faculty = TestDbContextFactory.Guid1.ToString(), Nationality = "USA", Identity = new IdentityDTO { DocumentNumber = "777"} },
            };

            var result = await _studentService.AddListStudentAsync(students);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task UpdateStudentAsync_ExistingStudent_ShouldReturnUpdatedStudent()
        {
            var updateStudentDTO = new StudentDTO 
            {
                Name = "Chau Ngoc Phat",
                Email = "ngocphat@gmail.com",
                Identity = new IdentityDTO
                {
                    DocumentNumber = "10291"
                }
            };

            var result = await _studentService.UpdateStudentAsync("ST001", updateStudentDTO);
            Assert.NotNull(result.Data);
            Assert.Equal(updateStudentDTO.Name, result.Data.Name);
            Assert.Equal(updateStudentDTO.Email, result.Data.Email);
            Assert.Equal(updateStudentDTO.Identity.DocumentNumber, result.Data?.Identity?.DocumentNumber);
        }

        [Fact]
        public async Task UpdateStudentAsync_NotExistingStudent_ShouldReturnResultFail()
        {
            var updateStudentDTO = new StudentDTO
            {
                Name = "Chau Ngoc Phat",
                Email = "ngocphat@gmail.com",
                Identity = new IdentityDTO
                {
                    DocumentNumber = "10291"
                }
            };

            var result = await _studentService.UpdateStudentAsync("ST999", updateStudentDTO);
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("STUDENT_NOT_FOUND", result.ErrorCode);
        }

        [Fact]
        public async Task UpdateStudentAsync_EmailExists_ShouldReturnResultFail()
        {
            var updateStudentDTO = new StudentDTO
            {
                Email = "jane@gmail.com"
            };

            var result = await _studentService.UpdateStudentAsync("ST001", updateStudentDTO);
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("DUPLICATE_EMAIL", result.ErrorCode);
        }

        [Fact]
        public async Task UpdateStudentAsync_PhoneExists_ShouldReturnResultFail()
        {
            var updateStudentDTO = new StudentDTO
            {
                Phone = "+84334567890"
            };

            var result = await _studentService.UpdateStudentAsync("ST001", updateStudentDTO);
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("DUPLICATE_PHONE", result.ErrorCode);
        }

        [Fact]
        public async Task UpdateStudentAsync_DocumentNumberExists_ShouldReturnResultFail()
        {
            var updateStudentDTO = new StudentDTO
            {
                Identity = new IdentityDTO
                {
                    DocumentNumber = "222",
                }
            };

            var result = await _studentService.UpdateStudentAsync("ST001", updateStudentDTO);
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("DUPLICATE_DOCUMENT_NUMBER", result.ErrorCode);
        }

        [Fact]
        public async Task UpdateStudentAsync_FacultyNotExists_ShouldReturnResultFail()
        {
            var updateStudentDTO = new StudentDTO
            {
                Faculty = Guid.NewGuid().ToString()
            };

            var result = await _studentService.UpdateStudentAsync("ST001", updateStudentDTO);
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("INVALID_FACULTY", result.ErrorCode);
        }

        [Fact]
        public async Task UpdateStudentAsync_StatusNotExists_ShouldReturnResultFail()
        {
            var updateStudentDTO = new StudentDTO
            {
                Status = Guid.NewGuid().ToString() // Giả sử Status không tồn tại
            };

            var result = await _studentService.UpdateStudentAsync("ST001", updateStudentDTO);

            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("INVALID_STATUS_UPDATE", result.ErrorCode);
        }

        [Fact]
        public async Task UpdateStudentAsync_LowerStatus_ShouldReturnResultFail()
        {
            var updateStudentDTO = new StudentDTO
            {
                Status = TestDbContextFactory.Guid1.ToString() // Order 2 to 1
            };

            var result = await _studentService.UpdateStudentAsync("ST002", updateStudentDTO);

            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("INVALID_STATUS_UPDATE", result.ErrorCode);
        }

        [Fact]
        public async Task UpdateStudentAsync_ProgramNotExists_ShouldReturnResultFail()
        {
            var updateStudentDTO = new StudentDTO
            {
                Program = Guid.NewGuid().ToString() // Giả sử Program không tồn tại
            };

            var result = await _studentService.UpdateStudentAsync("ST001", updateStudentDTO);

            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.Equal("INVALID_PROGRAM", result.ErrorCode);
        }
    }
}
