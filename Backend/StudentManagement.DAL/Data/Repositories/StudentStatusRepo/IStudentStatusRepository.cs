using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.StudentStatusRepo
{
    public interface IStudentStatusRepository
    {
        Task<StudentStatus?> GetStudentStatusByIdAsync(int id);

        Task<StudentStatus?> GetStudentStatusByNameAsync(string name);

        Task<IEnumerable<StudentStatus>> GetAllStudentStatusesAsync();

        Task<bool> AddStudentStatusAsync(StudentStatus studentStatus);

        Task<bool> UpdateStudentStatusAsync(StudentStatus studentStatus);
    }
}
