using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Domain.Utils;
using StudentManagement.Domain.Models;
using StudentManagement.DAL.Data.Utils;
namespace StudentManagement.DAL.Data.Repositories.StudentRepo;

public interface IStudentRepository
{
    Task<(IEnumerable<Student> students, int total)> GetAllStudentsAsync(int page, int pageSize, string? faculty, string? program, string? status, string? key);

    Task<Student?> GetStudentByIdAsync(string studentId);

    Task<IEnumerable<Student?>> GetStudentsByNameAsync(string name);

    Task<IEnumerable<Student>> AddStudentAsync(IEnumerable<Student> students);

    Task<Student?> UpdateStudentAsync(Student student);

    Task DeleteStudentAsync(string studentId);

    Task<bool> IsEmailDuplicateAsync(string email);

    Task<int> GetLatestStudentIdAsync(int course);

}