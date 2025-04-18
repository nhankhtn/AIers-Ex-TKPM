using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StudentManagement.BLL.DTOs;
using StudentManagement.BLL.DTOs.Students;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.BLL.Services.StudentService
{
    public interface IStudentService
    {
        Task<Result<GetStudentsDTO>> GetAllStudentsAsync(int? page, int? pageSize, string? faculty, string? program, string? status, string? key);

        Task<Result<IEnumerable<StudentDTO>>> AddListStudentAsync(IEnumerable<StudentDTO> studentDTOs);

        Task<Result<StudentDTO>> UpdateStudentAsync(string userId, StudentDTO studentDTO);

        Task<Result<StudentDTO>> GetStudentByIdAsync(string studentId);

        Task<Result<string>> DeleteStudentByIdAsync(string studentId);

        Task<Result<IEnumerable<StudentDTO>>> GetStudentsByNameAsync(string name);
    }
}