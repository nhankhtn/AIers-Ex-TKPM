using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StudentManagement.BLL.DTOs;
using StudentManagement.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.StudentService
{
    public interface IStudentService
    {
        Task<Result<GetStudentsDto>> GetAllStudentsAsync(int page, int pageSize, string? key);

        Task<Result<StudentDTO>> AddStudentAsync(StudentDTO studentDTO);

        Task<Result<StudentDTO>> UpdateStudentAsync(string userId, UpdateStudentDTO studentDTO);

        Task<Result<StudentDTO?>> GetStudentByIdAsync(string studentId);

        Task<Result<string>> DeleteStudentByIdAsync(string studentId);

        Task<Result<IEnumerable<StudentDTO>>> GetStudentsByNameAsync(string name);
    }
}