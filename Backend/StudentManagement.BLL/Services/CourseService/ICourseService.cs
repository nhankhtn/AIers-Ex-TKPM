using StudentManagement.BLL.DTOs.Course;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.CourseService
{
    public interface ICourseService
    {
        Task<Result<AddCourseDTO>> AddCourseAsync(AddCourseDTO courseDTO);
        Task<Result<string>> DeleteCourseAsync(string courseId);
        Task<Result<UpdateCourseDTO>> UpdateCourseByIdAsync(string courdeId, UpdateCourseDTO course);

        Task<Result<GetAllCoursesDTO>> GetAllCourseAsync(int page, int limit, Guid? facultyId, string? courseId, bool isDeleted);
        Task<Result<GetCourseDTO>> GetAllCourseByIdAsync(string courseId);
    }
}
