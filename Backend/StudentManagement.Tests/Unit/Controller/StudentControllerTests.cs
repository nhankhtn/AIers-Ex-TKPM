using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using StudentManagement.API.Controllers;
using StudentManagement.API.Utils;
using StudentManagement.BLL;
using StudentManagement.BLL.Checker;
using StudentManagement.BLL.DTOs.Identity;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.BLL.Services.EmailService;
using StudentManagement.BLL.Services.StudentService;
using StudentManagement.BLL.Validators;
using StudentManagement.DAL.Data.Repositories.FacultyRepo;
using StudentManagement.DAL.Data.Repositories.ProgramRepo;
using StudentManagement.DAL.Data.Repositories.SettingRepository;
using StudentManagement.DAL.Data.Repositories.StudentRepo;
using StudentManagement.DAL.Data.Repositories.StudentStatusRepo;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Unit.Controller
{
    public class StudentControllerTests
    {
        private readonly StudentsController _studentsController;

        public StudentControllerTests()
        {
            var _context = TestDbContextFactory.Create();

            var _facultyRepository = new FacultyRepository(_context);
            var _programRepository = new ProgramRepository(_context);
            var _studentStatusRepository = new StudentStatusRepository(_context);
            var _studentRepository = new StudentRepository(_context);
            var _settingRepository = new SettingRepository(_context);
            var _studentChecker = new StudentChecker(
                _studentRepository,
                _facultyRepository,
                _programRepository,
                _studentStatusRepository
                );
            var _studentValidator = new StudentValidator(
                new EmailService(_settingRepository)
                );

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

            var _studentService = new StudentService(
                _studentRepository,
                _facultyRepository,
                _studentStatusRepository,
                _programRepository,
                _studentValidator,
                _studentChecker,
                _mapper
                );

            _studentsController = new StudentsController(_studentService);
        }

        [Fact]
        public async Task GetAllStudents_StudentsExist_ReturnsStudents()
        {
            var page = 1;
            var limit = 10;

            var result = await _studentsController.GetAllStudents(page, limit, null, null, null, null);

            var actionResult = Assert.IsType<ActionResult<GetStudentsDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var response = Assert.IsType<GetStudentsDTO>(okResult.Value);

            Assert.NotNull(response.Data);
            Assert.IsType<List<StudentDTO>>(response.Data);
            Assert.Equal(5, response.Data.Count());
            Assert.Equal(5, response.Total);  // Giả sử total bạn đang trả về là 10
        }

        [Fact]
        public async Task GetAllStudents_StudentsNotExist_ReturnsNotFound()
        {
            var page = 1;
            var limit = 10;

            var result = await _studentsController.GetAllStudents(page, limit, null, null, null, Guid.NewGuid().ToString());

            var actionResult = Assert.IsType<ActionResult<GetStudentsDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var response = Assert.IsType<GetStudentsDTO>(okResult.Value);

            Assert.NotNull(response.Data);
            Assert.IsType<List<StudentDTO>>(response.Data);
            Assert.Empty(response.Data);
            Assert.Equal(0, response.Total); 
        }

        [Fact]
        public async Task AddStudents_AcceptAndUnacceptStudents_ReturnsResultMultiState()
        {
            var students = new List<StudentDTO>
            {
                new StudentDTO { Id = "ST006", Name = "John Doe", DateOfBirth = new DateTime(2000, 1, 1), Gender = "Male", Email = "john22@gmail.com", Course = 2022, Phone = "+84363459789", PermanentAddress = "Address 1", Program = TestDbContextFactory.Guid1.ToString(), Status = TestDbContextFactory.Guid1.ToString(), Faculty = TestDbContextFactory.Guid1.ToString(), Nationality = "USA", Identity = new IdentityDTO { DocumentNumber = "666"} },
                new StudentDTO { Id = "ST007", Name = "John Doe", DateOfBirth = new DateTime(2000, 1, 1), Gender = "Male", Email = "john223@gmail.com", Course = 2022, Phone = "+84363459700", PermanentAddress = "Address 1", Program = TestDbContextFactory.Guid1.ToString(), Status = TestDbContextFactory.Guid1.ToString(), Faculty = TestDbContextFactory.Guid1.ToString(), Nationality = "USA", Identity = new IdentityDTO { DocumentNumber = "777"} },
            };

            var result = await _studentsController.AddStudents(students);

            var actionResult = Assert.IsType<ActionResult<ApiResponse<IEnumerable<StudentDTO>>>>(result);
            var okResult = Assert.IsType<ObjectResult>(actionResult.Result);

            var response = Assert.IsType<ApiResponse<IEnumerable<StudentDTO>>>(okResult.Value);

            Assert.NotNull(response.Data);
            Assert.Equal(2, response.Data.Count());
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

            var result = await _studentsController.UpdateStudent("ST001", updateStudentDTO);
            var actionResult = Assert.IsType<ActionResult<ApiResponse<StudentDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<StudentDTO>>(okResult.Value);
            var data = response.Data;

            Assert.NotNull(data);
            Assert.Equal(updateStudentDTO.Name, data.Name);
            Assert.Equal(updateStudentDTO.Email, data.Email);
            Assert.Equal(updateStudentDTO.Identity.DocumentNumber, data.Identity?.DocumentNumber);
        }

        [Fact]
        public async Task UpdateStudentAsync_NotExistingStudent_ShouldReturnUpdatedStudent()
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

            var result = await _studentsController.UpdateStudent("ST999", updateStudentDTO);
            var actionResult = Assert.IsType<ActionResult<ApiResponse<StudentDTO>>>(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<StudentDTO>>(okResult.Value);
            var data = response.Data;

            Assert.Null(data);
            Assert.Equal("STUDENT_NOT_FOUND", response.Error?.Code);
        }

        [Fact]
        public async Task UpdateStudentAsync_DuplicateEmail_ShouldReturnUpdatedStudent()
        {
            var updateStudentDTO = new StudentDTO
            {
                Email = "jane@gmail.com"
            };

            var result = await _studentsController.UpdateStudent("ST001", updateStudentDTO);
            var actionResult = Assert.IsType<ActionResult<ApiResponse<StudentDTO>>>(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<StudentDTO>>(okResult.Value);
            var data = response.Data;

            Assert.Null(data);
            Assert.Equal("DUPLICATE_EMAIL", response.Error?.Code);
        }

        [Fact]
        public async Task UpdateStudentAsync_DuplicatePhone_ShouldReturnUpdatedStudent()
        {
            var updateStudentDTO = new StudentDTO
            {
                Phone = "+84345678901"
            };

            var result = await _studentsController.UpdateStudent("ST001", updateStudentDTO);
            var actionResult = Assert.IsType<ActionResult<ApiResponse<StudentDTO>>>(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<StudentDTO>>(okResult.Value);
            var data = response.Data;

            Assert.Null(data);
            Assert.Equal("DUPLICATE_PHONE", response.Error?.Code);
        }

        [Fact]
        public async Task UpdateStudentAsync_DuplicateDocumentNumber_ShouldReturnUpdatedStudent()
        {
            var updateStudentDTO = new StudentDTO
            {
                Identity = new IdentityDTO
                {
                    DocumentNumber = "222"
                }
            };

            var result = await _studentsController.UpdateStudent("ST001", updateStudentDTO);
            var actionResult = Assert.IsType<ActionResult<ApiResponse<StudentDTO>>>(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<StudentDTO>>(okResult.Value);
            var data = response.Data;

            Assert.Null(data);
            Assert.Equal("DUPLICATE_DOCUMENT_NUMBER", response.Error?.Code);
        }

        [Fact]
        public async Task UpdateStudentAsync_FacultyNotExists_ShouldReturnUpdatedStudent()
        {
            var updateStudentDTO = new StudentDTO
            {
                Faculty = Guid.NewGuid().ToString()
            };

            var result = await _studentsController.UpdateStudent("ST001", updateStudentDTO);
            var actionResult = Assert.IsType<ActionResult<ApiResponse<StudentDTO>>>(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<StudentDTO>>(okResult.Value);
            var data = response.Data;

            Assert.Null(data);
            Assert.Equal("INVALID_FACULTY", response.Error?.Code);
        }

        [Fact]
        public async Task UpdateStudentAsync_ProgramNotExists_ShouldReturnUpdatedStudent()
        {
            var updateStudentDTO = new StudentDTO
            {
                Program = Guid.NewGuid().ToString()
            };

            var result = await _studentsController.UpdateStudent("ST001", updateStudentDTO);
            var actionResult = Assert.IsType<ActionResult<ApiResponse<StudentDTO>>>(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<StudentDTO>>(okResult.Value);
            var data = response.Data;

            Assert.Null(data);
            Assert.Equal("INVALID_PROGRAM", response.Error?.Code);
        }

        [Fact]
        public async Task UpdateStudentAsync_StatusNotExists_ShouldReturnUpdatedStudent()
        {
            var updateStudentDTO = new StudentDTO
            {
                Status = Guid.NewGuid().ToString()
            };

            var result = await _studentsController.UpdateStudent("ST001", updateStudentDTO);
            var actionResult = Assert.IsType<ActionResult<ApiResponse<StudentDTO>>>(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<StudentDTO>>(okResult.Value);
            var data = response.Data;

            Assert.Null(data);
            Assert.Equal("INVALID_STATUS_UPDATE", response.Error?.Code);
        }

        [Fact]
        public async Task UpdateStudentAsync_LowerStatus_ShouldReturnUpdatedStudent()
        {
            var updateStudentDTO = new StudentDTO
            {
                Status = TestDbContextFactory.Guid1.ToString()
            };

            var result = await _studentsController.UpdateStudent("ST002", updateStudentDTO);
            var actionResult = Assert.IsType<ActionResult<ApiResponse<StudentDTO>>>(result);
            var okResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<StudentDTO>>(okResult.Value);
            var data = response.Data;

            Assert.Null(data);
            Assert.Equal("INVALID_STATUS_UPDATE", response.Error?.Code);
        }
    }
}
