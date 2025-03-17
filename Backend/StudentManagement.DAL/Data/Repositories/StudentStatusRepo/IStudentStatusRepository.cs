using StudentManagement.Domain.Models;
using StudentManagement.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.StudentStatusRepo
{
    public interface IStudentStatusRepository
    {
        Task<Result<StudentStatus?>> GetStudentStatusByIdAsync(int id);

        Task<Result<StudentStatus?>> GetStudentStatusByNameAsync(string name);

        Task<Result<IEnumerable<StudentStatus>>> GetAllStudentStatusesAsync();

        Task<Result<StudentStatus>> AddStudentStatusAsync(StudentStatus studentStatus);

        Task<Result<StudentStatus>> UpdateStudentStatusAsync(StudentStatus studentStatus);
    }
}
