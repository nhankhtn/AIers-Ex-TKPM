using StudentManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentManagement.DAL.Data.Repositories.StudentStatusRepo
{
    public interface IStudentStatusRepository
    {
        Task<StudentStatus?> GetStudentStatusByIdAsync(string id);
        Task<StudentStatus?> GetStudentStatusByIdAsync(Guid id);
        Task<StudentStatus?> GetStudentStatusByNameAsync(string name);
        Task<IEnumerable<StudentStatus>> GetAllStudentStatusesAsync();
        Task<StudentStatus?> AddStudentStatusAsync(StudentStatus studentStatus);
        Task<StudentStatus?> UpdateStudentStatusAsync(StudentStatus studentStatus);
        Task DeleteStudentStatusAsync(Guid studentStatus);
    }
}
