using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.ClassStudentRepo
{
    public interface IClassStudentRepository
    {
        Task<IEnumerable<ClassStudent>> GetClassStudentsAsync(string? classId = null, string? studentId = null, int? page = null, int? limit = null);
        Task<ClassStudent?> GetClassStudentByIdAsync(string classId, string studentId);
        Task<IEnumerable<ClassStudent?>> GetClassStudentByIdAndCourseAsync(string studentId, string courseId);
        Task<ClassStudent?> AddClassStudentAsync(ClassStudent classStudent);
        Task UpdateClassStudentAsync(ClassStudent classStudent);
        Task DeleteClassStudentAsync(string classId, string studentId);
    }
}
