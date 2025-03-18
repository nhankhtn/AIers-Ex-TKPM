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
    Task<Result<(IEnumerable<Student> students, int total)>> GetAllStudentsAsync(int page, int pageSize, string? key);

    Task<Result<Student?>> GetStudentByIdAsync(string studentId);

    Task<Result<IEnumerable<Student?>>> GetStudentsByNameAsync(string name);

    Task<Result<IEnumerable<Student>>> AddStudentAsync(IEnumerable<Student> students);

    Task<Result<string>> UpdateStudentAsync(Student student);

    Task<Result<string>> DeleteStudentAsync(string studentId);

    Task<Result<string>> IsEmailDuplicateAsync(string email);

    Task<int> GetLatestStudentIdAsync(int course);

}