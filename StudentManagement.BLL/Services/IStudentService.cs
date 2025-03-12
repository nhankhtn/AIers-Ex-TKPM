using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StudentManagement.BLL.DTOs;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services
{
    public interface IStudentService
    {
        Task<Result<IEnumerable<StudentDTO>>> GetAllStudentsAsync(int page, int pageSize, string? name, string? id);

        Task<Result<string>> AddStudentAsync(StudentDTO studentDTO);

        Task<Result<string>> DeleteStudentAsync(string studentId);

        Task<Result<string>> UpdateStudentAsync(string userId, UpdateStudentDTO studentDTO);

        Task<Result<StudentDTO?>> GetStudentByIdAsync(string studentId);

        Task<Result<IEnumerable<StudentDTO>>> GetStudentsByNameAsync(string name);
    }
}
