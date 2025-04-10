using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.CourseRepo
{
    public interface ICourseRepository
    {
        Task<(IEnumerable<Course>, int)> GetAllCoursesAsync(int page, int limit, Guid? facultyId, string? courseId, bool isDeleted);
        Task<Course?> GetCourseByIdAsync(string courseId);
        Task<Course> AddCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(string courseId);

        Task<bool> HasAnyClassesAsync(string courseId);

        Task<bool> CheckHasAnyStudentInCourseAsync(string courseId);
    }
}
