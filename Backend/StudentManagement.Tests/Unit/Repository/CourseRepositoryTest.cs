using Moq;

using StudentManagement.DAL.Data.Repositories.CourseRepo;

using StudentManagement.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StudentManagement.Tests.Unit.Repository
{
    public class CourseRepositoryTest
    {
        private readonly Mock<ICourseRepository> _mockCourseRepository;
        private readonly List<Course> _mockCourses;

        public CourseRepositoryTest()
        {
            _mockCourseRepository = new Mock<ICourseRepository>();
            _mockCourses = new List<Course>
           {
               new Course { CourseId = "CS111", CourseName = "Math", Credits = 3 },
               new Course { CourseId = "CS222", CourseName = "Science", Credits = 4 }
           };
        }

        [Fact]
        public async Task GetAllCoursesAsync_ValidParameters_ReturnsAllCourses()
        {
            // Arrange  
            int page = 1;
            int limit = 2;
            Guid? facultyId = null;
            string? courseid = null;
            _mockCourseRepository.Setup(repo => repo.GetAllCoursesAsync(page, limit, facultyId, courseid, false)).ReturnsAsync((_mockCourses, 2));

            // Act    
            var result = await _mockCourseRepository.Object.GetAllCoursesAsync(page, limit, facultyId, courseid, false);

            // Assert    
            Assert.NotNull(result.Item1);
            Assert.Equal(2, result.Item2);
        }

        [Fact]
        public async Task GetCourseByIdAsync_ValidId_ReturnsCourse()
        {
            // Arrange    
            string courseId = "CS111";
            _mockCourseRepository.Setup(repo => repo.GetCourseByIdAsync(courseId)).ReturnsAsync(_mockCourses.First(c => c.CourseId == courseId));

            // Act    
            var result = await _mockCourseRepository.Object.GetCourseByIdAsync(courseId);

            // Assert    
            Assert.NotNull(result);
            Assert.Equal("Math", result.CourseName);
        }

        [Fact]
        public async Task AddCourseAsync_ValidCourse_AddsCourse()
        {
            // Arrange    
            var newCourse = new Course { CourseId = "CS333", CourseName = "History", Credits = 2 };
            _mockCourseRepository.Setup(repo => repo.AddCourseAsync(newCourse)).ReturnsAsync(newCourse);

            // Act    
            await _mockCourseRepository.Object.AddCourseAsync(newCourse);

            // Assert    
            _mockCourseRepository.Verify(repo => repo.AddCourseAsync(newCourse), Times.Once);
        }

        [Fact]
        public async Task DeleteCourseAsync_ValidId_DeletesCourse()
        {
            // Arrange    
            string courseId = "CS111";
            _mockCourseRepository.Setup(repo => repo.DeleteCourseAsync(courseId)).Returns(Task.CompletedTask);

            // Act    
            await _mockCourseRepository.Object.DeleteCourseAsync(courseId);

            // Assert    
            _mockCourseRepository.Verify(repo => repo.DeleteCourseAsync(courseId), Times.Once);
        }
    }
}
