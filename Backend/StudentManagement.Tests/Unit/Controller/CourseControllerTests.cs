using Microsoft.AspNetCore.Mvc;
using Moq;
using StudentManagement.API.Controllers;
using StudentManagement.BLL.DTOs.Course;
using StudentManagement.BLL.Services.CourseService;
using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Unit.Controller
{
    public class CourseControllerTests
    {
        private readonly Mock<ICourseService> _mockCourseService;
        private readonly CourseController _controller;

        public CourseControllerTests()
        {
            _mockCourseService = new Mock<ICourseService>();
            _controller = new CourseController(_mockCourseService.Object);
        }

        [Fact]
        public async Task AddCourse_ValidCourseDTO_ReturnsOk()
        {
            // Arrange  
            var courseDTO = new AddCourseDTO();
            _mockCourseService.Setup(s => s.AddCourseAsync(courseDTO))
               .ReturnsAsync(Result<AddCourseDTO>.Ok(courseDTO));

            // Act  
            var result = await _controller.AddCourse(courseDTO);

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(courseDTO, okResult.Value);
        }

        [Fact]
        public async Task AddCourse_InvalidCourseDTO_ReturnsOk()
        {
            // Arrange  
            var courseDTO = new AddCourseDTO();
            _mockCourseService.Setup(s => s.AddCourseAsync(courseDTO))
                .ReturnsAsync(Result<AddCourseDTO>.Fail("ADD_COURSE_FAILED"));

            // Act  
            var result = await _controller.AddCourse(courseDTO);

            // Assert  
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCourse_IdExist_ReturnsOk()
        {
            // Arrange  
            var courseId = "123";
            _mockCourseService.Setup(s => s.DeleteCourseAsync(courseId))
                .ReturnsAsync(Result<string>.Ok(courseId));

            // Act  
            var result = await _controller.DeleteCourse(courseId);

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCourse_InvalidId_ReturnsBadRequest()
        {
            // Arrange  
            var courseId = "123";
            _mockCourseService.Setup(s => s.DeleteCourseAsync(courseId))
                .ReturnsAsync(Result<string>.Fail("COURSE_NOT_FOUND"));

            // Act  
            var result = await _controller.DeleteCourse(courseId);

            // Assert  
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateCourseById_ValidInput_ReturnsOk()
        {
            // Arrange  
            var courseId = "123";
            var updateCourseDTO = new UpdateCourseDTO();
            _mockCourseService.Setup(s => s.UpdateCourseByIdAsync(courseId, updateCourseDTO))
                .ReturnsAsync(Result<UpdateCourseDTO>.Ok(updateCourseDTO));

            // Act  
            var result = await _controller.UpdateCourseById(courseId, updateCourseDTO);

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updateCourseDTO, okResult.Value);
        }

        [Fact]
        public async Task UpdateCourseById_InvalidInput_ReturnsBadRequest()
        {
            // Arrange  
            var courseId = "123";
            var updateCourseDTO = new UpdateCourseDTO();
            _mockCourseService.Setup(s => s.UpdateCourseByIdAsync(courseId, updateCourseDTO))
                .ReturnsAsync(Result<UpdateCourseDTO>.Fail("UPDATE_COURSE_FAILED", "The course already has enrolled students"));

            // Act  
            var result = await _controller.UpdateCourseById(courseId, updateCourseDTO);

            // Assert  
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetAllStudent_ReturnsOk_WhenServiceReturnsSuccess()
        {
            // Arrange  
            var res = new GetAllCoursesDTO
            {
                Data = new List<GetCourseDTO>(),
                Total = 0
            };
            _mockCourseService.Setup(s => s.GetAllCourseAsync(1, 10, null, null, false))
                .ReturnsAsync(Result<GetAllCoursesDTO>.Ok(res));

            // Act  
            var result = await _controller.GetAllStudent(1, 10, null, null, false);

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllStudent_ReturnsBadRequest_WhenServiceReturnsFailure()
        {
            // Arrange  
            _mockCourseService.Setup(s => s.GetAllCourseAsync(1, 10, null, null, false))
                .ReturnsAsync(Result<GetAllCoursesDTO>.Fail("GET_COURSES_FAILED"));

            // Act  
            var result = await _controller.GetAllStudent(1, 10, null, null, false);

            // Assert  
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetStudentById_ReturnsOk_WhenServiceReturnsSuccess()
        {
            // Arrange  
            var courseId = "123";
            _mockCourseService.Setup(s => s.GetAllCourseByIdAsync(courseId))
                .ReturnsAsync(Result<GetCourseDTO>.Ok(new GetCourseDTO()));

            // Act  
            var result = await _controller.GetStudentById(courseId);

            // Assert  
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetStudentById_ReturnsBadRequest_WhenServiceReturnsFailure()
        {
            // Arrange  
            var courseId = "123";
            _mockCourseService.Setup(s => s.GetAllCourseByIdAsync(courseId))
                .ReturnsAsync(Result<GetCourseDTO>.Fail("GET_COURSE_FAILED"));

            // Act  
            var result = await _controller.GetStudentById(courseId);

            // Assert  
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        
    }
}
