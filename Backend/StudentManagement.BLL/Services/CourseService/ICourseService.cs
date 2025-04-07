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
        Task<Result<int>> DeleteCourseAsync(int courseId);
    }
}
