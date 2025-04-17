using AutoMapper;
using Moq;
using StudentManagement.BLL.DTOs.Course;
using StudentManagement.BLL.Services.CourseService;
using StudentManagement.DAL.Data.Repositories.CourseRepo;
using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests.Unit.Service
{
    public class CourseServiceTests
    {
        private readonly Mock<ICourseRepository> _courseRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICourseService _courseService;

        public CourseServiceTests()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _mapperMock = new Mock<IMapper>();
            _courseService = new CourseService(_courseRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddCourseAsync_ValidCourse_ReturnsSuccess()
        {
            // Arrange  
            var courseDTO = new AddCourseDTO { CourseName = "Test Course", Credits = 3 };
            var course = new Course { CourseName = "Test Course", Credits = 3 };
            _mapperMock.Setup(m => m.Map<Course>(courseDTO)).Returns(course);
            _courseRepositoryMock.Setup(r => r.AddCourseAsync(course)).ReturnsAsync(course);
            _mapperMock.Setup(m => m.Map<AddCourseDTO>(course)).Returns(courseDTO);

            // Act  
            var result = await _courseService.AddCourseAsync(courseDTO);

            // Assert  
            Assert.True(result.Success);
            Assert.Equal(courseDTO, result.Data);
        }

        [Fact]
        public async Task DeleteCourseAsync_CourseNotFound_ReturnsFail()
        {
            // Arrange  
            var courseId = "nonexistent";
            _courseRepositoryMock.Setup(r => r.GetCourseByIdAsync(courseId)).ReturnsAsync((Course)null);

            // Act  
            var result = await _courseService.DeleteCourseAsync(courseId);

            // Assert  
            Assert.False(result.Success);
            Assert.Equal("COURSE_NOT_FOUND", result.ErrorCode);
        }

        [Fact]
        public async Task GetAllCourseAsync_CoursesExist_ReturnsCourses()
        {
            // Arrange  
            var courses = new List<Course> { new Course { CourseName = "Course1" }, new Course { CourseName = "Course2" } };
            var courseDTOs = new List<GetCourseDTO> { new GetCourseDTO { CourseName = "Course1" }, new GetCourseDTO { CourseName = "Course2" } };
            _courseRepositoryMock.Setup(r => r.GetAllCoursesAsync(1, 10, null, null, false)).ReturnsAsync((courses, 2));
            _mapperMock.Setup(m => m.Map<GetCourseDTO>(It.IsAny<Course>())).Returns((Course c) => new GetCourseDTO { CourseName = c.CourseName });

            // Act  
            var result = await _courseService.GetAllCourseAsync(1, 10, null, null, false);

            // Assert  
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Total);
            Assert.Equal(courseDTOs.Count, result.Data.Data.Count);
        }

        [Fact]
        public async Task UpdateCourseByIdAsync_CourseNotFound_ReturnsFail()
        {
            // Arrange  
            var courseId = "nonexistent";
            var updateDTO = new UpdateCourseDTO { CourseName = "Updated Course", Credits = 4 };
            _courseRepositoryMock.Setup(r => r.GetCourseByIdAsync(courseId)).ReturnsAsync((Course)null);

            // Act  
            var result = await _courseService.UpdateCourseByIdAsync(courseId, updateDTO);

            // Assert  
            Assert.False(result.Success);
            Assert.Equal("COURSE_NOT_FOUND", result.ErrorCode);
        }
    }
}
